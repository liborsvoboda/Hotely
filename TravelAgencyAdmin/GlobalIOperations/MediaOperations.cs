using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TravelAgencyAdmin.GlobalOperations {

    internal class MediaOperations {

        public static ImageSource ByteToImage(byte[] imageData) {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public static async void SaveAppScreenShot(MainWindow MainWindow) {
            try {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)MainWindow.ActualWidth, (int)MainWindow.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(MainWindow);
                encoder.Frames.Add(BitmapFrame.Create(bmp));

                SaveFileDialog dlg = new SaveFileDialog() { DefaultExt = ".png", Filter = "Image files |*.png;" };
                if (dlg.ShowDialog() == true) {
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Create);
                    encoder.Save(fs);
                    fs.Close();
                }
            } catch (Exception ex) { await MainWindow.ShowMessage(true, ex.Message + Environment.NewLine + ex.StackTrace); }
        }

        /// <summary>
        /// Important Closing connections of openned files by Form and binding this is solution for
        /// close oppened file after load Solution for All Files
        /// </summary>
        /// <param name="path"></param>
        public static BitmapImage GetImageImmediatelly(string path) {
            var image = new BitmapImage();
            using (var stream = File.OpenRead(path)) {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }

        /// <summary>
        /// Cresate Bitmap Image from DB array to Image for show preview
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static BitmapImage ArrayToImage(byte[] array) {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(array);
            image.EndInit();
            return image;
        }
    }
}