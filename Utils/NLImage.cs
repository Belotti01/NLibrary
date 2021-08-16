using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace NL.Utils {
    public static class NLImage {

        public enum GraphicsMode {
            Normal,
            Fast,
            HighQuality
        }

        public static Graphics CreateGraphics(Image img, GraphicsMode mode = GraphicsMode.Normal)
            => CreateGraphics((Bitmap)img, mode);

        public static Graphics CreateGraphics(Bitmap img, GraphicsMode mode = GraphicsMode.Normal) {
            Graphics gx = Graphics.FromImage(img);
            LoadMode(ref gx, mode);
            return gx;
        }

        public static void LoadMode(ref Graphics graphics, GraphicsMode mode) {
            graphics.CompositingMode = CompositingMode.SourceCopy;

            graphics.CompositingQuality = mode switch {
                GraphicsMode.HighQuality => CompositingQuality.HighQuality,
                GraphicsMode.Fast => CompositingQuality.HighSpeed,
                _ => CompositingQuality.Default,
            };
            graphics.InterpolationMode = mode switch {
                GraphicsMode.HighQuality => InterpolationMode.HighQualityBicubic,
                GraphicsMode.Fast => InterpolationMode.Bilinear,
                _ => InterpolationMode.Default,
            };
            graphics.SmoothingMode = mode switch {
                GraphicsMode.HighQuality => SmoothingMode.HighQuality,
                GraphicsMode.Fast => SmoothingMode.HighSpeed,
                _ => SmoothingMode.Default
            };
            graphics.PixelOffsetMode = mode switch {
                GraphicsMode.HighQuality => PixelOffsetMode.HighQuality,
                GraphicsMode.Fast => PixelOffsetMode.HighSpeed,
                _ => PixelOffsetMode.Default,
            };
        }

        /// <summary>
        ///     High-quality resizing of an image to the specified width and height.
        /// </summary>
        /// <param name="image">
        ///     The image to resize.
        /// </param>
        /// <param name="width">
        ///     The width to resize to.
        /// </param>
        /// <param name="height">
        ///     The height to resize to.
        /// </param>
        /// <param name="resizingMode">
        ///     The speed and quality of the resizing process.
        /// </param>
        /// <returns>
        ///     The resized image.
        /// </returns>
        public static Bitmap ResizeImage(Image image, int width, int height, GraphicsMode resizingMode = GraphicsMode.Normal) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            Graphics graphics = Graphics.FromImage(destImage);
            LoadMode(ref graphics, resizingMode);

            using ImageAttributes wrapMode = new(); 
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            graphics.Dispose();

            return destImage;
        }

        /// <inheritdoc cref="ResizeImage(Image, int, int, GraphicsMode)"/>
        public static Bitmap ResizeImage(Bitmap image, int width, int height, GraphicsMode resizingMode = GraphicsMode.Normal)
            => ResizeImage(image as Image, width, height, resizingMode);

        public static void SetGrayscale(ref Bitmap bitmap) {
            Color pixel;
            for(int c = 0; c < bitmap.Width; c++) {
                for(int r = 0; r < bitmap.Height; r++) {
                    pixel = bitmap.GetPixel(c, r);
                    byte gray = (byte)(.299 * pixel.R + .587 * pixel.G + .114 * pixel.B);
                    bitmap.SetPixel(c, r, Color.FromArgb(gray, gray, gray));
                }
            }
        }

        public static void SetGrayscale(ref Image image) {
            Bitmap bm = image as Bitmap;
            SetGrayscale(ref bm);
            image = bm;
        }

        public static void ToMonochrome(ref Bitmap bitmap, Color color) {
            Color pixel;
            for(int c = 0; c < bitmap.Width; c++) {
                for(int r = 0; r < bitmap.Height; r++) {
                    pixel = bitmap.GetPixel(c, r);
                    if(pixel.A != 0) {
                        bitmap.SetPixel(c, r, color);
                    }
                }
            }
        }

        public static void ToMonochrome(ref Image image, Color color) {
            Bitmap bm = image as Bitmap;
            ToMonochrome(ref bm, color);
            image = bm;
        }

    }
}
