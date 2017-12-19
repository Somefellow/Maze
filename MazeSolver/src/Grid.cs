using System.Linq;

namespace MazeSolver.src
{
    class Grid<T>
    {
        int _Width, _Height;
        T[] _Data;

        public int Width
        {
            get
            {
                return _Width;
            }
        }

        public int Height
        {
            get
            {
                return _Height;
            }
        }

        public Grid(int Width, int Height)
        {
            _Width = Width;
            _Height = Height;
            _Data = new T[_Width * _Height];
        }

        public Grid(Grid<T> Original)
        {
            _Data = Original._Data.Clone() as T[];
        }

        public void Fill(T Value)
        {
            _Data.SetValue(Value, Enumerable.Range(0, _Data.Length).ToArray());
        }

        public void SetValue(T Value, int X, int Y)
        {
            _Data.SetValue(Value, X + (Y * Width));
        }

        public T GetValue(int X, int Y)
        {
            return (T)_Data.GetValue(X + (Y * Width));
        }
    }
}
