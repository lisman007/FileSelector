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
		public FolderSelector()
		{
			InitializeComponent();

			DataContext = new ItemsManager().Root;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
