/*
* Build and design by BloddyRose
* This software is free to use
* Version 2.0
*/
#region Main
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Converter
{
    public partial class Converter : Form
    {
        public Converter()
        {
            InitializeComponent();

        }
        #region Convert Function
        private void convertButton_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Interval = openFileDialog1.SafeFileName.Length;
                timer1.Start();
                string file_path = openFileDialog1.FileName;
                string name = Path.GetFileNameWithoutExtension(file_path);
                string command = string.Format("/C ffmpeg.exe -i {0} {1}.mp3", file_path, name);
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = command
                };
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Eror while converting file " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        #region Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            done.Increment(1);
            if (done.Value == done.Maximum)
            {
                timer1.Stop();
                string file_path = fileInput.Text;
                string name = Path.GetFileNameWithoutExtension(file_path) + ".mp3";
                MessageBox.Show("Done converting " + file_path + "\nNew File created " + name, "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                done.Value = done.Minimum;
                label2.Show();
            }
        }
        #endregion
        #region Dispose Resources
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Process processs in Process.GetProcessesByName("ffmpeg.exe"))
            {
                processs.Kill();
            }
            foreach (Process processs in Process.GetProcessesByName("Converter.exe"))
            {
                processs.Kill();

            }
            Environment.Exit(0);
            Application.Exit();
            Dispose();
        }
        #endregion
        #region Open File Dialog

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                string path = openFileDialog1.FileName;
                fileInput.Text = path;

                if (DialogResult == DialogResult.Cancel)
                {
                    fileInput.Text = "Select a file ...";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Eror while opening file (File must be an mp4 file!!)" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion
        #region Open Git Page
        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Opening Browser to Converter Github Repository...", "Help", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Process.Start("https://github.com/BloddyRose/Converter#hint");
            }
        }
        #endregion
        #region Open Folder
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string files_folder = Directory.CreateDirectory("files").FullName;

                string folder = Environment.CurrentDirectory;


                string[] items = System.IO.Directory.GetFiles(folder, "*.mp3", System.IO.SearchOption.TopDirectoryOnly);
                foreach (string filePath in items)
                {
                    string newFile = System.IO.Path.Combine(files_folder, System.IO.Path.GetFileName(filePath));
                    if (File.Exists(newFile))
                    {
                        MessageBox.Show("File already exists!!\n Overwriting!", "Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // File.Delete(file);
                        File.Delete(filePath);
                        continue;
                    }

                    File.Move(filePath, newFile);

                }

                string path = Directory.GetCurrentDirectory();
                string folder1 = "files";
                string full_path = Path.Combine(path, folder1);

                Process.Start("explorer.exe", full_path);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Eror while moving files " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    #endregion
    }
}
#endregion
