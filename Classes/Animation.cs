using System.Windows.Media.Animation;
using System.Windows;

namespace DreamTimer.Classes
{
    internal class Animation
    {
        /// <summary>
        /// Performs a fade animation on the specified UI element.
        /// </summary>
        /// <param name="animationSettings">A dictionary containing animation settings, including 'from' (starting opacity), 'to' (ending opacity), and 'duration' (duration of the animation in seconds).</param>
        /// <param name="uiElement">The UI element on which the fade animation is to be performed.</param>
        /// <param name="completionAction">An optional action to be executed upon completion of the animation.</param>
        public static void FadeAnimation(Dictionary<string, double> animationSettings, UIElement uiElement, Action completionAction)
        {
            DoubleAnimation animation = new()
            {
                From = animationSettings["from"],
                To = animationSettings["to"],
                Duration = TimeSpan.FromSeconds(animationSettings["duration"])
            };

            animation.Completed += (sender, e) =>
            {
                if (completionAction != null)
                    completionAction?.Invoke();
            };


            Storyboard storyboard = new();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, uiElement);

            Storyboard.SetTargetProperty(animation, new PropertyPath(FrameworkElement.OpacityProperty));

            // Start Animation
            storyboard.Begin();
        }
    }
}
