using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {

    public interface IControl {
        string Map { get; }
        bool IsNeedData { get; }
        HTMLContainer InnerData { get; set; }

        void Init(System.Collections.Specialized.NameValueCollection CreateTag, string map, int Index);
        void Handle(ThreadEntity CurrentThreadEntity, Page.PageAbstract CurrentPageClass, HTMLContainer contentContainer);
        //IControl CreateInitedInstance(HTMLContainer ContentContainer);
    }
}
