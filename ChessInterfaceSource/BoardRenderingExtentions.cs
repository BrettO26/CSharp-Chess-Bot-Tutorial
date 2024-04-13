using ChessEngineSource;

namespace ChessInterfaceSource
{
    public static class BoardRenderingExtensions
    {
        public static void Render(this Board Board)
        {
            Console.WriteLine($"====[Eval: {0}]==>");
            for (int Y = 7; Y >= 0; Y--)
            {
                for (int X = 0; X < 8; X++)
                {
                    Piece Piece = Board[X, Y];
                    if (Piece.Type == PieceType.Null) //Note: Can remove brackets
                    {
                        if ((X + Y) % 2 == 0)
                             Console.Write(ChessVector.ToLetterLocation(X, Y), ConsoleColor.Gray, ConsoleColor.DarkGray);
                        else Console.Write(ChessVector.ToLetterLocation(X, Y), ConsoleColor.DarkGray, ConsoleColor.Gray);
                    }
                    else Console.Write(ChessVector.ToLetterLocation(X, Y), ConsoleColor.Gray, GetPieceColor(Piece));
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        public static ConsoleColor GetPieceColor(Piece Piece)
        {
            if (Piece.Color) return Piece.Type switch
            {
                PieceType.Pawn => ConsoleColor.Magenta,
                PieceType.Knight => ConsoleColor.Red,
                PieceType.Bishop => ConsoleColor.Green,
                PieceType.Rook => ConsoleColor.Blue,
                PieceType.Queen => ConsoleColor.Cyan,
                PieceType.King => ConsoleColor.Yellow,
                _ => ConsoleColor.Black,
            };
            else return Piece.Type switch
            {
                PieceType.Pawn => ConsoleColor.DarkMagenta,
                PieceType.Knight => ConsoleColor.DarkRed,
                PieceType.Bishop => ConsoleColor.DarkGreen,
                PieceType.Rook => ConsoleColor.DarkBlue,
                PieceType.Queen => ConsoleColor.DarkCyan,
                PieceType.King => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Black,
            };
        }
    }
}
