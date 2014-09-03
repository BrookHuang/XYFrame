using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.Web {
    public class UrlAnalyzer {
        /// <summary>
        /// the result like 'http'
        /// </summary>
        public string Protocol { get; private set; }

        /// <summary>
        /// the result like 'www.xiaoyang.me'
        /// </summary>
        public string Domain { get; private set; }

        /// <summary>
        /// the result like 'http://www.xiaoyang.me/'
        /// </summary>
        public string Site { get; private set; }

        /// <summary>
        /// the result like '/Dir/innerDir/'
        /// </summary>
        public string Dir { get; private set; }

        /// <summary>
        /// the result like '/Dir/innerDir/page.html'
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// the result like 'page.html'
        /// </summary>
        public string File { get; private set; }

        /// <summary>
        /// the result like '?param=value'
        /// </summary>
        public string Param { get; private set; }
        public bool HasParam { get; private set; }

        /// <summary>
        /// is a name-value collection for params
        /// </summary>
        public System.Collections.Specialized.NameValueCollection Params { get; private set; }

        /// <summary>
        /// use by your site in a folder
        /// </summary>
        public string Folder { get; private set; }

        /// <summary>
        /// physical file
        /// </summary>
        public string PhysicalPath { get { return System.AppDomain.CurrentDomain.BaseDirectory + Path.Replace('/', '\\'); } }

        public string BasePath { get { return string.Concat(Site + Folder); } }


        private string originalUrl;

        public override string ToString() {
            return originalUrl;
        }

        public UrlAnalyzer(string url) {
            originalUrl = url;
            if (url.IndexOf("://") > 0) {
                Protocol = url.Substring(0, url.IndexOf("://"));
                Domain = url.Substring(Protocol.Length + 3, url.IndexOf("/", Protocol.Length + 3) - (Protocol.Length + 3));
                Site = Protocol + "://" + Domain + "/";
            } else {
                Protocol = string.Empty;
                Domain = url.Substring(0, url.IndexOf("/"));
                if (!string.IsNullOrEmpty(Domain)) {
                    Site = Domain;
                } else {
                    Site = "/";
                }
            }

            if (url.IndexOf('?') > 0) {
                HasParam = true;
                Path = url.Substring(Site.Length - 1, url.IndexOf('?') - Site.Length + 1);
                Param = url.Substring(url.IndexOf('?'));
                string[] temp = Param.Substring(1).Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                Params = new System.Collections.Specialized.NameValueCollection();
                for (int i = 0; i < temp.Length; i++) {
                    string _temp = System.Web.HttpUtility.UrlDecode(temp[i]);
                    int equalIndex = _temp.IndexOf('=');
                    string key, value;
                    if (equalIndex > 0) {
                        key = _temp.Substring(0, equalIndex);
                        value = _temp.Substring(equalIndex + 1);
                    } else {
                        key = _temp;
                        value = string.Empty;
                    }
                    Params.Add(key, value);
                }
            } else {
                HasParam = false;
                Path = url.Substring(Site.Length - 1);
                Param = string.Empty;
            }
            Dir = Path.Substring(0, Path.LastIndexOf('/') + 1);
            File = Path.Substring(Dir.Length);
        }

        public void SetRoot(string folder) {
            Folder = string.Concat(string.Concat(folder, '/'));
            if (Path.IndexOf(Folder) == 1) {
                Path = Path.Substring(Folder.Length);
            }
        }

        public bool HasRoot(string folder) {
            return Path.IndexOf(string.Concat('/', folder, '/')) == 0;
        }

        public void setPort(string port) {
            if (!string.IsNullOrEmpty(Protocol)) {
                if (Domain.IndexOf(':') > -1) {
                    Domain = Domain.Substring(0, Domain.IndexOf(':'));
                }
                Domain += ':' + port;
                Site = Protocol + "://" + Domain + "/";
            }
        }
    }
}
