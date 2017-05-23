using System;
using GalaSoft.MvvmLight;
using LUWPer.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace LUWPer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private LoopPlayer player;
        private bool playing;
        public ObservableCollection<BeatInfo> LoadedBeats { get; private set; } = new ObservableCollection<BeatInfo>();

        public MainViewModel()
        {
            player = new LoopPlayer();
            Initialize();
        }

        private  async Task Initialize()
        {
            await player.InitializingTasks;
            foreach (var item in player.DefaultSounds)
            {
                LoadedBeats.Add(item);
            }
        }
        
        public void GridViewItemClick(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (playing)
            {
                player.StopBeat();
                playing = false;
            }
            var control = sender as Windows.UI.Xaml.Controls.GridView;
            if (control.SelectedIndex != -1)
            {
                LoadBeat((BeatInfo)control.SelectedItem);
            }
        }

        public async void LoadBeat(BeatInfo beat)
        {
            await player.LoadBeat(beat);
        }

        public void PlayStop()
        {
            if (playing)
            {
                player.StopBeat();
                playing = false;
            }
            else
            {
                player.PlayBeat();
                playing = true;
            }
        }
    }
}
