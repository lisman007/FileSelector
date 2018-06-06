using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace FileSelector.Data
{
	class AsyncObservableCollection<T> : ObservableCollection<T>
	{
		//https://www.thomaslevesque.com/2009/04/17/wpf-binding-to-an-asynchronous-collection/

		private SynchronizationContext _syncContext = SynchronizationContext.Current;

		#region CTORs
		public AsyncObservableCollection()
		{

		}

		public AsyncObservableCollection(IEnumerable<T> list) : base(list)
		{

		}
		#endregion

		#region Override ObservableCollection events
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (_syncContext == SynchronizationContext.Current)
			{
				//Execute the CollectionChanged event on the current thread
				RunCollectionChanged(e);
			}
			else
			{
				//Execute the CollectionChanged event on the creator thread
				_syncContext.Send(RunCollectionChanged, e);
			}
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (_syncContext == SynchronizationContext.Current)
			{
				//Execute the PropertyChanged event on the current thread
				RunPropertyChanged(e);
			}
			else
			{
				//Execute the PropertyChanged event on the creator thread
				_syncContext.Send(RunPropertyChanged, e);
			}
		}
		#endregion

		#region Event runners
		private void RunCollectionChanged(object arg)
		{
			//Creator thread: call the base implementation
			base.OnCollectionChanged((NotifyCollectionChangedEventArgs)arg);
		}

		private void RunPropertyChanged(object arg)
		{
			//Creator thread: call the base implementation
			base.OnPropertyChanged((PropertyChangedEventArgs)arg);
		}
		#endregion
	}
}
