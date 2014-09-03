using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Xy.Web {
    public sealed class HttpModule : IHttpModule {

        private static URLManage.URLManager _urlManager = null;
        private static IGlobal global = null;

        public void Init(HttpApplication context) {
            System.Xml.XmlDocument _document = new System.Xml.XmlDocument();
            _document.Load(Xy.Tools.IO.File.foundConfigurationFile("App", Xy.AppSetting.FILE_EXT));
            foreach (System.Xml.XmlNode _item in _document.GetElementsByTagName("Global")) {
                string _className = _item.InnerText;
                Type _tempCtonrlType = Type.GetType(_className, false, true);
                IGlobal _tempGlobal;
                if (_tempCtonrlType == null) {
                    System.Reflection.Assembly asm = System.Reflection.Assembly.Load(_className.Split(',')[0]);
                    _tempCtonrlType = asm.GetType(_className.Split(',')[1], false, true);
                }
                _tempGlobal = System.Activator.CreateInstance(_tempCtonrlType) as IGlobal;
                if (_tempGlobal != null) {
                    global = _tempGlobal;
                }
            }
            if (global == null) { global = new EmptyGlobal(); }
            global.ApplicationInit(context);

            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.Error += new EventHandler(context_Error);
            context.EndRequest += new EventHandler(context_EndRequest);
            _urlManager = URLManage.URLManager.GetInstance();
        }

        public void Dispose() {
            global.ApplicationDispose();
        }

        private void context_BeginRequest(object sender, EventArgs e) {

            global.RequestStart(sender, e);
            HttpApplication _application = sender as HttpApplication;
#if DEBUG
            Xy.Tools.Debug.Log.StartWorkflowLog();
            Xy.Tools.Debug.Log.WriteEventLog("start " + _application.Context.Request.Url.ToString());
#endif
            Xy.Tools.Web.UrlAnalyzer _url = new Tools.Web.UrlAnalyzer(_application.Context.Request.Url.ToString());

            URLManage.URLCollection _urlCollection = _urlManager.GetUrlItemCollection(_url);
            if (_urlCollection == null) {
                throw new Exception(string.Format("can not found URL collection: {0}", _url.ToString()));
            } else {
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("get url collection.");
#endif
                Xy.WebSetting.WebSettingItem _webSetting = _urlCollection.WebConfig;
                if (!string.IsNullOrEmpty(_webSetting.Root)) {
                    _url.SetRoot(_webSetting.Root);
                }
                if (!string.IsNullOrEmpty(_webSetting.Port)) {
                    _url.setPort(_webSetting.Port);
                }
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("get websetting item.");
#endif
                URLManage.URLItem _urlItem = _urlCollection.GetUrlItem(_url.Path);
                if (_urlItem == null) {
                    if(!_webSetting.Compatible) throw new Exception(string.Format("can not found URL item: {0}", _url.ToString()));
                } else {
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("get url item.");
#endif
                    ThreadEntity _entity = new ThreadEntity(_application.Context, _webSetting, _urlItem, _url);
                    global.HandleStart(_entity);
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("created thread entity");
#endif
                    _entity.Handle();
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("thread entity handled");
#endif
                    if (_entity.Content.HasContent) _application.Response.BinaryWrite(_entity.Content.ToArray());
                    global.HandleEnd(_entity);
#if DEBUG
            Xy.Tools.Debug.Log.WriteEventLog("content outputed");
            Xy.Tools.Debug.Log.EndWorkflowLog();
#endif
                    _application.Context.Response.End();
                }
            }
        }

        private void context_Error(object sender, EventArgs e) {
            HttpApplication _application = sender as HttpApplication;
            Exception exception = _application.Context.Server.GetLastError();
            _application.Context.Server.ClearError();
            Xy.WebSetting.WebSettingItem _webSetting = null;
            try {
                Xy.Tools.Web.UrlAnalyzer _url = new Xy.Tools.Web.UrlAnalyzer(_application.Context.Request.Url.ToString());
                URLManage.URLCollection _urlCollection = URLManage.URLManager.GetInstance().GetUrlItemCollection(_url);
                if (_urlCollection != null) {
                    URLManage.URLItem _urlItem = null;
                    string _errorURL = _url.Dir.TrimEnd('/');
                    do {
                        _urlItem = _urlCollection.GetUrlItem(_errorURL + "/error.aspx");
                        if (_urlItem != null) break;
                        _errorURL = _errorURL.Substring(0, Math.Max(0, _errorURL.LastIndexOf('/')));
                    } while (_errorURL.Length > 0);
                    if (_urlItem != null) {
                        if (_urlItem.ContentType != URLManage.URLType.Prohibit) {
                            _webSetting = Xy.WebSetting.WebSettingCollection.GetWebSetting(_urlCollection.WebConfigName);
                            ThreadEntity _entity = new ThreadEntity(_application.Context, _webSetting, _urlItem, _url);
                            //_entity.Handle();
                            _application.Response.ContentType = _urlItem.Mime;
                            Xy.Web.Page.ErrorPage _errpage = Runtime.Web.PageClassLibrary.Get(_urlItem.PageClassName) as Xy.Web.Page.ErrorPage;
                            if (_errpage != null) {
                                _errpage.setError(exception);
                                _errpage.Init(_entity, _webSetting);
                                _errpage.Handle("@PageDir:" + _urlItem.PagePath, _urlItem.PagePath, _urlItem.EnableScript, false);
                                if (_errpage.HTMLContainer.HasContent) {
                                    _application.Response.BinaryWrite(_errpage.HTMLContainer.ToArray());
                                    _application.Context.Response.End();
                                    return;
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                exception = new Exception("Error page class exception:" + ex.Message, exception);
            }
            StringBuilder errorsb = new StringBuilder();
            errorsb.AppendLine(@"<!DOCTYPE HTML><html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><title>Error Page</title></head><body>");
            errorsb.AppendLine("<strong>Wrong Time:</strong>" + DateTime.Now.ToString());
            errorsb.AppendLine("<strong>Client IP:</strong>" + _application.Context.Request.UserHostAddress);
            errorsb.AppendLine("<strong>Browser:</strong>" + _application.Context.Request.Browser.Browser);
            errorsb.AppendLine("<strong>Offending URL:</strong> " + _application.Request.Url.ToString());
            Exception inex = exception;
            int i = 1;
            while (inex != null) {
                errorsb.AppendLine("=============================Exception No." + i + ": " + inex.Message.Replace(Environment.NewLine, string.Empty) + "=============================");
                if (_webSetting != null && _webSetting.DebugMode) {
                    errorsb.AppendLine("<strong>Source: </strong>" + inex.Source);
                    if (inex.TargetSite != null)
                        errorsb.AppendLine("<strong>TargetMethod: </strong>" + inex.TargetSite.ToString());
                    errorsb.AppendLine("<strong>Data: </strong>" + inex.Data.ToString());
                    errorsb.AppendLine("<strong>StackTrace: </strong>" + inex.StackTrace);
                    errorsb.AppendLine();
                }
                inex = inex.InnerException; i++;
            }
            errorsb.Append(@"</body></html>");
            _application.Context.Response.BinaryWrite(_application.Context.Request.ContentEncoding.GetBytes(errorsb.ToString().Replace(Environment.NewLine, "<br />" + Environment.NewLine)));
            _application.Context.Response.End();
        }

        private void context_EndRequest(object sender, EventArgs e) {
            global.RequestEnd(sender, e);
            return;
        }

    }
}
