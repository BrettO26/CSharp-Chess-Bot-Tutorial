using SysConsole = System.Console;

namespace ChessInterfaceSource
{
    public static class Console
    {
        //Constants
        public const ConsoleColor DefaultForeground = ConsoleColor.White, DefaultBackground = ConsoleColor.Black;
        
        //Foreground
        private static ConsoleColor _ForegroundColor = ConsoleColor.White;
        public static ConsoleColor ForegroundColor
        {
            get => _ForegroundColor;
            set
            {
                SysConsole.ForegroundColor = value;
                _ForegroundColor = value;
            }
        }

        //Background
        private static ConsoleColor _BackgroundColor = ConsoleColor.Black;
        public static ConsoleColor BackgroundColor
        {
            get => _BackgroundColor;
            set
            {
                SysConsole.BackgroundColor = value;
                _BackgroundColor = value;
            }
        }

        //Sync
        private static readonly object Sync = new();

        //Colors
        public static void SetColors(ConsoleColor ForegroundColor, ConsoleColor BackgroundColor)
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
        }
        public static void ResetColor()
        {
            ForegroundColor = DefaultForeground;
            BackgroundColor = DefaultBackground;
        }

        //Writing
        public static void WriteLine() => SysConsole.WriteLine();
        public static void WriteLine(object Object, ConsoleColor Color = DefaultForeground) =>
            Write($"{Object ?? "null"}\n", Color);
        public static void WriteLine(object Object) =>
            Write($"{Object ?? "null"}\n");

        public static void Write(object Object, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor)
        {
            lock (Sync)
            {
                Object ??= "null";
                SysConsole.ForegroundColor = ForegroundColor;
                SysConsole.BackgroundColor = BackgroundColor;
                SysConsole.Write(Object);
                SysConsole.ForegroundColor = Console.ForegroundColor;
                SysConsole.BackgroundColor = Console.BackgroundColor;
            }
        }
        public static void Write(object Object, ConsoleColor ForegroundColor = DefaultForeground)
        {
            lock (Sync)
            {
                Object ??= "null";
                SysConsole.ForegroundColor = ForegroundColor;
                SysConsole.Write(Object);
                SysConsole.ForegroundColor = Console.ForegroundColor;
            }
        }
        public static void Write(object Object)
        {
            lock (Sync)
            {
                Object ??= "null";
                SysConsole.Write(Object);
            }
        }

        //Reading
        public static string ReadLine() =>
            SysConsole.ReadLine();
        public static ConsoleKeyInfo ReadKey() =>
            SysConsole.ReadKey();
        public static bool ReadBool()
        {
            GetBool:
            switch (ReadLine().ToLower())
            {
                case "yes": return true;
                case "no": return false;
                case "y": return true;
                case "n": return false;
                case "1": return true;
                case "0": return false;
                case "w": return true;
                case "b": return false;
                case "white": return true;
                case "black": return false;
                default:
                    WriteLine("Could not parse response, please try again.", ConsoleColor.Yellow);
                    goto GetBool;
            }
        }
    }
}