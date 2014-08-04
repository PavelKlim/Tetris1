using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace Tetris1
{
    public partial class Form1 : Form
    {
        bool started = false;
        Model model;
        View view;
        Controller controller;
        Figure currentFigure;
        Figure nextFigure;

        bool grid;
        bool pause;
        bool playerControl;
        
        int collisionCounter;
        int Score = 50;

        public Form1(Model model, View view)
        {
            InitializeComponent();
            this.Controls.Add(pictureBox1);
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(figureControl);
            //this.KeyPress += new KeyPressEventHandler(figureControl);
            //this.model = model;
            this.grid = checkBox1.Checked;
            controller = new Controller();
            controller.timer.Elapsed += new ElapsedEventHandler(timerEvent);
            collisionCounter = 0;

            this.model = model;
            this.view = view;
            //this.controller = controller;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                started = !started;
                startBut.Text = "Continue";
                startBut.Enabled = !startBut.Enabled; 
            }
            else
            {
                pauseBut.Enabled = !pauseBut.Enabled;
                startBut.Enabled = !startBut.Enabled;
                controller.timer.Start();
            }
            gameStart();
            controller.timer.Enabled = true;
            pause = false;
            playerControl = true;
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            pauseBut.Enabled = !pauseBut.Enabled;
            startBut.Enabled = !startBut.Enabled;
            controller.timer.Stop();
            pause = true;
            playerControl = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currentFigure=gameStart();
            started = true;
            startBut.Enabled = false;
            pauseBut.Enabled = true;
            playerControl = true;
            pause = false;
            controller.timer.Start();
            controller.timer.Interval = 500;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

//---------------------------------------------------------------------------------
        public Figure gameStart()
        {
            model = new Model(20, 18);
            currentFigure = new Figure(model);
            nextFigure = new Figure(model);
            pauseBut.Visible = true;
            restartBut.Visible = true;
            view.background = Color.FromName(comboBox1.SelectedItem.ToString());
            textBox1.Text = currentFigure.name.ToString();
            //view.modelDraw(model,figure, pictureBox1, bl);
            //this.Invalidate();
            return currentFigure;
        }
        
//--------------------------------------------------------------------------------- 
        protected override void  OnPaint(PaintEventArgs e)
        {
            if (model != null && view != null && currentFigure != null)
            {
                base.OnPaint(e);
                int _x = 0;
                int _y = 0;
                SolidBrush br1 = new SolidBrush(view.background);
                Pen pen1 = new Pen(Color.Black);
                SolidBrush br2 = new SolidBrush(Color.Black);
                Pen pen2 = new Pen(Color.Green);
                Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics myGraph = Graphics.FromImage(bitmap);

                myGraph.FillRectangle(br1, new Rectangle(0, 0, view.screenWidth, view.screenHeight));
                pictureBox1.Image = bitmap;

                for (int y = 0; y < model.cells.GetLength(0); y++)
                {
                    _x = 0;
                    for (int x = 0; x < model.cells.GetLength(1); x++)
                    {
                        if (model.cells[y, x] || currentFigure.isFill(x, y))
                        {
                            myGraph.FillRectangle(br2, new Rectangle(_x, _y, view.cellSize, view.cellSize));
                            myGraph.DrawRectangle(pen2, new Rectangle(_x, _y, view.cellSize, view.cellSize));
                        }
                        else
                        {
                            if (grid)
                            {
                                myGraph.DrawRectangle(pen1, new Rectangle(_x, _y, view.cellSize, view.cellSize));
                                pictureBox1.Image = bitmap;
                            }
                            else
                            {
                                myGraph.DrawRectangle(pen1, new Rectangle(0, 0, view.screenWidth - 1, view.screenHeight - 1));
                            }
                        }
                        _x += view.cellSize;
                    }
                    _y += view.cellSize;
                }
                pictureBox1.Image = bitmap;

                this.nextFigurePreview(nextFigure);
            }
        }
        
//---------------------------------------------------------------------------------
        private void timerEvent(object source, ElapsedEventArgs e)
        {
            if (currentFigure.MoveDown(model))
            {
                this.Invalidate();
                //view.modelDraw(model, figure, pictureBox1, grid);
            }    
            else
                if (collisionCounter > 0)
                {
                    model.Update(currentFigure);
                    controller.timer.Interval = 500;
                    currentFigure = nextFigure;
                    nextFigure = new Figure(model);                 
                    Score += 50;
                    collisionCounter = 0;
                }
                else
                    if (currentFigure.isCollisionBottom(model))
                        collisionCounter++;
            if (model.isEndOfGame())
            {
                pause = true;
                controller.timer.Stop();
                MessageBox.Show("Конец игры! \nВаш счёт: " + Score);
            }
            if (model.fullLinesClear())
                Score += 200;
        }
//---------------------------------------------------------------------------------  
        void figureControl(object sender, KeyEventArgs e)
        {
            if(!pause && playerControl)
                switch (e.KeyCode)
                {
                    case Keys.A:
                        currentFigure.MoveLeft(model);
                        break;
                    case Keys.D:
                        currentFigure.MoveRight(model);
                        break;
                    case Keys.S:
                        if( !currentFigure.isCollisionBottom(model) )
                            controller.timer.Interval = 50;
                        break;
                    case Keys.E:
                        currentFigure.rotateRight(model);
                        break;
                    case Keys.Q:
                        currentFigure.rotateLeft();
                        break;
                }
            //view.modelDraw(model, figure, pictureBox1, grid);
            this.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            grid = checkBox1.Checked;
        }

        public void nextFigurePreview(Figure figure)
        {
            int _x = 0;
            int _y = 0;
            SolidBrush br1 = new SolidBrush(view.background);
            Pen pen1 = new Pen(Color.Black);
            SolidBrush br2 = new SolidBrush(Color.Black);
            Pen pen2 = new Pen(Color.Green);
            Bitmap bitmap = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics myGraph = Graphics.FromImage(bitmap);

            for (int y = 0; y < figure.form.GetLength(0); y++)
            {
                _x = 0;
                for (int x = 0; x < figure.form.GetLength(1); x++)
                {
                    if (figure.form[y,x])
                    {
                        myGraph.FillRectangle(br2, new Rectangle(_x, _y, view.cellSize * 2, view.cellSize * 2));
                        myGraph.DrawRectangle(pen2, new Rectangle(_x, _y, view.cellSize * 2, view.cellSize * 2));
                    }
                    _x += view.cellSize * 2;
                }
                _y += view.cellSize * 2;
            }
            pictureBox2.Image = bitmap;
        }
    }
}
