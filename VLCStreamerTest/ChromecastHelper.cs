using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace VLCStreamerTest
{
    public class ChromeCastHelper : IDisposable
    {
        private readonly LibVLC _libVLC;

        public ChromeCastHelper()
        {
            // Load native libvlc library
            Core.Initialize();
            _libVLC = new LibVLC();
            // Redirect log output to the console
            _libVLC.Log += (sender, e) => Console.WriteLine($"[{e.Level}] {e.Module}:{e.Message}");

            // Start finding chromecasts
            DiscoverChromecasts();
        }

        public List<RendererItem> RendererItems { get; set; } = new List<RendererItem>();

        public void DiscoverChromecasts()
        {
            var rendererDiscoverer = new RendererDiscoverer(_libVLC);
            rendererDiscoverer.ItemAdded += RendererDiscoverer_ItemAdded;
            rendererDiscoverer.Start();
        }

        public bool StartCasting(Uri stream, RendererItem rendererItem)
        {
            Console.WriteLine($"Starting cast with stream: {stream} on renderer {rendererItem.Name} or type {rendererItem.Type}");

            using var media = new Media(_libVLC, stream);

            using var mediaPlayer = new MediaPlayer(_libVLC);

            mediaPlayer.SetRenderer(rendererItem);

            // start the playback
            return mediaPlayer.Play(media);
        }

        protected void RendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            Console.WriteLine($"New chromecast discovered: {e.RendererItem.Name} of type {e.RendererItem.Type}");

            RendererItems.Add(e.RendererItem);
        }

        public void Dispose()
        {
            _libVLC.Dispose();
        }
    }
}
