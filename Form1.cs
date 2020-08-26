﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            timer1.Start();
            string file_path = fileInput.Text;
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

        private void debugarea_TextChanged(object sender, EventArgs e)
        {

        }

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
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Process processs in Process.GetProcessesByName("ffmpeg.exe"))
            {
                processs.Kill();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
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
                    continue;
                }

                File.Move(filePath, newFile);
            }
        }
    }
}