# Implementation status — 9 November 2026

This file distinguishes code foundations from features that have been imported, built, tested and verified in Unity. A source file or helper class is not proof of a finished game feature.

## Present in source (Unity compilation not yet verified)

- Runtime-generated pitch, goals, 22-player prototype, ball physics, basic AI movement, mobile touch controls and follow camera.
- Procedural player silhouettes and simple limb swing; these are placeholders, not professional rigged footballers.
- Animator bridge for future licensed humanoid prefabs. It reads Speed and IsSprinting and supports Kick, Tackle and Celebrate triggers, but no rigged character or animation controller is imported yet.
- Tactical role positioning, player attributes, stamina, local career save, squad/card data foundations, localization and Android quality settings.
- Offside decision called when the controlled player passes; the defender line includes the goalkeeper. The geometry still does not cover all IFAB exceptions or AI-team passes.
- Referee foundation for foul registration, direct free-kick/penalty classification, restart notifications, reckless-challenge/DOGSO card inputs, per-player yellow-card tracking and second-yellow dismissal. Contact detection and restart gameplay are not fully integrated.
- Statistics fields for shots, passes, tackles, fouls, corners, offsides, possession, cards, saves, clearances, interceptions, free kicks, penalties, throw-ins and goal kicks. Some counters still have no reliable event producer and therefore are not yet complete match statistics.
- Procedural stands, crowd silhouettes, floodlight objects, advertising boards and synthesized ambient crowd audio. These are lightweight placeholders, not a production stadium.
- Three-second red intro and Android build configuration.

## Not complete

- Professional footballer models, realistic football kits, face/body customization, skeleton rigs, motion-captured football animation, ball-contact IK, goalkeeper dive/save animations, headers and celebrations.
- Full IFAB referee simulation: automatic foul/contact classification, advantage, handball, DOGSO context, referee positioning, VAR, restart placement, free-kick wall, penalty taker/keeper control, throw-ins, corners, goal kicks and all restart controls.
- Complete event-based statistics with verified pass completion, shots on target, saves, interceptions, clearances and player-level match ratings.
- Finished main menu, team selection, roster editor, player-card collection/progression screens, tactics, settings and controller remapping.
- Online services integration: authentication, matchmaking, lobbies, invites, authoritative server simulation, synchronization, reconnects, anti-cheat and 22-player 11v11.
- Actual Unity asset imports, successful Unity Editor compile, automated play-mode tests, Android package build, install test or device performance measurements.

## Build status

The prior GitHub Actions Unity build stopped at the Unity license gate before Unity Editor compilation. Recent source edits have not been validated by a Unity compile. No APK artifact is currently verified. The cloud build route still requires the Unity account and GitHub repository to be connected in Unity Build Automation.

## Free asset/service shortlist

See [FREE_ASSETS_AND_FEATURE_ROADMAP.md](FREE_ASSETS_AND_FEATURE_ROADMAP.md) for Mixamo, Human Basic Motions FREE, Kenney, free crowd audio and Unity multiplayer service options with licensing and cost cautions.

## Asset policy

Use original assets or assets whose licenses permit the intended game distribution. Do not copy proprietary FIFA/eFootball/PES player models, kits, logos, audio or animation clips.

## Engineering order

1. Establish a clean Unity compile and a first Android APK using Unity Build Automation.
2. Import a properly licensed rigged humanoid model and configure the Animator bridge.
3. Add football-specific animations, ball contact and goalkeeper states.
4. Implement a deterministic referee/restart state machine and connect every gameplay event to statistics.
5. Finish menus, squad/card/tactics flows.
6. Add server-authoritative multiplayer, beginning with 1v1 before scaling.
7. Profile on physical low-memory Android hardware and optimize before increasing crowd/player detail.
