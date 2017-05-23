using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Audio;
using Windows.Media.Playback;
using Windows.Media.Render;
using Windows.Storage;
using Windows.UI.Popups;

namespace LUWPer.Models
{
    class LoopPlayer
    {
        private MediaPlayer mdPlayer;

        private AudioGraphSettings audioGraphSettings = new AudioGraphSettings(AudioRenderCategory.Media)
        {
            DesiredRenderDeviceAudioProcessing = AudioProcessing.Default,
            QuantumSizeSelectionMode = QuantumSizeSelectionMode.SystemDefault,
            DesiredSamplesPerQuantum = 6
        };
        private AudioFileInputNode inputNode;
        private AudioDeviceOutputNode outputNode;
        private StorageFile loopStorageFile;
        public  AudioGraph Graph { get; private set; }
        public List<BeatInfo> DefaultSounds { get; private set; } = new List<BeatInfo>();
        public Task InitializingTasks { get; private set; }
      

        public LoopPlayer()
        {
            InitMediaPlayer();
            InitializingTasks = Initialize();
        }

        
        private async Task Initialize()
        {
            DefaultSounds = await BeatLocator.FindBeats();
        }

        private void InitMediaPlayer()
        {
            mdPlayer = Helpers.Singleton<MediaPlayer>.Instance;
            var controls = Windows.Media.SystemMediaTransportControls.GetForCurrentView();
            controls.IsEnabled = true;
            controls.IsPlayEnabled = true;
            controls.IsPauseEnabled = true;
            controls.ButtonPressed += SystemMediaTransportControls_ButtonPressed;
        }

        public async Task LoadBeat(BeatInfo beat)
        {
            if (Graph != null)
            {
                Graph.Dispose();
            }
            await InitializingTasks;
            loopStorageFile = await StorageFile.GetFileFromPathAsync(beat.FilePath);
            CreateAudioGraphResult graphResult = await AudioGraph.CreateAsync(audioGraphSettings);
            if (graphResult.Status == AudioGraphCreationStatus.Success)
            {
                Graph = graphResult.Graph;
                
            }
            var input = await Graph.CreateFileInputNodeAsync(loopStorageFile);
            if (input.Status != AudioFileNodeCreationStatus.Success)
            {
                Debug.WriteLine("Input node Create Failed " + input.Status);
            }
            var output = await Graph.CreateDeviceOutputNodeAsync();
            if (output.Status != AudioDeviceNodeCreationStatus.Success)
            {
                Debug.WriteLine("Output node create failed " + output.Status); 
            }
            outputNode = output.DeviceOutputNode;
            inputNode = input.FileInputNode;
            inputNode.LoopCount = null;
            inputNode.OutgoingGain = .5;
            inputNode.AddOutgoingConnection(outputNode);
        }

        public void PlayBeat()
        {
            if (Graph == null)
            {
                Debug.WriteLine("Play Function Can't find Instance of AudioGraph");
                return;
            }
            Graph.ResetAllNodes();
            Graph.Start();
        }

        public void StopBeat()
        {
            Graph.Stop();
            Graph.ResetAllNodes();
        }

        private void SystemMediaTransportControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {

        }
    }
}
