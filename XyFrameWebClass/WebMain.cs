using System;
using System.Collections.Generic;
using System.Text;

namespace XyFrameWebClass {
    public class WebMain : Xy.Web.Page.PageAbstract {
        public override void onGetRequest() {
            Response.Write("abcdefg");
            Session["abc"] = "ABC";
        }
    }
}