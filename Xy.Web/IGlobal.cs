using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web {
    public interface IGlobal {
        void ApplicationInit(System.Web.HttpApplication application);
        void RequestStart(object sender, EventArgs e);
        void HandleStart(ThreadEntity inThreadEntity);
        void HandleEnd(ThreadEntity inThreadEntity);
        void RequestEnd(object sender, EventArgs e);
        void ApplicationDispose();
    }

    internal class EmptyGlobal : IGlobal {
        public void ApplicationInit(System.Web.HttpApplication application) { return; }
        public void HandleStart(ThreadEntity inThreadEntity) { return; }
        public void HandleEnd(ThreadEntity inThreadEntity) { return; }
        public void ApplicationDispose() { return; }
        public void RequestStart(object sender, EventArgs e) { return; }
        public void RequestEnd(object sender, EventArgs e) { return; }
    }
}
