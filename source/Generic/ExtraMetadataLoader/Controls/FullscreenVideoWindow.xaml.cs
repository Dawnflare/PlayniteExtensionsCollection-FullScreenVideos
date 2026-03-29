using Playnite.SDK;
using System;
using System.Windows;
using System.Windows.Input;

namespace EmlFullscreen
{
    /// <summary>
    /// Fullscreen video playback window. Spawned by VideoPlayerControl
    /// to display video trailers in a borderless, maximized window.
    /// </summary>
    public partial class FullscreenVideoWindow : Window
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private readonly TimeSpan _startPosition;
        private readonly bool _startPlaying;
        private readonly bool _shouldLoop;
        private bool _hasAppliedStartPosition;

        /// <summary>
        /// The playback position at the time the window was closed.
        /// Read by the parent VideoPlayerControl to restore state.
        /// </summary>
        public TimeSpan ExitPosition { get; private set; }

        /// <summary>
        /// Whether the video was actively playing when the window was closed.
        /// </summary>
        public bool WasPlaying { get; private set; }

        /// <summary>
        /// Creates and initializes the fullscreen video window.
        /// </summary>
        /// <param name="source">Video file URI to play.</param>
        /// <param name="startPosition">Position to seek to after media opens.</param>
        /// <param name="volume">Volume level (0.0 to 1.0).</param>
        /// <param name="startPlaying">Whether to begin playback immediately.</param>
        /// <param name="shouldLoop">Whether the video should loop on completion.</param>
        public FullscreenVideoWindow(Uri source, TimeSpan startPosition, double volume, bool startPlaying, bool shouldLoop)
        {
            InitializeComponent();

            _startPosition = startPosition;
            _startPlaying = startPlaying;
            _shouldLoop = shouldLoop;
            _hasAppliedStartPosition = false;

            fsPlayer.Volume = volume;

            try
            {
                fsPlayer.Source = source;

                if (_startPlaying)
                {
                    fsPlayer.Play();
                    WasPlaying = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to initialize fullscreen video source.");
                ExitPosition = startPosition;
                WasPlaying = false;
                Close();
            }
        }

        private void FsPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Seek to the start position once the media is loaded.
            // This must happen in MediaOpened because seeking before
            // the media is ready has no effect.
            if (!_hasAppliedStartPosition)
            {
                _hasAppliedStartPosition = true;
                fsPlayer.Position = _startPosition;
            }
        }

        private void FsPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_shouldLoop)
            {
                fsPlayer.Position = TimeSpan.Zero;
                fsPlayer.Play();
            }
            else
            {
                WasPlaying = false;
            }
        }

        private void TogglePlayPause()
        {
            if (WasPlaying)
            {
                fsPlayer.Pause();
                WasPlaying = false;
            }
            else
            {
                fsPlayer.Play();
                WasPlaying = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseFullscreen();
            }
            else if (e.Key == Key.Space)
            {
                TogglePlayPause();
                e.Handled = true;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                TogglePlayPause();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CloseFullscreen();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CloseFullscreen();
        }

        private void CloseFullscreen()
        {
            try
            {
                ExitPosition = fsPlayer.Position;
                // WasPlaying is already tracked; capture final state
                fsPlayer.Stop();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error capturing fullscreen exit state.");
                ExitPosition = TimeSpan.Zero;
                WasPlaying = false;
            }

            Close();
        }
    }
}
