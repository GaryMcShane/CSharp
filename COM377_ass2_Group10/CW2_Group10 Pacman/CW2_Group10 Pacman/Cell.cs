using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CellClass
{
    public class Cell
    {
        const int cellSize = 20; //cell dimension in pixels
        private int x, y;
        private char type;
        private bool isVisible = true;

        //Constructor
        public Cell(int x, int y, char type)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }

        //Set cell as visited (no pill inside it)
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }

            set
            {
                isVisible = value;

            }
        }

        public char CellType
        {
            get { return type; }
            set { type = value; }
        }


        //draw the cell 
        public void DrawBackground(Graphics g)
        {

            int xBase = 0;
            int yBase = 0;

            switch (type)
            {
                case 'e': //corral exit
                    //create solid brush
                    // brush = new SolidBrush(Color.White);
                    g.FillRectangle(Brushes.White, x * cellSize, y * cellSize + cellSize / 2 - 10, cellSize, 3);
                    break;
                case '-': //horizontal line
                    //create solid brush
                    //brush = new SolidBrush(Color.Blue);
                    g.FillRectangle(Brushes.Blue, x * cellSize, y * cellSize + cellSize / 2 - 1, cellSize, 3);
                    break;
                case '|': //vertical line
                    //create solid brush
                    //brush = new SolidBrush(Color.Blue);
                    g.FillRectangle(Brushes.Blue, x * cellSize + cellSize / 2 - 1, y * cellSize, 3, cellSize);
                    break;
                case '1': //northeast corner
                    xBase = x * cellSize - cellSize / 2;
                    yBase = y * cellSize + cellSize / 2;
                    DrawCorner(g, xBase, yBase);
                    break;
                case '2': //northwest corner
                    xBase = x * cellSize + cellSize / 2;
                    yBase = y * cellSize + cellSize / 2;
                    DrawCorner(g, xBase, yBase);
                    break;
                case '3': //southeast corner
                    xBase = x * cellSize - cellSize / 2;
                    yBase = y * cellSize - cellSize / 2;
                    DrawCorner(g, xBase, yBase);
                    break;
                case '4': //southwest corner
                    xBase = x * cellSize + cellSize / 2;
                    yBase = y * cellSize - cellSize / 2;
                    DrawCorner(g, xBase, yBase);
                    break;
                case 'o':
                    break; //empty navigable cell

                case 's': //navigable cell with pill
                    if (isVisible)
                    {
                        // brush = new SolidBrush(Color.White);
                        g.FillRectangle(Brushes.White, x * cellSize + cellSize / 2 - 1, y * cellSize + cellSize / 2 - 1, 3, 3);
                    }
                    break;

                case 'p': //navigable cell with power pellet
                    if (isVisible)
                    {
                        // brush = new SolidBrush(Color.Pink);
                        g.FillEllipse(Brushes.Pink, x * cellSize + cellSize / 2 - 7, y * cellSize + cellSize / 2 - 7, 13, 13);
                    }
                    break;
                case 'x': //empty non-navigable cell
                case 'c': //the Corral
                default:
                    break;
            }
        }

        //draw a rounded corner 3 pixels thick
        public virtual void DrawCorner(Graphics g, int xBase, int yBase)
        {
            RectangleF oldClip = g.ClipBounds;
            g.SetClip(new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize));
            g.DrawEllipse(new Pen(Color.Blue, 3), xBase, yBase, cellSize, cellSize);
            g.SetClip(oldClip);
        }

    }

}
