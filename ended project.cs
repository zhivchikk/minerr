using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minerr
{
    public partial class Form1 : Form
    {
        int width = 10;
        int height = 10;
        int offset = 30;
        int distanceBetweenButtons = 35;
        int bombPercent = 10;
        bool isFirstClick = true;
        
        FieldButton[,] field;
        int cellsOpened = 0;
        int bombs = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void FieldSize()
        {

        }
        private void X20ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.Width = 700;
            this.Height = 712;
            width = 20;
            height = 20;
            field = new FieldButton[width, height];

            GenerateField();

        }
        private void X10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            this.Width = 391;
            this.Height = 409;
            width = 10;
            height = 10;
            field = new FieldButton[width, height];
            
            GenerateField();
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 237;
            this.Height = 246;
            width = 5;
            height = 5;
            field = new FieldButton[width, height];
            
            GenerateField();
        }
        private void EASYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bombPercent = 10;
        }

        private void MEDIUMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bombPercent = 20;
        }

        private void HARDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bombPercent = 30;
        }

        public void GenerateField() ///tworzy pole
        {
            Random random = new Random();
            for (int y = 0; y < height; y++) ///tworzy pole przyciskow(button) (kolumny)
            {
                for (int x = 0; x < width; x++) ///tworzy pole przyciskow(button) (wiersz)
                {
                    FieldButton newButton = new FieldButton();/// tworzy obiekt
                    newButton.Location = new Point(x * offset+40, y * offset+40); ///ustawienie pozycje przyciskow
                    newButton.Size = new Size(offset, offset);///ustawienie rozmiaru przuciskow
                    newButton.isClickable = true; ///przycisk jest klikalny
                    if (random.Next(0, 100) <= bombPercent) ///tworzy bomby
                    {
                        newButton.isBomb = true;
                        bombs++;
                    }
                    newButton.xCoord = x;
                    newButton.yCoord = y;
                    Controls.Add(newButton); /// dodawanie przuciskow
                    newButton.MouseUp += new MouseEventHandler(FieldButtonClick);   ///zapisuje na kazde pole zdarzenie, które wywoła metodę FieldButtonClick
                    field[x, y] = newButton;
                }
            }
        }

        void FieldButtonClick(object sender, MouseEventArgs e) /// funkcja, która zostanie wywołana po naciśnięciu przycisku
        {
            FieldButton clickedButton = (FieldButton)sender; /// zmieniamy object na FeildButton   

            if (e.Button == MouseButtons.Left && clickedButton.isClickable)  
            {
                if (clickedButton.isBomb)
                {
                    if (isFirstClick) /// robi to, aby nie wybuchnac od razu
                    {
                        clickedButton.isBomb = false;
                        bombs--;
                        OpenRegion(clickedButton.xCoord, clickedButton.yCoord, clickedButton); ///otwarcie pola
                    }
                    else
                    {
                        Explode(); // wybuch
                    }
                }
                else
                {
                    EmptyFieldButtonClick(clickedButton); ///otwiera pola, jeśli nie ma bomb
                }
                isFirstClick = false;
            }
            if  (e.Button == MouseButtons.Right) ///jeśli klikniesz prawym przyciskiem myszy
            {

                clickedButton.isClickable = !clickedButton.isClickable;
                if (!clickedButton.isClickable)
                {
                    clickedButton.Text = "B";
                }
                else
                {
                    clickedButton.Text = "";
                }
            }
            CheckWin();
        }

       
        
        
        
        void Explode()
        {
            
            foreach (FieldButton button in field)
            {
                if (button.isBomb)
                {
                    button.Text = "*";
                }
            }
            MessageBox.Show("You lose :(");
            Application.Restart();
        }
        
        
        void EmptyFieldButtonClick(FieldButton clickedButton) ///otwiera pola, jeśli nie ma bomb
        {

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (field[x, y] == clickedButton)
                    {
                        
                        OpenRegion(x, y, clickedButton);
                    }
                }
            }

            
        }
        void OpenRegion(int xCoord, int yCoord, FieldButton clickedButton)/// otwiera pola
        {
            Queue<FieldButton> queue = new Queue<FieldButton>();
            queue.Enqueue(clickedButton);
            clickedButton.wasAdded = true;
            while (queue.Count > 0)
            {
                FieldButton currentCell = queue.Dequeue();
                OpenCell(currentCell.xCoord, currentCell.yCoord, currentCell);
                cellsOpened++;
                if (CountBombsAround(currentCell.xCoord, currentCell.yCoord) == 0)
                {
                    for (int y = currentCell.yCoord - 1; y <= currentCell.yCoord + 1; y++)
                    {
                        for (int x = currentCell.xCoord - 1; x <= currentCell.xCoord + 1; x++)
                        {
                            if (x == currentCell.xCoord && y == currentCell.yCoord)
                            {
                                continue;
                            }
                            if (x >= 0 && x < width && y < height && y >= 0)
                            {
                                if (!field[x, y].wasAdded)
                                {
                                    queue.Enqueue(field[x, y]);
                                    field[x, y].wasAdded = true;
                                }
                            }
                        }
                    }
                }
            }

        }

        void OpenCell(int x, int y, FieldButton clickedButton) ///pokazuje ilosc bobmb 
        {
            int bombsAround = CountBombsAround(x, y);
            if (bombsAround == 0)
            {

            }
            else
            {
                clickedButton.Text = "" + bombsAround;
            }
            clickedButton.Enabled = false;
        }
        int CountBombsAround(int xCoord, int yCoord) /// zapisuje ilosc bomb
        {
            int bombsAround = 0;
            for (int x = xCoord - 1; x <= xCoord + 1; x++)
            {
                for (int y = yCoord - 1; y <= yCoord + 1; y++)
                {
                    if (x >= 0 && x < width && y >= 0 && y < height)
                    {
                        if (field[x, y].isBomb == true)
                        {
                            bombsAround++;
                        }
                    }
                }
            }
            return bombsAround;
        }
        void CheckWin() ///Pokazuje, kiedy gracz wygrał
        {
            int cells = width * height;
            int emptyCells = cells - bombs;
            if (cellsOpened == emptyCells)
            {
                MessageBox.Show("you win! ;)");
                Application.Restart();
                
            }
        }

        private void SIZEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FieldButton1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        
    }
   

    


    public class FieldButton : Button
    {
        public bool isBomb;
        public bool isClickable;
        public bool wasAdded;
        public int xCoord;
        public int yCoord;
    }
}