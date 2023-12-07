namespace Tetris {

    //Class: Made to dictate the position of the tetris block in control
    public class Position {

        public int Row { get; set; }

        public int Column { get; set; }

        //Constructor: For position of made block by user
        public Position(int row, int column) {
            Row = row;
            Column = column;
        }

        


    }


}