using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Runtime {
    public class CompilerItem {
        public string[] Identity { get; set; }
        public string Name { get; set; }
    }
    public interface IFactory<T> {
        T GetInstance(string key);
    }

    public class Compiler<T> {

        public string[] ReferencedAssemblies { get; set; }
        public CompilerItem[] CompilerItems { get; set; }
        public string ErrorMessage { get; set; }

        public IFactory<T> GetFactory(bool throwErrorWhildNotFound = true) {
            if (CompilerItems.Length == 0) return null;
            string typeName = typeof(T).ToString();
            System.CodeDom.Compiler.CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");
            System.CodeDom.Compiler.CompilerParameters cp = new System.CodeDom.Compiler.CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.TempFiles = new System.CodeDom.Compiler.TempFileCollection(Xy.AppSetting.LogDir);
            //cp.OutputAssembly = Xy.AppSetting.LogDir + "Xy.Tools.RuntimeComplier.dll";
            for (int i = 0; i < ReferencedAssemblies.Length; i++) {
                cp.ReferencedAssemblies.Add(ReferencedAssemblies[i]);
            }
            cp.ReferencedAssemblies.Add(Xy.AppSetting.BinDir + "Xy.Tools.dll");            

            StringBuilder code = new StringBuilder();
            code.AppendLine("namespace Xy.Tools.Runtime {");
            code.AppendLine("public class Factory : Xy.Tools.Runtime.IFactory<" + typeName + "> {");
            code.AppendLine("public " + typeName + " GetInstance(string key) {");
            code.AppendLine("switch (key) {");
            for (int i = 0; i < CompilerItems.Length; i++) {
                for (int j = 0; j < CompilerItems[i].Identity.Length; j++) {
                    code.AppendLine("case \"" + CompilerItems[i].Identity[j] + "\":");
                }
                code.AppendLine("return new " + CompilerItems[i].Name + "();");
            }
            if (throwErrorWhildNotFound) {
                code.AppendLine("default: throw new System.Exception(string.Format(\"" + ErrorMessage + "\", key));");
            } else {
                code.AppendLine("default: return null;");
            }
            code.AppendLine("}");
            code.AppendLine("} } }");
            
            System.CodeDom.Compiler.CompilerResults cr = provider.CompileAssemblyFromSource(cp, code.ToString());
            if (cr.Errors.HasErrors) {
                Exception ex = null;
                foreach (System.CodeDom.Compiler.CompilerError _error in cr.Errors) {
                    string[] codes = code.ToString().Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
                    if (_error.Line > 0) {
                        ex = new Exception(string.Format("error:\"{0}\" on \"{1}\"", _error.ErrorText, codes[_error.Line - 1]), ex);
                    } else {
                        ex = new Exception(string.Format("error:\"{0}\"", _error.ErrorText), ex);
                    }
                }
                throw ex;
            }
            System.Reflection.Assembly assembly = cr.CompiledAssembly;
            return (IFactory<T>)assembly.CreateInstance("Xy.Tools.Runtime.Factory");
        }
    }
}
