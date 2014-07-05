using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Web {

    #region base on ArrayList
    //public class HTMLContainer : IDisposable {
    //    private System.Text.Encoding _encoding;
    //    private System.Collections.ArrayList _byteContent;

    //    private bool _isStringDirty = true;

    //    private string _cleanString = string.Empty;
    //    private byte[] _cleanByteArray = new byte[0];

    //    private bool _hasContent = false;
    //    public bool HasContent { get { return _hasContent; } }
    //    public int Length { get { if (_isStringDirty)flushString(); return _cleanString.Length; } }

    //    public HTMLContainer(System.Text.Encoding encoding) {
    //        _encoding = encoding;
    //        _byteContent = new System.Collections.ArrayList();
    //    }

    //    public void Write(byte[] content) { Insert(content, -1); }

    //    public void Write(string content) { Insert(content, -1); }

    //    public void Write(HTMLContainer content) { Insert(content, -1); }

    //    public void Insert(byte[] content, int index) {
    //        if (index < 0) {
    //            _byteContent.AddRange(content);
    //        } else {
    //            _byteContent.InsertRange(index, content);
    //        }
    //        _isStringDirty = false;
    //        _hasContent = true;
    //    }


    //    public void Insert(HTMLContainer content, int index) {
    //        Insert(content.ToArray(), index);
    //    }

    //    public void Insert(string content, int index) {
    //        Insert(_encoding.GetBytes(content), index);
    //    }

    //    public byte[] ToArray() {
    //        return (byte[])_byteContent.ToArray(typeof(System.Byte));
    //    }

    //    public override string ToString() {
    //        if (!_isStringDirty) flushString();
    //        return _cleanString;
    //    }

    //    private void flushString() {
    //        _cleanString = _encoding.GetString((byte[])_byteContent.ToArray(typeof(System.Byte)));
    //        _isStringDirty = true;
    //    }


    //    public void Dispose() {
    //        GC.SuppressFinalize(this);
    //    }
    //}
    #endregion

    #region base on byte array
    //public class HTMLContainer : IDisposable {
    //    private System.Text.Encoding _encoding;
    //    private byte[] _byteContent;

    //    private bool _isStringDirty = true;

    //    private string _cleanString = string.Empty;
    //    private byte[] _cleanByteArray = new byte[0];

    //    private bool _hasContent = false;
    //    public bool HasContent { get { return _hasContent; } }
    //    public int Length { get { if (_isStringDirty)flushString(); return _cleanString.Length; } }

    //    public HTMLContainer(System.Text.Encoding encoding) {
    //        _encoding = encoding;
    //        _byteContent = new byte[0];
    //    }

    //    public void Write(byte[] content) { Insert(content, -1); }

    //    public void Write(string content) { Insert(content, -1); }

    //    public void Write(HTMLContainer content) { Insert(content, -1); }

    //    public void Insert(byte[] content, int index) {
    //        int originLength = _byteContent.Length;
    //        Array.Resize<byte>(ref _byteContent, _byteContent.Length + content.Length);
    //        if (index < 0) {
    //            Buffer.BlockCopy(content, 0, _byteContent, originLength, content.Length);
    //        } else {
    //            Buffer.BlockCopy(_byteContent, index, _byteContent, index + content.Length, originLength - index);
    //            Buffer.BlockCopy(content, 0, _byteContent, index, content.Length);
    //        }
    //        _isStringDirty = false;
    //        _hasContent = true;
    //    }


    //    public void Insert(HTMLContainer content, int index) {
    //        Insert(content.ToArray(), index);
    //    }

    //    public void Insert(string content, int index) {
    //        Insert(_encoding.GetBytes(content), index);
    //    }

    //    public byte[] ToArray() {
    //        return _byteContent;
    //    }

    //    public override string ToString() {
    //        if (!_isStringDirty) flushString();
    //        return _cleanString;
    //    }

    //    private void flushString() {
    //        _cleanString = _encoding.GetString(_byteContent);
    //        _isStringDirty = true;
    //    }


    //    public void Dispose() {
    //        GC.SuppressFinalize(this);
    //    }
    //}
    #endregion

    #region base on memoryStream
    public class HTMLContainer : IDisposable {
        private System.IO.MemoryStream _memoryStream;

        private bool _isStringDirty = true;
        private bool _isByteDirty = true;

        private string _cleanString = string.Empty;
        private byte[] _cleanByteArray = new byte[0];

        private System.Text.Encoding _encoding;
        public System.Text.Encoding Encoding { get { return _encoding; } }

        public bool HasContent { get { return _memoryStream.Length > 0; } }
        public long Length { get { return _memoryStream.Length; } }
        public System.IO.MemoryStream BaseStream { get { return _memoryStream; } }

        public HTMLContainer(System.Text.Encoding encoding) {
            _encoding = encoding;
            _memoryStream = new System.IO.MemoryStream();
        }

        public void Write(byte[] content) {
            _memoryStream.Write(content, 0, content.Length);
            _isStringDirty = false;
            _isByteDirty = false;
        }

        public void Write(string content) { Write(_encoding.GetBytes(content)); }

        public void Write(HTMLContainer content) { 
            content._memoryStream.WriteTo(_memoryStream);
            _isStringDirty = false;
            _isByteDirty = false;
        }

        public byte[] ToArray() {
            if (!_isByteDirty) flushByte();
            return _cleanByteArray;
        }

        public override string ToString() {
            if (!_isStringDirty) flushString();
            return _cleanString;
        }

        private void flushString() {
            _cleanString = _encoding.GetString(_memoryStream.ToArray());
            _isStringDirty = true;
        }

        private void flushByte() {
            _cleanByteArray = _memoryStream.ToArray();
            _isByteDirty = true;
        }

        public void Clear() {
            _memoryStream = new System.IO.MemoryStream();
        }

        public void Dispose() {
            _memoryStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
    #endregion

    #region base on stringBuilder
    //public class HTMLContainer : IDisposable {
    //    private System.Text.Encoding _encoding;
    //    private System.Text.StringBuilder _stringBuilder;

    //    private bool _isStringDirty;
    //    private bool _isByteDirty;

    //    private string _cleanString;
    //    private byte[] _cleanByteArray;

    //    private bool _hasContent = false;
    //    public bool HasContent { get { return _hasContent; } }
    //    public int Length { get { if (_isStringDirty)flushString(); return _cleanString.Length; } }
    //    public System.Text.Encoding Encoding { get { return _encoding; } }

    //    public HTMLContainer(System.Text.Encoding encoding) {
    //        _encoding = encoding;
    //        Clear();
    //    }

    //    public void Write(byte[] content) { Insert(content, -1); }

    //    public void Write(string content) { Insert(content, -1); }

    //    public void Write(HTMLContainer content) { Insert(content, -1); }

    //    public void Insert(byte[] content, int index) {
    //        Insert(_encoding.GetString(content), index);
    //    }

    //    public void Insert(HTMLContainer content, int index) {
    //        if (content._isStringDirty) content.flushString();
    //        Insert(content._cleanString, index);
    //    }

    //    public void Insert(string content, int index) {
    //        if (index < 0) {
    //            _stringBuilder.Append(content);
    //        } else {
    //            _stringBuilder.Insert(index, content);
    //        }
    //        _isStringDirty = true;
    //        _isByteDirty = true;
    //        _hasContent = true;
    //    }

    //    public void Clear() {
    //        _cleanString = string.Empty;
    //        _cleanByteArray = new byte[0];
    //        _isStringDirty = false;
    //        _isByteDirty = false;
    //        _stringBuilder = new StringBuilder();
    //    }

    //    public byte[] ToArray() {
    //        if (_isByteDirty) flushByte();
    //        return _cleanByteArray;
    //    }

    //    public override string ToString() {
    //        if (_isStringDirty) flushString();
    //        return _cleanString;
    //    }

    //    private void flushString() {
    //        _cleanString = _stringBuilder.ToString();
    //        _isStringDirty = true;
    //    }

    //    private void flushByte() {
    //        _cleanByteArray = _encoding.GetBytes(_stringBuilder.ToString());
    //        _isByteDirty = true;
    //    }

    //    public void Dispose() {
    //    }
    //}
    #endregion
}
