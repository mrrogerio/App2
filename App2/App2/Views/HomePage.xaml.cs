using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
        {
            LoadingLabel.IsVisible = false;

            //o_back.IsEnabled = o_webview.CanGoBack;
            //o_forward.IsEnabled = o_webview.CanGoForward;

        }
        void webOnNavigating(object sender, WebNavigatedEventArgs e)
        {
        }

        protected void forwardClicked()
        {
        }

        protected void backClicked()
        {
        }
    }
}