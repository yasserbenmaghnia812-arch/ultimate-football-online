# Ultimate Football Online

**An original football game project by Yasser Benmaghnia.**  
Engine: **Unreal Engine 5.8** · Foundation: [Ascertainty FC](https://github.com/grain-by-grain/ascertainty-fc)

This repository replaces the previous Unity prototype with the Unreal Engine 5.8 Blueprint project. It retains the upstream project files and licensing notices. Planned features are not represented as completed until implemented and tested.

## Included gameplay foundation
- Playable football match prototype
- Player movement, sprint and stamina HUD
- Ball control, charged shooting and passing
- AI teammates and opponents
- Match map and minimap
- Blueprint-only project; no C++ compile step

## Project identity and target
- Game title: **Ultimate Football Online**
- Creator credit: **Yasser Benmaghnia**
- Planned intro: red cinematic screen, centered white game title for 3 seconds, then main menu
- Target: Android APK, pending Android toolchain configuration and physical-device testing

## Open in Unreal Engine
1. Install Unreal Engine **5.8**.
2. Install Git and Git LFS; run `git lfs install`.
3. Clone this repository.
4. Run `git lfs pull` to retrieve the binary assets now stored in this repository's LFS storage.
5. Open `Football.uproject` and press Play.

## Android status
Android packaging is not yet verified. Configure the UE 5.8 Android SDK, NDK and JDK toolchain, package the project, and test the APK on a physical Android device.

## Roadmap
See [Docs/BRANDING_AND_FEATURE_ROADMAP.md](Docs/BRANDING_AND_FEATURE_ROADMAP.md).

## Credits and licensing
Based on **Ascertainty FC** by Grain by Grain. The original README is preserved as [UPSTREAM_README.md](UPSTREAM_README.md). Preserve [LICENSE](LICENSE) and [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).
- Original Ascertainty FC content is identified by the upstream project as MIT-licensed.
- Some redistributed Unreal template content is governed by the Unreal Engine EULA, not MIT.
- Third-party Fab assets are not included and remain under their own licenses.
- Do not add unlicensed club marks, player likenesses, commentary recordings, music or other third-party assets.
