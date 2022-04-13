using Snake.Business;

namespace Snake.ConsoleUI
{
    public static class Program
    {
        private static Dictionary<ConsoleKey, (int, int)> KeyToDirection = new Dictionary<ConsoleKey, (int, int)>()
    {
        { ConsoleKey.D, (0, 1) },
        { ConsoleKey.A, (0, -1) },
        { ConsoleKey.S, (1, 0) },
        { ConsoleKey.W, (-1, 0) },
        { ConsoleKey.RightArrow, (0, 1) },
        { ConsoleKey.LeftArrow, (0, -1) },
        { ConsoleKey.DownArrow, (1, 0) },
        { ConsoleKey.UpArrow, (-1, 0) },
    };

        public static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.CursorVisible = false;
            Field field = new Field(20, 20);
            DisplayBorder(field);
            Thread listener = new Thread(new ParameterizedThreadStart(Listen));
            listener.Start(field);
            while (true)
            {
                Display(field);
                Thread.Sleep(100);
                if (!field.Next())
                {
                    return;
                }
            }

            Console.SetCursorPosition(0, field.Height + 4);
            Console.WriteLine("GAME OVER");
        }

        private static void Listen(object o)
        {
            Field field = (Field)o;
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                if (KeyToDirection.TryGetValue(key, out (int, int) direction) && (-direction.Item1, -direction.Item2) != field.Direction)
                {
                    field.Direction = direction;
                }
            }
        }

        private static void Display(Field field)
        {
            Console.SetCursorPosition(2 + field.LastTailPosition.Item2 * 2, 1 + field.LastTailPosition.Item1);
            Console.Write("  ");

            foreach (var pair in field.Snake)
            {
                Console.SetCursorPosition(2 + pair.Item2 * 2, 1 + pair.Item1);
                Console.Write("██");
            }

            Console.SetCursorPosition(2 + field.Food.Item2 * 2, 1 + field.Food.Item1);
            Console.Write("$$");
        }

        private static void DisplayBorder(Field field)
        {
            string horizontalBorder = new string('#', field.Width * 2 + 4);
            Console.SetCursorPosition(0, 0);
            Console.Write(horizontalBorder);
            Console.SetCursorPosition(0, field.Height + 1);
            Console.Write(horizontalBorder);
            for (int i = 0; i < field.Height + 2; ++i)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("##");
            }

            for (int i = 0; i < field.Height + 2; ++i)
            {
                Console.SetCursorPosition(field.Width * 2 + 2, i);
                Console.Write("##");
            }
        }
    }
}