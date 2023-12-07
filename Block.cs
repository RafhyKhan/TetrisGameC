using System.Collections.Generic;

namespace Tetris
{
    //Making a class Called Block, can label ATTRIBUTES
    public abstract class Block {
        protected abstract Position[][] Tiles { get; }     //Position currently
        protected abstract Position StartOffset { get; }  //Where it starts
        public abstract int Id { get; } //Id of created Object

        private int rotationState;
        private Position offset; 

        //Constructor: Values defined
        public Block() {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        //Updating position with offset
        public IEnumerable<Position> TilePositions () {
            foreach (Position p in Tiles[rotationState]) {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        //Method: Rotates the block ClockWise
        public void RotateCW() {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        //Method: Rotates the block Clounter Clouckwise
        public void RotateCCW() {
            if (rotationState == 0) {
                rotationState = Tiles.Length - 1;
            }
            else {
                rotationState--;
            }
        }


        //Method: Moves block by given number of rows and columns
        public void Move(int rows, int columns) {
            offset.Row += rows;
            offset.Column += columns; 
        }


        //Method: Resets rotation and position
        public void Reset() {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column; 
        }
    }  
}