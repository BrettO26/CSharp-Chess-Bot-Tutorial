

namespace ChessEngineSource
{
    public struct ChessVector
    {
        public int X = 0, Y = 0;
        public readonly bool IsWithinBoard => X > -1 && Y > -1 && X < 8 && Y < 8;
        public readonly int Index => X + (Y * 8);

        public ChessVector() { }
        public ChessVector(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public ChessVector(int Index)
        {
            X = Index % 8;
            Y = Index / 8;
        }

        public readonly string ToLetterLocation() => X switch
        {
            0 => 'a',
            1 => 'b',
            2 => 'c',
            3 => 'd',
            4 => 'e',
            5 => 'f',
            6 => 'g',
            7 => 'h',
            _ => '?',
        } + (Y + 1).ToString()[..1];
        public static string ToLetterLocation(int X, int Y) => X switch
        {
            0 => 'a',
            1 => 'b',
            2 => 'c',
            3 => 'd',
            4 => 'e',
            5 => 'f',
            6 => 'g',
            7 => 'h',
            _ => '?',
        } + (Y + 1).ToString()[..1];

        /// <summary>
        /// Parses strings to vectors that are in the following formats: A1 or 00
        /// </summary>
        public static ChessVector Parse(string RawVector)
        {
            RawVector = RawVector.Trim().ToLower();
            char XRaw = RawVector[0], YRaw = RawVector[1];
            if (char.IsLetter(XRaw) && char.IsDigit(YRaw))
            {
                int X = XRaw - 'a', Y = YRaw - '0' - 1;
                return new ChessVector(X, Y);
            }
            else if (char.IsDigit(XRaw) && char.IsDigit(YRaw))
            {
                int X = XRaw - '0', Y = YRaw - '0' - 1;
                return new ChessVector(X, Y);
            }
            else throw new ArgumentException($"Invalid vector data: \"{RawVector}\" is not in a valid format.");
        }
    }
}