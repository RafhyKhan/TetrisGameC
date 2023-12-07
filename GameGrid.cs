
namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid; //for the gamegrid, first column if rows, second is columnns
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c] //Doing INDEXING
        {
            get => grid[r,c];
            set => grid[r,c] = value;
        }

        //Constructor
        public GameGrid(int rows, int columns) {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        //Method: Check if given row or column is inside grid or not
        public bool IsInside(int r, int c) {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        //Method: Is the given cell empty or not
        public bool IsEmpty(int r, int c) {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        //Method: Checks if the row is full
        public bool IsRowFull(int r) {
            for (int c = 0; c < Columns; c++) {
                if (grid[r, c] == 0) {
                    return false;
                }
            }

            return true;
        }

        //Method: Check if the rows are Empty
        public bool IsRowEmpty(int r) {
            for (int c = 0; c < Columns; c++) {
                if (grid[r,c] != 0) {
                    return false;
                }
            }
            return true;
        }

        //Method: Clears a row, resets it to 0 value
        private void ClearRow(int r) {
            for (int c =0; c < Columns; c++) {
                grid[r,c] = 0;
            }
        }

        //Method: Moves a row down
        private void MoveRowDown(int r, int numRows) {
            for (int c = 0; c < Columns; c++) {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        //Method: Clears all rows that are fulled, returns number of full cleared rows
        public int ClearFullRows() {
            int cleared = 0;

            for (int r = Rows-1; r >= 0; r--) {
                if (IsRowFull(r)) {
                    ClearRow(r);
                    cleared++;
                }

                else if (cleared > 0) {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }
    }
}