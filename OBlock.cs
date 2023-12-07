namespace Tetris {

    public class OBlock : Block{

        //Method: O block is SQUARE, so rotating it means the same position, no Effect, just 1 position
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };

        public override int Id => 4;

        protected override Position StartOffset => new Position(0,4);
        protected override Position[][] Tiles => tiles;
    }
}