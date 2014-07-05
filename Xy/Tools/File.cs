using System;
using System.Collections.Generic;
using System.Text;

namespace Xy.Tools.IO {
    public class File {
        public static string ifNotExistsThenCreate(string path) {
            if (!System.IO.Directory.Exists(path)) {
                System.IO.Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string foundConfigurationFile(string filePath, string[] extArray) {
            for (int i = 0; i < extArray.Length; i++) {
                string _fileName = string.Format("{0}{1}.{2}", Xy.AppSetting.AppDir, filePath, extArray[i]);
                if (System.IO.File.Exists(_fileName)) {
                    return _fileName;
                }
            }
            throw new System.IO.FileNotFoundException(string.Format("cannot found file '{0}' with '{1}' ext name", filePath, string.Join(",", extArray)));
        }

        public static bool IsClientCached(string requestHeaders, DateTime contentModified) {
            if (!string.IsNullOrEmpty(requestHeaders)) {
                DateTime isModifiedSince;
                if (DateTime.TryParse(requestHeaders, out isModifiedSince)) {
                    return isModifiedSince > contentModified;
                }
            }
            return false;
        }
    }
}
