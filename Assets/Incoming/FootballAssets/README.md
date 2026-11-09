# Incoming licensed FBX assets

Place FBX files in this folder before opening the project in Unity Editor.

## Recommended inputs

- One rigged footballer FBX with a Humanoid-compatible skeleton and its textures.
- Separate FBX animation clips for Idle, Walk, Run, Sprint, Dribble, Pass, Kick/Shoot, Header, Tackle, Celebrate, Dive, Save, and ThrowIn.
- Prefer animation-only FBX files that use the same skeleton as the imported player.

## Import

1. Open the project with Unity 6000.0.32f1.
2. Copy the licensed FBX files into this folder.
3. Wait for Unity to finish importing.
4. Run **Ultimate Football → Assets → Import FBX And Build Player Prefabs**.
5. Review the generated Avatar, materials, clips, and prefabs in the Inspector.
6. Run **Ultimate Football → Validate Project**.

The import pipeline sets the FBX rig type to Humanoid and extracts animation clips. It does not fetch paid or account-gated files, bypass asset licenses, or guarantee that unrelated skeletons retarget correctly.

Do not upload Mixamo or other source assets to this public repository unless their license explicitly permits public redistribution. Use a private repository or local-only import for assets whose license permits in-game use but prohibits redistribution.
