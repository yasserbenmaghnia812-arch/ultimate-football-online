# YASSER GTA

Custom project based on SanAndreasUnity.

## Upstream
- Source: https://github.com/in0finite/SanAndreasUnity
- License: MIT (see upstream LICENSE)
- Engine: Unity
- Upstream default branch: dev

## Important requirements
- This repository is an open-source reimplementation of the GTA San Andreas engine, not a distribution of the original commercial game.
- You must provide your own legally obtained GTA San Andreas game data. Original game assets are not included.
- The upstream project documents Android support and multiplayer support, including dedicated server and host/client modes.
- Use the upstream Unity version and setup instructions in its README and Docs.

## YASSER GTA custom targets
- Android APK build and device validation.
- Arabic-first UI and touch controls.
- Red cinematic splash screen for 3 seconds, with "Yasser Benmaghnia" in white.
- Private multiplayer server with a maximum of 8 players and shared invite-code authentication.
- In-game network chat attached to each spawned player.
- Client reconnection attempts after unexpected disconnects (up to 5 attempts, 3 seconds apart).
- Private-server connection panel for server IP and invite code.
- Existing upstream Android touch-control system is retained (`Assets/Scripts/UI/TouchInput.cs`); it already exposes movement, sprint, jump, aim, fire and vehicle controls.

## Implemented in this repository
- Imported the complete upstream Unity project and its submodules.
- Set the NetworkManager prefab default capacity to 8 players.
- Clamped NetManager.maxNumPlayers to a range of 1–8, preventing server startup code from requesting more than 8 slots.
- Added a runtime red splash overlay that displays “Yasser Benmaghnia” in white for 3 seconds.

## Not yet verified or completed
- The Unity Editor has not compiled this modified project in this environment.
- No Android APK has been built or installed/tested.
- Dynamic room creation, unique per-room invite codes, and persistent player accounts are not implemented; the current invite code is a shared server password.
- Original GTA San Andreas game data is not included; provide your own legally obtained files.
- The repository itself is still named `ultimate-football-online`; automatic renaming is not available through the connected GitHub actions used here.


## Private server quick configuration

- Default invite code in the NetworkManager prefab: `YASSER8`. Change it in the Unity Inspector before distributing a server build.
- Default network port: `7777` (the upstream prefab uses Telepathy transport; allow/forward TCP port 7777 on the server host).
- Server capacity: `8` maximum, enforced in both the prefab and `NetManager.maxNumPlayers`.
- Headless server launch arguments supported by the upstream command-line handler: `-startServer -portNum 7777 -maxNumPlayers 8 -scene Main`.
- Friends enter the server IP and shared invite code in the YASSER GTA private-server panel.

## Validation status

The repository source checks validate that the new scripts and prefab references exist. A Unity Editor compile, Android build, live two-device network test, and dedicated-server deployment have not yet been run in this environment. Do not treat the feature set as release-tested until those checks pass.
