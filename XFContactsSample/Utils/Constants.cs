using System;
using System.Collections.Generic;
using System.IO;

namespace XFContactsSample.Utils
{
    public static class Constants
    {
        public const string DatabaseFilename = "ContactsSample.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;


        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        public static readonly List<string> MobileTypes = new List<string>
        {
            "NoLabel",
            "Mobile",
            "Work",
            "Home",
            "Main",
            "WorkFax",
            "HomeFax",
            "Pager",
            "Other"
        };

        public static readonly List<string> AddressTypes = new List<string>
        {
            "NoLabel",
            "Home",
            "Work",
            "Other"
        };

        public static readonly List<string> AIMTypes = new List<string>
        {
            "NoLabel",
            "AIM",
            "WindowsLive",
            "Yahoo",
            "Skype",
            "QQ",
            "Hangouts",
            "ICQ",
            "Jabber"
        };

        public static readonly List<string> DateTypes = new List<string>
        {
            "NoLabel",
            "Birthday",
            "Anniversary",
            "Other"
        };

        public static readonly List<string> RelationshipTypes = new List<string>
        {
            "Assistant",
            "Brother",
            "Child",
            "Partner",
            "Father",
            "Friend",
            "Supervisor",
            "Mother",
            "Parent",
            "WorkAssociate",
            "Referrer",
            "Sister",
            "Spouse"
        };
    }
}
