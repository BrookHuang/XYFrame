using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Control {
    public class Tag {
        private static System.Text.RegularExpressions.Regex _decodeReg;
        static Tag() {
            _decodeReg = new System.Text.RegularExpressions.Regex(@"(@|\!|#)?\w+(=(""[^""]+""|'[^']+'))?", System.Text.RegularExpressions.RegexOptions.Compiled);
        }
        public static System.Collections.Specialized.NameValueCollection Decode(string markString) {
            System.Text.RegularExpressions.MatchCollection _matckresult = _decodeReg.Matches(markString);
            if (_matckresult.Count > 0) {
                System.Collections.Specialized.NameValueCollection marks = new System.Collections.Specialized.NameValueCollection();
                for (int i = 0; i < _matckresult.Count; i++) {
                    System.Text.RegularExpressions.Match match = _matckresult[i];
                    string tempname, tempvalue;
                    if (match.Value.IndexOf('=') > 0) {
                        tempname = match.Value.Substring(0, match.Value.IndexOf('='));
                        tempvalue = match.Value.Substring(match.Value.IndexOf('=') + 1);
                    } else {
                        tempname = match.Value;
                        tempvalue = string.Empty;
                    }
                    tempname = tempname.Trim();
                    if (!string.IsNullOrEmpty(tempvalue)) {
                        tempvalue = tempvalue.Trim(' ', tempvalue[0]);
                    }
                    marks.Add(tempname, tempvalue);
                }
                return marks;
            } else {
                throw new Exception(string.Format("Can not analyze \"{0}\"", markString));
            }
        }

        public static string TransferValue(string key, string value, Xy.Web.Page.PageAbstract page) {
            string _value = value;
            string tempGetName = key;
            if (_value.IndexOf(':') > 0) {
                tempGetName = _value.Substring(_value.LastIndexOf(':') + 1);
                _value = _value.Substring(0, _value.LastIndexOf(':'));
            }

            switch (_value) {
                case "Request":
                    _value = page.Request[tempGetName];
                    break;
                case "Url":
                    _value = page.Request.QueryString[tempGetName];
                    break;
                case "Form":
                    _value = page.Request.Form[tempGetName];
                    break;
                case "Group":
                    _value = page.Request.GroupString[tempGetName];
                    break;
                case "Values":
                    _value = page.Request.Values[tempGetName];
                    break;
                case "Data":
                    _value = page.PageData[tempGetName] == null ? string.Empty : page.PageData[tempGetName].GetDataString();
                    break;
                case "Config":
                    if (tempGetName == key) {
                        _value = page.WebSetting.Name;
                    } else {
                        _value = page.WebSetting.Config[tempGetName];
                    }
                    break;
            }
            return _value;
        }

        //public static System.Collections.Specialized.NameValueCollection Decode(string markString) {
        //    System.Collections.Specialized.NameValueCollection marks = new System.Collections.Specialized.NameValueCollection();
        //    bool _valueMode = false;
        //    char _qoute = ' ';
        //    StringBuilder _item = new StringBuilder();
        //    for (int i = 0; i < markString.Length; i++) {
        //        char c = markString[i];
        //        if (!_valueMode && (c == ' ' || c == '\r' || c == '\n')) {
        //            foundone(marks, _item.ToString());
        //            _item = new StringBuilder();
        //            continue;
        //        }
        //        _item.Append(c);
        //        if (c == _qoute) {
        //            _valueMode = false;
        //            _qoute = ' ';
        //            foundone(marks, _item.ToString());
        //            _item = new StringBuilder();
        //            continue;
        //        }
        //        if (!_valueMode && (c == '\'' || c == '\"')) {
        //            _valueMode = true;
        //            _qoute = c;
        //        }
        //        if (i == markString.Length - 1) 
        //            foundone(marks, _item.ToString());
        //    }
        //    if (marks.Count == 0) throw new Exception(string.Format("Can not analyze \"{0}\"", markString));
        //    return marks;
        //}

        //private static void foundone(System.Collections.Specialized.NameValueCollection nvc, string item) {
        //    if (string.IsNullOrEmpty(item)) return;
        //    //Console.WriteLine(item);
        //    string tempname, tempvalue;
        //    if (item.IndexOf('=') > 0) {
        //        tempname = item.Substring(0, item.IndexOf('='));
        //        tempvalue = item.Substring(item.IndexOf('=') + 1);
        //    } else {
        //        tempname = item;
        //        tempvalue = string.Empty;
        //    }
        //    tempname = tempname.Trim();
        //    tempvalue = tempvalue.Trim('\"').Trim('\'');
        //    nvc.Add(tempname, tempvalue);
        //}
    }
}
