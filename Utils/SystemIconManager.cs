﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileSelector.Utils
{
	sealed class SystemIconManager
	{
		public static ImageSource GetIcon(string path, bool smallIcon)
		{
			bool isFileOrDirectoryValid = false;
			bool isDirectory = false;

			if (System.IO.File.Exists(path))
				isFileOrDirectoryValid = true;
			else
			{
				if (System.IO.Directory.Exists(path))
				{
					isFileOrDirectoryValid = true;
					isDirectory = true;
				}
			}

			if (!isFileOrDirectoryValid)
				return null;

			uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;
			if (smallIcon)
				flags |= SHGFI_SMALLICON;

			uint attributes = FILE_ATTRIBUTE_NORMAL;
			if (isDirectory)
				attributes |= FILE_ATTRIBUTE_DIRECTORY;

			SHFILEINFO shfi;
			if (0 != SHGetFileInfo(path, attributes, out shfi, (uint)Marshal.SizeOf(typeof(SHFILEINFO)), flags))
			{
				return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(shfi.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			return null;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		[DllImport("shell32")]
		private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

		private const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
		private const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
		private const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		private const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
		private const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
		private const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
		private const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
		private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
		private const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
		private const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
		private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
		private const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
		private const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;

		private const uint SHGFI_ICON = 0x000000100;
		private const uint SHGFI_DISPLAYNAME = 0x000000200;
		private const uint SHGFI_TYPENAME = 0x000000400;
		private const uint SHGFI_ATTRIBUTES = 0x000000800;
		private const uint SHGFI_ICONLOCATION = 0x000001000;
		private const uint SHGFI_EXETYPE = 0x000002000;
		private const uint SHGFI_SYSICONINDEX = 0x000004000;
		private const uint SHGFI_LINKOVERLAY = 0x000008000;
		private const uint SHGFI_SELECTED = 0x000010000;
		private const uint SHGFI_ATTR_SPECIFIED = 0x000020000;
		private const uint SHGFI_LARGEICON = 0x000000000;
		private const uint SHGFI_SMALLICON = 0x000000001;
		private const uint SHGFI_OPENICON = 0x000000002;
		private const uint SHGFI_SHELLICONSIZE = 0x000000004;
		private const uint SHGFI_PIDL = 0x000000008;
		private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
	}
}



