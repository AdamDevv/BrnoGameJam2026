#if UNITY_EDITOR

using System.Collections.Generic;

namespace APX.Extra.GameManagement
{
    public class HistoryBuffer<T> where T : class
    {
        public T Current => _currentIndex >= 0 ? _history[_currentIndex] : null;
        public bool HasPrevious => _currentIndex > 0;
        public bool HasNext => _currentIndex < _history.Count - 1;

        private int _currentIndex = -1;
        private readonly List<T> _history;
        private readonly int _maxHistory;

        public HistoryBuffer(int maxHistory = 15)
        {
            _maxHistory = maxHistory;
            _history = new List<T>(_maxHistory);
        }

        public bool TryGetPrevious(out T result)
        {
            if (_currentIndex <= 0)
            {
                result = null;
                return false;
            }

            var selected = Current;
            while (_currentIndex > 0 && _history[--_currentIndex].Equals(selected))
            {
                _history.RemoveAt(_currentIndex);
            }

            result = Current;
            return true;
        }

        public bool TryGetNext(out T result)
        {
            if (_currentIndex == _history.Count - 1)
            {
                result = null;
                return false;
            }

            var selected = Current;
            while (_currentIndex < _history.Count - 1 && _history[++_currentIndex].Equals(selected))
            {
                _history.RemoveAt(_currentIndex--);
            }

            result = Current;
            return true;
        }

        public void Add(T record)
        {
            if (record == null || record.Equals(Current))
                return;

            if (_history.Count == _maxHistory)
            {
                _history.RemoveAt(0);
                _currentIndex--;
            }

            _currentIndex++;
            _history.RemoveRange(_currentIndex, _history.Count - _currentIndex);

            _history.Add(record);
        }
    }
}

#endif