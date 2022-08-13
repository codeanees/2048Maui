using System.Diagnostics;
using _2048Game.Data;
using _2048Game.Enums;
using _2048Game.ViewModels;
using SkiaSharp.Extended.UI.Controls;

namespace _2048Game;

public partial class MainPage : ContentPage
{
    private bool canHitMauiRobot = true;
	public MainPage()
    {
        InitializeComponent();
        DisplayConfettiAnimation();
    }

    private void DisplayConfettiAnimation()
    {
        var sus = new SKConfettiSystem()
        {
            Lifetime = 2,
            Colors = new SKConfettiColorCollection(ConfettiConfig.Colors),
            Shapes = new SKConfettiShapeCollection(ConfettiConfig.GetShapes(ConfettiConfig.Shapes).SelectMany(s => s)),
            MinimumInitialVelocity = 30,
            MaximumInitialVelocity = 150,
            Emitter = SKConfettiEmitter.Burst(100),
            EmitterBounds = SKConfettiEmitterBounds.Center,
        };
        skConfetti.Systems.Add(sus);
    }

    void MainPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainPageViewModel.State) &&
            ((MainPageViewModel)BindingContext).State == LevelState.GameOver)
        {
            //TrophyAnimation.PlayAnimation();
        }
    }
    void OnSwiped(object sender, SwipedEventArgs e)
    {
        switch (e.Direction)
        {
            case SwipeDirection.Left:
                // Handle the swipe
                break;
            case SwipeDirection.Right:
                // Handle the swipe
                break;
            case SwipeDirection.Up:
                // Handle the swipe
                break;
            case SwipeDirection.Down:
                // Handle the swipe
                break;
        }
    }

    private SwipeDirection? swipedDirection;
    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            //case GestureStatus.Started:
            //    HandleTouchStart(e.TotalX, e.TotalY);
            //    break;
            case GestureStatus.Running:
                HandleTouch(e.TotalX, e.TotalY);
                break;
            case GestureStatus.Completed:
                HandleTouchEnd(swipedDirection);
                break;
        }
    }
    double eTotalXStart = 0;
    double eTotalYStart = 0;

    private int swipeThreshold = 20;
    private int swipeVelocityThreshold = 100;

    private int velocityX = 110;//In native android we have the way
    private int velocityY = 110;//In native android we have the way

    private void HandleTouchStart(double eTotalX, double eTotalY)
    {
        eTotalXStart = eTotalX;
        eTotalYStart = eTotalY;
    }
    private void HandleTouchEndNew(double eTotalXEnd, double eTotalYEnd)
    {
        var currentViewModel = (MainPageViewModel)BindingContext;
        try
        {
            var diffY = eTotalYEnd - eTotalYStart;
            var diffX = eTotalXEnd - eTotalXStart;
            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                if (Math.Abs(diffX) > swipeThreshold && Math.Abs(velocityX) > swipeVelocityThreshold)
                {
                    if (diffX > 0)
                    {
                        Debug.WriteLine("Left to Right swipe gesture");
                        currentViewModel.LeftSwipeCommand.Execute(null);
                    }
                    else
                    {
                        Debug.WriteLine("Right to Left swipe gesture");
                        currentViewModel.RightSwipeCommand.Execute(null);
                    }
                }
            }
            else if (Math.Abs(diffY) > swipeThreshold && Math.Abs(velocityY) > swipeVelocityThreshold)
            {
                if (diffY > 0)
                {
                    Debug.WriteLine("Top to Botton swipe gesture");
                    currentViewModel.DownSwipeCommand.Execute(null);
                }
                else
                {
                    Debug.WriteLine("Bottom to Top swipe gesture");
                    currentViewModel.UpSwipeCommand.Execute(null);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void HandleTouch(double eTotalX, double eTotalY)
    {
        swipedDirection = null;
        const int delta = 10;
        if (eTotalX > delta)
        {
            swipedDirection = SwipeDirection.Right;
        }
        else if (eTotalX < -delta)
        {
            swipedDirection = SwipeDirection.Left;
        }
        else if (eTotalY > delta)
        {
            swipedDirection = SwipeDirection.Down;
        }
        else if (eTotalY < -delta)
        {
            swipedDirection = SwipeDirection.Up;
        }
    }
    private void HandleTouchEnd(SwipeDirection? swiped)
    {
        if (swiped == null)
        {
            return;
        }
        var currentViewModel = (MainPageViewModel)BindingContext;
        switch (swiped)
        {
            case SwipeDirection.Right:
                currentViewModel.RightSwipeCommand.Execute(null);
                break;
            case SwipeDirection.Left:
                currentViewModel.LeftSwipeCommand.Execute(null);
                break;
            case SwipeDirection.Up:
                currentViewModel.UpSwipeCommand.Execute(null);

                break;
            case SwipeDirection.Down:
                currentViewModel.DownSwipeCommand.Execute(null);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void UndoButton_Clicked(System.Object sender, System.EventArgs e)
    {
        DisplayConfettiAnimation();
    }
    async void MauiRobotTapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
    {
        if (canHitMauiRobot)
        {
            canHitMauiRobot = false;
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            phraseLayout.IsVisible = true;
            //Translate effect  
            await phraseLayout.TranslateTo(10, 30, 1000, Easing.CubicOut);
            await phraseLayout.TranslateTo(0, 0, 1000, Easing.CubicIn);
            phraseLayout.IsVisible = false;
            canHitMauiRobot = true;
        }
    }
}

