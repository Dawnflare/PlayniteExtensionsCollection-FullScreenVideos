# Fullscreen Video Capability - Extra Metadata Loader

This update adds the ability to view game videos in a borderless, maximized fullscreen window within the Extra Metadata Loader extension for Playnite.

## Features

- **Multiple Triggers**: 
  - Click the new **Fullscreen (⛶)** button in the video player's control bar.
  - **Double-click** anywhere on the video surface.
- **State Preservation**: The video continues from its current position, volume level, and play/pause state when switching between embedded and fullscreen modes.
- **Exit Methods**:
  - Press the **Escape** key.
  - **Double-click** the fullscreen video.
  - Click the **✕** overlay button in the top-right corner.
- **Auto-Looping**: Respects the "Repeat trailer videos" setting from the plugin configuration.

## Installation & Testing Instructions

### 1. Build and Import

To test these changes, you need to compile the project and manually replace the extension files in your Playnite installation.

1.  **Build the Project**:
    Open a terminal in the project root and run:
    ```powershell
    msbuild source\Generic\ExtraMetadataLoader\ExtraMetadataLoader.csproj /p:Configuration=Debug /t:Build
    ```
    This will produce `ExtraMetadataLoader.dll` in `source\Generic\ExtraMetadataLoader\bin\Debug\`.

2.  **Locate Playnite Extensions**:
    Open Playnite, go to `Main Menu > About Playnite > User data directory`.
    Navigate to the `Extensions` folder (**not** `ExtensionsData`).
    Look for a folder named `ExtraMetadataLoader` or `705fdbca-e1fc-4004-b839-1d040b8b4429` (the Extra Metadata Loader GUID).

3.  **Replace Files**:
    - **Close Playnite** completely.
    - Copy the newly built `ExtraMetadataLoader.dll` from your build output to the extensions folder, overwriting the existing one.
    - Ensure the `Localization` and `Controls` folders (if applicable) are also synced if you made XAML changes that aren't embedded.

### 2. Verification Checklist

Follow these steps to verify the feature:

1.  **Launch Playnite**: Open a game that has a video trailer.
2.  **Toggle Button**: Hover over the video to reveal the control bar. Click the ⛶ button. The video should pop into fullscreen.
3.  **Double-Click**: Exit fullscreen, then double-click the video surface. It should enter fullscreen.
4.  **Exit Triggers**: While in fullscreen, verify that **Escape**, **Double-clicking**, and the **top-right X button** all return you to the Playnite interface.
5.  **State Restore**: Pause a video at `0:10`, enter fullscreen. It should be paused at `0:10`. Play it to `0:15`, exit fullscreen. It should be playing (or paused as appropriate) at `0:15` in the embedded player.
6.  **Volume Sync**: Change the volume in Playnite. Enter fullscreen. The volume should match. Change it in fullscreen (if controls are present) and verify it persists after exiting.

## Technical Fixes Included

This branch also includes critical fixes for pre-existing build errors that were preventing successful compilation:
- Resolved `MouseDoubleClick` missing on `Grid` elements.
- Fixed a namespace collision where `ExtraMetadataLoader` (class) conflicted with the namespace.
- Fixed 18 pre-existing errors in `VideosDownloader.cs`, `SteamMetadataProvider.cs`, and `ExtraMetadataLoader.cs` related to an incomplete service-layer refactoring.
