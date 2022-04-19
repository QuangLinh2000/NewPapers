using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var listCategory = new ObservableCollection<CategoryModel>();

            listCategory.Add(new CategoryModel()
            {
                Title = "Trang Chủ",
                LinkRss = "https://vnexpress.net/rss/tin-moi-nhat.rss"

            });
            listCategory.Add(new CategoryModel()
            {
                Title = "Thế Giới",
                LinkRss = "https://vnexpress.net/rss/the-gioi.rss"

            });
            listCategory.Add(new CategoryModel()
            {
                Title = "Thời Sự",
                LinkRss = "https://vnexpress.net/rss/thoi-su.rss"

            });

            listCategory.Add(new CategoryModel()
            {
                Title = "Giải trí",
                LinkRss = "https://vnexpress.net/rss/thoi-su.rss"

            });


            ControlPvot.ItemsSource = listCategory;

            foreach (var categoryModel in listCategory)
            {
                //lấy dữ liệu từ rss
                var http = new HttpClient();
                String content = await http.GetStringAsync(categoryModel.LinkRss);

                //LinQ Read XML
                var xDocument = XDocument.Parse(content);
                var listItem = from item in xDocument.Descendants("item")
                               select new ItemNewsPaper()
                               {
                                   title = item.Element("title").Value,
                                   thumb = getThumbLink(item.Element("description").Value),
                                   Link = item.Element("link").Value

                               };

                foreach (var item in listItem)
                {
                    categoryModel.NewsItem.Add(item);
                    //await Task.Delay(1000);
                }

            }





        }
        private String getThumbLink(String des)
        {
            if (!des.Contains("<img")) return "https://quangcaoviet.info/wp-content/uploads/2017/05/vnexpress.jpg";
            var arr = des.Split('<');
            foreach (var item in arr)
            {
                if (item.Contains("img") && item.Contains("src"))
                {
                    var arrItem = item.Split('"');

                    return arrItem[1].Trim();
                }
            }
            String result = arr.Length + "";

            return result;
        }

        private void listItemNewsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var newsItem = (sender as ListBox).SelectedItem as ItemNewsPaper;
            if (newsItem == null) return;
            Frame.Navigate(typeof(NewsDetails),newsItem.Link);
        }
    }
}
