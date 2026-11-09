# Ultimate Football Online

An original Unity 6 football game prototype for Android, authored by **Yasser Benmaghnia**.

## Current implemented prototype

- Runtime-generated football pitch, goal frames, match camera, and a three-second red cinematic intro with the creator name in white.
- 11 players per side (22 total), currently represented by procedural low-poly humanoid silhouettes rather than imported rigged assets.
- Basic tactical role targets for goalkeeper, defenders, midfielders, and strikers; support movement, formation recovery, player spacing, and lightweight CPU behaviour.
- Player switching, movement, sprint, passing, shooting, and a close-range ball-dispossession/tackle prototype.
- Rigidbody ball with a dedicated component for air drag, spin force, rolling resistance, speed limits, and calibrated kick impulses.
- Player ratings and stamina affect movement and shooting. Fatigue and sprint drain are prototype tuning values.
- Basic two-half match state, score events, goal-height check, kickoff reset, and simple match timer.
- Original player-card data model with progression points and attribute upgrades.
- Arabic / French / English string foundation; Arabic is the default and language preference is saved locally.
- Touch controls and keyboard fallback.
- Adaptive 30/60 FPS target and low-cost rendering defaults.

## Repository layout

- `Assets/Scripts/FootballPrototype.cs` — runtime prototype and touch controls.
- `Assets/Scripts/AdvancedFootballPhysics.cs` — ball physics component.
- `Assets/Scripts/FootballPlayerAttributes.cs` — ratings, stamina, fatigue and shot tuning.
- `Assets/Scripts/FootballTacticalAI.cs` — role-based positioning helpers.
- `Assets/Scripts/FootballCharacterVisual.cs` — procedural player silhouette and simple limb motion.
- `Assets/Scripts/FootballMatchRules.cs` — match state, score and discipline event foundation.
- `Assets/Scripts/FootballPlayerCard.cs` — ScriptableObject player-card and progression model.
- `Assets/Scripts/FootballLocalization.cs` — Arabic/French/English string foundation.
- `Assets/Editor/FootballBuildScript.cs` — creates a minimal runtime scene and builds Android APK.
- `Docs/RESEARCH_AND_ROADMAP.md` — design research, source links, implementation decisions, and staged roadmap.

## Unity project

- Unity editor version: **6000.0.32f1**
- Android target: ARM64 APK
- Package manifest: `Packages/manifest.json`
- The build entry point creates `Assets/Scenes/Main.unity` when missing, then invokes Unity's Android BuildPipeline.

Open the repository root as a Unity project. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK through Unity Hub.

## Build an APK with GitHub Actions

1. Create or activate a valid Unity license.
2. In GitHub, open **Settings → Secrets and variables → Actions**.
3. Add repository secret `UNITY_LICENSE` containing the Unity license file content in the format supported by GameCI.
4. Open **Actions → Build Android APK → Run workflow** (or push to `main`).
5. Download `Ultimate-Football-Android-APK` from a successful run's artifacts.

The workflow uses GameCI Unity Builder v4 and the custom static method `FootballBuildScript.BuildAndroid`. A successful build still depends on a valid Unity license and a successful Unity import/compile/build. No APK is claimed until a successful run and artifact are verified.

## Research references

- EA SPORTS FC 26 gameplay deep dive: competitive/authentic tuning, movement, shooting, first touch, physicality, defensive positioning, and goalkeeper decisions.
- eFootball Dream Team: player progression, skill development, tactical styles, and player-card systems.
- Unity physics documentation: Rigidbody collision detection and performance trade-offs.
- Open-source Unity football projects are listed in the research document. Their licenses and asset terms must be checked before any code or asset reuse.

See [Docs/RESEARCH_AND_ROADMAP.md](Docs/RESEARCH_AND_ROADMAP.md).

## Not yet complete — important

This repository is now a more complete prototype scaffold, **not** a finished FIFA/PES-scale game. The following still require substantial implementation, licensed assets, testing, or external service setup:

- Production-quality rigged 3D players, stadiums, kits, animations, crowds, commentary, and original UI art.
- Full football rules: robust offside detection, fouls, free kicks, penalties, throw-ins, corners, substitutions, VAR, referee positioning, and complete goalkeeper animation/decision logic.
- Proper physical tackling, shielding, headers, ball possession, first touch, and animation-driven contact. The current tackle is a simple close-range ball-dispossession prototype.
- Full squad/club management, card collection UI, progression screens, transfers, and persistence.
- Complete Arabic RTL layout, French and English translation coverage, menus, accessibility, and settings.
- Online multiplayer rooms, invites, matchmaking, authoritative server simulation, anti-cheat, reconnect, and backend services. No multiplayer service is currently connected.
- Device profiling, QA, store signing, and verified APK output.

All shipped code, models, animation, audio, club marks, kits, player identities, and UI must be original or appropriately licensed. Commercial games are gameplay-design references only; no proprietary EA/Konami code or assets are included.
