namespace Snake.Business
{
    public class Field
    {
        private readonly List<(int, int)> _snake;
        private (int, int) _direction;
        private (int, int) _lastTailPosition = (0, 3);
        private Random _random;

        public Field(int width, int height)
        {
            Width = width;
            Height = height;
            _snake = new List<(int, int)> { (0, 2), (0, 1), (0, 0) };
            _direction = (0, 1);
            _random = new Random((int)DateTime.Now.Ticks);
            Spawn_food();
        }

        public int Width { get; }

        public int Height { get; }

        public bool Next()
        {
            (int, int) head = (_snake[0].Item1 + this._direction.Item1, this._snake[0].Item2 + this._direction.Item2);
            if (head.Item1 >= 0 && head.Item1 < Height && head.Item2 >= 0 && head.Item2 < Width && !this._snake.Contains(head))
            {
                this._snake.Insert(0, head);
                if (head != Food)
                {
                    this._lastTailPosition = _snake[^1];
                    _snake.RemoveAt(this._snake.Count - 1);
                }
                else
                {
                    Spawn_food();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public (int, int) Direction
        {
            get => this._direction;
            set
            {
                this._direction = value;
            }
        }

        public IReadOnlyList<(int, int)> Snake => this._snake;

        public (int, int) Food { get; private set; }

        public (int, int) LastTailPosition => this._lastTailPosition;

        private void Spawn_food()
        {
            (int, int)[] freeSquares = GetAllFreeSquares().ToArray();
            (int, int) _food = freeSquares[_random.Next(0, freeSquares.Length)];
            Food = _food;
        }

        private IEnumerable<(int, int)> GetAllFreeSquares()
        {
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    if (!this._snake.Contains((i, j)))
                    {
                        yield return (i, j);
                    }
                }
            }
        }
    }
}
