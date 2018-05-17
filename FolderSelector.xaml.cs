using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using FileSelector.Data;
using System.Collections.ObjectModel;

namespace FileSelector
{
	/// <summary>
	/// Interaction logic for FolderSelector.xaml
	/// </summary>
	public partial class FolderSelector : Window
	{
		#region Properties
		public string SelectedPath
		{
			get;
			private set;
		}

		public string InitialPath
		{
			set
			{
				string initialPath = value;
				BaseItem foundItem = (DataContext as BaseItem).Find(initialPath);
			}
		}

		public bool ShowNewFolderButton
		{
			get
			{
				return btnNewFolder.IsVisible;
			}
			set
			{
				if (value == false)
					btnNewFolder.Visibility = Visibility.Hidden;
				else
					btnNewFolder.Visibility = Visibility.Visible;
			}
		}
		#endregion

		public FolderSelector()
		{
			InitializeComponent();

			DataContext = new ItemsManager().Root;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			SelectedPath = tbSelectedFolder.Text;
			Close();
		}

		private void tvFolders_Selected(object sender, RoutedEventArgs e)
		{
			TreeViewItem tvi = e.OriginalSource as TreeViewItem;
			tvi.BringIntoView();
		}

		
	}
}
