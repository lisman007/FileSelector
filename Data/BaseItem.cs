﻿using System;
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
					int numberOfBackslashes = _path.Replace("\\", "").Length;
					int lastIndexOfBackSlash = _path.LastIndexOf("\\");

					if (numberOfBackslashes > 2)
						name = _path.Substring(lastIndexOfBackSlash + 1);
					else
						name = _path.Substring(0, lastIndexOfBackSlash - 1);
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
				}
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


		public BaseItem(string path) : this(path, true)
		{
			
		}

		private BaseItem(string path, bool getNextLevel = false)
		{
			Path = path;
			Items = new ObservableCollection<BaseItem>();

			if (!string.IsNullOrEmpty(Path) && getNextLevel)
				LoadItemsContent(false);
		}

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
					Items.Add(directoryItem);
				}
			}
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