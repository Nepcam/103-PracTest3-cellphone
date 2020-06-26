using System;
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

        //Create lists to store the data
        List<string> locationList = new List<string>();
        List<int> eastingList = new List<int>();
        List<int> northingList = new List<int>();
        List<double> powerList = new List<double>();
        
        
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
        /// Calculates the y position of the tower
        /// </summary>
        /// <param name="northing">The northing value of the tower</param>
        /// <returns>The y position of the tower</returns>
        private int CalculateY(int northing)
        {
            double percentUp = (double)(northing - MIN_NORTHING) / (MAX_NORTHING - MIN_NORTHING);
            int y = (int)(pictureBoxMap.Height - (pictureBoxMap.Height * percentUp));
            return y;
        }

        /// <summary>
        /// Counts the towers in the powerList data
        /// </summary>
        /// <param name="maxPower"></param>
        /// <returns></returns>
        private int CountTowers(double maxPower)
        {
            int count = 0;
            //FOR each power value in the lists
            for (int i = 0; i < powerList.Count; i++)
            {
                //Check if the current power value <= the power value passed to the method
                if (powerList[i] <= maxPower)
                {
                    count++;
                }
            }
            return count;
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
            Graphics paper = pictureBoxMap.CreateGraphics();
            StreamReader reader;
            string line = "";
            string[] csvArray;
            string licensee = "";
            string location = "";
            int easting = 0;
            int northing = 0;
            double power = 0;
            int x = 0;
            int y = 0;


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
                            //Extract values from the array into separate variable
                            licensee = csvArray[0];
                            location = csvArray[1];
                            easting = int.Parse(csvArray[2]);
                            northing = int.Parse(csvArray[3]);
                            power = double.Parse(csvArray[4]);

                            //Add the data to the lists
                            locationList.Add(location);
                            eastingList.Add(easting);
                            northingList.Add(northing);
                            powerList.Add(power);

                            //Display the values in the listbox neatly padded out
                            listBoxData.Items.Add(licensee.PadRight(20) + location.PadRight(30) + easting.ToString().PadRight(10) + northing.ToString().PadRight(10) + power.ToString());

                            //Calculate the X position of the tower
                            x = CalculateX(easting);
                            //Calculate the Y position of the tower
                            y = CalculateY(northing);

                            //Draw the tower centred around the x and y position
                            DrawTower(paper, x, y, power, Color.Blue);                            
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
                }
                //Close the file
                reader.Close();
            }
        }

        /// <summary>
        /// Count towers gets a power value from the user and then displays the number of towers which 
        /// have a power rating less than or equal to the given power value in a message window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void countTowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double maxPower = double.Parse(textBoxMaxPower.Text);
            int numTowers = CountTowers(maxPower);
            MessageBox.Show(numTowers.ToString());
        }
    }
}
