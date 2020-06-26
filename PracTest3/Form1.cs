﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PracTest2
{
    public partial class Form1 : Form
    {
        //Name: 
        //ID:

        //The smallest Easting value on the NZMG260 S14 (Hamilton) map
        const int MIN_EASTING = 2690000;
        //The largest Easting value on the NZMG260 S14 (Hamilton) map
        const int MAX_EASTING = 2730000;
        //The smallest Northing value on the NZMG260 S14 (Hamilton) map
        const int MIN_NORTHING = 6370000;
        //The largest Northing value on the NZMG260 S14 (Hamilton) map
        const int MAX_NORTHING = 6400000;

        //Filter for CSV files
        const string FILTER = "CSV Files|*.csv|ALL Files|'.'";
        
        
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Draws a cell tower centered at the given x and y position
        /// in the colour specified.
        /// </summary>
        /// <param name="paper">Where to draw the tower</param>
        /// <param name="x">X position of the centre of the tower</param>
        /// <param name="y">Y position of the centre of the tower</param>
        /// <param name="power">The range of the tower, i.e. the radius of the circle</param>
        /// <param name="towerColour">Colour to draw the tower in</param>
        private void DrawTower(Graphics paper, int x, int y, double power, Color towerColour)
        {
            //The size of a side of the rectangle to represent a tower
            const int TOWER_SIZE = 6;
            //Brush and pen to draw the tower in the given colour
            SolidBrush br = new SolidBrush(towerColour);
            Pen pen1 = new Pen(towerColour, 2);
            //Caluclate the radius of the circle to represent the power as an integer
            int radius = (int) power;

            //Draw the tower centered around the given x and y point
            paper.FillRectangle(br, x - TOWER_SIZE / 2, y - TOWER_SIZE / 2, TOWER_SIZE, TOWER_SIZE);
            //Draw the circle to represent the range cenetred around the given x and y point
            paper.DrawEllipse(pen1, x - radius, y - radius, radius * 2, radius * 2);
        }

        /// <summary>
        /// This method will calculate the correct x coordinate value of the cell tower
        /// based on the given easting value.
        /// </summary>
        /// <param name="easting">The easting value of the cell tower</param>
        /// <returns>The x coordinate of the cell tower in the picturebox.</returns>
        private int CalculateX(int easting)
        {
            //calculate x position of easting value, must cast one of the values to a double
            //otherwise will perform integer division
            double ratio = (double) (easting - MIN_EASTING) / (MAX_EASTING - MIN_EASTING);
            int x = (int)(ratio * pictureBoxMap.Width);
            return x;
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Opens a CSV file of tower information and displays the information in the listbox and draws the towers in the picturebox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader reader;
            string line = "";
            string[] csvArray;
            string licensee = "";
            string location = "";
            int easting = 0;
            int northing = 0;
            double power = 0;

            //Set the filter for the dialog control
            openFileDialog1.Filter = FILTER;
            //Check if the user has selected the file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the selected file
                reader = File.OpenText(openFileDialog1.FileName);

                //WHILE not end of file
                while(!reader.EndOfStream)
                {
                    try
                    {
                        //Read an entire csv line from the file
                        line = reader.ReadLine();
                        //Split the values in the line using the array
                        csvArray = line.Split(',');
                        //Check if the array has the correct number of elements
                        if (csvArray.Length == 5)
                        {

                        }
                        else
                        {
                            Console.WriteLine("Error: " + line);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error: " + line);
                }
                //Close the file
                reader.Close();
            }
        }
    }
}
