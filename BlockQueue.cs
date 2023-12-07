using System;

namespace Tetris{


    //Method: 
    public class BlockQueue {

        //Method: To go through all the block types
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };


        //Method: Random number
        private readonly Random random = new Random();

        //Method: Property for next block in queue, should preview in UI, for player
        //Could replace with array to return new FEW blocks, if you want plater to see all
        public Block NextBlock { get; private set; }

        //CONSTRUCTOR: Intilize next block with a random block
        public BlockQueue () {
            NextBlock = RandomBlock();
        }

        //Method: Returns a RANDOM block type
        private Block RandomBlock() {
            return blocks[random.Next(blocks.Length)];
        }

        //Method: Returns the next block, and updates the property
        // We dont want to get the same block in a row, so keep picking until we
        // get a new one!
        public Block GetAndUpdate() {
            Block block = NextBlock;

            do {
                NextBlock = RandomBlock();
            }while(block.Id == NextBlock.Id);

            return block;
        }
    }
}