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


		
    public class FieldButton : Button
    {
        public bool isBomb;
        public bool isClickable;
        public bool wasAdded;
        public int xCoord;
        public int yCoord;
    }
}