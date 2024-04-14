namespace ChessEngineSource
{
    public enum GameState : byte
    {
        Playing,
        WhiteVictory,
        BlackVictory,
        Draw,
    }
    public class Board
    {
        public static class Fens
        {
            //General
            public const string Default = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w"; // In description
            public const string Debug = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w";

            //Checkmates
            public const string Checkmate1B = "rnb1k1nr/ppp2p1p/4p1pb/3p4/2P5/3P1PqP/PP2P3/RNBQKBNR b";
            public const string PromoteCheckmateW = "4kbnr/1P1ppppp/8/8/8/8/1PPPPPPP/RNBQKBNR w";

            //Benchmarking
            public const string Benchmark_GetLegal = "r3kb1r/3n1pp1/pqpp1n1p/1p2p1B1/1P2P1b1/PQPP1N1P/3N1P2/R3KB1R w";
            public const string Benchmark_GetLegal2 = "r3kb1r/32pp1/pqpp3p/1p2p1B1/1P2P1b1/PQPP3P/5P2/R3KB1R w";

            //Mono
            public const string MonoPawn = "8/8/8/4P3/8/8/8/8 w";
            public const string MonoKnight = "8/8/8/4N3/8/8/8/8 w";
            public const string MonoBishop = "8/8/8/4B3/8/8/8/8 w";
            public const string MonoRook = "8/8/8/4R3/8/8/8/8 w";
            public const string MonoQueen = "8/8/8/4Q3/8/8/8/8 w";
            public const string MonoKing = "8/8/8/4K3/8/8/8/8 w";
            public const string MonoPawn_Filled = "pppppppp/pppppppp/pppppppp/ppppPppp/pppppppp/pppppppp/pppppppp/pppppppp w";
            public const string MonoKnight_Filled = "pppppppp/pppppppp/pppppppp/ppppNppp/pppppppp/pppppppp/pppppppp/pppppppp w";
            public const string MonoBishop_Filled = "pppppppp/pppppppp/pppppppp/ppppBppp/pppppppp/pppppppp/pppppppp/pppppppp w";
            public const string MonoRook_Filled = "pppppppp/pppppppp/pppppppp/ppppRppp/pppppppp/pppppppp/pppppppp/pppppppp w";
            public const string MonoQueen_Filled = "pppppppp/pppppppp/pppppppp/ppppQppp/pppppppp/pppppppp/pppppppp/pppppppp w";
            public const string MonoKing_Filled = "pppppppp/pppppppp/pppppppp/ppppKppp/pppppppp/pppppppp/pppppppp/pppppppp w";
        }

        public Piece[] Pieces = new Piece[64];
        public bool WhiteToMove = true;
        public Stack<PastMove> PastMoves = new();
        public GameState GameState;

        public Piece this[int X, int Y] =>
            Pieces[X + (Y * 8)];
        public Piece this[int Index] =>
            Pieces[Index];

        #region Construction
        public Board(string Fen = Fens.Default) =>
            LoadFen(Fen);

        public void LoadFen(string Fen)
        {
            if (string.IsNullOrEmpty(Fen)) return;

            ChessVector WritePosition = new(X: 0, Y: 7);
            for (int SymbolIndex = 0; SymbolIndex < Fen.Length; SymbolIndex++)
            {
                char Symbol = Fen[SymbolIndex];
                if (Symbol == ' ') break;
                else if (char.IsLetter(Symbol))
                {
                    Pieces[WritePosition.Index] = new(Symbol);
                    WritePosition.X++;
                }
                else if (Symbol == '/')
                {
                    WritePosition = new(X: 0, Y: WritePosition.Y - 1);
                    continue;
                }
                else WritePosition.X += Symbol - '0';
            }

            WhiteToMove = Fen[^1] == 'w';
        }
        #endregion

        #region Moving
        public void MakeMove(Move Move)
        {
            int StartIndex = Move.From.Index, EndIndex = Move.Too.Index;

            Piece PieceAtStart = Pieces[StartIndex];
            Piece PieceAtEnd = Pieces[EndIndex];

            Pieces[EndIndex] = PieceAtStart;
            Pieces[StartIndex] = Piece.Null;

            PastMoves.Push(new(StartIndex, EndIndex, PieceAtStart, PieceAtEnd));
            WhiteToMove = !WhiteToMove;
        }
        
        public void UndoMoves(int Count)
        {
            for (int MoveIndex = 0; MoveIndex < Count && PastMoves.Count > 0; MoveIndex++)
                UndoMove();
        }
        public void UndoMove()
        {
            if (PastMoves.Count > 0)
            {
                PastMove PastMove = PastMoves.Pop();

                Pieces[PastMove.From] = PastMove.OriginalStartPiece;
                Pieces[PastMove.Too] = PastMove.OriginalEndPiece;
            }
        }
        #endregion
    }
}