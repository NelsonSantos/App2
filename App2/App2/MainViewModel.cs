using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {

        public static string downloadFolder = "";
        public ICommand GoToPageCommand { get; }
        public MainViewModel()
        {
            this.GoToPageCommand = new Command(async () => await this.GoToPage());
            
            this.Items = new ObservableCollection<DataItem>
            {
                new DataItem{ Description = "Link test 01", Link = "https://stackoverflow.com/questions/53738395/adding-long-press-gesture-recognizer-in-xamarin-forms"},
                new DataItem{ Description = "Link test 02", Link = ""},
                new DataItem{ Description = "Link test 03", Link = "https://www.google.com/search?q=xamarin+forms+launcher.openasync&rlz=1C5CHFA_enBR942BR942&oq=Xamarin+forms+lau&aqs=chrome.2.69i57j0i19l5j0i19i22i30l4.10609j0j7&sourceid=chrome&ie=UTF-8"},
                new DataItem{ Description = "Link test 04", Link = ""},
                new DataItem{ Description = "Link test 05", Link = "https://stackoverflow.com/questions/63364489/button-is-not-clickable-in-collectionview"},
                new DataItem{ Description = "Link test 06", Link = "https://github.com/alexrainman/CarouselView"},
                new DataItem{ Description = "Link test 07", Link = "https://stackoverflow.com/questions/53738395/adding-long-press-gesture-recognizer-in-xamarin-forms"},
                new DataItem{ Description = "Link test 08", Link = "https://www.google.com/search?q=xamarin+forms+launcher.openasync&rlz=1C5CHFA_enBR942BR942&oq=Xamarin+forms+lau&aqs=chrome.2.69i57j0i19l5j0i19i22i30l4.10609j0j7&sourceid=chrome&ie=UTF-8"},
                new DataItem{ Description = "Link test 09", Link = "https://stackoverflow.com/questions/63364489/button-is-not-clickable-in-collectionview"},
                new DataItem{ Description = "Link test 10", Link = "https://github.com/alexrainman/CarouselView"},
            };
        }

        private async Task GoToPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Page2());
        }
        
        public ObservableCollection<DataItem> Items { get; set; }
    }

    public class LabelDataTemplate : DataTemplateSelector
    {
        public DataTemplate LinkDataTemplate { get; set; }
        public DataTemplate NoLinkDataTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var dataItem = (DataItem)item;
            return string.IsNullOrEmpty(dataItem.Link) ? NoLinkDataTemplate : LinkDataTemplate;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class DataItem
    {
        public DataItem()
        {
            this.GoLinkCommand = new Command<string>(async (link) => await GoLink(link));
        }

        private async Task GoLink(string link)
        {
            await Browser.OpenAsync(link);
        }

        public ICommand GoLinkCommand { get; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class CustomCollectionView : CollectionView
    {
        public CustomCollectionView()
        {
            this.SelectionChanged += CustomCollectionView_SelectionChanged;
        }
        private void CustomCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && this.ItemTappedCommand != null && this.ItemTappedCommand.CanExecute(e.CurrentSelection.FirstOrDefault()))
            {
                if (this.SelectedItem != null)
                    this.ItemTappedCommand.Execute(e.CurrentSelection.FirstOrDefault());
            }

            if (this.IsSelectedItemNullOnTapped)
                this.SelectedItem = null;
        }
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(
            nameof(ItemTappedCommand),
            typeof(ICommand),
            typeof(CustomCollectionView),
            null);

        public ICommand ItemTappedCommand
        {
            get { return (ICommand)this.GetValue(ItemTappedCommandProperty); }
            set { this.SetValue(ItemTappedCommandProperty, value); }
        }
        public static readonly BindableProperty IsSelectedItemNullOnTappedProperty =
            BindableProperty.Create(nameof(CustomCollectionView.IsSelectedItemNullOnTapped), typeof(bool),
                typeof(CustomCollectionView), true, BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var control = bindable as CustomCollectionView;
                    control.IsSelectedItemNullOnTapped = (bool)newValue;
                });

        public bool IsSelectedItemNullOnTapped
        {
            get { return (bool)this.GetValue(IsSelectedItemNullOnTappedProperty); }
            set { this.SetValue(IsSelectedItemNullOnTappedProperty, value); }
        }
        
    }
}