# Unity Build Automation — Android setup

This project is prepared for a cloud Android build from the public GitHub repository:

- Repository: https://github.com/yasserbenmaghnia812-arch/ultimate-football-online
- Branch: `main`
- Unity Editor: `6000.0.32f1` (read from `ProjectSettings/ProjectVersion.txt`)
- Unity project root: repository root (the folder containing `Assets`, `Packages`, and `ProjectSettings`)
- Android package ID: `com.yasserbenmaghnia.ultimatefootball`
- Orientation: landscape
- Minimum Android API: 23
- Target architecture in project build script: ARM64

## Dashboard configuration

Unity Build Automation is a cloud service. It must be linked to the Unity account in the Unity Dashboard; a repository commit cannot authorize that account or create a dashboard configuration.

1. Open https://cloud.unity.com/ and sign in with the Unity ID that owns this project.
2. Open **DevOps → Build Automation** and launch the service for a Unity Cloud project.
3. In **Settings → Source control**, select **GitHub**. This repository is public, so use the public-repository connection option if offered. If Unity requires a token, create a fine-grained GitHub token limited to this repository with **Contents: Read-only**. Do not paste the token into ChatGPT or commit it to this repository.
4. Select `yasserbenmaghnia812-arch/ultimate-football-online` and branch `main`.
5. Create a build configuration with:
   - Target platform: **Android**
   - Unity version: **6000.0.32f1** (or auto-detect from ProjectVersion.txt)
   - Project subfolder: empty / repository root
   - Output: **APK**, not Android App Bundle (AAB), for direct installation on an Android phone
   - Scene: `Assets/Scenes/Main.unity`
   - Scripting backend: **IL2CPP**
   - Architecture: **ARM64**
   - Build type: **Release** for normal install testing; use Development Build only for diagnosis.
6. Save the configuration and start the first build. Do not enable automatic builds until the first manual build succeeds.

## Expected result

A successful Android build should provide an APK artifact in Unity Build Automation's Build History. Download and install it on a real Android device for smoke testing. The first build is not guaranteed: Unity may reveal C# compilation, package, graphics, or Android toolchain issues. Those must be corrected from the build log and rebuilt.

## Free-tier and account constraints

Unity DevOps advertises a free usage tier for Unity ID accounts. Usage beyond the free limits is blocked unless a payment method is added for pay-as-you-go. Check the current limits in Unity's official pricing page before starting repeated builds:
https://activation.unity3d.com/products/pricing-updates

## Why GitHub Actions is not the fallback yet

The repository's GitHub Actions workflow uses GameCI and requires a supported Unity CI license configuration. The previous manual Personal-license activation workflow was removed because Unity's current Personal activation flow is through Unity Hub; the obsolete manual activation action is not a valid Personal-license path. No license secrets are present, so GitHub Actions cannot compile this project at this time.

## Official Unity documentation

- Connect Git repositories: https://docs.unity.com/en-us/build-automation/get-started-with-build-automation/connect-your-version-control-system
- Configure Android builds: https://docs.unity.com/en-us/build-automation/basic-build-configuration/overview
- Unity DevOps free tier: https://activation.unity3d.com/products/pricing-updates
