using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Data.Runtime {
    public class DataProvideLibrary {
        internal static List<string> Assemblies;
        internal static List<string> ClassFullNames;

        static DataProvideLibrary() {
            Assemblies = new List<string>();
            ClassFullNames = new List<string>();
        }

        public static void Add(string classFullName) {
            Add(classFullName.Substring(0, classFullName.IndexOf(',')), classFullName);
        }

        public static void Add(string assembly, string classFullName) {
            assembly = Xy.AppSetting.BinDir + assembly + ".dll";
            if (!Assemblies.Contains(assembly)) Assemblies.Add(assembly);
            if (!ClassFullNames.Contains(classFullName)) ClassFullNames.Add(classFullName);
        }

        private static Xy.Tools.Runtime.IFactory<Xy.Data.IDataBase> _factory;
        public static Xy.Data.IDataBase Get(string className) {
            if (_factory == null) _factory = builderFactory();
            return _factory.GetInstance(className);
        }

        private static Tools.Runtime.IFactory<Xy.Data.IDataBase> builderFactory() {
            Xy.Tools.Runtime.Compiler<Xy.Data.IDataBase> _compiler = new Tools.Runtime.Compiler<Xy.Data.IDataBase>();
            _compiler.ReferencedAssemblies = Assemblies.ToArray();
            _compiler.ErrorMessage = "Can not found data provider class with key: {0}";
            _compiler.CompilerItems = new Tools.Runtime.CompilerItem[ClassFullNames.Count];
            for (int i = 0; i < ClassFullNames.Count; i++) {
                _compiler.CompilerItems[i] = new Tools.Runtime.CompilerItem() { 
                    Identity = new string[] { ClassFullNames[i], ClassFullNames[i].Substring(ClassFullNames[i].IndexOf(',') + 1) },
                    Name = ClassFullNames[i].Substring(ClassFullNames[i].IndexOf(',') + 1)
                };
            }
            return _compiler.GetFactory();
        }
    }
}
