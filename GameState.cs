using System.Windows.Input;

namespace Tetris {


    //Class: Which handles interactions between paths written in Block Queue
    public class GameState {
        private Block currentBlock;

        //Method: Setting properties for CurrentBlock object
        public Block CurrentBlock {
            get => currentBlock;
            private set {
                currentBlock = value;
                currentBlock.Reset(); //reset method, to set right start position/orientation


                //Move down two rows if nothing is in the way
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }

            }
        }

        //Set all properties
        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }

        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        //CONSTURCTOR: Initialize grid, block qeue for random block, and update
        public GameState() {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }


        //Method: Checks if current block type, is in a right position or not,
        //  Legal Position, it is hitting something/overlapping a position?
        private bool BlockFits() {
            foreach (Position p in CurrentBlock.TilePositions()) {
                if (!GameGrid.IsEmpty(p.Row, p.Column)) {
                    return false;
                }
            }

            return true;
        }


        //Method: Holds the block for later, if CanHold method
        // returns true
        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            //If you have no block in hand, than the hold block is a new block
            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }

            //If you have a block in hand, than hold block is original block
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;

        }


        //Method: To check if its POSSIBLE to rotate current block, is it hitting
        // something, if yes than NOT POSSIBLE. It rotates it back if not LEGAL
        public void RotateBlockCW() {
            CurrentBlock.RotateCW();

            if (!BlockFits()) {
                CurrentBlock.RotateCCW();
            }
        }

        //Method: Same a method before, but if user rotates CCW, and is it overlapping
        // or not. if YES than rotate back CW
        public void RotateBlockCCW() {
            CurrentBlock.RotateCCW();

            if (!BlockFits()) {
                CurrentBlock.RotateCW();
            }
        }


        //Method: User moves Block Left, is it overlapping, if yes, than we move it
        // back right.
        public void MoveBlockLeft() {
            CurrentBlock.Move(0, -1);

            if (!BlockFits()) {
                CurrentBlock.Move(0, 1);
            }
        }

        //Method: User moves Block Right, is it overlapping, if yes, than we move it
        // back Left.
        public void MoveBlockRight() {
            CurrentBlock.Move(0, 1);

            if (!BlockFits()) {
                CurrentBlock.Move(0, -1);
            }
        }


        //Method: Checks if the game is over? If either of hidden rows at the top are
        // NOT EMPTY, than the game is LOST
        private bool IsGameOver() {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }


        //METHOD: when current block cannot be moved down. Your placing a block into
        // the game, can you??? SHould be gameover if you cant!!!
        private void PlaceBlock() {
            foreach (Position p in CurrentBlock.TilePositions()) {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver()) {
                GameOver = true;
            }
            else {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }


        //Method: MoveBlockDown method, like other move methods, but call the 
        // place block method, IF you cannot move down anymore...
        // Like in game, next one pops in if you hit something.
        public void MoveBlockDown() {
            CurrentBlock.Move(1, 0);

            if (!BlockFits()) {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }



        //Method: Can find out how many rows, current block can be
        // moved down
        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }



        //Method: To knwo the istacne, invoking for all blocks
        public int BlockDropDistance() {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions() )
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }



        //Method: Drops the block down, the right distance length
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        



    }
}