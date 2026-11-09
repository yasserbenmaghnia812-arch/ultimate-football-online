- [Licensed humanoid footballer import pipeline](Docs/CHARACTER_IMPORT_PIPELINE.md)
- [Free assets, licensing and feature integration roadmap](Docs/FREE_ASSETS_AND_FEATURE_ROADMAP.md)
# Ultimate Football Online

An original Unity 6 football game prototype for Android, authored by **Yasser Benmaghnia**.

- [Implementation status and remaining work](Docs/IMPLEMENTATION_STATUS.md)

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
- `Assets/Scripts/FootballCareerProfile.cs` — local career profile, progression, rewards, and save/load.
- `Assets/Scripts/FootballOffsideDetector.cs` — geometric offside-position helper (not yet wired to a referee/pass event pipeline).
- `Assets/Scripts/FootballSquadSystem.cs` — roster validation, formation selection, and substitution bookkeeping.
- `Assets/Scripts/FootballMatchStatistics.cs` — shots, passing, tackles, fouls, corners, offsides, and possession counters.
- `Assets/Scripts/FootballRuntimeQuality.cs` — Android-oriented low/balanced/high quality presets.
- `Assets/Editor/FootballBuildScript.cs` — creates a minimal runtime scene and builds Android APK.
- `Docs/RESEARCH_AND_ROADMAP.md` — design research, source links, implementation decisions, and staged roadmap.

## Unity project

- Unity editor version: **6000.0.32f1**
- Android target: ARM64 APK
- Package manifest: `Packages/manifest.json`
- The build entry point creates `Assets/Scenes/Main.unity` when missing, then invokes Unity's Android BuildPipeline.

Open the repository root as a Unity project. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK through Unity Hub.

## Build an APK with GitHub Actions

The Android build workflow is prepared, but an APK has **not** yet been produced. The latest verified run stopped at the Unity-license check before Unity Editor started, so C# compilation and Android packaging have not yet been tested.

The previous repository instructions referenced GameCI's old manual Personal-license activation flow. That route is not valid for Unity Personal: Unity's current documentation says Personal licenses are activated by signing in through Unity Hub, and manual license-request activation is not available for Personal. Do not upload a manually generated Personal `.ulf` from the obsolete activation workflow.

The preferred phone-only route is Unity Build Automation: Unity's 2026 DevOps pricing update says Unity ID users can start on the free tier without checkout, with 100 Linux Micro minutes and 200 Windows Micro minutes monthly (plus 100 Mac Standard minutes); usage beyond free limits is blocked unless a payment method is added. Unity Build Automation supports Android and connects to Git repositories. The remaining external authorization is linking this repository in the Unity Dashboard and creating an Android build configuration. I cannot accept Unity's account authorization or configure private dashboard settings through the repository API. The GitHub Actions route remains available only if a supported CI license setup is provided.

Workflow: [Build Android APK](.github/workflows/build-android.yml). A downloadable APK will only be announced after a successful run and artifact verification.

## Research references

- EA SPORTS FC 26 gameplay deep dive: competitive/authentic tuning, movement, shooting, first touch, physicality, defensive positioning, and goalkeeper decisions.
- eFootball Dream Team: player progression, skill development, tactical styles, and player-card systems.
- Unity physics documentation: Rigidbody collision detection and performance trade-offs.
- Open-source Unity football projects are listed in the research document. Their licenses and asset terms must be checked before any code or asset reuse.

Cloud Android build setup: [Docs/UNITY_CLOUD_BUILD_SETUP.md](Docs/UNITY_CLOUD_BUILD_SETUP.md). Research and roadmap: [Docs/RESEARCH_AND_ROADMAP.md](Docs/RESEARCH_AND_ROADMAP.md).

## Not yet complete — important

This repository is now a more complete prototype scaffold, **not** a finished FIFA/PES-scale game. The following still require substantial implementation, licensed assets, testing, or external service setup:

- Production-quality rigged 3D players, stadiums, kits, animations, crowds, commentary, and original UI art.
- Full football rules: offside helper exists but is not integrated; robust foul detection, free kicks, penalties, throw-ins, corners, VAR, referee positioning, and complete goalkeeper animation/decision logic remain incomplete.
- Proper physical tackling, shielding, headers, ball possession, first touch, and animation-driven contact. The current tackle is a simple close-range ball-dispossession prototype.
- Full squad/club management UI, card collection UI, progression screens, transfers, and cloud persistence. Local career save and basic squad/substitution bookkeeping are foundations only.
- Complete Arabic RTL layout, French and English translation coverage, menus, accessibility, and settings.
- Online multiplayer rooms, invites, matchmaking, authoritative server simulation, anti-cheat, reconnect, and backend services. No multiplayer service is currently connected.
- Device profiling, QA, store signing, and verified APK output. The current GitHub Actions runs stop before Unity compilation because Unity Personal license secrets are not configured.

All shipped code, models, animation, audio, club marks, kits, player identities, and UI must be original or appropriately licensed. Commercial games are gameplay-design references only; no proprietary EA/Konami code or assets are included.

## Current implementation guides

- [Online multiplayer status and room setup](Docs/ONLINE_MULTIPLAYER_STATUS.md)
- [Unity character and animation import pipeline](Docs/CHARACTER_IMPORT_PIPELINE.md)
- [Implementation status and verification](Docs/IMPLEMENTATION_STATUS.md)
