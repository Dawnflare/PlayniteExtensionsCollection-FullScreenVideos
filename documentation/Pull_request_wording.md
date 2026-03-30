### Description
This PR introduces a highly requested Fullscreen Video Playback feature to the Extra Metadata Loader. 

Currently, the WPF `MediaElement` is constrained by the parent grid (such as the Harmony theme's game details panel). This enhancement allows users to seamlessly pop the video out into a borderless, topmost, maximized window for a proper viewing experience.

### Implementation Details
* Added a new Fullscreen toggle button to the existing EML video control bar.
* Implemented a `MouseDoubleClick` event on the video surface as a secondary trigger.
* Built a seamless state-handoff: When triggered, the embedded player pauses, captures the current `TimeSpan` position, volume, and mute state, and passes them to a dynamically generated maximized WPF `Window` containing a new `MediaElement`. 
* Closing the fullscreen window (via the Escape key, double-click, or the UI exit button) returns the timestamp, volume, and mute state back to the embedded player and resumes playback seamlessly.
* **New Fullscreen Transport Controls**: Added an unobtrusive, animated control bar at the bottom of the fullscreen window. Features include:
  * Play/Pause toggle (also mapped to Spacebar and single-click).
  * Seekable timeline slider with live position updates.
  * Volume control slider and a dedicated mute button (also mapped to the 'M' key).
  * **Premium Aesthetics**: The control bar and exit button rest at a subtle 15% opacity and seamlessly fade in to 90% opacity over 200ms when hovered.
* **Bug Fixes**: Included robust WPF workarounds to ensure initial frames properly render when opening the player in a paused state, preventing black screens or unintended stream resets.

### Testing
Compiled and tested successfully locally against standard 1080p H.264 MP4 files. The audio sink states cleanly translate back to the embedded view without getting de-synced.