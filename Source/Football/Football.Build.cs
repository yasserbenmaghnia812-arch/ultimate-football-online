using UnrealBuildTool;

public class Football : ModuleRules
{
    public Football(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        PublicDependencyModuleNames.AddRange(new string[]
        {
            "Core", "CoreUObject", "Engine", "InputCore", "UMG", "Slate", "SlateCore"
        });
    }
}
