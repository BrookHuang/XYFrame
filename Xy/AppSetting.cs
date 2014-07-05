using System;
using System.Collections.Generic;
using System.Text;

namespace Xy {
    public class AppSetting {
        public const string DEFAULTENCRYPTKEY = "THISISXYFRAMEENCRYPTKEY";
        public const string DEFAULTENCRYPTIV = "VITPYRCNEEMARYXSISIHT";
        public const string DEFAULTSESSIONID = "XyFrameSessionID";

        private static string _binDir;
        public static string BinDir { get { return _binDir; } }

        private static string _appDir;
        public static string AppDir { get { return _appDir; } }

        private static string _themeDir;
        public static string ThemeDir { get { return _themeDir; } }

        private static string _logDir;
        public static string LogDir { get { return _logDir; } }

        private static string _dataDir;
        public static string DataDir { get { return _dataDir; } }

        private static string _cacheDir;
        public static string CacheDir { get { return _cacheDir; } }

        public static readonly string[] FILE_EXT = new string[2] { "xml", "aspx" };
        public static readonly byte[] START_MARK = new byte[2] { (byte)'<', (byte)'%' };
        public static readonly byte[] END_MARK = new byte[2] { (byte)'%', (byte)'>' };
        public static readonly byte[] INNER_START_MARK = new byte[2] { (byte)'[', (byte)'%' };
        public static readonly byte[] INNER_END_MARK = new byte[2] { (byte)'%', (byte)']' };


        public const string APP_PATH = "Xy_App\\";
        public const string PAGE_PATH = "Xy_Page\\";
        public const string THEME_PATH = "Xy_Theme\\";
        public const string XSLT_PATH = "Xy_Xslt\\";
        public const string CSS_PATH = "Xy_Css\\";
        public const string SCRIPT_PATH = "Xy_Script\\";
        public const string INCLUDE_PATH = "Xy_Include\\";
        public const string CACHE_PATH = "Xy_Cache\\";
        public const string Log_Path = "Xy_Log\\";
        public const string DATA_PATH = "Xy_Data\\";

        static AppSetting() {
            string BaseDirectoryPath = System.AppDomain.CurrentDomain.BaseDirectory;
            _appDir = Xy.Tools.IO.File.ifNotExistsThenCreate(BaseDirectoryPath + APP_PATH);
            _themeDir = Xy.Tools.IO.File.ifNotExistsThenCreate(BaseDirectoryPath + THEME_PATH);
            _logDir = Xy.Tools.IO.File.ifNotExistsThenCreate(BaseDirectoryPath + Log_Path);
            _dataDir = Xy.Tools.IO.File.ifNotExistsThenCreate(BaseDirectoryPath + DATA_PATH);
            _cacheDir = Xy.Tools.IO.File.ifNotExistsThenCreate(BaseDirectoryPath + CACHE_PATH);
            if (System.AppDomain.CurrentDomain.FriendlyName.IndexOf("W3SVC") >= 0) {
                _binDir = BaseDirectoryPath + "Bin\\";
            } else {
                _binDir = BaseDirectoryPath;
            }
            
        }
    }
}
