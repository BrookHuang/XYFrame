using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public class ErrorPage : Xy.Web.Page.PageAbstract {

        protected string _errorTemplate =
@"<table>
    <caption style=""font-size: 24px; text-align: left; font-weight: bold;"">Brief information</caption>
    <tr>
        <td width=""120""><strong>Wrong Time:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""WrongTime"" %></td>
    </tr>
    <tr>
        <td><strong>Client IP:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""ClientIP"" %></td>
    </tr>
    <tr>
        <td><strong>Client Browser:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""ClientBrowser"" %></td>
    </tr>
    <tr>
        <td><strong>UserAgent:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""UserAgent"" %></td>
    </tr>
    <tr>
        <td><strong>URL:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""URL"" %></td>
    </tr>
    <tr>
        <td><strong>Message:</strong></td>
        <td><% @Tag Provider=""Data"" Name=""Message"" %></td>
    </tr>
</table>
<% @Tag Provider=""Config"" Name=""DebugMode"" Mode=""Compare""  CompareMode=""Equal"" CompareValue=""True"" EnableScript=""True"" %>
<br />
<table border=""1"" style=""font-size: 12px;"">
    <caption style=""font-size: 24px; text-align: left; font-weight: bold;"">Detail information</caption>
    <% @Data Provider=""Data"" Name=""Error"" %>
        <tr>
            <td width=""120""><strong style=""font-size:14px"">Message</strong></td><td><strong style=""font-size:14px""><xsl:value-of select=""Message"" disable-output-escaping=""yes"" /></strong></td>
        </tr>
        <tr>
            <td width=""120"">Source</td>
            <td><xsl:value-of select=""Source"" disable-output-escaping=""yes"" /></td>
        </tr>
        <tr>
            <td>TargetSite</td>
            <td><xsl:value-of select=""TargetSite"" disable-output-escaping=""yes"" /></td>
        </tr>
        <tr>
            <td>Data</td>
            <td><xsl:value-of select=""Data"" disable-output-escaping=""yes"" /></td>
        </tr>
        <tr>
            <td>StackTrace</td>
            <td><xsl:value-of select=""StackTrace"" disable-output-escaping=""yes"" /></td>
        </tr>
    <% @End %>    
</table>
<% @End %>";

        private Exception _receiveException = null;
        internal void setError(Exception ex) {
            _receiveException = ex;
        }

        public override void onGetRequest() {
            onGetError(_receiveException);
        }

        public virtual void onGetError(Exception ex) {
            StringBuilder _errorString = new StringBuilder();
            Exception exception = ex;
            PageData.Add("WrongTime", DateTime.Now.ToString());
            _errorString.Append("WrongTime:").AppendLine(DateTime.Now.ToString());
            PageData.Add("ClientIP", Request.UserHostAddress);
            _errorString.Append("ClientIP:").AppendLine(Request.UserHostAddress);
            PageData.Add("ClientBrowser", string.Format("{0} | {1}", Request.Browser.Type, Request.Browser.Browser));
            _errorString.Append("ClientBrowser:").AppendLine(string.Format("{0} | {1} | {2}", Request.Browser.Type, Request.Browser.Browser));
            PageData.Add("UserAgent", Request.UserAgent);
            _errorString.Append("UserAgent:").AppendLine(Request.UserAgent);
            PageData.Add("URL", Request.Url.ToString());
            _errorString.Append("URL:").AppendLine(Request.Url.ToString());
            PageData.Add("Message", exception.Message);
            _errorString.Append("Message:").AppendLine(exception.Message);

            Exception inex = exception;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Message");
            dt.Columns.Add("Source");
            dt.Columns.Add("TargetSite");
            dt.Columns.Add("Data");
            dt.Columns.Add("StackTrace");
            int i = 1;
            while (inex != null) {
                System.Data.DataRow dr = dt.NewRow();
                _errorString.AppendLine("=============================Exception No." + i + ": " + inex.Message.Replace(Environment.NewLine, string.Empty) + "=============================");
                dr["Message"] = string.IsNullOrEmpty(inex.Message) ? "None" : inex.Message.Replace("\r\n", "<br />");
                _errorString.AppendLine("Message:" + inex.Message);
                dr["Source"] = string.IsNullOrEmpty(inex.Source) ? "None" : inex.Source.Replace("\r\n", "<br />");
                _errorString.AppendLine("Source:" + inex.Source);
                dr["TargetSite"] = inex.TargetSite == null ? "None" : inex.TargetSite.ToString();
                _errorString.AppendLine("TargetSite:" + inex.TargetSite);

                StringBuilder _tsb = new StringBuilder();
                if (inex.Data != null) {
                    foreach (object _entry in inex.Data.Keys) {
                        _tsb.Append(string.Format("{0}:{1}<br />", _entry.ToString(), inex.Data[_entry]));
                    }
                    _errorString.AppendLine("Data:" + _tsb.ToString());
                    dr["Data"] = _tsb.Length == 0 ? "None" : _tsb.ToString();
                }

                dr["StackTrace"] = string.IsNullOrEmpty(inex.StackTrace) ? "None" : inex.StackTrace.Replace("\r\n", "<br />");
                _errorString.AppendLine("StackTrace:" + inex.StackTrace);
                dt.Rows.Add(dr);
                inex = inex.InnerException;
            }
            PageData.Add("Error", dt);
            Xy.Tools.Debug.Log.WriteErrorLog(_errorString.ToString());
            Response.StatusCode = 404;
        }

        public override string LoadSourceFile(string sourceFilePath) {
            if (!System.IO.File.Exists(sourceFilePath)) {
                //using (System.IO.FileStream _errTemplate = new System.IO.FileStream(sourceFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                //    using (System.IO.StreamWriter _sw = new System.IO.StreamWriter(_errTemplate, WebSetting.Encoding)) {
                //        _sw.Write(_errorTemplate);
                //        _sw.Flush();
                //        _sw.Close();
                //    }
                //    _errTemplate.Close();
                //}
                HTMLContainer _errTemplate = new HTMLContainer(WebSetting.Encoding);
                _errTemplate.Write(_errorTemplate);
                SetContent(_errTemplate);
            }
            return sourceFilePath;
        }
    }
}
