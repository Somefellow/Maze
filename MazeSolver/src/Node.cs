using Priority_Queue;

namespace MazeSolver.src
{
    class Node<T> : FastPriorityQueueNode
    {
        Node<T> _Parent;
        T _Value;
        int _Depth;

        public Node(Node<T> Parent, T Value)
        {
            _Parent = Parent;
            _Value = Value;
            _Depth = _Parent == null ? 0 : _Parent.Depth + 1;
        }

        public bool Contains(T Value)
        {
            bool result = false;
            Node<T> N = _Parent;
            while (N != null)
            {
                if (N.Value.Equals(Value))
                {
                    result = true;
                    break;
                }
                N = N._Parent;
            }
            return result;
        }

        public int Depth
        {
            get
            {
                return _Depth;
            }
        }

        public T[] ToArray()
        {
            T[] Arr = new T[Depth + 1];
            Node<T> N = this;
            while (N != null)
            {
                Arr[N.Depth] = N.Value;
                N = N.Parent;
            }
            return Arr;
        }

        public Node<T> Parent
        {
            get
            {
                return _Parent;
            }
        }

        public T Value
        {
            get
            {
                return _Value;
            }
        }
    }
}
