using HelixToolkit.Wpf;
using Newtonsoft.Json;
using TravelAgencyAdmin.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TravelAgencyAdmin.GlobalFunctions;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.GlobalStyles;
using System.Net;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using TravelAgencyAdmin.GlobalClasses;
using Microsoft.Win32;
using System.Net.Mail;
using HelixToolkit.Wpf.SharpDX;
using System.Net.Http;
using System.Threading;
using TravelAgencyAdmin.SystemCoreExtensions;
using CefSharp.DevTools.Network;
using System.Web;
using MahApps.Metro.Controls;
using System.Windows.Threading;
using static Xamarin.Essentials.Permissions;


namespace TravelAgencyAdmin.Pages
{
    public partial class HotelImagesListPage : UserControl {
        public static DataViewSupport dataViewSupport = new DataViewSupport();
        public static HotelImagesList selectedRecord = new HotelImagesList();


        private List<HotelImagesList> hotelImagesList = new List<HotelImagesList>();
        private List<HotelList> hotelList = new List<HotelList>();
        private int selectedHotel = 0;

        public static PhotoCollection Photos = new PhotoCollection();
        public static Photo _photo;


        public HotelImagesListPage() {
            InitializeComponent();

            _ = MediaFunctions.SetLanguageDictionary(Resources, JsonConvert.DeserializeObject<Language>(App.Setting.DefaultLanguage).Value);

            //mi_server.Header = Resources["server"].ToString();
            //mi_loadFromServer.Header = Resources["loadFromServer"].ToString();
            //mi_saveToServer.Header = Resources["saveToServer"].ToString();
            //mi_cleanLocalStorage.Header = Resources["cleanLocalStorage"].ToString();

            mi_images.Header = Resources["images"].ToString();
            lbl_default.Content = Resources["default"].ToString();
            mi_insertNew.Header = Resources["insertNew"].ToString();
            mi_deleteSelected.Header = Resources["deleteSelected"].ToString();
            mi_imageFace.Header = Resources["imageFace"].ToString();
            mi_imageColor.Header = Resources["imageColor"].ToString();
            mi_imageSize.Header = Resources["imageSize"].ToString();
            mi_imageInfo.Header = Resources["imageInfo"].ToString();


            gd_Photos.DataContext = Photos;

            _ = LoadDataList();
            SetRecord();
        }


        public async Task<bool> LoadDataList() {
            MainWindow.ProgressRing = Visibility.Visible;
            try
            {
                cb_hodelId.ItemsSource = hotelList = await ApiCommunication.GetApiRequest<List<HotelList>>(ApiUrls.HotelList, null, App.UserData.Authentification.Token);
                if (hotelList.Count > 0) { cb_hodelId.SelectedIndex = 0; } else { gd_Photos.IsEnabled = false; }
                await LoadFromServer();
            }
            catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }

