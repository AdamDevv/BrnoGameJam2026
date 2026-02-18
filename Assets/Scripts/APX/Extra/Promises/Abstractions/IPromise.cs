using System;

namespace APX.Extra.Promises.Abstractions
{
	public interface IBasePromise
	{
		/// <summary>
		/// Handle a callback as the Promise updates.
		/// </summary>
		/// <param name="listener">The callback.</param>
		IBasePromise Progress(Action<float> listener);
		/// <summary>
		/// Handle a callback if the Promise fails to deliver.
		/// </summary>
		/// The callback must handle an Exception
		/// <param name="listener">The callback.</param>
		IBasePromise Fail(Action<Exception> listener);
		/// <summary>
		/// Trigger a callback after the Promise has concluded its business.
		/// </summary>
		/// <param name="listener">The callback.</param>
		IBasePromise Finally(Action listener);

		/// <summary>
		/// Report an Exception thrown in the course of attempting to fulfill the Promise.
		/// </summary>
		/// If there's an OnFail callback, fire it.
		/// <param name="ex">The exception.</param>
		void ReportFail(Exception ex);
		/// <summary>
		/// Report progress in the course of attempting to fulfill the Promise.
		/// </summary>
		/// <param name="progress">A value representing the percentage of completion. By convention, this 
		/// value is either a float 0-1 or a float 0-100, but the specific implementer is a matter for the
		/// designer of the Promise.</param>
		void ReportProgress(float progress);

		/// <summary>
		/// Removes all listeners.
		/// </summary>
		void RemoveAllListeners();
		/// <summary>
		/// Removes the progress listeners.
		/// </summary>
		void RemoveProgressListeners();
		/// <summary>
		/// Removes the fail listeners.
		/// </summary>
		void RemoveFailListeners();

		/// <summary>
		/// Implement in concrete class. The number of clients for this Promise.
		/// </summary>
		/// <returns>The count.</returns>
		int ListenerCount();

		BasePromise.PromiseState State { get; }
	}
	public interface IPromise : IBasePromise
	{
		IPromise Then(Action action);
		void Dispatch();
		void RemoveListener(Action action);
	}

	public interface IPromise<T> : IBasePromise
	{
		IPromise<T> Then(Action<T> action);
		void Dispatch(T t);
		void RemoveListener(Action<T> action);
	}

	public interface IPromise<T, U> : IBasePromise
	{
		IPromise<T, U> Then(Action<T, U> action);
		void Dispatch(T t, U u);
		void RemoveListener(Action<T, U> action);
	}

	public interface IPromise<T, U, V> : IBasePromise
	{
		IPromise<T, U, V> Then(Action<T, U, V> action);
		void Dispatch(T t, U u, V v);
		void RemoveListener(Action<T, U, V> action);
	}

	public interface IPromise<T, U, V, W> : IBasePromise
	{
		IPromise<T, U, V, W> Then(Action<T, U, V, W> action);
		void Dispatch(T t, U u, V v, W w);
		void RemoveListener(Action<T, U, V, W> action);
	}


}
