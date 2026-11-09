# Branding, performance, and feature roadmap

Owner / in-game credit: **Yasser Benmaghnia**  
Engine baseline: **Unreal Engine 5.8**  
Target platform: Android APK, including entry-level phones with 4 GB RAM

## Opening screen and visual identity
- [x] Creator wordmark: **YASSER BENMAGHNIA**, with bold typography and a subtle shadow.
- [x] Very light red full-screen background; purple removed from the opening design.
- [x] Four-second opening timer, followed by a transition to the game's existing interface.
- [ ] Import a licensed TTF or OTF font as an Unreal Slate Font asset and assign it to the wordmark. The current C++ implementation uses the engine's bold typeface fallback.
- [ ] Compile and verify the opening screen and transition on Android.

## Android performance
- [x] Disable expensive desktop rendering paths, including Lumen, ray tracing, virtual shadow maps, and Substrate.
- [x] Add a conservative Android profile with 60% render resolution, reduced view distance, disabled dynamic shadows and post-processing, a smaller texture pool, and reduced foliage, particle, and mesh detail.
- [x] Add an Android_Low fallback profile with 55% render resolution and a 224 MB texture-streaming pool.
- [ ] Measure CPU/GPU frame time, memory usage, and thermal throttling on the Galaxy A06. The 30 FPS cap is a target, not a verified performance result. The reported 16 FPS still requires an on-device capture to identify the bottleneck.

## Arabic localization and text quality
- [x] Set Arabic as the initial runtime culture in the native GameInstance.
- [ ] Translate all Blueprint and UMG labels and menus using Unreal's Localization Dashboard.
- [ ] Verify right-to-left (RTL) layout, Arabic text shaping, and a bundled Arabic-capable font on Android.
- [ ] Proofread all player-facing Arabic and English text for spelling, grammar, and consistent terminology.

## Camera and visual direction
- [ ] Implement a wide, dynamic broadcast camera that tracks the ball and adjusts zoom smoothly. This requires editing the binary match Blueprint in Unreal Editor.
- [ ] Simplify the visual palette: remove purple and retain very light red accents, neutral surfaces, and high-contrast text.

## Gameplay and release
- [ ] Audit movement, acceleration, collisions, animation blending, ball physics, passing, shooting, stamina, fouls, cards, and match statistics.
- [ ] Design authoritative multiplayer simulation, lobbies, invitations, and reconnection handling.
- [ ] Configure the UE 5.8 Android SDK, NDK, and JDK; build a signed APK; and test it on physical hardware.

## Build and verification status
The repository's GitHub Actions validation checks project structure and selected configuration/text invariants. It does not compile Unreal Engine C++ code or package an Android APK. A full build requires an installed Unreal Engine 5.8 toolchain and the matching Android SDK, NDK, and JDK.

## Binary asset limitation
The match map and gameplay Blueprints are Git LFS binary assets. Camera, in-world material, and Blueprint UI changes must be made in Unreal Editor after Git LFS objects have been downloaded. Do not overwrite binary assets through GitHub's text editor.
