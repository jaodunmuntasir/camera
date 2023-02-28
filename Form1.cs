using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AForge.Imaging;
using AForge.Imaging.Filters;
//using AForge.Imaging.Matching;
using static AForge.Imaging.Image;
using System.Text.RegularExpressions;

namespace HackathonTest2
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap capturedImage;

        public Form1()
        {
            InitializeComponent();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in videoDevices)
            {
                comboBox1.Items.Add(device.Name);
            }
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
                capturedImage = null;
                videobox.Image = null;
                pictureBox.Image = null;
            }
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            int deviceIndex = comboBox1.SelectedIndex;
            if (deviceIndex >= 0 && deviceIndex < videoDevices.Count)
            {
                videoSource = new VideoCaptureDevice(videoDevices[deviceIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
        }

        private void capturebutton_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                // Loading reference images and converting to grayscale
                /*Bitmap refImage1 = (Bitmap)AForge.Imaging.Image.FromFile("E:\\Test\\MD Jaodun Muntasir.jpg").Clone();
                Bitmap refImage2 = (Bitmap)AForge.Imaging.Image.FromFile("E:\\Test\\MD Jaodun Muntasir_Picture.jpg").Clone();*/
                Bitmap capturedImage = (Bitmap)videobox.Image.Clone();

                /*// Resizing the image
                int width = 100; // setting the desired width of the image
                int height1 = (int)(refImage1.Height * ((float)width / refImage1.Width)); // calculating the proportional height based on the desired width
                refImage1 = new Bitmap(refImage1, new Size(width, height1));
                int height2 = (int)(refImage2.Height * ((float)width / refImage2.Width)); // calculating the proportional height based on the desired width
                refImage2 = new Bitmap(refImage2, new Size(width, height2));

                // Applying color filter to remove background/noise
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.White);
                filter.Radius = 100;
                filter.ApplyInPlace(refImage1);
                filter.ApplyInPlace(refImage2);
                filter.ApplyInPlace(capturedImage);

                // Converting captured image to grayscale
                Grayscale filter3 = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap grayImage = filter3.Apply(capturedImage);
                int height3 = (int)(grayImage.Height * ((float)width / grayImage.Width)); // calculating the proportional height based on the desired width
                grayImage = new Bitmap(grayImage, new Size(width, height3));

                // Comparing captured image to reference images
                ExhaustiveTemplateMatching matcher = new ExhaustiveTemplateMatching(0);
                TemplateMatch[] matches1 = matcher.ProcessImage(refImage1, grayImage);
                TemplateMatch[] matches2 = matcher.ProcessImage(refImage2, grayImage);

                // Finding the best match for each reference image
                TemplateMatch bestMatch1 = null;
                TemplateMatch bestMatch2 = null;
                double maxScore = capturedImage.Width * capturedImage.Height;
                foreach (TemplateMatch match in matches1)
                {
                    if (bestMatch1 == null || match.Similarity > bestMatch1.Similarity)
                    {
                        bestMatch1 = match;
                    }
                }
                foreach (TemplateMatch match in matches2)
                {
                    if (bestMatch2 == null || match.Similarity > bestMatch2.Similarity)
                    {
                        bestMatch2 = match;
                    }
                }

                // Calculating match percentages
                double matchPercent1 = bestMatch1.Similarity * 100 / maxScore;
                double matchPercent2 = bestMatch2.Similarity * 100 / maxScore;

                // Displaying results
                videobox.Image = refImage1;
                pictureBox.Image = refImage2;*/
                //matchbox.Image = capturedImage;
                pictureBox.Image = capturedImage;
               /* label1.Text = $"Match 1: {matchPercent1:0.00}%";
                label2.Text = $"Match 2: {matchPercent2:0.00}%";*/
            }
        }

        private void savebutton_Click(object sender, EventArgs e)
        {
            if (capturedImage != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    capturedImage.Save(saveFileDialog.FileName);
                }
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            videobox.Image = bitmap;
        }

    }
}