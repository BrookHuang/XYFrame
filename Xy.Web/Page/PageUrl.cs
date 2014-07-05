using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public class PageUrl : Xy.Tools.Web.UrlAnalyzer.UrlAnalyzer {
        public URLManage.URLItem UrlItem { get; internal set; }
        public string AbsolutePath { get { return UrlItem.PagePath; } }
        internal Uri MsUri { get; set; }
        public string HOST { get { return MsUri.Host; } }

        public PageUrl(ThreadEntity te)
            : this(te.URL.ToString()) {
            this.MsUri = te.HttpContext.Request.Url;
            this.UrlItem = te.UrlItem;
        }

        public PageUrl(string url)
            : base(url) {
        }
    }
}
