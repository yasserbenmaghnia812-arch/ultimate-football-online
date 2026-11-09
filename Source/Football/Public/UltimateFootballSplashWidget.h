#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "TimerManager.h"
#include "UltimateFootballSplashWidget.generated.h"


UCLASS()
class FOOTBALL_API UUltimateFootballSplashWidget : public UUserWidget
{
    GENERATED_BODY()

protected:
    virtual void NativeConstruct() override;

private:
    void DismissSplash();

    FTimerHandle SplashTimerHandle;
};
