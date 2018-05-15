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

			//ObservableCollection<BaseItem> root = new ObservableCollection<BaseItem>();
			// 			BaseItem root = new BaseItem("");
			// 
			// 			BaseItem temp = new BaseItem(@"C:\Temp");
			// 			BaseItem ab = new BaseItem(@"C:\Temp\ab");
			// 			BaseItem a = new BaseItem(@"C:\Temp\ab\a");
			// 			BaseItem a1 = new BaseItem(@"C:\Temp\ab\a\a1.txt");
			// 			BaseItem a2 = new BaseItem(@"C:\Temp\ab\a\a2.xlsx");
			// 			BaseItem b = new BaseItem(@"C:\Temp\ab\b");
			// 			BaseItem b1 = new BaseItem(@"C:\Temp\ab\b\b1.txt");
			// 
			// 			temp.Add(ab);
			// 			ab.Add(a);
			// 			ab.Add(b);
			// 			a.Add(a1);
			// 			a.Add(a2);
			// 			b.Add(b1);
			// 
			// 			root.Add(temp);

			DataContext = new ItemsManager().Root;// root;
			//DataContext = root;
		}
	}
}
