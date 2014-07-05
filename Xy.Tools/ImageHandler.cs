using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Xy.Tools {
    public class ImageHandler : IDisposable {

        public enum ResizeType {
            stretch = 1, //拉伸一条边
            ignore = 2, //忽略一条边
            fit = 4, //适应一条边
            prune = 8, //将图像拉伸至整个框框,并去掉多余的部分,必须两边都设此属性
            open = 16, //将图像拉伸至填满整个框框(有白边),必须两边都设此属性
            close = 32 //将图像拉伸至填满整个框框(无白边),必须两边都设此属性
        }

        public struct ResizeFrame {
            public int backgroundWidth;
            public int backgroundHeight;
            public int imageX;
            public int imageY;
            public int imageWidth;
            public int imageHeight;
        }


        private string _filename;
        private string _currentType;
        private System.Drawing.Image original_image;
        private System.Drawing.Imaging.ImageFormat _format;
        private System.Drawing.Bitmap _final_image;

        public string Name { get { return _filename; } }
        public string Type { get { return _currentType; } }
        public System.Drawing.Bitmap Final_image { get { return _final_image; } }

        public ImageHandler(string name, Stream stream) {
            _filename = name;
            original_image = System.Drawing.Image.FromStream(stream);
            _final_image = new System.Drawing.Bitmap(stream);
            _currentType = name.Substring(name.LastIndexOf('.')).ToLower();
            switch (_currentType) {
                case ".jpg":
                case ".jpeg":
                    _format = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    _format = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case ".png":
                    _format = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case ".gif":
                    _format = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
            }

        }

        private ResizeFrame GetSize(int width, int height, ResizeType widthResizeMethod, ResizeType heightResizeMethod) {
            decimal original_ratio = (decimal)original_image.Width / original_image.Height;
            decimal target_ratio = (decimal)width / height;

            #region prune
            if (((widthResizeMethod & ResizeType.prune) == ResizeType.prune && (heightResizeMethod & ResizeType.prune) != ResizeType.prune) || ((widthResizeMethod & ResizeType.prune) != ResizeType.prune && (heightResizeMethod & ResizeType.prune) == ResizeType.prune)) throw new Exception("can not only prune one side");
            if ((widthResizeMethod & ResizeType.prune) == ResizeType.prune && (heightResizeMethod & ResizeType.prune) == ResizeType.prune)
                return new ResizeFrame() {
                    backgroundWidth = width, backgroundHeight = height,
                    imageWidth = original_image.Width, imageHeight = original_image.Height,
                    imageX = (width - original_image.Width) / 2, imageY = (height - original_image.Height) / 2
                };
            #endregion

            #region open
            if (((widthResizeMethod & ResizeType.open) == ResizeType.open && (heightResizeMethod & ResizeType.open) != ResizeType.open) || ((widthResizeMethod & ResizeType.open) != ResizeType.open && (heightResizeMethod & ResizeType.open) == ResizeType.open)) throw new Exception("can not only smart one side");
            if ((widthResizeMethod & ResizeType.open) == ResizeType.open && (heightResizeMethod & ResizeType.open) == ResizeType.open) {
                if (target_ratio > original_ratio) {
                    heightResizeMethod = heightResizeMethod | ResizeType.stretch;
                } else {
                    widthResizeMethod = widthResizeMethod | ResizeType.stretch;
                }
            }
            #endregion
            if (((widthResizeMethod & ResizeType.close) == ResizeType.close && (heightResizeMethod & ResizeType.close) != ResizeType.close) || ((widthResizeMethod & ResizeType.close) != ResizeType.close && (heightResizeMethod & ResizeType.close) == ResizeType.close)) throw new Exception("can not only close one side");
            if ((widthResizeMethod & ResizeType.close) == ResizeType.close && (heightResizeMethod & ResizeType.close) == ResizeType.close) {
                if (target_ratio > original_ratio) {
                    widthResizeMethod = widthResizeMethod | ResizeType.stretch;
                } else {
                    heightResizeMethod = heightResizeMethod | ResizeType.stretch;
                }
            }
            if ((widthResizeMethod & ResizeType.stretch) != ResizeType.stretch && (heightResizeMethod & ResizeType.stretch) != ResizeType.stretch) throw new Exception("please set a side resize method to strech at lest.");
            ResizeFrame _rf = new ResizeFrame();
            _rf.backgroundWidth = width;
            _rf.backgroundHeight = height;
            if ((widthResizeMethod & ResizeType.stretch) == ResizeType.stretch) {
                _rf.imageWidth = width;
                if ((heightResizeMethod & ResizeType.stretch) == ResizeType.stretch) {
                    _rf.imageHeight = height;
                    _rf.imageX = _rf.imageY = 0;
                } else {
                    _rf.imageHeight = (int)Math.Ceiling(original_image.Height * (Convert.ToDecimal(width) / original_image.Width));
                    _rf.imageX = 0;
                    _rf.imageY = (height - _rf.imageHeight) / 2;
                    if ((heightResizeMethod & ResizeType.fit) == ResizeType.fit) {
                        _rf.imageY = 0;
                        _rf.backgroundHeight = _rf.imageHeight;
                    }
                }
                return _rf;
            } else if ((heightResizeMethod & ResizeType.stretch) == ResizeType.stretch) {
                _rf.imageHeight = height;
                if ((widthResizeMethod & ResizeType.stretch) == ResizeType.stretch) {
                    _rf.imageWidth = width;
                    _rf.imageX = _rf.imageY = 0;
                } else {
                    _rf.imageWidth = (int)Math.Ceiling(original_image.Width * (Convert.ToDecimal(height) / original_image.Height));
                    _rf.imageX = (width - _rf.imageWidth) / 2;
                    _rf.imageY = 0;
                    if ((widthResizeMethod & ResizeType.fit) == ResizeType.fit) {
                        _rf.imageX = 0;
                        _rf.backgroundWidth = _rf.imageWidth;
                    }
                }
            }
            return _rf;
        }

        public void GetImage(int width, int height, ResizeType widthResizeMethod, ResizeType heightResizeMethod) {
            ResizeFrame _rf = GetSize(width, height, widthResizeMethod, heightResizeMethod);
            System.Drawing.Bitmap final_image = new System.Drawing.Bitmap(_rf.backgroundWidth, _rf.backgroundHeight);
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(final_image);
            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Transparent), new System.Drawing.Rectangle(0, 0, _rf.backgroundWidth, _rf.backgroundHeight));
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
            graphic.DrawImage(original_image, _rf.imageX, _rf.imageY, _rf.imageWidth, _rf.imageHeight);
            _final_image = final_image;
        }

        public void Save(string path) {
            _final_image.Save(path, _format);
        }

        public void Dispose() {
            if (original_image != null) original_image.Dispose();
        }
    }
}
