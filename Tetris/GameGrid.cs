using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] _grid;

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new int[rows,columns];
        }

        public int Rows { get; }
        public int Columns { get; }

        public int this[int r , int c]
        {
            get => _grid[r , c];
            set => _grid[r , c] = value;
        }


        // verifier si la cellule est dans la grille
        public bool IsInsideGrid(int r,int c)
        {
            return r >= 0 && r < Rows && c>=0 && c < Columns;
        }

        // verifier si la cellule est remplie ou pas 
        public bool IsCellEmpty(int r , int c)
        {
            return IsInsideGrid(r,c) && _grid[r , c] == 0;
        }

        // verifier si une ligne est complete 
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if(_grid[r,c] == 0)
                    return false;
            }
            return true;
        }

        // verifier si une ligne est vide
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (_grid[r, c] != 0)
                    return false;
            }
            return true;
        }

        // effacer une ligne
        public void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                _grid[r, c] = 0;
            }     
        }

        // deplacer une ligne vers le bas 
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                _grid[r + numRows, c] = _grid[r, c];
                _grid[r, c] = 0;
            }
        }

        // effacer tout la ligne
        public int ClearFullRows()
        {
            int cleared = 0;
            for (int r = Rows-1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared>0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }

    }
}
