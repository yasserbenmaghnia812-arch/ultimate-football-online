#pragma once

#include "CoreMinimal.h"
#include "Blueprint/UserWidget.h"
#include "UltimateFootballSplashWidget.generated.h"

class UCanvasPanel;
class UTimerHandle;

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
