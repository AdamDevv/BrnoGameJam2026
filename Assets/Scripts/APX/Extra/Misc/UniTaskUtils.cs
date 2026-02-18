using System;
using APX.Extra.Signals;
using Cysharp.Threading.Tasks;

namespace APX.Extra.Misc
{
    public static class UniTaskUtils
    {
        public static UniTask WaitForCallback(Action<Action> callback)
        {
            var source = new UniTaskCompletionSource();
            callback.Invoke(() => source.TrySetResult());
            return source.Task;
        }

        public static UniTask<T> WaitForCallback<T>(Action<Action<T>> callback)
        {
            var source = new UniTaskCompletionSource<T>();
            callback.Invoke(r => source.TrySetResult(r));
            return source.Task;
        }

        public static UniTask<(T, T2)> WaitForCallback<T, T2>(Action<Action<T, T2>> callback)
        {
            var source = new UniTaskCompletionSource<(T, T2)>();
            callback.Invoke((err, r) => source.TrySetResult(new ValueTuple<T, T2>(err, r))); 
            return source.Task;
        }

        public static UniTask Await(this Signal signal)
        {
            var source = new UniTaskCompletionSource();
            signal.AddListener(OnSignal);
            return source.Task;
            
            void OnSignal() { signal.RemoveListener(OnSignal); source.TrySetResult(); }
        }
        
        public static UniTask<T> Await<T>(this Signal<T> signal)
        {
            var source = new UniTaskCompletionSource<T>();
            signal.AddListener(OnSignal);
            return source.Task;
            
            void OnSignal(T value) { signal.RemoveListener(OnSignal); source.TrySetResult(value); }
        }
        
        public static UniTask<(T, T2)> Await<T, T2>(this Signal<T, T2> signal)
        {
            var source = new UniTaskCompletionSource<(T, T2)>();
            signal.AddListener(OnSignal);
            return source.Task;
            
            void OnSignal(T value, T2 value2) { signal.RemoveListener(OnSignal); source.TrySetResult(new ValueTuple<T, T2>(value, value2)); }
        }
        
        public static UniTask<(T, T2, T3)> Await<T, T2, T3>(this Signal<T, T2, T3> signal)
        {
            var source = new UniTaskCompletionSource<(T, T2, T3)>();
            signal.AddListener(OnSignal);
            return source.Task;
            
            void OnSignal(T value, T2 value2, T3 value3) { signal.RemoveListener(OnSignal); source.TrySetResult(new ValueTuple<T, T2, T3>(value, value2, value3)); }
        }
        
        public static UniTask<(T, T2, T3, T4)> Await<T, T2, T3, T4>(this Signal<T, T2, T3, T4> signal)
        {
            var source = new UniTaskCompletionSource<(T, T2, T3, T4)>();
            signal.AddListener(OnSignal);
            return source.Task;
            
            void OnSignal(T value, T2 value2, T3 value3, T4 value4) { signal.RemoveListener(OnSignal); source.TrySetResult(new ValueTuple<T, T2, T3, T4>(value, value2, value3, value4)); }
        }
    }
}