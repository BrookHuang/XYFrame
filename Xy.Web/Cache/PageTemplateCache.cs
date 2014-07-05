using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Web.Cache {
    public class PageTemplateCache {
        private static Dictionary<string, PageHtmlCacheItem> _list;
        static PageTemplateCache() {
            _list = new Dictionary<string, PageHtmlCacheItem>();
        }

        internal static PageHtmlCacheItem Get(string path, ThreadEntity CurrentThreadEntity) {
            return Get(path, null,CurrentThreadEntity);
        }

        internal static PageHtmlCacheItem Get(string path, List<byte> datas, ThreadEntity CurrentThreadEntity) {
            string _key = GenerateKey(path, CurrentThreadEntity);
            PageHtmlCacheItem _temp;
            if (_list.TryGetValue(_key, out _temp)) {
                return _temp;
            } else {
                if (datas == null) {
                    return Add(path, CurrentThreadEntity);
                } else {
                    return Add(path, datas, CurrentThreadEntity);
                }
            }
        }

        internal static PageHtmlCacheItem Add(string path, ThreadEntity CurrentThreadEntity) {
            return Add(path, null, CurrentThreadEntity);
        }

        internal static PageHtmlCacheItem Add(string path, List<byte> datas, ThreadEntity CurrentThreadEntity) {
            string _key = GenerateKey(path, CurrentThreadEntity);
            PageHtmlCacheItem temp;
            if (datas == null) {
                temp = new PageHtmlCacheItem(path, CurrentThreadEntity);
            } else {
                temp = new PageHtmlCacheItem(datas, CurrentThreadEntity);
            }
            _list.Add(_key, temp);
            return temp;
        }

        private static string GenerateKey(string path, ThreadEntity CurrentThreadEntity) {
            return path + "_" + CurrentThreadEntity.URLItem.PagePath + "_" + CurrentThreadEntity.WebSetting.Name;
        }

        public static void Clear() {
            _list.Clear();
        }
    }

    internal class PageHtmlCacheItem {
        /// <summary>
        /// 原始HTML
        /// </summary>
        internal List<byte> _originalHtml;
        /// <summary>
        /// 解析过后,去除标签的HTML
        /// </summary>
        internal List<byte> _analyzedHtml;
        internal Control.ControlCollection _controlCollection;

        internal PageHtmlCacheItem(List<byte> datas, ThreadEntity CurrentThreadEntity) {
            _originalHtml = datas;
            _analyzedHtml = new List<byte>(datas);
            _controlCollection = new Control.ControlAnalyze(_analyzedHtml, CurrentThreadEntity).ControlCollection;
        }

        internal PageHtmlCacheItem(string path, ThreadEntity CurrentThreadEntity):this(ReadOriginalFile(path, CurrentThreadEntity.WebSetting.Encoding),CurrentThreadEntity) {
        }

        //internal void UpdateThreadEntity(ref ThreadEntity inThreadEntity) {
        //    foreach (Xy.Web.Control.IControl _item in _controlCollection) {
        //        _item.ThreadEntity = inThreadEntity;
        //    }
        //}

        private static List<byte> ReadOriginalFile(string path, Encoding encoding) {
            if (!System.IO.File.Exists(path)) return new List<byte>();
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                using (StreamReader sr = new StreamReader(fs)) {
                    List<byte> temp = new List<byte>(encoding.GetBytes(sr.ReadToEnd()));
                    sr.Close();
                    fs.Close();
                    return temp;
                }
            }
        }
    }
}
