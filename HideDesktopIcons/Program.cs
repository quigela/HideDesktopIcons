using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace HideDesktopIcons
{
    class Program
    {
        static void Main(string[] args)
        {
            string userDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ToggleItems(Directory.GetFiles(userDesktop));
            ToggleItems(Directory.GetDirectories(userDesktop));

            string publicDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            ToggleItems(Directory.GetFiles(publicDesktop));
            ToggleItems(Directory.GetDirectories(publicDesktop));
        }

        public static void ToggleItems(string[] items)
        {
            string[] BlockList = LoadBlocklist();
            foreach (string s in items)
            {
                if (OnBlocklist(s, BlockList))
                {
                    continue;
                }
                FileAttributes a = File.GetAttributes(s);
                bool isHidden = (a & FileAttributes.Hidden) == FileAttributes.Hidden;

                if (isHidden)
                {
                    FileInfo fi = new FileInfo(s);
                    fi.Attributes = FileAttributes.Normal;
                }
                else
                {
                    FileInfo fi = new FileInfo(s);
                    fi.Attributes = FileAttributes.Hidden;
                }

            }
        }

        private static bool OnBlocklist(string s, string[] BlockList)
        {
            bool fexists = Array.Exists(
                BlockList,
                delegate (string _) { return _.Equals(Path.GetFileName(s), StringComparison.InvariantCultureIgnoreCase); }
            );

            bool dexists = Array.Exists(
                BlockList,
                delegate (string _) { return _.Equals(Path.GetDirectoryName(s), StringComparison.InvariantCultureIgnoreCase); }
            );

            if (fexists || dexists)
            {
                return true;
            }
            return false;
        }

        private static string[] LoadBlocklist()
        {
            if (File.Exists("blocklist.txt"))
            {
                return File.ReadAllLines("blocklist.txt");
            }
            else
            {
                // return default list of only desktop.ini
                return new string[] { "desktop.ini", "Thumbs.db" };
            }
        }
    }
}
