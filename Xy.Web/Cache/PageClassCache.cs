using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Cache {
    public class PageClassCache {
        private static Dictionary<string, Page.Page> _list;
        static PageClassCache() {
            _list = new Dictionary<string, Page.Page>();
        }
        internal static Page.Page Get(string Key) {
            Page.Page _page;
            if (_list.TryGetValue(Key, out _page)) {
                return _page.CreateInstance();
            } else {
                return Add(Key, Key.Split(',')[0], Key.Split(',')[1]);
            }
        }

        internal static Page.Page Add(string Key, string AssemblyName, string ClassFullName) {
            Page.Page _page; Type _pageType;
            //_pageType = Type.GetType(ClassFullName, false, true);
            //if (_pageType == null) {
                System.Reflection.Assembly asm = System.Reflection.Assembly.Load(AssemblyName);
                _pageType = asm.GetType(ClassFullName, false, true);
            //}
            if (_pageType == null) { throw new Exception(string.Format("{0},{1} 类对象未找到", AssemblyName, ClassFullName)); }
            _page = System.Activator.CreateInstance(_pageType) as Page.Page;
            inAdd(Key, _page);
            return _page;
        }

        private static void inAdd(string key, Page.Page instance) {
            if (instance == null) { new Exception(string.Format("创建页面类缓存失败:{0}", key)); }
            if (!_list.ContainsKey(key)) {
                _list.Add(key, instance);
            }
        }

        public static void Clear() {
            _list.Clear();
        }
    }
}
