using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GiphyIntegrationWpfApp.Models;
using GiphyRequestorLib;
using GiphyRequestorLib.Enums;

namespace GiphyIntegrationWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int IMAGE_HEIGHT = 200;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearStackPanel()
        {
            stackImages.Children.Clear();
        }

        private void AddImageToStackPanel(string imagePath)
        {
            Image newImage = new Image();

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(imagePath, UriKind.Absolute);
            src.EndInit();

            newImage.Source = src;
            newImage.Stretch = Stretch.Uniform;
            newImage.Height = IMAGE_HEIGHT;
            stackImages.Children.Add(newImage);
        }

        private async void SearchAndFetchGifsAsyc(object sender, RoutedEventArgs e)
        {
            ClearStackPanel();
            string[] paramsList;
            var searchString = searchText.Text;
            if (string.IsNullOrEmpty(searchString))
            {
                MessageBox.Show("Please enter text in Search field");
            }
            else
            {
                paramsList = new string[] { searchString };
                var urlList = await GiphyIntegrationModel.GetGiphyUrlsListAsync(EGiphyRequestTypes.Search, paramsList);
                ShowImagesInStackPanel(urlList);
            }
        }


        private async void FetchTrendingGifsAsync(object sender, RoutedEventArgs e)
        {
            ClearStackPanel();
            var urlList = await GiphyIntegrationModel.GetGiphyUrlsListAsync(EGiphyRequestTypes.Trending, null);
            ShowImagesInStackPanel(urlList);
        }

        private void ShowImagesInStackPanel(List<string> uriList)
        {
            foreach (var uriPath in uriList)
            {
                AddImageToStackPanel(uriPath);
            }
        }
    }
}
