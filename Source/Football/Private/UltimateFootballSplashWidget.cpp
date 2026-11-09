#include "UltimateFootballSplashWidget.h"

#include "Blueprint/WidgetTree.h"
#include "Fonts/SlateFontInfo.h"
#include "Components/Border.h"
#include "Components/CanvasPanel.h"
#include "Components/CanvasPanelSlot.h"
#include "Components/TextBlock.h"
#include "Components/VerticalBox.h"
#include "Components/VerticalBoxSlot.h"
#include "Engine/World.h"
#include "TimerManager.h"

void UUltimateFootballSplashWidget::NativeConstruct()
{
    Super::NativeConstruct();
    if (!WidgetTree) return;

    UCanvasPanel* Root = WidgetTree->ConstructWidget<UCanvasPanel>(UCanvasPanel::StaticClass(), TEXT("SplashRoot"));
    WidgetTree->RootWidget = Root;

    // Very light red cinematic opening; purple is intentionally removed.
    UBorder* Background = WidgetTree->ConstructWidget<UBorder>(UBorder::StaticClass(), TEXT("LightRedBackground"));
    Background->SetBrushColor(FLinearColor(1.0f, 0.66f, 0.69f, 1.0f));
    UCanvasPanelSlot* BackgroundSlot = Root->AddChildToCanvas(Background);
    BackgroundSlot->SetAnchors(FAnchors(0.0f, 0.0f, 1.0f, 1.0f));
    BackgroundSlot->SetOffsets(FMargin(0.0f));

    UVerticalBox* Content = WidgetTree->ConstructWidget<UVerticalBox>(UVerticalBox::StaticClass(), TEXT("SplashContent"));
    Background->SetContent(Content);

    UTextBlock* Eyebrow = WidgetTree->ConstructWidget<UTextBlock>(UTextBlock::StaticClass(), TEXT("StudioEyebrow"));
    Eyebrow->SetText(FText::FromString(TEXT("AN ORIGINAL FOOTBALL EXPERIENCE")));
    Eyebrow->SetColorAndOpacity(FSlateColor(FLinearColor(0.38f, 0.06f, 0.09f, 0.9f)));
    Eyebrow->SetJustification(ETextJustify::Center);
    FSlateFontInfo EyebrowFont = Eyebrow->GetFont();
    EyebrowFont.Size = 13;
    EyebrowFont.TypefaceFontName = FName(TEXT("Bold"));
    Eyebrow->SetFont(EyebrowFont);
    UVerticalBoxSlot* EyebrowSlot = Content->AddChildToVerticalBox(Eyebrow);
    EyebrowSlot->SetHorizontalAlignment(HAlign_Center);
    EyebrowSlot->SetPadding(FMargin(8.0f, 0.0f, 8.0f, 18.0f));

    // Custom wordmark treatment for the creator credit.
    UTextBlock* Logo = WidgetTree->ConstructWidget<UTextBlock>(UTextBlock::StaticClass(), TEXT("YasserCustomWordmark"));
    Logo->SetText(FText::FromString(TEXT("YASSER BENMAGHNIA")));
    Logo->SetColorAndOpacity(FSlateColor(FLinearColor(0.20f, 0.025f, 0.045f, 1.0f)));
    Logo->SetJustification(ETextJustify::Center);
    Logo->SetShadowColorAndOpacity(FLinearColor(1.0f, 0.93f, 0.93f, 0.85f));
    Logo->SetShadowOffset(FVector2D(2.0f, 3.0f));
    FSlateFontInfo LogoFont = Logo->GetFont();
    LogoFont.Size = 48;
    LogoFont.TypefaceFontName = FName(TEXT("Bold"));
    Logo->SetFont(LogoFont);
    UVerticalBoxSlot* LogoSlot = Content->AddChildToVerticalBox(Logo);
    LogoSlot->SetHorizontalAlignment(HAlign_Center);
    LogoSlot->SetVerticalAlignment(VAlign_Center);
    LogoSlot->SetPadding(FMargin(14.0f, 8.0f, 14.0f, 8.0f));

    UTextBlock* GameTitle = WidgetTree->ConstructWidget<UTextBlock>(UTextBlock::StaticClass(), TEXT("GameTitle"));
    GameTitle->SetText(FText::FromString(TEXT("ULTIMATE FOOTBALL ONLINE")));
    GameTitle->SetColorAndOpacity(FSlateColor(FLinearColor(0.30f, 0.035f, 0.055f, 0.92f)));
    GameTitle->SetJustification(ETextJustify::Center);
    FSlateFontInfo GameTitleFont = GameTitle->GetFont();
    GameTitleFont.Size = 16;
    GameTitleFont.TypefaceFontName = FName(TEXT("Bold"));
    GameTitle->SetFont(GameTitleFont);
    UVerticalBoxSlot* GameTitleSlot = Content->AddChildToVerticalBox(GameTitle);
    GameTitleSlot->SetHorizontalAlignment(HAlign_Center);
    GameTitleSlot->SetPadding(FMargin(8.0f, 18.0f, 8.0f, 0.0f));

    if (UWorld* World = GetWorld())
    {
        World->GetTimerManager().SetTimer(SplashTimerHandle, this, &UUltimateFootballSplashWidget::DismissSplash, 4.0f, false);
    }
}

void UUltimateFootballSplashWidget::DismissSplash()
{
    RemoveFromParent();
}