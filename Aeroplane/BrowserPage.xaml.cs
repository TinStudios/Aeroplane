using System;
using System.Web;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using control = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Aeroplane
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserPage : Page
    {   
        public static Action NewTab;

        public BrowserPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            
            control.TabViewItem tabViewItem = ((IPasser)e.Parameter).Tab as control.TabViewItem;
            NewTab = ((IPasser)e.Parameter).Function as Action;
            webView.CoreWebView2Initialized += (sendero, eto) => {
                sendero.CoreWebView2.DocumentTitleChanged += (sendert, ett) => {
                    tabViewItem.Header = sendert.DocumentTitle;
                };

                sendero.CoreWebView2.SourceChanged += (sendert, ett) => {
                    addressBar.Text = sendert.Source;
                    backButton.IsEnabled = sendert.CanGoBack;
                    forwardButton.IsEnabled = sendert.CanGoForward;
                };

                sendero.CoreWebView2.NavigationStarting += (sendert, ett) => {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(new control.ProgressRing() { Height = 11, Width = 11, Margin = new Thickness(0, 0, 9, 0) });
                    stackPanel.Children.Add(new TextBlock() { Text = "Loading..." });

                    tabViewItem.IconSource = null;
                    tabViewItem.Header = stackPanel;

                    reloadButton.Content = "\xE711";
                };

                sendero.CoreWebView2.NavigationCompleted += (sendert, ett) => {
                    tabViewItem.Header = sendert.DocumentTitle;

                    reloadButton.Content = "\xE72C";
                };

                sendero.CoreWebView2.FaviconChanged += async (sendert, ett) => {
                    try {
                        BitmapImage bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync((await sendert.GetFaviconAsync(0)));
                        tabViewItem.IconSource = new control.ImageIconSource() { ImageSource = bitmapImage };
                    } catch { }
                };
            };
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                if(Uri.IsWellFormedUriString(((TextBox)sender).Text, UriKind.Absolute)) {
                    webView.CoreWebView2.Navigate(((TextBox)sender).Text);
                } else {
                     webView.CoreWebView2.Navigate("https://www.bing.com/search?q=" + HttpUtility.UrlEncode(((TextBox)sender).Text));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.GoForward();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(reloadButton.Content == "\xE72C") {
                webView.CoreWebView2.Reload();
            } else {
                webView.CoreWebView2.Stop();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            NewTab();
        }

        private async void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(MainPage), null);   
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click_5(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuFlyoutItem_Click_6(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuFlyoutItem_Click_7(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click_8(object sender, RoutedEventArgs e)
        {

        }
    }
}
