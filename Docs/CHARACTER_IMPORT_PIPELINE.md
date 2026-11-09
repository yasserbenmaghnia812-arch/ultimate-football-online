# Licensed footballer import pipeline

This project does not contain redistributed third-party player models or animation files. Add assets only when their licence permits use in the shipped game.

## Prefab locations

Create these folders in Unity:

- `Assets/Resources/Footballers/`
- `Assets/Resources/Footballers/Animations/`

Import and configure two humanoid prefabs, then save them with these exact names:

- `Assets/Resources/Footballers/BluePlayer.prefab`
- `Assets/Resources/Footballers/RedPlayer.prefab`

The runtime prototype loads the blue and red prefabs automatically. If either prefab is absent, that side continues using the procedural placeholder.

## Model import settings

For each FBX model:

1. In the Model Importer, set **Rig > Animation Type** to **Humanoid**.
2. Set Avatar Definition to **Create From This Model**, then Apply.
3. Open Configure and verify the bone mapping; the Avatar must be valid.
4. Create a prefab with an Animator component and assign the Animator Controller.
5. Keep the model at approximately 1.8 metres tall and orient its forward axis along local +Z.
6. Disable root motion. The prototype moves the player through its gameplay controller.

Unity's Humanoid Avatar retargeting requires a configured Humanoid Avatar on each model. See the [Unity 6.0 retargeting guide](https://docs.unity.com/en-us/engine/6000.0/manual/animation-section/animation-mecanim/avatar-creationand-setup/retargeting).

## Animator parameters

The bridge detects parameters by name and type; absent optional parameters are ignored.

- Float: `Speed`
- Bool: `IsSprinting`
- Triggers: `Kick`, `Pass`, `Tackle`, `Header`, `Dive`, `Save`, `Dribble`, `ThrowIn`, `Celebrate`

Use a locomotion Blend Tree driven by `Speed` and add transitions for the action triggers. The current gameplay controller sends kick/pass/tackle events; the other triggers are hooks for later event integration.

## Asset sources and licence checks

- [Mixamo](https://www.mixamo.com/) provides rigged humanoid characters and general-purpose motion clips under its stated licence; do not assume every search result is football-specific.
- [Human Basic Motions FREE](https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271) can help with general locomotion, not a complete football move set.
- [TechLabID free soccer assets](https://github.com/techlabid/techlabid/releases) lists a free soccer-player model/animation sample. Verify the included licence and current release contents before shipping.
- [Player Motion soccer clips](https://playermotion.games/playbook/free-soccer-animations-for-unity) offers free general-purpose samples; soccer-specific clips are generally paid. Check the licence on each downloaded file.

No character or animation asset is downloaded or imported by this repository change. Unity Editor access and the asset files are required to complete that step.
