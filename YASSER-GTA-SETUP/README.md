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

- No usable invite secret is committed to this public repository. Set a private code in the NetworkManager prefab Inspector (`serverInviteCode`) or set the dedicated server environment variable `YASSER_GTA_INVITE_CODE`. Never commit the real invite code.
- Default network port: `7777` (the upstream prefab uses Telepathy transport; allow/forward TCP port 7777 on the server host).
- Server capacity: `8` maximum, enforced in both the prefab and `NetManager.maxNumPlayers`.
- Headless server launch arguments supported by the upstream command-line handler: `-startServer -portNum 7777 -maxNumPlayers 8 -scene Main`.
- Friends enter the server IP and the private shared invite code in the YASSER GTA private-server panel.

## Validation status

The repository source checks validate that the new scripts and prefab references exist. A Unity Editor compile, Android build, live two-device network test, and dedicated-server deployment have not yet been run in this environment. Do not treat the feature set as release-tested until those checks pass.


## Automated build pipeline

The repository includes `.github/workflows/build-yasser-gta.yml`:
- Builds an Android APK and a Linux x86_64 dedicated-server binary with Unity `2020.3.26f1`.
- Uses the GameCI Unity Builder and uploads each output as a GitHub Actions artifact.
- Requires a valid Unity CI license stored as the repository Actions secret `UNITY_LICENSE`. Optional `UNITY_EMAIL` and `UNITY_PASSWORD` secrets may be required by the selected Unity license flow.
- If `UNITY_LICENSE` is absent, the workflow reports the missing prerequisite and skips both builds; it does not create a fake APK.

## Dedicated server deployment requirements

- The server binary is a Linux x86_64 standalone build. Launch it headlessly with: `-batchmode -nographics -startServer -portNum 7777 -maxNumPlayers 8 -scene Main`.
- The current prefab uses Telepathy TCP transport on port `7777`; the host must accept public inbound TCP connections on that port.
- Set `YASSER_GTA_INVITE_CODE` as a private environment variable on the server host. Do not put the real code in Git.
- The server host must also have the game data required by SanAndreasUnity. The upstream project does not distribute the commercial GTA San Andreas data files.
- Render Free Web Services are HTTP-proxy services and spin down after 15 minutes without inbound HTTP/WebSocket traffic; they are not a suitable public raw-TCP game-server endpoint for the current Telepathy transport. A reachable Linux host/VPS with inbound TCP 7777 is still required.

The build workflow is configured but has not produced artifacts until a valid Unity license secret is supplied. A dedicated server is not considered deployed until a reachable host is configured and a real client connection is tested.
