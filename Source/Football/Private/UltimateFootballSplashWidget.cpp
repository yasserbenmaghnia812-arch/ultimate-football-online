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

    if (!WidgetTree)
    {
        return;
    }

    UCanvasPanel* Root = WidgetTree->ConstructWidget<UCanvasPanel>(UCanvasPanel::StaticClass(), TEXT("SplashRoot"));
    WidgetTree->RootWidget = Root;

    UBorder* RedBackground = WidgetTree->ConstructWidget<UBorder>(UBorder::StaticClass(), TEXT("RedBackground"));
    RedBackground->SetBrushColor(FLinearColor(0.72f, 0.015f, 0.025f, 1.0f));

    UCanvasPanelSlot* BackgroundSlot = Root->AddChildToCanvas(RedBackground);
    BackgroundSlot->SetAnchors(FAnchors(0.0f, 0.0f, 1.0f, 1.0f));
    BackgroundSlot->SetOffsets(FMargin(0.0f));

    UVerticalBox* Content = WidgetTree->ConstructWidget<UVerticalBox>(UVerticalBox::StaticClass(), TEXT("SplashContent"));
    RedBackground->SetContent(Content);

    UTextBlock* Title = WidgetTree->ConstructWidget<UTextBlock>(UTextBlock::StaticClass(), TEXT("GameTitle"));
    Title->SetText(FText::FromString(TEXT("ULTIMATE FOOTBALL ONLINE")));
    Title->SetColorAndOpacity(FSlateColor(FLinearColor::White));
    Title->SetJustification(ETextJustify::Center);
    FSlateFontInfo TitleFont = Title->GetFont();
    TitleFont.Size = 48;
    Title->SetFont(TitleFont);

    UVerticalBoxSlot* TitleSlot = Content->AddChildToVerticalBox(Title);
    TitleSlot->SetHorizontalAlignment(HAlign_Center);
    TitleSlot->SetVerticalAlignment(VAlign_Center);
    TitleSlot->SetPadding(FMargin(12.0f));

    UTextBlock* Credit = WidgetTree->ConstructWidget<UTextBlock>(UTextBlock::StaticClass(), TEXT("CreatorCredit"));
    Credit->SetText(FText::FromString(TEXT("Yasser Benmaghnia")));
    Credit->SetColorAndOpacity(FSlateColor(FLinearColor(1.0f, 1.0f, 1.0f, 0.92f)));
    Credit->SetJustification(ETextJustify::Center);
    FSlateFontInfo CreditFont = Credit->GetFont();
    CreditFont.Size = 22;
    Credit->SetFont(CreditFont);

    UVerticalBoxSlot* CreditSlot = Content->AddChildToVerticalBox(Credit);
    CreditSlot->SetHorizontalAlignment(HAlign_Center);
    CreditSlot->SetVerticalAlignment(VAlign_Center);
    CreditSlot->SetPadding(FMargin(8.0f));

    if (UWorld* World = GetWorld())
    {
        World->GetTimerManager().SetTimer(
            SplashTimerHandle,
            this,
            &UUltimateFootballSplashWidget::DismissSplash,
            3.0f,
            false
        );
    }
}

void UUltimateFootballSplashWidget::DismissSplash()
{
    RemoveFromParent();
}
