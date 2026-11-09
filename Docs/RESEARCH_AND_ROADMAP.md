# Ultimate Football Online — Technical Research & Build Plan

Updated: 2026-11-09

This document records an original implementation plan informed by public game-design references and Unity documentation. It does not copy proprietary code, game assets, player likenesses, club badges, commentary recordings, or protected UI from EA SPORTS FC/FIFA, eFootball/PES, or other commercial games.

## 1. Product target

- Unity 6; Android APK first.
- Full 11v11 match as the target, with local AI before online multiplayer.
- Arabic RTL, French, and English.
- Touch controls, match HUD, player switching, passing, shooting, dribbling, tackling, goalkeeper behaviour, match rules, formations, substitutions, and replay/presentation layers.
- Online rooms, friend invites, reconnect, and server-authoritative match state are later milestones.
- Target hardware includes low-memory Android devices; provide scalable quality presets and profile on physical devices.

## 2. Research findings translated into original mechanics

### EA SPORTS FC / FIFA design references
EA's FC 26 gameplay deep dive describes a split between Competitive and Authentic presets. It also documents refinements to dribbling, pace, passing, tackling, shooting, goalkeepers, positioning, first touch, and physicality. Its physicality discussion highlights shielding, strength-dependent contested space, shoulder challenges, and recovery/avoidance when players stumble. The goalkeeper section discusses reading shot trajectories and directing deflections away from dangerous areas.

Implementation implications:
- Keep two tuning profiles: **Authentic** (slower tempo, more physics influence, stronger positional AI) and **Competitive** (faster response and tighter input control).
- Make first-touch quality depend on incoming ball speed, ball height, player balance/control, body orientation, and opponent pressure.
- Make shielding and shoulder challenges depend on strength, balance, momentum, and fatigue—not a single fixed knockback.
- Give goalkeeper saves a target/deflection decision: catch, parry wide, parry to space, or block toward a teammate.
- Add player recovery and separation logic so collisions do not leave characters interpenetrating or stuck.

### eFootball / PES design references
Konami's official Dream Team pages describe level training, progression points, player progression, additional skills (up to five), position training, player fusion, team playstyles, and stat boosters. Its official overview lists PvP and AI modes, Quick Match, and Friend Match.

Implementation implications:
- Use an original player progression system with skill groups and earned points.
- Represent roles and team tactics as data, not hard-coded player-specific scripts.
- Keep player ratings as gameplay inputs; do not copy Konami's card art, layout, labels, or proprietary player database.
- Design original card tiers, typography, frames, icons, and color language.

### Unity physics guidance
Unity's Rigidbody documentation says continuous collision detection is useful for fast bodies that may tunnel through colliders, but Continuous Dynamic is the slowest mode and should be reserved for selected fast objects. Unity Physics Materials expose friction and bounciness; tune them deliberately and profile on device.

Implementation implications:
- Use Continuous Dynamic for the ball only when its speed warrants it; keep most player bodies on cheaper collision modes.
- Use a physically scaled ball (approximately 0.22 m diameter) and a ground contact point aligned to the pitch surface.
- Combine Rigidbody integration with modest aerodynamic drag, spin-induced lateral force, ground rolling resistance, and calibrated bounce.
- Do not simulate every visual detail with expensive rigidbodies. Use animation/kinematic movement for most player locomotion and selected physical interactions for tackles/contacts.

## 3. Ball model and tuning plan

Ball state:
- Mass target: about 0.43 kg.
- Radius target: about 0.11 m.
- Linear velocity and angular velocity are both meaningful.
- Forces: gravity, aerodynamic drag, spin/Magnus approximation, ground friction/rolling resistance, and collision response.
- Kicks are velocity impulses calibrated in metres/second, with lift and spin derived from player attributes and input direction.

Shot model:
- Base speed varies by shot type (pass, normal shot, power shot, volley, header).
- Modify speed by kicking power and current stamina.
- Modify aim error by finishing, balance, body orientation, movement speed, pressure, and weak-foot proficiency.
- Add spin from shot type and directional input.
- Avoid deterministic identical shots; use bounded, attribute-scaled variation.

Initial tuning is provisional. It must be validated through repeatable in-game tests and should not be presented as measured real-world data.

## 4. Player attributes and stamina

Core attributes:
- Pace, acceleration, shooting, finishing, passing, ball control, dribbling, balance, strength/physical contact, stamina, defending, tackling, awareness, jumping, and goalkeeper-specific attributes.

