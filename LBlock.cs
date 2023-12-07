namespace Tetris{


    public class LBlock : Block
    {
        //Method: For L, You want to account for the different spaces J block takes in each different position
        //as you rotate, clockwise, 4 positions
        private readonly Position[][] tiles = new Position[][] {
            new Position[] { new(0,2), new(1,0), new(1,1), new(1,2) },
            new Position[] { new(0,1), new(1,1), new(2,1), new(2,2) },
            new Position[] { new(1,0), new(1,1), new(1,2), new(2,0) },
            new Position[] { new(0,0), new(0,1), new(1,1), new(2,1) }
        };

        public override int Id => 3;

        protected override Position StartOffset => new Position(0, 3);
        protected override Position[][] Tiles => tiles; 
    }
}