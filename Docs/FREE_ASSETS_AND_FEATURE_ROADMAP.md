# Free asset and systems integration plan

Updated: 9 November 2026
Project: Ultimate Football Online — Unity 6.0.32f1, Android

## Important status

The repository now contains an Animator bridge, a referee decision layer, restart/card bookkeeping, and expanded statistics fields. These are integration foundations, not proof of a complete football simulation. No third-party asset package has been imported into the project, because the Unity Asset Store import flow requires access to the Unity Editor/Unity account. No Android build or device performance run is verified.

## Recommended free asset and service options

### Rigged characters and animation

1. Mixamo — https://www.mixamo.com/
   - Adobe states Mixamo is free with an Adobe ID and that characters/animations can be used royalty-free in personal, commercial, and non-profit video games.
   - Use a suitable humanoid base character, auto-rig it, then download FBX characters and individual animations.
   - Replace the prototype capsule/silhouette with the imported character prefab; configure its Animator parameters to match FootballCharacterAnimationDriver: Speed (float), IsSprinting (bool), and optional Kick, Tackle, Celebrate triggers.
   - Mixamo animations are general humanoid motions, not a complete football animation library. Football-specific dribbling, passing, shooting, tackling, goalkeeper dives, headers and ball-contact clips still need original or separately licensed assets.
   - Keep source files and license/terms evidence with the project. Do not redistribute raw asset packs separately.

2. Human Basic Motions FREE — https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271
   - Free sample with humanoid idle, walk, run, sprint, turn, jump and other retargetable clips.
   - It is a general movement pack, not football-specific. Verify the current Asset Store EULA and Unity-version compatibility before importing.

### Stadium audio and UI

1. Kenney assets — https://kenney.nl/assets
   - Prefer CC0 packs for menu/UI icons and generic interface sounds. Retain the pack license/readme in the repository.
2. SFXMint stadium crowd sounds — https://sfxmint.com/sounds/stadium-crowd
   - The catalog describes its crowd effects as CC0. Verify the license on each selected file and keep license evidence alongside the imported audio.
3. Unity top free assets — https://assetstore.unity.com/top-assets/top-free
   - Browse assets marked FREE; check exact package license, file size, URP compatibility and Android performance before import.

### Multiplayer

Use Unity's official multiplayer stack as the first prototype option:
- Netcode for GameObjects: https://docs.unity.com/ugs/en-us/manual/ngo/manual/introduction
- Lobby and Relay / Multiplayer Services: https://unity.com/features/multiplayer
- UGS pricing: https://unity.com/products/gaming-services/pricing

The published free allowances are limited and may change. Monitor bandwidth, concurrent users and connectivity hours; exceeding free limits can incur charges or block service depending on account configuration. Do not promise a permanently free 22-player production server.

Build multiplayer in stages:
1. 1v1 networked test with synchronized ball and player movement.
2. 2v2 and 5v5 with latency/reconnect testing.
3. 11v11 only after authoritative simulation, server ownership, anti-cheat, match recovery and load testing are implemented.
4. Use dedicated server authority for competitive 11v11. A player-hosted prototype is not an anti-cheat-safe production architecture.

## Code added in this repository

- FootballCharacterAnimationDriver.cs: optional Animator bridge for licensed humanoid rigs.
- FootballRefereeSystem.cs: pass-time offside check, foul/restart classification, penalty decision hook, card issue hook and penalty-area geometry helper.
- FootballMatchRules.cs: restart awards, per-player yellow-card tracking, second-yellow dismissal.
- FootballMatchStatistics.cs: additional counters for cards, saves, clearances, interceptions, free kicks, penalties, throw-ins and goal kicks.
- FootballPrototype.cs: attaches the animation bridge and triggers kick/tackle actions; offside line now includes the goalkeeper in the defender array.

## Remaining work before calling features complete

- Import and configure a real rigged football character and football-specific clips in Unity Editor.
- Add animation layers/blend trees, foot planting, ball-contact IK, goalkeeper state machine and kit variants.
- Detect fouls from actual contact/tackle events; distinguish careless/reckless/excessive force, advantage, DOGSO, handball and second-yellow cases. The current referee layer does not infer these decisions from physics automatically.
- Implement restart placement and playable restart controls for direct/indirect free kicks, penalties, throw-ins, corners, goal kicks and kick-offs.
- Connect every actual match event to statistics. New counters are available but saves, clearances, interceptions, shots-on-target and pass completion still need reliable event producers.
- Build actual front-end scenes and interactive roster/card/tactics screens. Existing squad and player-card classes are data foundations, not complete UI.
- Add a tested online service configuration and server-authoritative 22-player simulation.
- Run Unity compilation, automated play-mode tests, Android build, install, thermal/memory profiling and sustained multiplayer tests.

## Device performance target

Treat Galaxy A06-class hardware (4 GB RAM) as a low-end baseline:
- Start with 30 FPS, URP mobile settings, no real-time shadows, compressed textures and simple crowd LODs.
- Use GPU instancing for crowd silhouettes and avoid hundreds of independent animated crowd GameObjects.
- Profile CPU/GPU frame time, memory, draw calls, thermal throttling and battery drain on a physical device.
- Do not raise visual quality or crowd density until a repeatable device profile passes.