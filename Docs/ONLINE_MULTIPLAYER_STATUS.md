# Online multiplayer implementation status

## Added to the Unity project

- Unity Netcode for GameObjects: `2.13.3` (Unity 6000.0 compatible release line).
- Unity Transport: `2.7.5`.
- Unity Multiplayer Services: `2.4.1`.
- Runtime `FootballOnlineRooms` component creates a Relay-backed session with a 22-player capacity, joins by join code, shows the current member count, copies an invite code, and leaves the room.
- The in-game panel is attached automatically by `FootballPrototype`.

## Required Unity Dashboard setup

1. Open the project in Unity Editor 6000.0.32f1.
2. Link the project to a Unity Cloud project.
3. Enable Authentication and Multiplayer Services / Relay for that project.
4. Confirm the project ID and environment in the Unity Services panel.
5. Build and run two Android instances on separate networks; create a room on one and join using the code on the other.

Unity's Multiplayer Services SDK provides session creation, join codes, and Relay network setup. The current implementation uses a Relay-backed session and join code. See the official [Create a session guide](https://docs.unity.com/en-us/mps-sdk/create-session) and [Join a session guide](https://docs.unity.com/en-us/mps-sdk/join-session).

## Important limits — not yet complete

- The session/room layer is implemented in source, but no live session has been tested against a linked Unity Cloud project.
- The 22-player room limit is only a lobby capacity. The current football scene still creates local GameObjects and AI players; it does not spawn/synchronize the ball, player transforms, possession, referee decisions, cards, goals, clock, or statistics through NetworkObjects/RPCs/NetworkVariables.
- A Relay room is not an authoritative dedicated game server. A production 11v11 competitive game needs server-authoritative simulation, anti-cheat validation, reconnect handling, latency compensation, and server deployment.
- Unity Multiplayer Play Mode is useful for early multi-client checks, but the official Unity manual describes simulation of up to four players on one development device; it does not prove a 22-client stress test.
- No online 11v11 match has been run or passed.

## Staged validation plan

1. Two clients: session create/join, disconnect, leave, and reconnect.
2. Two clients: synchronize one networked ball and one controlled player.
3. Four clients: synchronize possession, goals, clock, restarts, and score.
4. Eleven clients per team: server-authoritative movement, collision, ball simulation, referee, statistics, and reconnection.
5. Load test with 22 clients on separate devices or automated clients, measure server tick rate, packet loss, bandwidth, CPU, memory, and latency.
