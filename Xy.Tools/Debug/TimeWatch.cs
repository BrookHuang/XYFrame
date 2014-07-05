using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Debug {
    public class TimeWatch {
        public delegate void WatchFunction();
        public event WatchFunction WatchEvent;
        public void Watch(int time, int repeat = 1, bool init = false, bool reset = true, bool forceGC = false) {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            if (init) _watch(1, reset, forceGC, "init");
            for (int i = 0; i < repeat; i++) {
                _watch(time, reset, forceGC, string.Empty);
            }
        }

        private void _watch(int time, bool reset = true, bool forceGC = false, string tag = "") {
            if (WatchEvent == null) return;
            List<WatchFunction> _eventList = new List<WatchFunction>();
            foreach (WatchFunction _delegateItem in WatchEvent.GetInvocationList()) {
                _eventList.Add(_delegateItem);
            }
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            StringBuilder _sb = new StringBuilder();
            for (int j = 0; j < _eventList.Count; j++) {
                WatchFunction _watchItem = _eventList[j];
                long _memory = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64;
                sw.Start();
                for (int k = 0; k < time; k++) {
                    _watchItem();
                }
                sw.Stop();
                _memory = System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64 - _memory;
                Console.WriteLine(string.Format("{0,20}: {1,10} (spend: {2,10}, Memory: {3,10})", tag + _watchItem.Method.Name, sw.ElapsedTicks, sw.ElapsedMilliseconds + "ms", _memory / 1024 + "kb"));
                if (reset) sw.Reset();
            }
            Console.WriteLine();
            if (forceGC) {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            }
        }
    }
}
