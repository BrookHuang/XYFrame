using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Security {
    public interface IUser {
        long ID { get; }
        string Name { get; }
        bool HasPower(int powerId);
        bool HasPower(string powerKey);
        bool InGroup(int groupId);
        bool InGroup(string groupKey);
        void WriteCookie();
    }
}
