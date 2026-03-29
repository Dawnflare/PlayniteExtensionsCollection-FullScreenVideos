# Product Requirements Document (PRD)

**Project:** Add Fullscreen Video Capability to Extra Metadata Loader

**Target Repository:** `PlayniteExtensionsCollection` (by darklinkpower)

**Extension Component:** Extra Metadata Loader (EML)

**Framework:** C# / WPF (.NET)

## 1. Overview and Objective

The Extra Metadata Loader currently plays local video trailers within an embedded WPF `MediaElement` constrained by the parent grid of the Playnite UI (e.g., the Harmony theme). The objective is to implement a seamless fullscreen viewing experience for these local `.mp4` and `.webm` files.

The user must be able to trigger fullscreen mode via two distinct methods: a dedicated UI button and a double-click action on the video surface.

## 2. Functional Requirements

### 2.1. Fullscreen Triggers (Entry)

- **Method A (Double-Click):** Implement a `MouseDoubleClick` event listener on the primary video rendering surface (the embedded `MediaElement` or its immediate parent container).
- **Method B (UI Button):** Introduce a new toggle button into the existing video control bar.
  - **Placement:** Based on the current control layout, the new button should be placed on the far-right side of the control bar, immediately following the volume slider.
  - **Iconography:** Use a standard "Maximize/Fullscreen" vector path or icon that matches the existing flat, monochromatic aesthetic of the EML control bar.

### 2.2. The Fullscreen Window Architecture

WPF restricts embedded elements from breaking out of their parent containers to cover the entire screen. Therefore, the fullscreen state must be handled by spawning a new `Window`.

- **Window Properties:** * `WindowStyle = WindowStyle.None`
  - `WindowState = WindowState.Maximized`
  - `Topmost = true`
  - `Background = Brushes.Black`
- **Video Handoff:**
  - Upon triggering fullscreen, capture the current `Source` (URI), `Position` (TimeSpan), and `IsPlaying` (Boolean) state of the embedded `MediaElement`.
  - Pause the embedded `MediaElement`.
  - Instantiate a new `MediaElement` in the fullscreen window.
  - Pass the captured `Source` and `Position` to the new `MediaElement` and resume playback if it was playing.

### 2.3. Fullscreen Controls & Exit Triggers

- **Exit Method A (Keyboard):** Pressing the `Escape` key while the fullscreen window is in focus must trigger the exit routine.
- **Exit Method B (Double-Click):** Double-clicking the video surface within the fullscreen window must trigger the exit routine.
- **Exit Method C (UI Button):** The fullscreen window must contain a minimal control bar (or overlay button) to manually exit fullscreen.
- **State Return Handoff:** * Capture the final `Position` and `IsPlaying` state from the fullscreen window.
  - Close and destroy the fullscreen window.
  - Update the original embedded `MediaElement` to the new `Position` and resume playback if it was active.

## 3. Implementation Plan for the AI Assistant

### Phase 1: XAML Modifications (UI)

1. Locate the primary XAML UserControl file responsible for the video player in the Extra Metadata Loader project (likely named something like `VideoPlayerControl.xaml` or similar within the EML views folder).
2. Add a `MouseDoubleClick` event handler to the `MediaElement` or its wrapper `Grid`.
3. Locate the `StackPanel` or `Grid` holding the control bar (Play, Autoplay, Mute, Volume).
4. Inject a new `Button` at the end of this layout stack for the Fullscreen action, utilizing a standard SVG/Path geometry for the icon.

### Phase 2: C# Code-Behind Logic

1. Open the corresponding `.xaml.cs` code-behind file.
2. Implement the event handlers for the new button click and the double-click events.
3. Create a method to handle the state capture and Window instantiation (e.g., `EnterFullscreen()`).

### Phase 3: The Fullscreen Window Class

1. Create a new WPF Window class (e.g., `FullscreenVideoWindow.xaml`).
2. Define the minimal UI for this window (a black background grid, a `MediaElement` bound to the window's dimensions, and a hidden/auto-hiding exit button).
3. Implement the constructor to accept the video URI and starting timestamp.
4. Implement the exit logic (`Escape` key listener, double-click listener) to fire an event back to the parent control containing the exit timestamp, then call `this.Close()`.

## 4. Edge Cases to Handle

- **Volume Sync:** Ensure the volume level of the fullscreen player matches the volume slider state of the embedded player.
- **Missing File/Stream Drop:** Handle exceptions gracefully if the `MediaElement` fails to initialize the source URI during the handoff.
- **Focus Loss:** Ensure the fullscreen window remains topmost unless explicitly minimized or closed by the user.