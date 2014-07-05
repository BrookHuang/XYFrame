using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.DataSetting {
    public sealed class DataSettingCollection {
        private static Dictionary<string, DataSettingItem> _connections = null;

        static DataSettingCollection() {
            if (_connections == null) {
                Init();
            }
        }

        private static void Init() {
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            _document.Load(Xy.Tools.IO.File.foundConfigurationFile("App", Xy.AppSetting.FILE_EXT));
            _connections = new Dictionary<string, DataSettingItem>();
            foreach (System.Xml.XmlNode _item in _document.SelectNodes("AppSetting/ConnectionStrings/Item")) {
                DataSettingItem _dbi = new DataSettingItem(
                    _item.Attributes["Name"].Value,
                    _item.Attributes["ConnectionString"].Value,
                    _item.Attributes["Provider"].Value,
                    _item.Attributes["TimeOut"] == null ? 30 : Convert.ToInt32(_item.Attributes["TimeOut"].Value));
                _connections.Add(_dbi.Name, _dbi);
            }
        }

        public static DataSettingItem GetDataBaseItem(string Name) {
            return _connections[Name];
        }

        public const string DEFAULTCONNECTIONNAME = "Default"; 

        public static void Clear() {
            Init();
        }
    }

    public sealed class DataSettingItem {
        public DataSettingItem(string name, string connectionString, string dataProvider, int timeout) {
            _name = name;
            _connectionString = connectionString;
            _dataProvider = dataProvider;
            Data.Runtime.DataProvideLibrary.Add(dataProvider);
            _timeout = timeout;
        }
        private string _name;
        public string Name { get { return _name; } }

        private string _connectionString;
        public string ConnectionString { get { return _connectionString; } }

        private string _dataProvider;
        public string DataProvider { get { return _dataProvider; } }

        private int _timeout;
        public int Timeout { get { return _timeout; } }
    }
}
