using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameState
    {
        private Block _currentBlock;
        public Block CurrentBlock
        {
            get => _currentBlock; 
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset();

                for (int i = 0; i < 2; i++)
                {
                    CurrentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        CurrentBlock.Move(-1, 0);
                    }
                }
            } 
        }
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22,10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        // Verifie si le block est dans une position correct
        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if(!GameGrid.IsCellEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }
            if(HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        // rotation dans le sens des aiguilles d'une montre
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        // roation dans le sens inverse des aiguilles d'une montre
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();
            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        // bouger le block sur la gauche
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }


        // bouger le block sur la droite
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        // fin du jeux
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        // lors du placement d'un block on boucle sur la grille pour assigner les nouveau Id 
        // on efface les lignes pleines en incrementant le score
        // on verifie si la partie est perdue si pas on met a jour les block courant
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock= BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        // deplacer les block vers la bas
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }


        private int TileDropDistance(Position p)
        {
            int drop = 0;
            while(GameGrid.IsCellEmpty(p.Row + drop + 1 , p.Column))
            {
                drop++;
            }
            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
