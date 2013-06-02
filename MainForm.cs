
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data;
using System.Configuration;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        //HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null,copyimg=null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        string[] LabelNameNId;
        int ContTrain, NumLabels, t;
        string name, names = null;
        static string ConnStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        OleDbConnection MyConn = new OleDbConnection(ConnStr);
        public FrmPrincipal()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {

                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Register new Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void btnDetect_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();

            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            btnDetect.Enabled = false;
        }


        private void btnRegisterNew_Click(object sender, System.EventArgs e)
        {
            if (frmLogin.admin == false)
            {
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
                return;
            }
            if (txtName.Text=="")
            {
                MessageBox.Show("Please enter name first.");
                return;
            }
            
            try
            {

                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
               
                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(txtName.Text + "|" + (ContTrain).ToString());

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(txtName.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Text = "";
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            

        }


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");

            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(473, 355, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Yellow), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);
                    
                    name = recognizer.Recognize(result);
                    string index = recognizer.RecognizeIndex(result);
                    if (index != string.Empty && name != String.Empty)
                    {
                        imageBox1.Image = trainingImages[Convert.ToInt32(index)];
                    }
                    else {
                        imageBox1.Image = null;
                    }
                    
                    LabelNameNId = name.Split('|');

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(LabelNameNId[0].ToString(), ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Yellow));
                    
                    btnLogInNLogOut.Enabled = true;

                    

                }
                
                
                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces
                        
                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                if (nnn < facesDetected[0].Length - 1)
                {
                    names = names + NamePersons[nnn] + ", ";
                }
                else
                {
                    names = names + NamePersons[nnn];
                }
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            if (imageBox1.Image==null)
                pictureBox2.Visible = true;
            else
                pictureBox2.Visible = false;

            if (names != String.Empty)
                pictureBox1.Visible = true;
            else
                pictureBox1.Visible = false;

            if (names == String.Empty)
            {
                imageBox1.Image = null;
                btnLogInNLogOut.Enabled = false;
            }
            else
            {
                btnLogInNLogOut.Enabled = true;
            }
            label4.Text = names;
            names = "";
            
            //Clear the list(vector) of names
            NamePersons.Clear();


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToString();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        private void btnLogInNLogOut_Click(object sender, EventArgs e)
        {
            if (int.Parse(label3.Text) > 1)
            {
                MessageBox.Show("Number of faces detected is more than one person. Plese try again.");
            }
            else
            {
                if (MyConn.State == ConnectionState.Open) MyConn.Close();
                MyConn.Open();
                string StrCmd = "";
                OleDbCommand Cmd = new OleDbCommand();

                    StrCmd = "select * from InOutTimeTable where EMPID=" + LabelNameNId[1] + " and DateDiff('d',DATETIMEIN,Date())=0";
                    Cmd = new OleDbCommand(StrCmd, MyConn);
                    OleDbDataReader dr = Cmd.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        StrCmd = "insert into InOutTimeTable (EMPID,NAME,DATETIMEIN)values(" + LabelNameNId[1] + ",'" + LabelNameNId[0] + "','" + DateTime.Now + "')";
                        Cmd = new OleDbCommand(StrCmd, MyConn);
                        Cmd.ExecuteNonQuery();
                        MessageBox.Show("Check In already.");
                    }
                    else
                    {
                        StrCmd = "update InOutTimeTable set DATETIMEOUT='" + DateTime.Now + "' where EMPID=" + LabelNameNId[1] + " and DateDiff('d',DATETIMEIN,Date())=0";
                        Cmd = new OleDbCommand(StrCmd, MyConn);
                        Cmd.ExecuteNonQuery();
                        MessageBox.Show("Check Out already.");
                    }
                    gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    //Face Detector
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face,
                    1.2,
                    10,
                    Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                    new Size(20, 20));
                    foreach (MCvAvgComp f in facesDetected[0])
                    {
                        copyimg = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                        break;
                    }
                    copyimg = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                if (!Directory.Exists(@Application.StartupPath + "/Stat/" + LabelNameNId[1].ToString()))
                    {
                      Directory.CreateDirectory(@Application.StartupPath + "/Stat/" + LabelNameNId[1].ToString());
                    }
                
                    copyimg.Save(Application.StartupPath + "/Stat/" + LabelNameNId[1].ToString() + "/" + DateTime.Now.ToString("yyyMMdd_HH_mm_ss") + ".bmp");



            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (frmLogin.admin==false)
            {
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
            }
            else
            {
                frmReport report = new frmReport();
                report.Show();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            frmLogin.admin = false;
            MessageBox.Show("Logout already.");
            btnLogin.Enabled = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin loginForm = new frmLogin();
            loginForm.Show();
            btnLogin.Enabled = false;
        }

        private void btnBroweImg_Click(object sender, EventArgs e)
        {
            if (frmLogin.admin == false)
            {
                frmLogin loginForm = new frmLogin();
                loginForm.Show();
                return;
            }
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter name first.");
                return;
            }
            openFileDialog1.FileName = String.Empty;
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.Title = "Please select an image file to training.";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                grabber = new Capture(openFileDialog1.FileName);
                currentFrame = grabber.QueryFrame().Resize(473, 355, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = currentFrame.Convert<Gray, Byte>();
                //gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = TrainedFace.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(txtName.Text + "|" + (ContTrain).ToString());

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(txtName.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Text = "";
            }
        }





    }
}