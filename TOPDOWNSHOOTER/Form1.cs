using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOPDOWNSHOOTER
{
    public partial class Form1 : Form
    {
        // start the variables
        bool goup; // this boolean will be used for the player to go up the screen
        bool godown; // this boolean will be used for the player to go down the screen
        bool goleft; // this boolean will be used for the player to go left to the screen
        bool goright; // this boolean will be used for the player to go right to the screen
        string facing = "up"; // this string is called facing and it will be used to guide the bullets
        double playerHealth = 100; // this double variable is called player health
        int speed = 10; // this integer is for the speed of the player
        int ammo = 10; // this integer will hold the number of ammo the player has at the start of the game
        int zombieSpeed = 3; // this integer will hold the speed at which the zombies move in the game
        int score = 0; // this integer will hold the score the player achieved throughout the game
        bool gameOver = false; // this boolean is false in the beginning and it will be used when the game is finished
        Random rnd = new Random(); // this is an instance of the random class we will use this to create a random number for this game
        // end of listing variables

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Enable double buffering to reduce flicker
            this.Paint += new PaintEventHandler(this.OnPaint);

            this.back.Image = Properties.Resources.zup;
            this.front.Image = Properties.Resources.up;

            Helper.BlendPictures(this.back, this.front);

        }
        
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (gameOver) return; // if game over is true then do nothing in this event

            switch (e.KeyCode)
            {
                case Keys.Left:
                    goleft = true;
                    break;
                case Keys.Right:
                    goright = true;
                    break;
                case Keys.Up:
                    goup = true;
                    break;
                case Keys.Down:
                    godown = true;
                    break;
            }

            UpdatePlayerImage();
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (gameOver) return; // if game is over then do nothing in this event

            switch (e.KeyCode)
            {
                case Keys.Left:
                    goleft = false;
                    break;
                case Keys.Right:
                    goright = false;
                    break;
                case Keys.Up:
                    goup = false;
                    break;
                case Keys.Down:
                    godown = false;
                    break;
                case Keys.Space when ammo > 0:
                    ammo--; // reduce ammo by 1 from the total number
                    shoot(facing); // invoke the shoot function with the facing string inside it
                    if (ammo < 1) // if ammo is less than 1
                    {
                        DropAmmo(); // invoke the drop ammo function
                    }
                    break;
            }

            UpdatePlayerImage();
        }

        private void UpdatePlayerImage()
        {
            if (goleft && goup)
            {
                front.Image = Properties.Resources.left; // use left image for up-left direction
                facing = "left";
            }
            else if (goleft && godown)
            {
                front.Image = Properties.Resources.left; // use left image for down-left direction
                facing = "left";
            }
            else if (goright && goup)
            {
                front.Image = Properties.Resources.right; // use right image for up-right direction
                facing = "right";
            }
            else if (goright && godown)
            {
                front.Image = Properties.Resources.right; // use right image for down-right direction
                facing = "right";
            }
            else if (goleft)
            {
                front.Image = Properties.Resources.left;
                facing = "left";
            }
            else if (goright)
            {
                front.Image = Properties.Resources.right;
                facing = "right";
            }
            else if (goup)
            {
                front.Image = Properties.Resources.up;
                facing = "up";
            }
            else if (godown)
            {
                front.Image = Properties.Resources.down;
                facing = "down";
            }
        }


         private void gameEngine(object sender, EventArgs e)
        {
            if (playerHealth > 1) // if player health is greater than 1
            {
                progressBar1.Value = Convert.ToInt32(playerHealth); // assign the progress bar to the player health integer
            }
            else
            {
                // if the player health is below 1
                front.Image = Properties.Resources.dead; // show the player dead image
                timer1.Stop(); // stop the timer
                gameOver = true; // change game over to true
            }
            label1.Text = "   Ammo:  " + ammo; // show the ammo amount on label 1
            label2.Text = "Kills: " + score; // show the total kills on the score
            // if the player health is less than 20
            if (playerHealth < 20)
            {
                progressBar1.ForeColor = System.Drawing.Color.Red; // change the progress bar colour to red. 
            }
            if (goleft && front.Left > 0)
            {
                front.Left -= speed;
                // if moving left is true AND player is 1 pixel more from the left 
                // then move the player to the LEFT
            }
            if (goright && front.Left + front.Width < 930)
            {
                front.Left += speed;
                // if moving RIGHT is true AND player left + player width is less than 930 pixels
                // then move the player to the RIGHT
            }
            if (goup && front.Top > 60)
            {
                front.Top -= speed;
                // if moving TOP is true AND player is 60 pixel more from the top 
                // then move the player to the UP
            }
            if (godown && front.Top + front.Height < 700)
            {
                front.Top += speed;
                // if moving DOWN is true AND player top + player height is less than 700 pixels
                // then move the player to the DOWN
            }
            // run the first for each loop below
            // X is a control and we will search for all controls in this loop
            foreach (Control x in this.Controls)
            {
                // if the X is a picture box and X has a tag AMMO
                if (x is PictureBox && x.Tag == "ammo")
                {
                    // check is X in hitting the player picture box
                    if (((PictureBox)x).Bounds.IntersectsWith(front.Bounds))
                    {
                        // once the player picks up the ammo
                        this.Controls.Remove(((PictureBox)x)); // remove the ammo picture box
                        ((PictureBox)x).Dispose(); // dispose the picture box completely from the program
                        ammo += 5; // add 5 ammo to the integer
                    }
                }
                // if the bullets hits the 4 borders of the game
                // if x is a picture box and x has the tag of bullet
                if (x is PictureBox && x.Tag == "bullet")
                {
                    // if the bullet is less the 1 pixel to the left
                    // if the bullet is more then 930 pixels to the right
                    // if the bullet is 10 pixels from the top
                    // if the bullet is 700 pixels to the buttom
                    if (((PictureBox)x).Left < 1 || ((PictureBox)x).Left > 930 || ((PictureBox)x).Top < 10 || ((PictureBox)x).Top > 700)
                    {
                        this.Controls.Remove(((PictureBox)x)); // remove the bullet from the display
                        ((PictureBox)x).Dispose(); // dispose the bullet from the program
                    }
                }
                // below is the if statement which will be checking if the player hits a zombie
                if (x is PictureBox && x.Tag == "zombie")
                {
                    // below is the if statement thats checking the bounds of the player and the zombie
                    if (((PictureBox)x).Bounds.IntersectsWith(front.Bounds))
                    {
                        playerHealth -= 1; // if the zombie hits the player then we decrease the health by 1
                    }
                    // move zombie towards the player picture box
                    if (((PictureBox)x).Left > front.Left)
                    {
                        ((PictureBox)x).Left -= zombieSpeed; // move zombie towards the left of the player
                        ((PictureBox)x).Image = Properties.Resources.zleft; // change the zombie image to the left
                    }
                    if (((PictureBox)x).Top > front.Top)
                    {
                        ((PictureBox)x).Top -= zombieSpeed; // move zombie towards the top of the player
                        ((PictureBox)x).Image = Properties.Resources.zup; // change the zombie image to up
                    }
                    if (((PictureBox)x).Left < front.Left)
                    {
                        ((PictureBox)x).Left += zombieSpeed; // move zombie towards the right of the player
                        ((PictureBox)x).Image = Properties.Resources.zright; // change the zombie image to the right
                    }
                    if (((PictureBox)x).Top < front.Top)
                    {
                        ((PictureBox)x).Top += zombieSpeed; // move zombie towards the bottom of the player
                        ((PictureBox)x).Image = Properties.Resources.zdown; // change the zombie image to down
                    }
                }
                // below is the second for each loop that will check the bullets and the zombies
                foreach (Control j in this.Controls)
                {
                    if ((j is PictureBox && j.Tag == "bullet") && (x is PictureBox && x.Tag == "zombie"))
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++; // increase the score by 1
                            this.Controls.Remove(j); // remove the bullet
                            j.Dispose(); // dispose the bullet
                            this.Controls.Remove(x); // remove the zombie
                            x.Dispose(); // dispose the zombie
                            MakeZombies(); // call the make zombies function to create another zombie to keep the game going
                        }
                    }
                }
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Custom painting logic here if needed
        }

        private void shoot(string direct)
        {
            bullet shoot = new bullet(); // create a new instance of the bullet class
            shoot.direction = direct; // assignment the direction to the bullet
            shoot.bulletLeft = front.Left + (front.Width / 2); // place the bullet to left half of the player
            shoot.bulletTop = front.Top + (front.Height / 2); // place the bullet on top half of the player
            shoot.mkBullet(this); // invoke the function from the bullet class
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox(); // create a picture box for the zombie
            zombie.Tag = "zombie"; // add a tag to it called zombie
            zombie.Image = Properties.Resources.zdown; // the default picture will be zombie down
            zombie.Left = rnd.Next(0, 900); // generate a number between 0 and 900 and place the zombie on the left
            zombie.Top = rnd.Next(0, 800); // generate a number between 0 and 800 and place the zombie at the top
            zombie.SizeMode = PictureBoxSizeMode.AutoSize; // set the picture size mode to auto size
            this.Controls.Add(zombie); // add the zombie to the screen
            front.BringToFront(); // bring the player to front
        }

        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox(); // create a new instance of the picture box
            ammo.Image = Properties.Resources.loot_Image; // assignment the ammo image to the picture box
            ammo.SizeMode = PictureBoxSizeMode.AutoSize; // set the size mode to auto size
            ammo.Left = rnd.Next(10, this.ClientSize.Width - ammo.Width); // generate a random left position
            ammo.Top = rnd.Next(60, this.ClientSize.Height - ammo.Height); // generate a random top position
            ammo.Tag = "ammo"; // set the tag to ammo
            this.Controls.Add(ammo); // add the ammo to the controls
            ammo.BringToFront(); // bring the ammo to front
            front.BringToFront(); // bring the player to front
        }
    }
}

