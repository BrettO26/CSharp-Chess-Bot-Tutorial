using ChessEngineSource;

namespace ChessInterfaceSource
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board GameBoard = new();
            GameBoard.Render();
            Console.ReadLine();
        }
    }
}