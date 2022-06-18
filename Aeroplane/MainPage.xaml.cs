using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using control = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Aeroplane
{
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
            if(sender.TabItems.Count < 1) {
                NewTab(sender);
            }
        }

        private void NewTab(control.TabView sender) {
            var newTab = new control.TabViewItem();
            newTab.IconSource = new control.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "New Tab";

            // The Content of a TabViewItem is often a frame which hosts a page.
            Frame frame = new Frame();
            frame.Navigate(typeof(WebViewPage), newTab);
            newTab.Content = frame;

            sender.TabItems.Add(newTab);

            // Set the new tab as the selected tab
            sender.SelectedItem = newTab;
        }
    }
}