        //change dataset prepare for working
        private void SetRecord() {
            MainWindow.DataGridSelected = false; MainWindow.DataGridSelectedIdListIndicator = false; MainWindow.dataGridSelectedId = selectedRecord.Id; MainWindow.DgRefresh = true;
            dataViewSupport.FormShown = false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedPhotoId" >selectedPhotoId -1 is Select Last</param>
        private void RefreshViewPhoto(int selectedPhotoId = 0) {

            if (PhotosListBox.SelectedItem != null || selectedPhotoId != 0)
            {
                if (selectedPhotoId != 0) PhotosListBox.SelectedItem = Photos.First(x => (selectedPhotoId > 0 && x.DbId == selectedPhotoId) || (selectedPhotoId == -1 && x.DbId == Photos.Max(a=> a.DbId) ));
                _photo = (Photo)PhotosListBox.SelectedItem;
                ViewedPhoto.Source = _photo.Image;

                chb_isPrimary.Checked -= IsPrimaryClick; chb_isPrimary.Unchecked -= IsPrimaryClick;
                chb_isPrimary.IsEnabled = true; chb_isPrimary.IsChecked = ((Photo)PhotosListBox.SelectedItem).IsPrimary;
                chb_isPrimary.Checked += IsPrimaryClick; chb_isPrimary.Unchecked += IsPrimaryClick;
            } else { PhotosListBox.SelectedItem = ViewedPhoto.Source = null; _photo = null; chb_isPrimary.IsEnabled = false;
                chb_isPrimary.Checked -= IsPrimaryClick; chb_isPrimary.Unchecked -= IsPrimaryClick;
                chb_isPrimary.IsEnabled = false; chb_isPrimary.IsChecked = false;
                chb_isPrimary.Checked += IsPrimaryClick; chb_isPrimary.Unchecked += IsPrimaryClick;
            }
        }

        private void OnPhotoMove(object sender, MouseEventArgs e) => RefreshViewPhoto();
        private void PhotoListBoxSelectClick(object sender, MouseButtonEventArgs e) => RefreshViewPhoto();
        private async void LoadFromServerClick(object sender, RoutedEventArgs e) => await LoadFromServer();
        private async void SaveToServerClick(object sender, RoutedEventArgs e) => await SaveImageToServer();
        private void CleanLocalStorageClick(object sender, RoutedEventArgs e) => ClearGallery();

        /// <summary>
        /// Last proccess
        /// </summary>
        /// <returns></returns>
        private async Task<bool> LoadFromServer(int selectedPhotoId = 0) {
            MainWindow.ProgressRing = Visibility.Visible;
            try
            {
                hotelImagesList = await ApiCommunication.GetApiRequest<List<HotelImagesList>>(ApiUrls.HotelImagesList, "HotelId/" + ((HotelList)cb_hodelId.SelectedItem).Id.ToString(), App.UserData.Authentification.Token);

                ClearGallery();
                hotelImagesList.ForEach(item => {
                    item.Hotel = hotelList.First(a => a.Id == item.HotelId).Name;
                    try { FileFunctions.ByteArrayToFile(Path.Combine(App.galleryFolder, item.FileName), item.Attachment); } catch { }
                    Photos.Add(Path.Combine(App.galleryFolder, item.FileName), item.Id, item.IsPrimary);
                });
                RefreshViewPhoto(selectedPhotoId);
            }
            catch (Exception autoEx) { App.ApplicationLogging(autoEx); }
            MainWindow.ProgressRing = Visibility.Hidden;return true;
        }

        private async Task<bool> SaveImageToServer(int? onlyThis = null) {
            MainWindow.ProgressRing = Visibility.Visible;
            DBResultMessage dBResult;

            try {
                if (onlyThis == null) { //Saving Full Galery 
                    foreach (Photo photo in Photos)
                    {
                        string fileName = Path.GetFileName(photo.Source);
                        selectedRecord = new HotelImagesList()
                        { Id = photo.DbId, HotelId = selectedHotel, IsPrimary = photo.IsPrimary, FileName = fileName,
                            Attachment = File.ReadAllBytes(photo.Source), UserId = App.UserData.Authentification.Id, TimeStamp = DateTimeOffset.Now.DateTime
                        };

                        string json = JsonConvert.SerializeObject(selectedRecord);
                        StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                        if (selectedRecord.Id == 0)
                        { dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelImagesList, httpContent, null, App.UserData.Authentification.Token);
                        } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelImagesList, httpContent, null, App.UserData.Authentification.Token); }

                        if (dBResult.recordCount > 0) { }
                        else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); }
                    }
                } else {  // Save Last image only
                    Photo photo = ((Photo)PhotosListBox.SelectedItem);
                    selectedRecord = new HotelImagesList()
                    {
                        Id = photo.DbId, HotelId = selectedHotel, IsPrimary = photo.IsPrimary, FileName = Path.GetFileName(photo.Source),
                        Attachment = File.ReadAllBytes(photo.Source), UserId = App.UserData.Authentification.Id, TimeStamp = DateTimeOffset.Now.DateTime
                    };

                    string json = JsonConvert.SerializeObject(selectedRecord);
                    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    if (selectedRecord.Id == 0)
                    { dBResult = await ApiCommunication.PutApiRequest(ApiUrls.HotelImagesList, httpContent, null, App.UserData.Authentification.Token);
                    } else { dBResult = await ApiCommunication.PostApiRequest(ApiUrls.HotelImagesList, httpContent, null, App.UserData.Authentification.Token); }

                    if (dBResult.recordCount > 0) { }
                    else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); return false; }
                }
                
            } catch (Exception autoEx) { App.ApplicationLogging(autoEx); return false; }
            MainWindow.ProgressRing = Visibility.Hidden; return true;
        }



        private async void InsertNewClick(object sender, RoutedEventArgs e) {
           
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images|*.jpg; *.jpeg; *.png; *.tiff; *.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (!MimeMapping.GetMimeMapping(openFileDialog.FileName).StartsWith("image/")) { await MainWindow.ShowMessage(false, await DBFunctions.DBTranslation("fileisNotImage")); }
                else {
                    try { FileFunctions.CopyFile(openFileDialog.FileName, Path.Combine(App.galleryFolder, openFileDialog.SafeFileName)); } catch { }
                    await SaveImageToServer(0);
                    await LoadFromServer(-1);
                }
            }
        }

        private async Task<bool> DeleteImageFromServer(int dbId) {
            MainWindow.ProgressRing = Visibility.Visible;
            DBResultMessage dBResult = await ApiCommunication.DeleteApiRequest(ApiUrls.HotelImagesList, dbId.ToString(), App.UserData.Authentification.Token);
            MainWindow.ProgressRing = Visibility.Hidden;

            if (dBResult.recordCount > 0) { return true; }
            else { await MainWindow.ShowMessage(false, "Exception Error : " + dBResult.ErrorMessage); return false; }
        }


        private async void DeleteSelectedClick(object sender, RoutedEventArgs e) {
            if (PhotosListBox.SelectedItem != null) {
                Photo deletePhoto= (Photo)PhotosListBox.SelectedItem;
                await DeleteImageFromServer(deletePhoto.DbId);
                await LoadFromServer();
            }
        }


        /// <summary>
        /// Phycical clear local storage and form
        /// </summary>
        private void ClearGallery() {
            Photos?.Clear();
            PhotosListBox.SelectedItem = ViewedPhoto.Source = null; _photo = null;
            try { FileFunctions.ClearFolder(App.galleryFolder); } catch { }
        }


        private async void IsPrimaryClick(object sender, RoutedEventArgs e) {
            _photo.IsPrimary = (bool)chb_isPrimary.IsChecked;int selectedPhoto = _photo.DbId;
            await SaveImageToServer(_photo.DbId);
            await LoadFromServer(selectedPhoto);
        }

        private async void SelectedHotelChanged(object sender, SelectionChangedEventArgs e) {
            selectedHotel = ((HotelList)cb_hodelId.SelectedItem).Id;
            await LoadFromServer();
        }


        private void GrayscaleClick(object sender, RoutedEventArgs e) => ImageEffects.Effects.Grayscale(_photo);
        private void NegativeClick(object sender, RoutedEventArgs e) => ImageEffects.Effects.Negative(_photo);

        
        
    }
}