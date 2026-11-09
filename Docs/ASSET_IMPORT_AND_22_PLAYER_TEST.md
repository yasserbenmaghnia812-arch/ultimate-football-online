# FBX, football animation and 22-player validation

## Current verified repository state

The repository contains an Animator bridge and procedural placeholder players. It does not currently contain licensed FBX player models, football animation clips, an Animator Controller, or a tested 22-client match. Do not describe the current build as having professional 3D players or playable online 11v11.

## Import a real player model

1. Obtain a rigged humanoid FBX whose licence explicitly permits use in a commercial game. Keep the licence and attribution where required.
2. In Unity, place the FBX and permitted textures under Assets/ThirdParty/Footballers/.
3. Open Unity 6 and run the menu: Ultimate Football → Import Pipeline → Build Player Prefabs From FBX.
4. The importer sets the model to Humanoid, creates an Avatar from the model and enables animation import.
5. Inspect the Avatar in Model Importer. It must pass Humanoid Configure validation; correct bone mapping if required.
6. The menu creates Assets/Resources/Footballers/BluePlayer.prefab. Duplicate it as RedPlayer.prefab and assign a separate team material. Confirm an Animator is present.
7. Add licensed football clips: locomotion, sprint, dribble, pass, shot, tackle, header, goalkeeper dive/save, celebration and throw-in. Configure clip ranges and loop settings.
8. Create an Animator Controller with Speed, IsSprinting and triggers Kick, Pass, Dribble, Tackle, Header, Dive, Save, ThrowIn and Celebrate. Assign it to the imported Animator.
9. Test each clip in Unity and on a low-end Android device. Verify scale, foot contact, root motion, transitions and frame rate.

The editor pipeline configures and packages files already present. It does not download or license third-party binary assets.

## Referee integration required

A complete Laws of the Game implementation must evaluate events from authoritative simulation:
- Ball crossing touchlines and goal lines; goal versus goal kick/corner.
- Offside position at the exact teammate pass frame, plus involvement/interference before sanction.
- Contact fouls, advantage, penalty-area location, reckless/excessive-force challenges, DOGSO and second-yellow dismissal.
- Direct/indirect free kicks, penalties, throw-ins, corners, goal kicks and kick-off.
- Half-time/full-time, substitutions, goalkeeper handling and restart restrictions.

Current referee code contains offside geometry, foul/card/restart helpers, but no validated contact adjudication, advantage, complete offside involvement logic, or full out-of-play integration. It remains a prototype.

## Online 11v11 gates

FootballOnlineRooms currently creates a Relay-backed 22-member session. FootballNetworkMatchState provides server-owned score, clock, phase and event variables once attached to a spawned NetworkObject. This is not yet a complete online match.

Before calling the game online 11v11:
1. Create a NetworkObject prefab for every player and the ball; add NetworkTransform or a custom tick-based snapshot synchronizer.
2. Spawn all 22 players and the ball only on the host/server. Do not create independent local match simulations on clients.
3. Make the server authoritative for movement, possession, ball physics, goals, offside, fouls, cards, restarts, substitutions and clock.
4. Replicate team/role assignments, input commands, snapshots, events and reconnect state. Validate client input on the server.
5. Add interpolation, bounded extrapolation, latency measurement, packet-loss recovery and anti-cheat checks.
6. Run automated 22-client load tests on separate processes/devices. Measure tick rate, RTT, jitter, packet loss, bandwidth, server CPU/RAM and mobile frame time. A Relay room with 22 slots is not evidence of 22 synchronized players.

## Test report template

Record Unity Editor version, commit SHA, Android device model, APK SHA-256, test date, client count, average/p95 RTT, packet loss, server tick rate, FPS, memory, CPU, reconnect outcome and desync incidents. Mark a test passed only when evidence is captured from an actual run.
