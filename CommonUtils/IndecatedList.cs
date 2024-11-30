namespace CommonUtils
{
    public class IndecatedList<T>
    {
        public bool SetIndexToFirstWhenAddItem { get; set; }
        private List<T> items = new List<T>();
        private int _index = 0;
        public IndecatedList()
        {
            SetIndexToFirstWhenAddItem = false;
        }

        public bool HasItem
        {
            get
            {
                return items.Count > 0 ? true : false;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Clear()
        {
            items.Clear();
            _index = -1;
        }

        public T Current
        {
            get
            {
                if( _index < 0)
                {
                    throw new InvalidOperationException("Previous item does not exists");
                }
                return items[_index];
            }
        }

        public bool HasPrevious
        {
            get
            {
                if (HasItem == false || _index <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        public T Previous
        {
            get
            {
                if(HasPrevious == false)
                {
                    throw new InvalidOperationException("Previous item does not exists");
                }
                _index--;
                return Current;
            }
        }

        public bool HasNext
        {
            get
            {
                if (HasItem == false || _index >= items.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }
        public T Next
        {
            get
            {
                if (HasNext == false)
                {
                    throw new InvalidOperationException("Next item does not exists");
                }
                _index++;
                return Current;
            }
        }

        public void Add(T item)
        {
            items.Add(item);
            
            if(SetIndexToFirstWhenAddItem)
            {
                _index = 0;
            }else
            {
                _index = items.Count - 1;
            }
        }
    }
}