Stamina:
- Starts at 100.
- Sprinting, repeated high-intensity movement, pressing, tackles, and physical contests consume stamina.
- Low-intensity movement/rest restores stamina slowly.
- Fatigue gradually affects acceleration, shot power, shot accuracy, recovery, and physical contests.
- Avoid hard cliffs; use smooth multipliers and tune after playtesting.

## 5. Contact and match simulation roadmap

1. Ball-ground alignment, reliable goal volumes, and calibrated ball physics.
2. Player stamina and attribute-driven movement/shooting.
3. First touch, dribble control, shielding, shoulder contact, tackles, falls, and recovery.
4. Rigged low-poly humanoids, locomotion/ball/shot/tackle/keeper animation state machines.
5. Team AI: formation shape, support runs, marking, pressing triggers, passing lanes, defensive line, goalkeeper positioning.
6. Rules: throw-ins, corners, goal kicks, fouls, cards, offside, penalties, substitutions, and match clock.
7. Original player cards, progression, skills, tactics, and squad management.
8. Online multiplayer only after deterministic/local match state is stable; use server-authoritative ownership and validate all client actions.
9. Arabic RTL/French/English localization, audio, accessibility, device profiling, and release pipeline.

## 6. Android performance constraints

- Start with low-poly rigged models, compressed textures, LODs, limited real-time lights, and baked/simple stadium lighting.
- Pool transient effects and avoid per-frame allocations.
- Limit active rigidbodies; use simple colliders.
- Use 30 FPS as the safe baseline on entry-level devices; offer 60 FPS only after thermal and frame-time profiling.
- Profile CPU, GPU, memory, and thermal throttling on the actual target phone before setting quality defaults.

## 7. Open-source research candidates

These are study references, not automatic dependencies. Review the exact repository license, asset licenses, dependency versions, and attribution requirements before copying or integrating any code/assets:
- League-Soccer: https://github.com/awest813/League-Soccer — 3D football simulation concepts, ball physics, team AI, and match rules.
- Multiplayer-Football: https://github.com/JoonaV556/Multiplayer-Football — Unity/Photon Fusion multiplayer architecture example.
- Football-Soccer: https://github.com/NikolaDjurovic5/Football-Soccer — Unity football mechanics example.
- Unity-Soccer: https://github.com/JustArmaan/Unity-Soccer — educational Rigidbody/player/ball example.
- Physics Simulation and Animation for Virtual Football: https://reetcho.itch.io/physics-simulation-and-animation-for-virtual-football — ball spin, bounce, air effects, and shot-model concepts; verify source and asset terms before reuse.
- Mixamo FAQ: https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html — animation/character use terms; verify current Adobe terms for the exact use.

## 8. Research sources

- EA SPORTS FC 26 Gameplay Deep Dive: https://www.ea.com/games/ea-sports-fc/fc-26/news/pitch-notes-fc26-gameplay-deep-dive
- EA SPORTS FC 26 Gameplay Features: https://www.ea.com/games/ea-sports-fc/fc-26/features/fc-26-gameplay
- eFootball Dream Team: https://www.konami.com/efootball/en/page/dreamteam
- eFootball gameplay overview: https://www.konami.com/efootball/en/page/overview
- Unity 6 Rigidbody collision detection: https://docs.unity.com/en-us/engine/6000.6/script-reference/unityengine/rigidbody/collisiondetectionmode
- Unity 6 CollisionDetectionMode: https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/collisiondetectionmode
- Unity Physics Material: https://docs.unity.com/en-us/engine/6000.6/manual/physics-section/physics-overview/collision-section/collider-surfaces/class-physics-material

## 9. Current implementation status

The repository currently contains a generated pitch, 22 capsule placeholders, basic AI movement, a Rigidbody ball, basic touch controls, a three-second red intro, and a GitHub Actions Android workflow. It is not yet a complete football game. This first build increment adds a dedicated ball-physics component and player fatigue attributes; subsequent increments must replace placeholder locomotion/AI and add animation, match rules, UI, assets, and networking.

## 10. Licensing and originality

Commercial games are references for broad gameplay concepts only. All code, models, animations, audio, logos, kits, cards, names, and UI shipped in this project must be original or used under a compatible license. Do not extract assets from FIFA/EA SPORTS FC/PES/eFootball or use real commentator voices without permission.
