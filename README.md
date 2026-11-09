# Ultimate Football Online

A mobile-first, original 3D football prototype built with Unity 6.

## Current stage
This repository starts with a lightweight playable prototype scaffold:
- Runtime-generated pitch, goals, player, ball, and follow camera
- Basic player movement and ball interaction
- On-screen movement controls for Android, plus keyboard controls for desktop testing
- Android APK build workflow for GitHub Actions

## Build
1. Open this repository in Unity **6000.0 LTS** or a compatible Unity 6 release.
2. Open the project and let Unity import the files.
3. Press Play to test the prototype.
4. For Android, install Android Build Support (SDK, NDK, OpenJDK) through Unity Hub.
5. Configure the GitHub repository secret `UNITY_LICENSE` with a valid Unity Personal license file.
6. Run **Actions → Build Android APK**.

The workflow also requires the Unity license secret. An APK is not produced until a valid license is configured and the workflow completes successfully.

## Planned milestones
1. Playable local match prototype.
2. Teams, goalkeeper, passing, shooting, and basic opponent AI.
3. Match rules, menus, and team selection.
4. Online rooms and multiplayer networking.
5. Android performance and device testing.

Online multiplayer is not implemented in this initial scaffold. All names, logos, kits, and assets will be original; this project does not include proprietary PES/eFootball assets.
