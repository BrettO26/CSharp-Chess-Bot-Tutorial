using ChessEngineSource;

namespace ChessInterfaceSource
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PlayGame();
        }

        public static void PlayGame()
        {
            Board GameBoard = new();
            GameBoard.Render();
            Console.WriteLine();

            while (GameBoard.GameState == GameState.Playing)
            {
                TakeUserTurn(GameBoard.WhiteToMove);
                GameBoard.Render();
                Console.WriteLine();
            }

            void TakeUserTurn(bool Color)
            {
                Console.WriteLine($"{(Color ? "White" : "Black")} Players Turn!");

                GetMove:
                string UserInput = Console.ReadLine().Trim().ToLower();
                if (UserInput == "undo") GameBoard.UndoMoves(2);
                else if (Move.TryParse(UserInput, out Move UserMove))
                {
                    
                    if (/*Validate Move (Make sure its legal)*/ true)
                        GameBoard.MakeMove(UserMove);
                    else
                    {
                        Console.WriteLine("Illegal move! Please try again.");
                        goto GetMove;
                    }
                }
                else
                {
                    Console.WriteLine("Could not parse move! Please try again.");
                    goto GetMove;
                }

            }
        }
    }
}