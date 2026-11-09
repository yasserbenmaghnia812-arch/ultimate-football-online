#include "UltimateFootballGameInstance.h"

#include "UltimateFootballSplashWidget.h"
#include "Engine/World.h"
#include "UObject/CoreUObjectDelegates.h"
#include "Blueprint/UserWidget.h"

void UUltimateFootballGameInstance::Init()
{
    Super::Init();
    FCoreUObjectDelegates::PostLoadMapWithWorld.AddUObject(this, &UUltimateFootballGameInstance::HandlePostLoadMap);
}

void UUltimateFootballGameInstance::Shutdown()
{
    FCoreUObjectDelegates::PostLoadMapWithWorld.RemoveAll(this);
    Super::Shutdown();
}

void UUltimateFootballGameInstance::HandlePostLoadMap(UWorld* LoadedWorld)
{
    if (bSplashShown || !LoadedWorld || (LoadedWorld->WorldType != EWorldType::Game && LoadedWorld->WorldType != EWorldType::PIE))
    {
        return;
    }

    bSplashShown = true;
    if (UUltimateFootballSplashWidget* Splash = CreateWidget<UUltimateFootballSplashWidget>(LoadedWorld, UUltimateFootballSplashWidget::StaticClass()))
    {
        Splash->AddToViewport(10000);
    }
}
