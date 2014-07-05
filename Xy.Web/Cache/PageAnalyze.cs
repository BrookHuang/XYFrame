using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Cache {
    public class PageAnalyze {
        //private static Dictionary<string, Dictionary<string, Dictionary<string, Control.ControlAnalyze>>> _controlCenter = new Dictionary<string, Dictionary<string, Dictionary<string, Control.ControlAnalyze>>>();
        private static Dictionary<string, Control.ControlAnalyze> _controlCenter = new Dictionary<string, Control.ControlAnalyze>();
        public static Control.ControlAnalyze GetInstance(ThreadEntity currentTheadEntity, Page.PageAbstract currentPageClass, string map, bool useInnerMark = false) {
            if (currentPageClass.WebSetting.DebugMode) return new Control.ControlAnalyze(currentTheadEntity, map, useInnerMark);
            string _key = string.Concat(currentPageClass.WebSetting.Name, currentTheadEntity.URLItem.PageClassName, map);
            if (!_controlCenter.ContainsKey(_key)) {
                _controlCenter.Add(_key, new Control.ControlAnalyze(currentTheadEntity, map, useInnerMark));
            }
            return _controlCenter[_key];
        }
    }
}
