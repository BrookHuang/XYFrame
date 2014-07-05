using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Control {
    public enum AnalyzeResultType {
        PureHTML,
        Control
    }
    public class AnalyzeResult {
        public AnalyzeResultType Type { get; internal set; }
        public string Map { get; internal set; }
        public HTMLContainer PureHTML { get; internal set; }
        public Xy.Web.Control.IControl Control { get; internal set; }

        internal void Handle(ThreadEntity currentThreadEntity, Page.PageAbstract currentPageClass, HTMLContainer contentContainer) {
            switch (Type) {
                case AnalyzeResultType.PureHTML:
                    contentContainer.Write(PureHTML);
                    break;
                case AnalyzeResultType.Control:
                    Control.Handle(currentThreadEntity, currentPageClass, contentContainer);
                    break;
            }
        }
    }

    public class AnalyzeResultCollection : List<AnalyzeResult> {
        public bool IsHandled { get; internal set; }
    }
}
