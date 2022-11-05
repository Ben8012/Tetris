using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Block
    {

        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }

        public abstract int Id { get; }

        private int _rotationState;
        private Position _offset;

        public Block()
        {
            _offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[_rotationState])
            {
                yield return new Position(p.Row + _offset.Row, p.Column + _offset.Column );
            }
        }

        // rotation sens des aiguilles d'une montre
        public void RotateCW()
        {
            _rotationState = (_rotationState + 1 ) % Tiles.Length;
        }

        // rotation sens inverse des aiguilles d'une montre
        public void RotateCCW()
        {
            if (_rotationState == 0)
            {
                _rotationState = Tiles.Length - 1;
            }
            else
            {
                _rotationState--;
            }
        }

        public void Move(int rows, int columns)
        {
            _offset.Row += rows;
            _offset.Column += columns;
        }

        public void Reset()
        {
            _rotationState = 0;
            _offset.Row = StartOffset.Row;
            _offset.Column = StartOffset.Column;
        }
    }
}
