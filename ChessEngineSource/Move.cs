namespace ChessEngineSource
{
    public struct Move
    {
        public static readonly Move Null = new(new(0, 0), new(0, 0));

        public ChessVector From, Too;

        public Move(ChessVector From, ChessVector Too)
        {
            this.From = From;
            this.Too = Too;
        }

        public static bool TryParse(string MoveText, out Move Move)
        {
            try
            {
                if (string.IsNullOrEmpty(MoveText))
                    goto BadMove;

                MoveText = MoveText.Trim();
                if (MoveText.Length == 4) // Likely: E2E4
                {
                    string FromRaw = MoveText[..2];
                    string TooRaw  = MoveText[2..];
                    Move = new(ChessVector.Parse(FromRaw), ChessVector.Parse(TooRaw));
                }
                else if (MoveText.Length == 5) // Likely: E2>E4
                {
                    if (MoveText[2] != '>')
                        goto BadMove;

                    string[] RawMoveSegments = MoveText.Split('>');
                    Move = new(ChessVector.Parse(RawMoveSegments[0]), ChessVector.Parse(RawMoveSegments[1]));
                }
                else goto BadMove;

                return true;

                BadMove:
                Move = Null;
                return false;

            }
            catch
            {
                Move = Null;
                return false;
            }
        }
    }
}