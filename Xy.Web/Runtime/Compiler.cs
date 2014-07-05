using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Runtime.Web {
    internal class Compiler {
        private static string binFileDir = AppDomain.CurrentDomain.BaseDirectory + "Bin\\";

        internal static IPageClassFactory GetPageClassFactory() {
            System.CodeDom.Compiler.CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");
            System.CodeDom.Compiler.CompilerParameters cp = new System.CodeDom.Compiler.CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.TempFiles = new System.CodeDom.Compiler.TempFileCollection(Xy.AppSetting.LogDir);

            for (int i = 0; i < PageClassLibrary.Assemblys.Count; i++) {
                cp.ReferencedAssemblies.Add(binFileDir + PageClassLibrary.Assemblys[i] + ".dll");
            }
            
            StringBuilder code = new StringBuilder();
            code.AppendLine("namespace Xy.Runtime.Web {");
            code.AppendLine("public class PageClassFacgory : Xy.Runtime.Web.IPageClassFactory {");
            code.AppendLine("public Xy.Web.Page.PageAbstract GetPageClass(string key) {");
            code.AppendLine("switch (key) {");
            for (int i = 0; i < PageClassLibrary.ClassFullNames.Count; i++) {
                code.AppendLine(string.Format("case \"{0}\": case \"{1}\": return new {1}();", PageClassLibrary.ClassFullNames[i], PageClassLibrary.ClassFullNames[i].Split(',')[1]));
            }
            code.AppendLine("default: throw new System.Exception(\"Can not found page class with key: \" + key);");
            code.AppendLine("}");
            code.AppendLine("} } }");

            System.CodeDom.Compiler.CompilerResults cr = provider.CompileAssemblyFromSource(cp, code.ToString());

            foreach (System.CodeDom.Compiler.CompilerError _error in cr.Errors) {
                string[] codes = code.ToString().Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
                if (_error.Line > 0) {
                    throw new Exception(string.Format("error:\"{0}\" on \"{1}\"", _error.ErrorText, codes[_error.Line - 1]));
                } else {
                    throw new Exception(string.Format("error:\"{0}\"", _error.ErrorText));
                }
            }
            return cr.CompiledAssembly.CreateInstance("Xy.Runtime.Web.PageClassFacgory") as IPageClassFactory;
        }

        internal static IControlFactory GetControlFactory() {
            
            System.CodeDom.Compiler.CodeDomProvider provider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");
            System.CodeDom.Compiler.CompilerParameters cp = new System.CodeDom.Compiler.CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            cp.TempFiles = new System.CodeDom.Compiler.TempFileCollection(Xy.AppSetting.LogDir);

            for (int i = 0; i < ControlLibrary.Assemblys.Count; i++) {
                cp.ReferencedAssemblies.Add(binFileDir + ControlLibrary.Assemblys[i] + ".dll");
            }

            StringBuilder code = new StringBuilder();
            code.AppendLine("namespace Xy.Runtime.Web {");
            code.AppendLine("public class ControlFactory : Xy.Runtime.Web.IControlFactory {");
            code.AppendLine("public Xy.Web.Control.IControl GetControl(string key) {");
            code.AppendLine("switch (key) {");
            for (int i = 0; i < ControlLibrary.Controls.Count; i++) {
                code.AppendLine(string.Format("case \"@{0}\": return new {1}();", ControlLibrary.Controls.Keys[i], ControlLibrary.Controls[i].Split(',')[1]));
            }
            code.AppendLine("default: throw new System.Exception(\"Can not found control with key: \" + key);");
            code.AppendLine("}");
            code.AppendLine("} } }");

            System.CodeDom.Compiler.CompilerResults cr = provider.CompileAssemblyFromSource(cp, code.ToString());

            foreach (System.CodeDom.Compiler.CompilerError _error in cr.Errors) {
                string[] codes = code.ToString().Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);
                if (_error.Line > 0) {
                    throw new Exception(string.Format("error:\"{0}\" on \"{1}\"", _error.ErrorText, codes[_error.Line - 1]));
                } else {
                    throw new Exception(string.Format("error:\"{0}\"", _error.ErrorText));
                }
            }
            return cr.CompiledAssembly.CreateInstance("Xy.Runtime.Web.ControlFactory") as IControlFactory;
        }
    }
}
