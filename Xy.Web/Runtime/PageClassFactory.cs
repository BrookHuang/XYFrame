using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Runtime.Web {

    public class PageClassLibrary {
        private static List<string> _assemblies;
        private static List<string> _classFullNames;

        static PageClassLibrary() {
            _assemblies = new List<string>();
            _classFullNames = new List<string>();
        }

        public static void Add(string classFullName) {
            Add(classFullName.Substring(0, classFullName.IndexOf(',')), classFullName);
        }

        public static void Add(string assembly, string classFullName) {
            assembly = Xy.AppSetting.BinDir + assembly + ".dll";
            if (!_assemblies.Contains(assembly)) _assemblies.Add(assembly);
            if (!_classFullNames.Contains(classFullName)) _classFullNames.Add(classFullName);
        }

        private static Xy.Tools.Runtime.IFactory<Xy.Web.Page.PageAbstract> _factory;
        public static Xy.Web.Page.PageAbstract Get(string className) {
            if (_factory == null) _factory = builderFactory();
            return _factory.GetInstance(className);
        }

        private static Tools.Runtime.IFactory<Xy.Web.Page.PageAbstract> builderFactory() {
            Xy.Tools.Runtime.Compiler<Xy.Web.Page.PageAbstract> _compiler = new Tools.Runtime.Compiler<Xy.Web.Page.PageAbstract>();
            _compiler.ReferencedAssemblies = _assemblies.ToArray();
            _compiler.ErrorMessage = "Can not found page class with key: {0}";
            _compiler.CompilerItems = new Tools.Runtime.CompilerItem[_classFullNames.Count];
            for (int i = 0; i < _classFullNames.Count; i++) {
                _compiler.CompilerItems[i] = new Tools.Runtime.CompilerItem() { 
                    Identity = new string[] { _classFullNames[i], _classFullNames[i].Substring(_classFullNames[i].IndexOf(',') + 1) },
                    Name = _classFullNames[i].Substring(_classFullNames[i].IndexOf(',') + 1)
                };
            }
            return _compiler.GetFactory();
        }
    }
}
