using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {

    public abstract class UserPage : PageAbstract {

        private Security.IUser _currentUser;
        public Security.IUser CurrentUser {
            get {
                if (_currentUser == null) {
                    _currentUser = GetCurrentUser();
                }
                return _currentUser;
            }
            set { _currentUser = value; }
        }

        protected abstract Security.IUser GetCurrentUser();

        public sealed override void Validate() {
            ValidateUser();
            ValidateUrl();
        }

        public virtual void ValidateUser() { }

        public virtual void ValidateUrl() { }
    }
}
