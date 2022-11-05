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

        public bool IsInsideGrid(int r,int c)
        {
            return r >= 0 && r < Rows && c < Columns;
        }

        public bool IsCellEmpty(int r , int c)
        {
            return IsInsideGrid(r,c) && _grid[r , c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if(_grid[r,c] == 0)
                    return false;
            }
            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (_grid[r, c] != 0)
                    return false;
            }
            return true;
        }

        public void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                _grid[r, c] = 0;
            }     
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                _grid[r + numRows, c] = _grid[r, c];
                _grid[r, c] = 0;
            }
        }

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
