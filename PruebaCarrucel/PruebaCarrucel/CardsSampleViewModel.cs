using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using FFImageLoading;
using PanCardView.Extensions;
using System.Threading.Tasks;
using System;

namespace PruebaCarrucel
{
    public sealed class CardsSampleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _currentIndex;
        private int _ImageCount = 500;

        public CardsSampleViewModel()
        {
            Items = new ObservableCollection<object>
            {
                new { Source = CreateSource(), Ind = _ImageCount++, Color = Color.Red },
                new { Source = CreateSource(), Ind = _ImageCount++, Color = Color.Green },
                new { Source = CreateSource(), Ind = _ImageCount++, Color = Color.Gold },
                new { Source = CreateSource(), Ind = _ImageCount++, Color = Color.Silver },
                new { Source = CreateSource(), Ind = _ImageCount++, Color = Color.Blue }
            };

            PanPositionChangedCommand = new Command(v =>
            {
                var val = (bool)v;
                if (val)
                {
                    CurrentIndex += 1;
                    return;
                }

                CurrentIndex -= 1;
            });

            RemoveCurrentItemCommand = new Command(() =>
            {
                if (!Items.Any())
                {
                    return;
                }
                Items.RemoveAt(CurrentIndex.ToCyclingIndex(Items.Count));
            });
        }

        public ICommand PanPositionChangedCommand { get; }

        public ICommand RemoveCurrentItemCommand { get; }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
            }
        }

        public ObservableCollection<object> Items { get; }

        private string CreateSource()
        {
            var source = $"https://picsum.photos/500/500?image={_ImageCount}";
            return source;
        }



        bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }


        ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            Items.Clear();

            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {

                for (int i = 0; i < 100; i++)
                    Items.Add(DateTime.Now.AddMinutes(i).ToString("F"));

                IsBusy = false;

                //page.DisplayAlert("Refreshed", "You just refreshed the page! Nice job!", "OK");
                return false;
            });
        }

    }
}
