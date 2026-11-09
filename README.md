# Ultimate Football Online

An original mobile football game project using Unity 6, targeting Android.

## Current prototype implementation
- Three-second dark-red cinematic intro with **Yasser Benmaghnia** centered in white, an animated reveal line, and automatic transition to gameplay.
- Generated 3D pitch, goals, ball physics (Rigidbody), and follow camera.
- Prototype 11-player teams on both sides (22 simple capsule players), with basic formation movement and lightweight CPU behaviours.
- Mobile touch controls: movement, pass, and shoot; keyboard controls remain available in the Editor.
- Basic score, match countdown, goal detection, and kickoff reset.
- Red controls with white labels.
- Adaptive 30/60 FPS target based on device memory/CPU count; low-cost rendering defaults and a larger physics timestep in 30 FPS mode.
- GitHub Actions Android APK workflow.

## Build
1. Open with Unity **6000.0.32f1** or a compatible Unity 6 editor.
2. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK.
3. Add a valid Unity license as the repository Actions secret `UNITY_LICENSE`.
4. Run **Actions → Build Android APK → Run workflow**.
5. Download the APK artifact after a successful run.

The workflow has not yet been verified by a successful Unity build. A valid Unity license is required.

## Target product
| Feature | Target |
|---|---|
| Engine / platform | Unity 6 / Android APK |
| Match format | 11 vs 11 |
| Modes | Local match vs AI; online rooms and friend invitations |
| Languages | Arabic RTL, French, English |
| Build | GitHub Actions |
| Hosting | Free tiers where practical |

## Roadmap (not yet complete)
1. Replace capsule placeholders with rigged football characters, animations, goalkeeper logic, player switching, and proper collision/ball control.
2. Improve AI tactics, passing lanes, fouls, offside, throw-ins, corners, penalties, substitutions, and full match rules.
3. Add a localized Arabic RTL / French / English menu and settings.
4. Add online multiplayer with authoritative server simulation, room codes, invites, reconnect, and anti-cheat.
5. Add original stadium audio, crowd ambience, and commentary. A licensed recording may be used where rights are available; no unlicensed imitation or recording of a real commentator is included.
6. Profile on real Android devices and tune quality, memory, thermal behaviour, and frame pacing.

## Important status
This is a playable-direction prototype scaffold, not a finished commercial football game. The 11v11 players are simple runtime placeholders with basic AI movement; online multiplayer, full football rules, finished animations/assets, and full localization are not implemented. APK generation and device testing are still pending.

All teams, badges, names, kits, and assets should be original or properly licensed. No proprietary PES/eFootball assets are included.
