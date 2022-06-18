using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using control = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Aeroplane
{

    interface IPasser
    {
        control.TabViewItem Tab { get; set; }
        Action Function { get; set; }
    }

    class Passer : IPasser
    {
        public Passer(control.TabViewItem tab, Action function)
        {
            Tab = tab;
            Function = function;
        }

        public control.TabViewItem Tab { get; set; }

        public Action Function { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            Window.Current.SetTitleBar(CustomDragRegion);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            NewTab(browserTabs);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayRightInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayLeftInset;
            }
            else
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayLeftInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
            }

            CustomDragRegion.Height = ShellTitlebarInset.Height = sender.Height;
        }

        // Add a new Tab to the TabView
        private void TabView_AddTabButtonClick(control.TabView sender, object args)
        {
            NewTab(sender);
        }

        // Remove the requested tab from the TabView
        private void TabView_TabCloseRequested(control.TabView sender, control.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
            if (sender.TabItems.Count < 1)
            {
                NewTab(sender);
            }
        }

        private async void NewTab(control.TabView sender)
        {
            control.TabViewItem newTab = new control.TabViewItem();
            newTab.IconSource = new control.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Tab";

            // Join arguments
            IPasser passer = new Passer(newTab, () => NewTab(sender));

            // The Content of a TabViewItem is often a frame which hosts a page.
            Frame frame = new Frame();
            frame.Navigate(typeof(BrowserPage), passer);
            newTab.Content = frame;

            sender.TabItems.Add(newTab);

            // Set the new tab as the selected tab
            sender.SelectedItem = newTab;
        }
    }
}
