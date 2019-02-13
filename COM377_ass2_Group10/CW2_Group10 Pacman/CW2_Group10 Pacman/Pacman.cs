using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using CellClass;
using System.IO;
using CW2_Group10_Pacman.Properties;


namespace CW2_Group10_Pacman
{
    public partial class Pacman : Form
    {
        private const int CellSize = 20;
        private Cell[,] map;

        private Image[] pacmanImage = new Image[4];
        private int xPosition = 530;
        private int yPosition = 450;
        private int currentMouthPosition = 0;

        private Image[] ghostImage = new Image[2];
        private int oPosition = 265;
        private int pPosition = 280;
        private int kPosition = 300;
        private int lPosition = 280;
        private int currentEyePosition = 0;
        

        private Random rnd = new Random();

        public int score, lives = 3;

        private SoundPlayer startTune = new SoundPlayer();
        private SoundPlayer dyingTune = new SoundPlayer();
        private SoundPlayer wakaTune = new SoundPlayer();

        public Pacman()
        {
            InitializeComponent();     
        }


        ///read a map file
        private Cell[,] CreateArray(string mapFile)
        {
            int numRows = 0;
            int numColumns = 0;

            string line = null;

            try
            {
                StreamReader mapData = new StreamReader(mapFile);
                line = mapData.ReadLine();
                numColumns = line.Length;

                while (line != null)
                {
                    numRows++;
                    line = mapData.ReadLine();
                }

                mapData.Close();

            }
            catch (System.IO.IOException e)
            {

                MessageBox.Show(e.Message);
            }

            Cell[,] map = new Cell[numColumns, numRows];

            try
            {
                StreamReader mapData = new StreamReader(mapFile);
                line = mapData.ReadLine();

                int rowIndex = 0;

                while (line != null)
                {
                    for (int columnIndex = 0; columnIndex < line.Length; columnIndex++)
                    {
                        map[columnIndex, rowIndex] = new Cell(columnIndex, rowIndex, line[columnIndex]);

                    }

                    rowIndex++;

                    line = mapData.ReadLine();
                }

                mapData.Close();
            }
            catch (System.IO.IOException e)
            {

                MessageBox.Show(e.Message);
            }

            return map;
        }

        private void Pacman_Load(object sender, EventArgs e)
        {
            //load map
            map = CreateArray("..\\Resources\\level1.txt");

            mazePictureBox.Width = map.GetLength(0) * CellSize;
            mazePictureBox.Height = map.GetLength(1) * CellSize;

            //load images
            pacmanImage[0] = Image.FromFile("..\\images\\pac32_left_close.png");
            pacmanImage[1] = Image.FromFile("..\\images\\pac32_left_open.png");
            pacmanImage[2] = Image.FromFile("..\\images\\pac32_left_wide.png");
            pacmanImage[3] = Image.FromFile("..\\images\\pac32_left_widest.png");
            //load ghosts
            ghostImage [0] = Image.FromFile("..\\images\\redGhost-left-32.jpg");
            ghostImage[1] = Image.FromFile("..\\images\\redGhost-left-32.jpg");


            //load start sound
            startTune.SoundLocation = "..\\Resources\\startTune.wav";
            startTune.Load();
            startTune.Play();

            //load dying sound
            dyingTune.SoundLocation = "..\\Resources\\dyingTune.wav";
            dyingTune.Load();

            //load waka movement sound
            wakaTune.SoundLocation = "..\\Resources\\wakaTune.wav";
            wakaTune.Load();

        }

        private void mazePictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Graphics gr = e.Graphics;

            for (int row = 0; row < map.GetLength(0); row++)
                for (int column = 0; column < map.GetLength(1); column++)
                {
                    map[row, column].DrawBackground(g);
                }

            if (currentMouthPosition >= 3) currentMouthPosition = 0;
            else currentMouthPosition++;

            gr.DrawImage(pacmanImage[currentMouthPosition], xPosition, yPosition, 32, 32);
            gr.DrawImage(ghostImage[currentEyePosition], oPosition, pPosition, 32, 32);
            gr.DrawImage(ghostImage[currentEyePosition], kPosition, lPosition, 32, 32);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            kPosition = rnd.Next(16, mazePictureBox.Width - 16);
            lPosition = rnd.Next(16, mazePictureBox.Height - 16);

