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
				//driveItem.Items.Add(new BaseItem(@"C:\Temp"));
				Root.Add(driveItem);
			}

			/*
			BaseItem temp = new BaseItem(@"C:\Temp");
			BaseItem ab = new BaseItem(@"C:\Temp\ab");
			BaseItem a = new BaseItem(@"C:\Temp\ab\a");
			BaseItem a1 = new BaseItem(@"C:\Temp\ab\a\a1.txt");
			BaseItem a2 = new BaseItem(@"C:\Temp\ab\a\a2.xlsx");
			BaseItem b = new BaseItem(@"C:\Temp\ab\b");
			BaseItem b1 = new BaseItem(@"C:\Temp\ab\b\b1.txt");

			temp.Add(ab);
			ab.Add(a);
			ab.Add(b);
			a.Add(a1);
			a.Add(a2);
			b.Add(b1);

			Root.Add(temp);*/
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
