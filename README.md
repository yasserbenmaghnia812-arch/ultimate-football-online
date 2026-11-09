# Ultimate Football Online

A mobile-first original 3D football game project built with Unity 6.

## Implemented in this first prototype
- Unity 6 project and registered main scene
- Three-second cinematic intro: deep red screen, centered white game title, animated reveal, then automatic entry into the match
- Red Android control buttons with white labels
- Runtime-generated football pitch, goal, player, ball, and follow camera
- Basic player movement and shooting interaction
- Low-end device baseline: 30 FPS target, disabled anti-aliasing and real-time shadows
- GitHub Actions workflow for Android APK builds

## Build an Android APK
1. Open the repository in Unity **6000.0.32f1** or a compatible Unity 6 release.
2. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK through Unity Hub.
3. Configure the GitHub Actions repository secret `UNITY_LICENSE` with a valid Unity license file.
4. Open **Actions → Build Android APK → Run workflow**.
5. Download the generated APK from the workflow's Artifacts section after a successful run.

A valid Unity license secret is required. The APK has not yet been built or device-tested.

## Game direction
- Platform: Android APK
- Target match format: 11 vs 11
- Modes planned: matches against AI, online multiplayer, private rooms, and friend invitations
- Interface: Arabic-first with right-to-left layout
- Optimization target: entry-level Android phones
- Backend: use free-tier services where practical; free tiers may have usage limits and sleeping instances

## Development roadmap
1. Complete the local match loop and mobile controls.
2. Add teams, player formations, goalkeeper, passing, shooting, match clock, and basic AI.
3. Build Arabic main menu, team selection, and match settings.
4. Add online rooms, invitations, and authoritative multiplayer networking.
5. Profile and test on low-memory Android devices.

**Status:** early prototype scaffold. Full 11v11 gameplay, online multiplayer, AI opponents, Arabic menus, and backend services are not implemented yet.

All teams, names, logos, kits, and assets will be original. This project does not include proprietary PES/eFootball content.
