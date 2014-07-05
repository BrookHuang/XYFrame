using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.IO {
    public partial class XML {
        private const string itemTemplte = "<{0}><![CDATA[{1}]]></{0}>";
        private static string itemTemplteWithFormat = "\t\t" + itemTemplte + Environment.NewLine;

        public static string ConvertDataTableToXML(System.Data.DataTable datatable) {
            return ConvertDataTableToXML(datatable, string.Empty);
        }
        public static string ConvertDataTableToXML(System.Data.DataTable datatable, string addstring) {
            if (datatable == null) return "<DataTable></DataTable>";
            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTable>");
            for (int i = 0; i < datatable.Rows.Count; i++) {
                sb.Append("<DataItem>");
                for (int j = 0; j < datatable.Columns.Count; j++) {
                    if (datatable.Rows[i][j] != DBNull.Value) {
                        sb.Append(string.Format(itemTemplte, datatable.Columns[j].ColumnName, datatable.Rows[i][j]));
                    }
                }
                sb.Append("</DataItem>");
            }
            if (!string.IsNullOrEmpty(addstring)) {
                sb.Append(addstring);
            }
            sb.Append("</DataTable>");
            return sb.ToString();
        }

        public static string ConvertDataTableToWebServiceResult(System.Data.DataTable Data, string FunctionName, string InstansName = null, string Xmlns = "http://tempuri.org/") {
            StringBuilder _sb = new StringBuilder();
            _sb.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><soap:Body>");
            _sb.Append(string.Format("<{0}Response xmlns=\"{1}\"><{0}Result>", FunctionName, Xmlns));
            if (!string.IsNullOrEmpty(InstansName)) _sb.Append(string.Format("<{0}>", InstansName));
            foreach (System.Data.DataRow _row in Data.Rows) {
                foreach (System.Data.DataColumn _column in Data.Columns) {
                    _sb.Append(string.Format("<{0}>{1}</{0}>", _column.ColumnName, _row[_column]));
                }
            }
            if (!string.IsNullOrEmpty(InstansName)) _sb.Append(string.Format("</{0}>", InstansName));
            _sb.Append(string.Format("</{0}Result></{0}Response>", FunctionName));
            _sb.Append("</soap:Body></soap:Envelope>");
            return _sb.ToString();
        }

        public static string ConvertDataTableToXMLWithFormat(System.Data.DataTable datatable) {
            return ConvertDataTableToXMLWithFormat(datatable, string.Empty);
        }
        public static string ConvertDataTableToXMLWithFormat(System.Data.DataTable datatable, string addstring) {
            if (datatable == null) return "<DataTable></DataTable>";
            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTable>" + Environment.NewLine);
            for (int i = 0; i < datatable.Rows.Count; i++) {
                sb.Append("\t<DataItem>" + Environment.NewLine);
                for (int j = 0; j < datatable.Columns.Count; j++) {
                    if (datatable.Rows[i][j] != DBNull.Value) {
                        sb.Append(string.Format(itemTemplteWithFormat, datatable.Columns[j].ColumnName, datatable.Rows[i][j]));
                    }
                }
                sb.Append("\t</DataItem>" + Environment.NewLine);
                if (!string.IsNullOrEmpty(addstring)) {
                    sb.Append(addstring);
                }
            }
            sb.Append("</DataTable>");
            return sb.ToString();
        }
        public static string ConvertDataTableToWebServiceResultWithFormat(System.Data.DataTable Data, string FunctionName, string InstansName = null, string Xmlns = "http://tempuri.org/") {
            StringBuilder _sb = new StringBuilder();
            _sb.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + Environment.NewLine + "\t<soap:Body>" + Environment.NewLine);
            _sb.Append(string.Format("\t\t<{0}Response xmlns=\"{1}\">" + Environment.NewLine + "\t\t\t<{0}Result>" + Environment.NewLine, FunctionName, Xmlns));
            if (!string.IsNullOrEmpty(InstansName)) _sb.Append(string.Format("\t\t\t\t<{0}>", InstansName) + Environment.NewLine);
            foreach (System.Data.DataRow _row in Data.Rows) {
                foreach (System.Data.DataColumn _column in Data.Columns) {
                    if (string.IsNullOrEmpty(InstansName)) {
                        _sb.Append(string.Format("\t\t\t\t<{0}>{1}</{0}>", _column.ColumnName, _row[_column]) + Environment.NewLine);
                    } else {
                        _sb.Append(string.Format("\t\t\t\t\t<{0}>{1}</{0}>", _column.ColumnName, _row[_column]) + Environment.NewLine);
                    }
                }
            }
            if (!string.IsNullOrEmpty(InstansName)) _sb.Append(string.Format("\t\t\t\t</{0}>", InstansName) + Environment.NewLine);
            _sb.Append(string.Format("\t\t\t</{0}Result>" + Environment.NewLine + "\t\t</{0}Response>" + Environment.NewLine, FunctionName));
            _sb.Append("\t</soap:Body>" + Environment.NewLine + "</soap:Envelope>");
            return _sb.ToString();
        }

        public static System.Data.DataTable ConvertXMLFileToDataSet(string xmlData) {
            System.Xml.XmlTextReader reader = null;
            try {
                System.Data.DataTable xmlDS = new System.Data.DataTable();
                reader = new System.Xml.XmlTextReader(new System.IO.StringReader(xmlData));
                xmlDS.ReadXml(reader);
                return xmlDS;
            } catch (System.Exception ex) {
                throw ex;
            } finally {
                if (reader != null) reader.Close();
            }
        }

        public static byte[] TransfromXmlStringToHtml(string xsltpath, string XmlString) {
            System.IO.StringReader xml = new System.IO.StringReader(XmlString);
            System.Xml.XPath.XPathDocument xmlDoc = new System.Xml.XPath.XPathDocument(xml);

            System.Xml.Xsl.XslCompiledTransform xsl = new System.Xml.Xsl.XslCompiledTransform();
            xsl.Load(xsltpath);

            return TransfromXmlStringToHtml(xsl, xmlDoc);
        }

        private static byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
        public static byte[] TransfromXmlStringToHtml(System.Xml.Xsl.XslCompiledTransform xsl, System.Xml.XPath.XPathDocument xpathDoc, System.Xml.Xsl.XsltArgumentList xpathParameter = null) {
            byte[] result;
            using (System.IO.Stream str = new System.IO.MemoryStream()) {
                xsl.Transform(xpathDoc, xpathParameter, str);
                str.Position = 0;
                if (str.ReadByte() == bomBuffer[0] && str.ReadByte() == bomBuffer[1] && str.ReadByte() == bomBuffer[2]) {
                    str.Position = 3;
                    result = new byte[str.Length - 3];
                } else {
                    str.Position = 0;
                    result = new byte[str.Length];
                }
                if (str.Length > 0) {
                    str.Read(result, 0, result.Length);
                } else {
                    result = new byte[0];
                }
                xsl.TemporaryFiles.Delete();
                return result;
            }
        }
    }
}