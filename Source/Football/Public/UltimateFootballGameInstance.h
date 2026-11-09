#pragma once

#include "CoreMinimal.h"
#include "Engine/GameInstance.h"
#include "UltimateFootballGameInstance.generated.h"

class UWorld;

UCLASS()
class FOOTBALL_API UUltimateFootballGameInstance : public UGameInstance
{
    GENERATED_BODY()

public:
    virtual void Init() override;
    virtual void Shutdown() override;

private:
    void HandlePostLoadMap(UWorld* LoadedWorld);

    bool bSplashShown = false;
};
