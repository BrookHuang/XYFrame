using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Cache {
    public class Page {
        private static Dictionary<string, Web.Page.Page> _list;
        static Page() {
            _list = new Dictionary<string, Web.Page.Page>();
        }
        internal static Web.Page.Page Get(string Key) {
            Web.Page.Page _page;
            if (_list.TryGetValue(Key, out _page)) {
                return _page.CreateInstance();
            } else {
                return Add(Key, Key.Split(',')[0], Key.Split(',')[1]);
            }
        }

        internal static Web.Page.Page Add(string Key, string AssemblyName, string ClassFullName) {
            Web.Page.Page _page; Type _pageType;
            //_pageType = Type.GetType(ClassFullName, false, true);
            //if (_pageType == null) {
                System.Reflection.Assembly asm = System.Reflection.Assembly.Load(AssemblyName);
                _pageType = asm.GetType(ClassFullName, false, true);
            //}
            if (_pageType == null) { throw new Exception(string.Format("{0},{1} 类对象未找到", AssemblyName, ClassFullName)); }
            _page = System.Activator.CreateInstance(_pageType) as Web.Page.Page;
            inAdd(Key, _page);
            return _page;
        }

        private static void inAdd(string key, Web.Page.Page instance) {
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
