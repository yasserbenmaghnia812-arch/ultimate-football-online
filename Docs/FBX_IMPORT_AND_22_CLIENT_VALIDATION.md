# FBX import, football animation, referee, and 22-client validation

## What is now automated in the Unity project

The Editor pipeline at `Assets/Editor/FootballFbxImportPipeline.cs` provides a repeatable import process:

1. Put legally obtained FBX files in `Assets/Incoming/FootballAssets/`.
2. Unity imports those FBX files as Humanoid rigs and imports embedded animation clips.
3. Run **Ultimate Football → Assets → Import FBX And Build Player Prefabs**.
4. The pipeline extracts animation clips to `Assets/Animations/Football/`, builds a Humanoid Animator Controller with a locomotion blend tree and available action triggers, and generates:
   - `Assets/Resources/Footballers/BluePlayer.prefab`
   - `Assets/Resources/Footballers/RedPlayer.prefab`
   - `Assets/Resources/Footballers/NetworkBall.prefab`
5. Player and ball prefabs are prepared with NGO NetworkObject / NetworkTransform components.

The pipeline does not download or redistribute third-party assets. The FBX files must be obtained by the project owner under their own account/license. No FBX binary has been imported into this repository by this commit.

## Asset sources and licensing

- TechLabID's soccer character and action FBX files: https://github.com/TechLabID/techlabid
  - The author describes its soccer character assets as rigged, Mixamo-ready, with included soccer animation, free for personal/commercial use subject to the included LICENSE.txt and restrictions on standalone redistribution.
  - Verify the exact asset's license before shipping.
- Mixamo: https://www.mixamo.com/
  - Download FBX for Unity from the user's Adobe account. Do not commit or redistribute raw Mixamo source files.
- KinemIQ free animation library: https://kinemiq.itch.io/free-game-animations
  - The published library includes CC0 clips and clips with attribution requirements. It is mainly general humanoid motion, not a complete professional football action set. Keep the supplied credits with any imported clips.

Recommended football action names to map to the controller: Idle, Walk, Run, Sprint, Dribble, Pass, Kick/Shoot, Header, Tackle/SlideTackle, Celebrate, Dive, Save, ThrowIn. The pipeline maps only clips whose names contain those tokens; it cannot invent missing actions or convert an unrelated clip into a true football motion.

## Referee system status

The existing referee code currently supports an offside-position check at pass time, foul/restart registration APIs, penalty-area classification, yellow/red card bookkeeping, second-yellow dismissal, and restart/statistics events. It is not a complete IFAB rules engine yet. The gameplay still needs reliable foul/contact detection, handball rules (if enabled), advantage, goalkeeper handling, ball-out boundary detection, corner/goal-kick/throw-in classification, penalty shootout logic, VAR/review policy, and exhaustive event wiring. Each rule must be validated against authoritative football-law test cases before calling it complete.

## Online replication status

The project currently has Relay-backed room creation/join codes and a 22-player session capacity. That is not a synchronized 11v11 match. The Editor pipeline prepares player/ball network prefabs, but the live scene still needs the full authoritative lifecycle:

- Register the generated prefabs on every client before joining.
- Spawn exactly 22 players and one ball from the authority; do not keep duplicate locally-created teams on clients.
- Assign each human client to a player/team slot and route movement/action input to the authority.
- Make ball possession, physics, goals, match clock, restarts, referee decisions, cards, substitutions, and statistics server-authoritative.
- Replicate state with NetworkVariables/RPCs and NetworkTransform; add prediction/reconciliation for player control and ball motion.
- Handle reconnects, disconnects, host migration, and malicious input.
- Test 2 clients, then 4, then 11, then 22 separate clients. Record tick rate, RTT, packet loss, bandwidth, CPU, memory, and dropped updates.

A 22-member lobby is not a 22-client gameplay test. No live 22-client test has passed.

## Infrastructure boundaries

- GitHub stores source, licensed asset references, and CI workflows. GitHub Actions can run Unity batchmode only when the Unity license/build secrets are configured.
- Unity Editor is required to import FBX files, generate prefabs, inspect humanoid avatars, and validate animation retargeting. Render and Vercel are not Unity asset importers or Unity game servers.
- Render can host a custom authoritative game server only after a compatible headless server build exists and the Render workspace/service is selected.
- Vercel is suitable for a web dashboard, account APIs, or matchmaking companion UI, not real-time UDP gameplay hosting.
