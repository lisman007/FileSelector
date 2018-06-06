using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelector.Data
{
	class ItemsManager : INotifyPropertyChanged
	{
		private BaseItem _root;
		private BaseItem _selectedItem;

		public BaseItem Root
		{
			get
			{
				return _root;
			}

			set
			{
				_root = value;
				OnPropertyChanged("Root");
			}
		}

		public BaseItem SelectedItem
		{
			get
			{
				return _selectedItem;
			}

			set
			{
				_selectedItem = value;
				OnPropertyChanged("SelectedItem");
			}
		}

		public ItemsManager()
		{
			Root = new BaseItem("");
			DriveInfo[] allDrives = DriveInfo.GetDrives();

			foreach (DriveInfo drive in allDrives)
			{
				BaseItem driveItem = new BaseItem(drive.RootDirectory.FullName);
				Root.Add(driveItem);
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
	}
}
