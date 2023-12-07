using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Method: We getting all images required for tiles color
        private readonly ImageSource[] tileImages = new ImageSource[] {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };


        //Method: We getting all images required for block types
        private readonly ImageSource[] blockImages = new ImageSource[] {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };



        //Method: Image control, 1 image control for every pixel in game grid
        private readonly Image[,] imageControls;

        //Want to increase diffculty, as score increases
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        //Method: A Game State Method
        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }


        //Method: Image controls array, 22 rows, 10 columns
           //Setting up the CANVAS of game
        private Image[,] SetupGameCanvas(GameGrid grid) {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25; //250, 500 in UI xaml file, for hieght and width. 25 is cell size, for each tile in grid


            //Loop through every row and colum in game grid, creating new iage control, with 25 pixel  W and H. 
            for (int r = 0; r < grid.Rows; r++) 
            { 
                for (int c = 0; c < grid.Columns; c++) 
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    
                    //Making pixel block for every cell in game grid, 25
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); //making sure top hidden rows are accounted for
                    Canvas.SetLeft(imageControl, c * cellSize); 
                    GameCanvas.Children.Add(imageControl); //Image pixel
                    imageControls[r, c] = imageControl;

                }
            }

            return imageControls;
        }

        //Method: Drawing the Game Grid
        private void DrawGrid(GameGrid grid)
        {
            //Loop through every row and colum in game grid
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];  //connecting to right image based on id
                }
            }
        }


        //Method: Drawing the curent block
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }


        //Method: Want to preview the next block given to user
        private void DrawNextBlock(BlockQueue blockQueue) {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        //Method: Draw the block that your holding
        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        //Method: We want to allow player to see where blok will 
        //land, if they hit spaceBar, sending it down ASAP
        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }


        //Method: Draws the block and game grid, displays score
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }


        //Method: So it goes down by itself, for game challenge, based on timer?
        private async Task GameLoop()
        {
            Draw(gameState);

            while(!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));

                await Task.Delay(delay);
                gameState.MoveBlockDown(); //Move down every 500 ms
                Draw(gameState); //Draws it again
            }


            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }


        //Action Listener: for Any Keys specificed button presses
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return; //Do nothing if press a key when game over
            }
            //Switch Method: Choosing the keys, for user inputs, into the game
            switch(e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        //Action Listener: for when game loads
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop(); //When game canvas is laoded, make the grid
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden; //Get rid of overlay
            await GameLoop();
        }
    }
}