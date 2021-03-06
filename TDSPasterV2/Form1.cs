﻿using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace TDSPasterV2
{
    public partial class Form1 : Form
    {
        
        string fileLocation, folderLocation;

        public Form1()
        {
            InitializeComponent();
        }

        private float getTubeCount(string currentLocation)
        {
            // Code that is going to fetch the tube count and other goodies!
            string line;
            int counter = 0;
            float tubeCount;

            StreamReader reader2 = new StreamReader(currentLocation);

            while ((line = reader2.ReadLine()) != null)
            {
                counter++;
            }
            float finalTubeCount = ((counter - 18f) / 3f);
            tubeCount = finalTubeCount;
            Debug.WriteLine("Tube count:" + finalTubeCount);
            reader2.Close();

            return tubeCount;
        }

        private void PopulateDataGridView(string[] dirs)
        {
            int dirSize = dirs.Count();

            for (int i = 0; i < dirSize; i++)
            {
                //Prepping the counters for the excel conversion
                int leftCounter = 1;
                int centerCounter = 2;
                int rightcounter = 3;
                int counter = 0;
                float tubeCount;
                
                string line;
                string currentLocation;

                tubeCount = getTubeCount(currentLocation = dirs[i]);
                tubeCount = (int)tubeCount;
                if (i == 0)
                {
                    for (int b = 1; b <= (tubeCount * 3) + 1; b++)
                    {
                        string tubeColumn = "Tube";
                        dataGridView1.Columns.Add(tubeColumn + b, b.ToString());
                    }
                }

                if (tubeCount * 3 > (dataGridView1.Columns.Count - 1))
                {
                    int difference = (int)(tubeCount*3) - dataGridView1.Columns.Count;
                    for (int b = -1; b <= difference; b++)
                    {
                        int current = dataGridView1.Columns.Count;
                        string tubeColumn = "Tube";
                        dataGridView1.Columns.Add(tubeColumn + (b + current),current.ToString());
                    }
                }

                dataGridView1.Rows.Add();

                StreamReader reader = new StreamReader(currentLocation);

                //variables for trimming process
                char[] charsToTrim = { '"', 'V' };
                while ((line = reader.ReadLine()) != null)
                {
                    counter++;

                    if (counter <= 1 && counter <= 5)
                    {
                        Debug.WriteLine("Left Readings");

                    }
                    //Printing left readings
                    if (counter >= 6 && counter <= (tubeCount + 5))
                    {
                        line = line.Trim(charsToTrim);
                        Debug.WriteLine(line);
                        dataGridView1.Rows[i].Cells[leftCounter].Value = line;
                        //xlWorkSheet.Cells[2, leftCounter] = line;
                        leftCounter = leftCounter + 3;

                    }
                    if (counter == tubeCount + 11)
                    {
                        Debug.WriteLine("Center Readings");
                    }
                    if (counter > (tubeCount + 11) && counter <= ((tubeCount * 2) + 11))
                    {
                        line = line.Trim(charsToTrim);
                        Debug.WriteLine(line);
                        dataGridView1.Rows[i].Cells[centerCounter].Value = line;
                        //xlWorkSheet.Cells[2, centerCounter] = line;
                        centerCounter = centerCounter + 3;
                    }
                    if (counter == (tubeCount * 2) + 17)
                    {
                        Debug.WriteLine("Right Readings");
                    }
                    if (counter > ((tubeCount * 2) + 17) && counter <= ((tubeCount * 3) + 17))
                    {
                        line = line.Trim(charsToTrim);
                        Debug.WriteLine(line);
                        dataGridView1.Rows[i].Cells[rightcounter].Value = line;
                        //xlWorkSheet.Cells[2, rightcounter] = line;
                        rightcounter = rightcounter + 3;
                    }
                }
            }
            return;
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            
            DialogResult userClickedOk = folderBrowser.ShowDialog();

            if (userClickedOk == DialogResult.OK)
            {
                //get the location of the folder that the user selected
                folderLocation = folderBrowser.SelectedPath;
                //Getting an array of all of the file paths for the files in the directory with the selected file
                string[] dirs = Directory.GetFiles(folderLocation);
                //dirs.ToList().ForEach(Console.WriteLine);

                PopulateDataGridView(dirs);
            }
        }
    }
}
