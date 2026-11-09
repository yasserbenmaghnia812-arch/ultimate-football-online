# Ultimate Football Online

**An original football game project by Yasser Benmaghnia.**  
Engine: **Unreal Engine 5.8** · Foundation: [Ascertainty FC](https://github.com/grain-by-grain/ascertainty-fc)

This repository combines native C++ runtime code and upstream Blueprint gameplay assets. Planned features are not represented as completed until implemented and tested.

## Included gameplay foundation
- Football match prototype with player movement, sprint and stamina HUD
- Ball control, charged shooting and passing
- AI teammates and opponents
- Match map and minimap
- Native C++ runtime module plus existing Blueprint gameplay assets

## Project identity and target
- Game title: **Ultimate Football Online**
- Creator credit: **Yasser Benmaghnia**
- Opening: red cinematic screen, centered white game title for 3 seconds
- Target: Android APK, pending Android toolchain configuration and physical-device testing

## Android performance profile
The Android profile now uses 60% render resolution, reduced view distance, disabled dynamic shadows and costly post-processing, a 256 MB texture streaming pool, reduced mesh/particle detail, and a 30 FPS cap. An Android_Low fallback uses 55% resolution and a 224 MB texture pool. These are conservative configuration changes, not a measured FPS guarantee. The reported 16 FPS must still be profiled on-device to determine CPU/GPU bottlenecks and thermal throttling.

## Arabic language status
The native GameInstance selects Arabic as the initial culture. This does **not** automatically translate hard-coded Blueprint labels. Arabic RTL localization resources, font support and widget layout still need to be completed and verified in Unreal Editor.

## Camera and visual direction
A wide dynamic broadcast camera that tracks the ball is still pending and requires editing the binary match Blueprint in Unreal Engine 5.8. The requested visual direction is a simplified palette retaining deep purple and saturated electric blue as the main accents, with restrained neutral surfaces and white text.

## Open in Unreal Engine
1. Install Unreal Engine **5.8**.
2. Install Git and Git LFS; run `git lfs install`.
3. Clone this repository.
4. Run `git lfs pull` to retrieve the binary assets.
5. Generate IDE project files if prompted, build the `FootballEditor` target for UE 5.8, then open `Football.uproject` and press Play.

## Opening splash implementation
The native `Football` GameInstance displays a full-screen red UMG splash with the centered white title **ULTIMATE FOOTBALL ONLINE** and the creator credit **Yasser Benmaghnia**. A 3-second timer removes the splash. Compile and in-engine runtime verification are still required.

## Roadmap
See [Docs/BRANDING_AND_FEATURE_ROADMAP.md](Docs/BRANDING_AND_FEATURE_ROADMAP.md).

## Credits and licensing
Based on **Ascertainty FC** by Grain by Grain. The original README is preserved as [UPSTREAM_README.md](UPSTREAM_README.md). Preserve [LICENSE](LICENSE) and [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).
- Original Ascertainty FC content is identified by the upstream project as MIT-licensed.
- Some redistributed Unreal template content is governed by the Unreal Engine EULA, not MIT.
- Third-party Fab assets are not included and remain under their own licenses.
- Do not add unlicensed club marks, player likenesses, commentary recordings, music or other third-party assets.
