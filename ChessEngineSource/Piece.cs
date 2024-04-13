namespace ChessEngineSource
{
    public enum PieceType : byte
    {
        Null = 0,
        Wall = 1,
        Pawn = 2,
        Knight = 3,
        Bishop = 4,
        Rook = 5,
        Queen = 6,
        King = 7,
    }

    public struct Piece
    {
        public PieceType Type = PieceType.Null;
        public bool Color = false;
        public bool HasMoved = false;

        public Piece() { }
        public Piece(PieceType Type, bool Color, bool HasMoved = false)
        {
            this.Type = Type;
            this.Color = Color;
            this.HasMoved = HasMoved;
        }
        public Piece(char Symbol)
        {
            Color = char.IsUpper(Symbol);
            Type = char.ToLower(Symbol) switch
            {
                'p' => PieceType.Pawn,
                'n' => PieceType.Knight,
                'b' => PieceType.Bishop,
                'r' => PieceType.Rook,
                'q' => PieceType.Queen,
                'k' => PieceType.King,
                _ => PieceType.Null
            };
        }
    }

    public struct CompactPiece
    {
        public class Cached
        {
            public const byte WRook = 0b_0000_1_101;
            public const byte BRook = 0b_0000_0_101;

            public const byte MWRook = 0b_1000_1_101;
            public const byte MBRook = 0b_1000_0_101;

            public const byte MWKing = 0b_1000_1_110;
            public const byte MBKing = 0b_1000_0_110;
        }

        public const bool White = true, Black = false;
        public const byte None = MovedMask;

        public const byte TypeMask =  0b00000111;
        public const byte ColorMask = 0b00001000;
        public const byte MovedMask = 0b10000000;

        public byte PieceData = 0;
        public readonly PieceType Type => (PieceType)(PieceData & TypeMask);
        public readonly bool Color => (PieceData & ColorMask) != 0;
        public readonly bool HasMoved => (PieceData & MovedMask) != 0;

        public readonly bool IsNull => (PieceData & TypeMask) == 0;
        public readonly bool IsWall => (PieceData & TypeMask) == 1;
        public readonly bool IsNullOrWall => (PieceData & TypeMask) <= 1;
        public readonly int Value => Type switch
        {
            PieceType.Pawn => 100,
            PieceType.Knight => 300,
            PieceType.Bishop => 300,
            PieceType.Rook => 500,
            PieceType.Queen => 900,
            PieceType.King => 20000,
            _ => 0
        };
        public readonly short SValue => Type switch
        {
            PieceType.Pawn => 100,
            PieceType.Knight => 300,
            PieceType.Bishop => 300,
            PieceType.Rook => 500,
            PieceType.Queen => 900,
            PieceType.King => 20000,
            _ => 0
        };

        public CompactPiece() { }
        public CompactPiece(byte PieceData) => this.PieceData = PieceData;
        public CompactPiece(bool Color, PieceType Piece, bool HasMoved = false) =>
            CreateNew(ref PieceData, Color, Piece, HasMoved);

        public static implicit operator byte(CompactPiece Piece) => Piece.PieceData;
        public static implicit operator CompactPiece(byte PieceData) => new(PieceData);

        //  M???CTTT
        public static byte AllocateNew(bool Color, PieceType Type, bool HasMoved = false) =>
            (byte)((Color ? ColorMask : 0) | (int)Type | (HasMoved ? MovedMask : 0));
        public static void CreateNew(ref byte Source, bool Color, PieceType Type, bool HasMoved = false) =>
            Source = (byte)((Color ? ColorMask : 0) | (int)Type | (HasMoved ? MovedMask : 0));
        public static void CreateNew(ref byte Source, char PieceDefinition)
        {
            bool Color = char.IsUpper(PieceDefinition);
            switch (char.ToLower(PieceDefinition))
            {
                case 'p': CreateNew(ref Source, Color, PieceType.Pawn); break;
                case 'n': CreateNew(ref Source, Color, PieceType.Knight); break;
                case 'b': CreateNew(ref Source, Color, PieceType.Bishop); break;
                case 'r': CreateNew(ref Source, Color, PieceType.Rook); break;
                case 'q': CreateNew(ref Source, Color, PieceType.Queen); break;
                case 'k': CreateNew(ref Source, Color, PieceType.King); break;
                default: CreateNew(ref Source, Color, PieceType.Null); break;
            }
        }
        public static void CreateNew(ref CompactPiece Piece, bool Color, PieceType Type, bool HasMoved = false) =>
            Piece = (byte)((Color ? ColorMask : 0) | (int)Type | (HasMoved ? MovedMask : 0));
        public static void CreateNew(ref CompactPiece Piece, char PieceDefinition)
        {
            bool Color = char.IsUpper(PieceDefinition);
            switch (char.ToLower(PieceDefinition))
            {
                case 'p': CreateNew(ref Piece, Color, PieceType.Pawn); break;
                case 'n': CreateNew(ref Piece, Color, PieceType.Knight); break;
                case 'b': CreateNew(ref Piece, Color, PieceType.Bishop); break;
                case 'r': CreateNew(ref Piece, Color, PieceType.Rook); break;
                case 'q': CreateNew(ref Piece, Color, PieceType.Queen); break;
                case 'k': CreateNew(ref Piece, Color, PieceType.King); break;
                default: CreateNew(ref Piece, Color, PieceType.Null); break;
            }
        }

        public static PieceType TypeOf(byte Source) => (PieceType)(Source & TypeMask);
        public static bool ColorOf(byte Source) => (Source & ColorMask) != 0;
        public static void ColorAndTypeOf(CompactPiece Source, out bool Color, out PieceType Type)
        {
            Color = (Source.PieceData & ColorMask) != 0;
            Type = (PieceType)(Source.PieceData & TypeMask);
        }
        public static void ColorAndTypeOf(byte Source, out bool Color, out PieceType Type)
        {
            Color = (Source & ColorMask) != 0;
            Type = (PieceType)(Source & TypeMask);
        }

        public static ConsoleColor ToConsoleColor(byte Piece, out bool OpenSpace)
        {
            ColorAndTypeOf(Piece, out bool Color, out PieceType Type);
            OpenSpace = (byte)Type <= 1;

            if (Color) return Type switch
            {
                PieceType.Pawn => ConsoleColor.Magenta,
                PieceType.Knight => ConsoleColor.Red,
                PieceType.Bishop => ConsoleColor.Green,
                PieceType.Rook => ConsoleColor.Blue,
                PieceType.Queen => ConsoleColor.Cyan,
                PieceType.King => ConsoleColor.Yellow,

                PieceType.Wall => ConsoleColor.Black,
                PieceType.Null => ConsoleColor.Black,
                _ => ConsoleColor.Black,
            };
            else return Type switch
            {
                PieceType.Pawn => ConsoleColor.DarkMagenta,
                PieceType.Knight => ConsoleColor.DarkRed,
                PieceType.Bishop => ConsoleColor.DarkGreen,
                PieceType.Rook => ConsoleColor.DarkBlue,
                PieceType.Queen => ConsoleColor.DarkCyan,
                PieceType.King => ConsoleColor.DarkYellow,

                PieceType.Wall => ConsoleColor.Black,
                PieceType.Null => ConsoleColor.Black,
                _ => ConsoleColor.Black,
            };
        }
    }
}