            CheckCollision();

            if (currentEyePosition == 0) currentEyePosition = 1;
            else currentEyePosition = 0;

            currentMouthPosition += 1;
            if (currentMouthPosition > 3) currentMouthPosition = 0;
            

            mazePictureBox.Invalidate();
        }
        //pacman ghost collision
        private void CheckCollision()
        {
            Rectangle pacmanRec = new Rectangle(this.mazePictureBox.Width / 2, this.mazePictureBox.Height / 2, 32, 32);
            Rectangle ghostRec = new Rectangle(oPosition, yPosition, 32, 32);
            Rectangle ghostRec_2 = new Rectangle(kPosition, lPosition, 32, 32);

            if (pacmanRec.IntersectsWith(ghostRec))
            {
                dyingTune.Play();
                timer.Stop();
                MessageBox.Show(new Form() { TopMost = true }, "Collision!", "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                   
                lives = lives - 1;
                MessageBox.Show(new Form() { TopMost = true }, "Remaining Lives = " + lives, "Lost a Life!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                if (lives == 0)
                {
                    MessageBox.Show(new Form() { TopMost = true }, "You have " + lives + " lives left" + Environment.NewLine + "Game Over!", 
                        "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    
                    this.Close();
                }

            }
            if (pacmanRec.IntersectsWith(ghostRec_2))
            {
                dyingTune.Play();
                timer.Stop();
                Console.Out.WriteLine("collision!");
                MessageBox.Show(new Form() { TopMost = true }, "Collision!", "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);


                lives = lives - 1;
                MessageBox.Show(new Form() { TopMost = true }, "Remaining Lives = " + lives, "Lost a Life!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                if (lives == 0)
                {
                    MessageBox.Show(new Form() { TopMost = true }, "You have " + lives + " lives left!" + Environment.NewLine + "Game Over!",
                        "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                    this.Close();
                }
            }
        }
        private void Pacman_KeyDown(object sender, KeyEventArgs e)
        {
            wakaTune.Play();



           if (e.KeyCode== Keys.Down)
            {
                pPosition= yPosition-40;
            }
           if (e.KeyCode == Keys.Up)
           {
               pPosition = yPosition - 40;
           }
           else if (e.KeyCode == Keys.Left)
           {
               oPosition = xPosition - 40;
           }
           else if (e.KeyCode == Keys.Right)
           {
               oPosition = xPosition - 40;
           }
            if (e.KeyCode == Keys.Down)
            {
                yPosition += 10;

            }
            else if (e.KeyCode == Keys.Up)
            {
                yPosition -= 10;
            }
            else if (e.KeyCode == Keys.Left)
            {
                xPosition -= 10;
            }
            else if (e.KeyCode == Keys.Right)
            {
                xPosition += 10;
            }
            
            mazePictureBox.Invalidate();
          
            if (xPosition < 0) xPosition = this.Width;
            else if (xPosition > this.Width) xPosition = 0;

            if (yPosition < 0) yPosition = this.Height;
            else if (yPosition > this.Height) yPosition = 0;

            mazePictureBox.Invalidate();

            if (e.KeyCode == Keys.P)
            {
                timer.Stop();
                wakaTune.Stop();
                MessageBox.Show(new Form() { TopMost = true }, "PAUSED");

            }
            if (e.KeyCode == Keys.R)
            {
                timer.Start();
                wakaTune.Play();
            }

            mazePictureBox.Invalidate();
        }

        private void livesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(new Form() { TopMost = true }, "Remaining Lives = " + lives, "Lives", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Group ID: CW2-10" + Environment.NewLine + Environment.NewLine + "Group Members: " + Environment.NewLine + "Jonathan McCrink (B00656761)"
                + Environment.NewLine + "Gary McShane (B00656764)" + Environment.NewLine + "Aine McCaul (B00686604)", "COM377 - CW2");
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            wakaTune.Stop();
            MessageBox.Show(new Form() { TopMost = true }, "Game has Paused");
        }

        private void gameRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRules rulesForm = new frmRules();
            rulesForm.Show();
        }


        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}
