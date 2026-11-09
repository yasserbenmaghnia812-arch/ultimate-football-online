# Branding, performance and feature roadmap

Owner / in-game credit: **Yasser Benmaghnia**  
Engine baseline: **Unreal Engine 5.8**  
Target: Android APK, including entry-level 4 GB RAM phones

## Opening identity
- [x] Creator wordmark: **YASSER BENMAGHNIA**, bold typographic treatment with shadow.
- [x] Very light red full-screen opening background; purple is removed from the opening design.
- [x] Four-second opening timer, then dismiss the splash to reveal the game's existing interface.
- [ ] For a true custom font file, import a licensed TTF or OTF font into Unreal as a Slate Font asset and assign it to the wordmark. Current C++ wordmark uses the engine's bold typeface fallback.
- [ ] Compile and verify the opening sequence and transition on Android.

## Performance pass — Android
- [x] Disable expensive desktop rendering paths: Lumen, ray tracing, virtual shadow maps and Substrate.
- [x] Add a conservative Android profile: 60% render resolution, reduced view distance, no dynamic shadows/post-processing, smaller texture pool and reduced foliage/particle/mesh detail.
- [x] Add an Android_Low fallback profile with 55% render resolution and a 224 MB texture streaming pool.
- [ ] Measure CPU/GPU frame time, memory and thermal throttling on the Galaxy A06; 30 FPS is a target, not a verified result. The reported 16 FPS still needs an on-device capture to identify the actual bottleneck.

## Arabic and localization
- [x] Set Arabic as the initial runtime culture in the native GameInstance.
- [ ] Translate all Blueprint/UMG labels and menus using Unreal Localization Dashboard resources.
- [ ] Verify RTL layout, Arabic shaping and a bundled Arabic-capable font on Android.

## Camera and visual direction
- [ ] Implement a wide, dynamic broadcast camera that tracks the ball and adjusts zoom smoothly. This requires editing the binary match Blueprint in Unreal Editor.
- [ ] Simplify the palette: remove purple, retain restrained light-red accents, neutral surfaces and high-contrast text.

## Gameplay and release
- [ ] Audit movement, acceleration, collisions, animation blending, ball physics, passing, shooting, stamina, fouls, cards and match statistics.
- [ ] Design authoritative multiplayer simulation, lobbies, invites and reconnect handling.
- [ ] Configure UE 5.8 Android SDK/NDK/JDK, build a signed APK, and test on physical hardware.

## Binary asset limitation
The match map and gameplay Blueprints are Git LFS binary assets. Camera and in-world material changes must be made in Unreal Editor after Git LFS objects are pulled; do not overwrite binary assets through GitHub's text editor.