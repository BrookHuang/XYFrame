using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Runtime.Web {

    public class ControlFactory {
        private static System.Collections.Specialized.NameValueCollection _controls;
        private static List<string> _assemblies;
        private static Xy.Tools.Runtime.IFactory<Xy.Web.Control.IControl> _factory;
        private static Dictionary<string, System.Xml.XmlNodeList> _config;

        static ControlFactory() {
            _controls = new System.Collections.Specialized.NameValueCollection();
            _assemblies = new List<string>();
            _config = new Dictionary<string, System.Xml.XmlNodeList>();
            LoadControl();
        }

        private static void LoadControl() {
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            _document.Load(Xy.Tools.IO.File.foundConfigurationFile("App", Xy.AppSetting.FILE_EXT));
            foreach (System.Xml.XmlNode _item in _document.SelectNodes("AppSetting/Controls/Control")) {
                string _name = _item.Attributes["Name"].InnerText;
                string _assembly = _item.Attributes["Type"].InnerText;
                if (string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_assembly)) throw new System.Xml.XmlException("Control format error");
                Add(_name, _assembly);
                if (_item.HasChildNodes) {
                    _config.Add(_name, _item.ChildNodes);
                }
            }
        }


        private static void Add(string controlName, string classFullName) {
            Add(controlName, classFullName.Substring(0, classFullName.IndexOf(',')), classFullName);
        }

        private static void Add(string controlName, string assembly, string classFullName) {
            assembly = Xy.AppSetting.BinDir + assembly + ".dll";
            if (!_assemblies.Contains(assembly)) _assemblies.Add(assembly);
            if (string.IsNullOrEmpty(_controls[controlName]))
                _controls.Add(controlName, classFullName);
            else
                throw new Exception("already has a control named " + controlName);
        }

        private static Tools.Runtime.IFactory<Xy.Web.Control.IControl> builderFactory() {
            Xy.Tools.Runtime.Compiler<Xy.Web.Control.IControl> _compiler = new Tools.Runtime.Compiler<Xy.Web.Control.IControl>();
            _compiler.ReferencedAssemblies = _assemblies.ToArray();
            _compiler.ErrorMessage = "Can not found control with key: {0}";
            _compiler.CompilerItems = new Tools.Runtime.CompilerItem[_controls.Count];
            for (int i = 0; i < _controls.Count; i++) {
                _compiler.CompilerItems[i] = new Tools.Runtime.CompilerItem() {
                    Identity = new string[] { "@" + _controls.Keys[i] },
                    Name = _controls[i].Substring(_controls[i].IndexOf(',') + 1)
                };
            }
            return _compiler.GetFactory();
        }

        public static Xy.Web.Control.IControl Get(string className) {
            if (_factory == null) _factory = builderFactory();
            return _factory.GetInstance(className);
        }

        public static System.Xml.XmlNodeList GetConfig(string className) {
            if (_config.ContainsKey(className)) return _config[className];
            return null;
        }
    }
}
