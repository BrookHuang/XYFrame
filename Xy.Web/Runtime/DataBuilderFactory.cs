using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Runtime.Web {
    public class DataBuilderFactory {
        private static System.Collections.Specialized.NameValueCollection _dataBuilders;
        private static List<string> _assemblies;
        private static Xy.Tools.Runtime.IFactory<Xy.Data.IDataBuilder> _factory;

        static DataBuilderFactory() {
            _dataBuilders = new System.Collections.Specialized.NameValueCollection();
            _assemblies = new List<string>();
            _assemblies.Add(Xy.AppSetting.BinDir + "Xy.Web.dll");
            LoadControl(ControlFactory.GetConfig("Data"));
        }

        private static void LoadControl(System.Xml.XmlNodeList _config) {
            if (_config == null) return;
            foreach (System.Xml.XmlNode _item in _config) {
                string _name = _item.Attributes["Name"].InnerText;
                string _assembly = _item.Attributes["Type"].InnerText;
                if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_assembly)) throw new System.Xml.XmlException("Control format error");
                Add(_name, _assembly);
            }
        }

        private static void Add(string controlName, string classFullName) {
            Add(controlName, classFullName.Substring(0, classFullName.IndexOf(',')), classFullName);
        }

        private static void Add(string controlName, string assembly, string classFullName) {
            assembly = Xy.AppSetting.BinDir + assembly + ".dll";
            if (!_assemblies.Contains(assembly)) _assemblies.Add(assembly);
            switch (controlName) {
                case "Procedure":
                case "Data":
                case "Request":
                case "XML":
                    throw new Exception("\"Procedure\", \"Data\", \"Request\", \"XML\" are XYFrame default data builder " + controlName);
            }
            if (string.IsNullOrEmpty(_dataBuilders[controlName]))
                _dataBuilders.Add(controlName, classFullName);
            else
                throw new Exception("already has a data builder named " + controlName);
        }

        private static Tools.Runtime.IFactory<Xy.Data.IDataBuilder> builderFactory() {
            Xy.Tools.Runtime.Compiler<Xy.Data.IDataBuilder> _compiler = new Tools.Runtime.Compiler<Xy.Data.IDataBuilder>();
            _compiler.ReferencedAssemblies = _assemblies.ToArray();
            _compiler.ErrorMessage = "Can not found data builder with key: {0}";
            _compiler.CompilerItems = new Tools.Runtime.CompilerItem[_dataBuilders.Count];
            for (int i = 0; i < _dataBuilders.Count; i++) {
                _compiler.CompilerItems[i] = new Tools.Runtime.CompilerItem() {
                    Identity = new string[] { _dataBuilders.Keys[i] },
                    Name = _dataBuilders[i].Substring(_dataBuilders[i].IndexOf(',') + 1)
                };
            }
            return _compiler.GetFactory(false);
        }

        public static Xy.Data.IDataBuilder Get(string className) {
            if (_factory == null) _factory = builderFactory();
            return _factory.GetInstance(className);
        }
    }
}
