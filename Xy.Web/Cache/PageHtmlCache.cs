using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Web.Cache {
    public class PageHtmlCache {
        public static void WriteCache(string content, string path) {
            string dir = path.Substring(0, path.LastIndexOf('\\') + 1);
            Xy.Tools.IO.File.ifNoExistsThenCreate(dir);
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fs);
            try {
                sw.Write(content);
            } finally {
                sw.Close();
                fs.Close();
            }
        }

        public static string GetCacheFilePath(string PageID, string ControlID, Xy.WebSetting.WebSettingItem websetting, Xy.Tools.Web.UrlAnalyzer url, bool cacheUrlParam = false) {
            StringBuilder _sb = new StringBuilder();
            _sb.Append(websetting.CacheDir);
            _sb.Append(url.Path.Replace('/', '\\'));
            if (cacheUrlParam)
                _sb.Append(url.Param.Replace('?', '#'));
            //_sb.Append('#');
            //int index = 0;
            //int inindex = 0;
            //int temp = 0;
            //foreach (System.Text.RegularExpressions.Group itemGroup in urlitem.Regex.Match(url.Path).Groups) {
            //    string tempName = urlitem.Regex.GroupNameFromNumber(index++);
            //    if (!int.TryParse(tempName, out temp)) {
            //        if (inindex++ != 0) _sb.Append('&');
            //        _sb.Append(tempName);
            //        _sb.Append('=');
            //        _sb.Append(itemGroup.Value);
            //    }
            //}
            if(!string.IsNullOrEmpty(PageID))
                _sb.Append("-" + PageID);
            if (!string.IsNullOrEmpty(ControlID))
                _sb.Append("-" + ControlID);
            _sb.Append(".cache");
            return _sb.ToString();
        }

        public static void ClearCache(string PageID, string ControlID, Xy.WebSetting.WebSettingItem websetting, Xy.Tools.Web.UrlAnalyzer url, bool cacheUrlParam = true) {
            ClearCache(GetCacheFilePath(PageID, ControlID, websetting, url, cacheUrlParam));
        }

        public static void ClearCache(string path) {
            System.IO.File.Delete(path);
        }
    }
}
