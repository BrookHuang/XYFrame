using System;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web.Page {
    public sealed class PageResponse {
        System.Web.HttpResponse _innerResponse;
        HTMLContainer _content;
        ThreadEntity _entity;

        internal PageResponse(ThreadEntity entity, HTMLContainer content) {
            _innerResponse = entity.WebContext.Response;
            _content = content;
            _entity = entity;
        }

        internal void SetNewContainer(HTMLContainer content) {
            _content = content;
        }

        #region MS function
        //public bool Buffer { get { return _innerResponse.Buffer; } set { _innerResponse.Buffer = value; } }
        
        //public bool BufferOutput { get { return _innerResponse.BufferOutput; } set { _innerResponse.BufferOutput = value; } }

        public HttpCachePolicy Cache { get { return _innerResponse.Cache; } }
        
        //public string CacheControl { get { return _innerResponse.CacheControl; } set { _innerResponse.CacheControl = value; } }
        
        //public string Charset { get { return _innerResponse.Charset; } set { _innerResponse.Charset = value; } }
        
        //public Encoding ContentEncoding { get { return _innerResponse.ContentEncoding; } set { _innerResponse.ContentEncoding = value; } }

        public string ContentType { get { return _innerResponse.ContentType; } set { _innerResponse.ContentType = value; } }

        public HttpCookieCollection Cookies { get { return _innerResponse.Cookies; } }

        public int Expires { get { return _innerResponse.Expires; } set { _innerResponse.Expires = value; } }

        public DateTime ExpiresAbsolute { get { return _innerResponse.ExpiresAbsolute; } set { _innerResponse.ExpiresAbsolute = value; } }
        
        //public System.IO.Stream Filter { get { return _innerResponse.Filter; } set { _innerResponse.Filter = value; } }

        public Encoding HeaderEncoding { get { return _innerResponse.HeaderEncoding; } set { _innerResponse.HeaderEncoding = value; } }

        public System.Collections.Specialized.NameValueCollection Headers { get { return _innerResponse.Headers; } }

        public bool IsClientConnected { get { return _innerResponse.IsClientConnected; } }

        public bool IsRequestBeingRedirected { get { return _innerResponse.IsRequestBeingRedirected; } }
        
        //public System.IO.TextWriter Output { get { return _innerResponse.Output; } }
        
        //public System.IO.Stream OutputStream { get { return _innerResponse.OutputStream; } }

        public string RedirectLocation { get { return _innerResponse.RedirectLocation; } set { _innerResponse.RedirectLocation = value; } }

        public string Status { get { return _innerResponse.Status; } set { _innerResponse.Status = value; } }

        public int StatusCode { get { return _innerResponse.StatusCode; } set { _innerResponse.StatusCode = value; } }

        public string StatusDescription { get { return _innerResponse.StatusDescription; } set { _innerResponse.StatusDescription = value; } }

        public int SubStatusCode { get { return _innerResponse.SubStatusCode; } set { _innerResponse.SubStatusCode = value; } }

        public bool SuppressContent { get { return _innerResponse.SuppressContent; } set { _innerResponse.SuppressContent = value; } }
        
        //public bool TrySkipIisCustomErrors { get { return _innerResponse.TrySkipIisCustomErrors; } set { _innerResponse.TrySkipIisCustomErrors = value; } }

        //public void AddCacheDependency(params System.Web.Caching.CacheDependency[] dependencies) { _innerResponse.AddCacheDependency(dependencies); }

        //public void AddCacheItemDependencies(System.Collections.ArrayList cacheKeys) { _innerResponse.AddCacheItemDependencies(cacheKeys); }

        //public void AddCacheItemDependencies(string[] cacheKeys) { _innerResponse.AddCacheItemDependencies(cacheKeys); }

        //public void AddCacheItemDependency(string cacheKey) { _innerResponse.AddCacheItemDependency(cacheKey); }

        //public void AddFileDependencies(System.Collections.ArrayList filenames) { _innerResponse.AddFileDependencies(filenames); }

        //public void AddFileDependencies(string[] filenames) { _innerResponse.AddFileDependencies(filenames); }

        //public void AddFileDependency(string filename) { _innerResponse.AddFileDependency(filename); }

        public void AddHeader(string name, string value) { _innerResponse.AddHeader(name, value); }

        public void AppendCookie(HttpCookie cookie) { _innerResponse.AppendCookie(cookie); }

        public void AppendHeader(string name, string value) { _innerResponse.AppendHeader(name, value); }

        //public void AppendToLog(string param) { _innerResponse.AppendToLog(param); }

        //public string ApplyAppPathModifier(string virtualPath) { return _innerResponse.ApplyAppPathModifier(virtualPath); }

        //public void BinaryWrite(byte[] buffer) { _innerResponse.BinaryWrite(buffer); }

        public void Clear() { _content.Clear(); }

        //public void ClearContent() { _innerResponse.ClearContent(); }

        //public void ClearHeaders() { _innerResponse.ClearHeaders(); }

        //public void Close() { _innerResponse.Close(); }

        //public void DisableKernelCache() { _innerResponse.DisableKernelCache(); }

        public void End() { throw new PageEndException(); }

        //public void Flush() { _innerResponse.Flush(); }

        //public void Pics(string value) { _innerResponse.Pics(value); }

        public void Redirect(string url) { _innerResponse.Redirect(url); }

        public void Redirect(string url, bool endResponse) { _innerResponse.Redirect(url, endResponse); }

        //public static void RemoveOutputCacheItem(string path) { System.Web.HttpResponse.RemoveOutputCacheItem(path); }

        //public void SetCookie(HttpCookie cookie) { _innerResponse.SetCookie(cookie); }

        //public void TransmitFile(string filename) { _innerResponse.TransmitFile(filename); }

        //public void TransmitFile(string filename, long offset, long length) { _innerResponse.TransmitFile(filename, offset, length); }

        //public void Write(char ch) { _innerResponse.Write(ch); }
        public void Write(char s) { _content.Write(s.ToString()); }

        //public void Write(object obj) { _innerResponse.Write(obj); }

        //public void Write(string s) { _innerResponse.Write(s); }
        public void Write(string s) { _content.Write(s); }

        //public void Write(char[] buffer, int index, int count) { _innerResponse.Write(buffer, index, count); }

        //public void WriteFile(string filename) { _innerResponse.WriteFile(filename); }

        //public void WriteFile(string filename, bool readIntoMemory) { _innerResponse.WriteFile(filename, readIntoMemory); }

        //public void WriteFile(IntPtr fileHandle, long offset, long size) { _innerResponse.WriteFile(fileHandle, offset, size); }

        //public void WriteFile(string filename, long offset, long size) { _innerResponse.WriteFile(filename, offset, size); }

        //public void WriteSubstitution(HttpResponseSubstitutionCallback callback) { _innerResponse.WriteSubstitution(callback); }
        #endregion

        public void Write(System.IO.Stream stream) {
            byte[] temp = new byte[stream.Length];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(temp, 0, temp.Length);
            _content.Write(temp);
        }
        public void Write(byte[] bytes) { _content.Write(bytes); }
        public void Write(HTMLContainer html) { _content.Write(html); }
    }
}
