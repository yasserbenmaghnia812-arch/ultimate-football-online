# Ultimate Football Online

**An original football game project by Yasser Benmaghnia.**  
Engine: **Unreal Engine 5.8** · Foundation: [Ascertainty FC](https://github.com/grain-by-grain/ascertainty-fc)

This repository combines native C++ runtime code and upstream Blueprint gameplay assets. Planned features are not represented as completed until implemented and tested.

## Included gameplay foundation
- Playable football match prototype
- Player movement, sprint and stamina HUD
- Ball control, charged shooting and passing
- AI teammates and opponents
- Match map and minimap
- Native C++ runtime module plus existing Blueprint gameplay assets

## Project identity and target
- Game title: **Ultimate Football Online**
- Creator credit: **Yasser Benmaghnia**
- Opening: red cinematic screen, centered white game title for 3 seconds
- Target: Android APK, pending Android toolchain configuration and physical-device testing

## Android performance update
The project now disables expensive desktop rendering features (Lumen, ray tracing, virtual shadow maps and Substrate) and includes an Android low-end device profile with reduced resolution, view distance, shadows and post-processing. The profile caps the frame rate at 30 FPS. This is a performance-oriented baseline, not a measured guarantee: compile and profile on the target phone before claiming a stable FPS.

## Arabic language status
The native GameInstance now selects Arabic as the initial culture. This does **not** automatically translate hard-coded Blueprint labels: Arabic RTL localization resources, fonts and widget layout still need to be added and verified in Unreal Editor.

## Camera status
A wide broadcast-style camera that tracks the ball is still pending. The match map and gameplay Blueprints are Git LFS binary assets and must be opened and edited in Unreal Engine 5.8; do not attempt to overwrite them through GitHub's text editor.

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
