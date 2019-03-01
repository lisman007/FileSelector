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
		private System.IO.FileSystemWatcher _watcher = new System.IO.FileSystemWatcher();

		private string _path;
		private bool _isExpanded = false;
		private bool _isSelected = false;
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
					int numberOfBackslashes = _path.Replace("\\", "").Length;
					int lastIndexOfBackSlash = _path.LastIndexOf("\\");

					if (numberOfBackslashes > 2)
						name = _path.Substring(lastIndexOfBackSlash + 1);
					else
						name = _path.Substring(0, lastIndexOfBackSlash);
				}

				return name;
			}
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
				OnPropertyChanged("Name");
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

				if (_isExpanded == true)
				{
					foreach (BaseItem item in Items)
						item.LoadItemsContent(false);

					_watcher.Path = Path;

					if (!string.IsNullOrEmpty(Path))
						_watcher.EnableRaisingEvents = true;
				}
				else
				{
					_watcher.EnableRaisingEvents = false;
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}

			set
			{
				_isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}

		public ObservableCollection<BaseItem> Items
		{
			get
			{
				return _items;
			}

			private set
			{
				_items = value;
				OnPropertyChanged("Items");
			}
		}

		public ImageSource ItemIcon
		{
			get
			{
				ImageSource icon = Utils.SystemIconManager.GetIcon(_path, true);
				return icon;
			}
		}
		#endregion

		#region Collection
		public void Add(BaseItem item)
		{
			if (!Contains(item))
			{
				_items.Add(item);
				OnCollectionChanged(NotifyCollectionChangedAction.Add);
				OnPropertyChanged("Items");
			}
		}

		public void Remove(BaseItem item)
		{
			if (_items.Contains(item))
			{
				_items.Remove(item);
				OnCollectionChanged(NotifyCollectionChangedAction.Remove);
				OnPropertyChanged("Items");
			}
		}
		
		public BaseItem GetChildItem(string itemName)
		{
			if (Items.Count == 0)
				LoadItemsContent(false);

			foreach(BaseItem item in Items)
			{
				if (item.Name == itemName)
					return item;
			}

			return null;
		}

		public bool Contains(BaseItem searchItem)
		{
			if (searchItem == null)
				return false;

			foreach (BaseItem item in Items)
			{
				if ((searchItem.Name == item.Name) && (searchItem.Path == item.Path))
					return true;
			}

			return false;
		}

		public BaseItem FindFullPath(string path)
		{
			List<string> pathItems = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			BaseItem foundItem = Find(pathItems);
			if (foundItem != null)
				foundItem.IsSelected = true;

			return foundItem;
		}
		#endregion

		#region CTORs
		public BaseItem(string path) : this(path, true)
		{

		}

		private BaseItem(string path, bool getNextLevel = false)
		{
			Path = path;
			Items = new AsyncObservableCollection<BaseItem>();

			if (!string.IsNullOrEmpty(Path) && getNextLevel)
				LoadItemsContent(false);

			_watcher.NotifyFilter = System.IO.NotifyFilters.LastAccess | System.IO.NotifyFilters.LastWrite | System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName;
			_watcher.EnableRaisingEvents = false;

			_watcher.Created += _watcher_Created;
			_watcher.Changed += _watcher_Changed;
			_watcher.Renamed += _watcher_Renamed;
			_watcher.Deleted += _watcher_Deleted;
		}
		#endregion

		#region Private methods
		private void LoadItemsContent(bool getNextLevel = true)
		{
			string[] directories = null;

			try
			{
				directories = System.IO.Directory.GetDirectories(Path);
			}
			catch(System.IO.IOException ioe)
			{

			}
			catch(System.UnauthorizedAccessException uae)
			{

			}

			if (directories != null)
			{
				foreach (string directory in directories)
				{
					BaseItem directoryItem = new BaseItem(directory, getNextLevel);

					//if (!Items.Contains(directoryItem))
					Add(directoryItem);
				}
			}
		}

		private BaseItem Find(List<string> pathItems)
		{
			if (pathItems.Count >= 1)
			{
				BaseItem item = GetChildItem(pathItems[0]);
				if (item != null)
				{
					IsExpanded = true;  //Expand this, which is the item's parent

					if (pathItems.Count == 1)
						return item;

					pathItems.RemoveAt(0);
					return item.Find(pathItems);
				}
			}

			return null;
		}
		#endregion

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

		#region Events
		private void _watcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
			BaseItem item = GetChildItem(e.Name);
			if (item != null)
				Remove(item);
		}

		private void _watcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
			BaseItem item = GetChildItem(e.OldName);
			if (item != null)
				item.Path = e.FullPath;
		}

		private void _watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
			BaseItem item = GetChildItem(e.Name);
			if (item != null)
				item.LoadItemsContent(false);
		}

		private void _watcher_Created(object sender, System.IO.FileSystemEventArgs e)
		{
			BaseItem item = new BaseItem(e.FullPath);
			Add(item);
		}
		#endregion

		#region General overrides
		public override string ToString()
		{
			return Name + " (" + Path + ")";
		}
		#endregion
	}
}