using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FileSelector.Data
{ 
	public class BaseItem : INotifyPropertyChanged, INotifyCollectionChanged
	{
		#region Data members
		private string _path;
		private bool _isExpanded = false;
		private ObservableCollection<BaseItem> _items;
		#endregion

		#region Properties
		public string Name
		{
			get
			{
				string name = "";
				if (!string.IsNullOrEmpty(_path))
				{
					int lastIndexOfBackSlash = _path.LastIndexOf(@"\");
					name = _path.Substring(lastIndexOfBackSlash + 1);
				}

				return name;
			}

// 			set
// 			{
// 				_name = value;
// 				OnPropertyChanged("Name");
// 			}
		}

		public string Path
		{
			get
			{
				return _path;
			}

			set
			{
				_path = value;
				OnPropertyChanged("Path");
			}
		}

		public bool IsExpanded
		{
			get
			{
				return _isExpanded;
			}
			set
			{
				_isExpanded = value;
				OnPropertyChanged("IsExpanded");

				if ((_isExpanded == true) && (_items.Count == 0))
					LoadItemsContent();

			}
		}

		public ObservableCollection<BaseItem> Items
		{
			get
			{
				return _items;
			}

			set
			{
				_items = value;
				OnPropertyChanged("Items");
			}
		}
		#endregion

		#region Collection
		public void Add(BaseItem item)
		{
			if (!_items.Contains(item))
			{
				_items.Add(item);
				OnCollectionChanged(NotifyCollectionChangedAction.Add);
				OnPropertyChanged("Items");
			}
		}
		#endregion

		public ImageSource ItemIcon
		{
			get
			{
				ImageSource icon = Utils.SystemIconManager.GetIcon(_path, true);
				return icon;
			}
		}


		public BaseItem(string path)
		{
			Items = new ObservableCollection<BaseItem>();
			Path = path;
		}

		private void LoadItemsContent()
		{

		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion INotifyPropertyChanged

		#region INotifyCollectionChanged
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action)
		{
			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
		}
		#endregion INotifyCollectionChanged
	}
}