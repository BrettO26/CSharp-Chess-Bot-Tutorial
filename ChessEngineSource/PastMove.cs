namespace ChessEngineSource
{
    public struct PastMove
    {
        public int From, Too;
        public Piece OriginalStartPiece, OriginalEndPiece;

        public PastMove(int From, int Too, Piece OriginalStartPiece, Piece OriginalEndPiece)
        {
            this.From = From;
            this.Too = Too;
            this.OriginalStartPiece = OriginalStartPiece;
            this.OriginalEndPiece = OriginalEndPiece;
        }
    }
}