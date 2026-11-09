# Branding and Feature Roadmap

Owner / in-game credit: **Yasser Benmaghnia**  
Engine baseline: **Unreal Engine 5.8**  
Target: Android APK

## Status discipline
This is a plan, not a claim that every feature is implemented. Verify each item in Unreal Editor and on target hardware before marking it complete.

## Stage 0 — project baseline
- [x] Replace the Unity prototype tree with the UE 5.8 Blueprint project.
- [x] Preserve upstream README and license notices.
- [x] Transfer upstream Git LFS objects into this repository's LFS storage.
- [ ] Clone, run `git lfs pull`, compile the native Football module, and test both reveal and match maps.

## Stage 1 — identity and opening
- [x] Set the product/game display name to Ultimate Football Online.
- [x] Add a native UMG red full-screen opening splash in C++ (runtime verification pending).
- [x] Add a centered white title with a 3-second dismissal timer (runtime timing verification pending).
- [x] Add “Yasser Benmaghnia” creator credit to the opening widget (runtime verification pending).
- [x] Set Lvl_Reveal as the startup map; verify its transition to the main menu and mobile safe areas in-editor.

## Stage 2 — core gameplay
- [ ] Audit movement, acceleration, collisions and animation blending.
- [ ] Tune ball mass, friction, restitution and rolling resistance.
- [ ] Implement charged shot power, loft and spin/curl.
- [ ] Add pass targeting and ball trapping.
- [ ] Add stamina drain/recovery and fatigue effects.
- [ ] Add player attributes and role-aware tactical AI.

## Stage 3 — match rules and presentation
- [ ] Implement fouls, advantage, free kicks and penalties.
- [ ] Implement yellow/red cards and second-yellow dismissal.
- [ ] Add substitutions, match clock, score and event statistics.
- [x] Set Arabic as the initial runtime culture; full Arabic RTL widget translation and font verification remain pending.
- [ ] Add English/Arabic localization resources for all Blueprint labels and menus.
- [ ] Add a wide broadcast-style follow camera with ball tracking and adjustable zoom; requires editing and testing the match Blueprint in Unreal Editor.
- [ ] Use only licensed or original models, animations, music and commentary.

## Stage 4 — teams and online
- [ ] Build team selection, formations and squad management.
- [ ] Validate a full 22-player local match.
- [ ] Design authoritative server simulation before competitive online play.
- [ ] Add lobbies, invites, reconnect handling and match-result validation.
- [ ] Load-test network traffic and server costs.

## Stage 5 — Android and release
- [x] Add Android package name, landscape orientation and ARM64 baseline settings.
- [x] Disable costly desktop renderer features and add an Android low-end device profile targeting up to 30 FPS.
- [ ] Configure and validate UE 5.8 Android SDK/NDK/JDK.
- [ ] Profile memory, thermals and frame time on target Android hardware; 30 FPS is a target, not a verified result.
- [ ] Tune low/medium/high scalability profiles from device measurements.
- [ ] Test touch controls, UI scaling, suspend/resume and network changes.
- [ ] Produce a signed APK and verify installation and gameplay on a physical Android device.

## Build constraint
GitHub Actions can package the project only with a compatible Unreal Engine 5.8 build environment and valid Epic/Unreal build access. No licensing bypass is used.

## Current technical limitation
The match map and gameplay Blueprints are Git LFS binary assets. GitHub's text-file editor cannot safely modify their camera nodes or existing widget labels. Those changes must be made in Unreal Editor after the LFS objects are pulled; do not replace the binary assets with text placeholders.
