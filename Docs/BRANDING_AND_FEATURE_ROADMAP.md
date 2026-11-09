# Branding, performance and feature roadmap

Owner / in-game credit: **Yasser Benmaghnia**  
Engine baseline: **Unreal Engine 5.8**  
Target: Android APK, including entry-level 4 GB RAM phones

## Current verified repository state
This document tracks repository configuration only. A setting is not proof of runtime performance; every result must be compiled and measured on a physical device.

## Performance pass — Android
- [x] Disable expensive desktop rendering paths: Lumen, ray tracing, virtual shadow maps and Substrate.
- [x] Add a conservative Android profile: 60% render resolution, reduced view distance, no dynamic shadows/post-processing, smaller texture pool and reduced foliage/particle/mesh detail.
- [x] Add an Android_Low fallback profile with 55% render resolution and a 224 MB texture streaming pool.
- [ ] Measure CPU/GPU frame time, memory and thermal throttling on the Galaxy A06; 30 FPS is a target, not a verified result. The reported 16 FPS still needs an on-device capture to identify the actual bottleneck.
- [ ] If frame time remains high, profile animation tick rates, AI perception/update frequency, physics sub-stepping, draw calls and material complexity before reducing gameplay quality further.

## Arabic and localization
- [x] Set Arabic as the initial runtime culture in the native GameInstance.
- [ ] Translate all Blueprint/UMG labels and menus using Unreal Localization Dashboard resources.
- [ ] Verify RTL layout, Arabic shaping and a bundled Arabic-capable font on Android.
- [ ] Add language selection and English fallback. Setting culture alone does not translate hard-coded widget strings.

## Camera and visual direction
- [ ] Implement a wide, dynamic broadcast camera that tracks the ball, blends smoothly between play phases and adjusts zoom without abrupt motion. This requires editing the binary match Blueprint in Unreal Editor.
- [ ] Visual palette: retain a deep purple base and saturated electric blue as the two identity accents; simplify the rest to neutral dark surfaces, restrained highlights and high-contrast white text. Avoid excessive bloom, neon gradients and competing accent colors.
- [ ] Validate camera readability and UI safe areas in landscape on a phone.

## Gameplay
- [ ] Audit movement, acceleration, collisions and animation blending.
- [ ] Tune ball mass, friction, restitution and rolling resistance.
- [ ] Implement charged shot power, loft and spin/curl.
- [ ] Add pass targeting and ball trapping.
- [ ] Add stamina drain/recovery and fatigue effects.
- [ ] Add fouls, advantage, free kicks, penalties and cards.
- [ ] Add substitutions, match clock, score and event statistics.
- [ ] Validate a full 22-player local match before online expansion.

## Online and release
- [ ] Design authoritative server simulation, lobbies, invites and reconnect handling.
- [ ] Configure and validate UE 5.8 Android SDK/NDK/JDK.
- [ ] Build, install and test a signed APK on physical hardware.
- [ ] Run sustained gameplay tests and record average FPS, 1% low FPS, memory and device temperature.

## Binary asset limitation
The match map and gameplay Blueprints are Git LFS binary assets. GitHub's text editor cannot safely alter their camera nodes, materials or widget labels. Those edits must be made in Unreal Editor after Git LFS objects are pulled; do not replace binary assets with text placeholders.
