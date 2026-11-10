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
- Private multiplayer server with a maximum of 8 players; invite-only authentication remains to be implemented.

## Implemented in this repository
- Imported the complete upstream Unity project and its submodules.
- Set the NetworkManager prefab default capacity to 8 players.
- Clamped NetManager.maxNumPlayers to a range of 1–8, preventing server startup code from requesting more than 8 slots.
- Added a runtime red splash overlay that displays “Yasser Benmaghnia” in white for 3 seconds.

## Not yet verified or completed
- The Unity Editor has not compiled this modified project in this environment.
- No Android APK has been built or installed/tested.
- Invite-only/password authentication is not implemented yet.
- Original GTA San Andreas game data is not included; provide your own legally obtained files.
- The repository itself is still named `ultimate-football-online`; automatic renaming is not available through the connected GitHub actions used here.
