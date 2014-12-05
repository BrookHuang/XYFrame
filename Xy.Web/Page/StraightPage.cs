using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public sealed class StraightPage : PageAbstract {
        public override void onGetRequest() {
            SetNewTemplate(string.Concat(WebSetting.PageDir, URL.File));
        }
    }
}
