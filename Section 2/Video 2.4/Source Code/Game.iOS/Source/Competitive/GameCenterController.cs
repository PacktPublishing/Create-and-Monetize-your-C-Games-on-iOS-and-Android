using Game.Shared.Competitive;
using GameKit;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Game.iOS.Source.Competitive
{
    /// <summary> The controller for Game Center to update achievements & leaderboards </summary>
    public class GameCenterController : CompetitiveController
    {
        /// <summary> Connects the controller to GameCenter </summary>
        /// <param name="onComplete"></param>
        public override void Connect(Action<Boolean> onComplete)
        {
            GKLocalPlayer.LocalPlayer.AuthenticateHandler = (viewController, error) =>
            {
                onComplete?.Invoke(GKLocalPlayer.LocalPlayer.Authenticated);
            };
        }

        /// <summary> Updates the achievement progress from the given achievement </summary>
        /// <param name="achievement"></param>
        public override void UpdateAchievement(Achievement achievement)
        {
            if (!GKLocalPlayer.LocalPlayer.Authenticated) return;
            GKAchievement gkAchievement = new GKAchievement(achievement.Id)
            {
                PercentComplete = achievement.PercentageComplete * 100
            };
            GKAchievement.ReportAchievements(new[] { gkAchievement }, (error) =>
            {
                if (error != null) Console.WriteLine($"Error reporting achievement - {error.Description}");
            });
        }

        /// <summary> Views the achievements on Game Center </summary>
        public override void ViewAchievements()
        {
            GKAchievementViewController achievementViewController = new GKAchievementViewController();
            achievementViewController.DidFinish += (Object sender, EventArgs e) => {
                achievementViewController.DismissViewController(true, null);
            };
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(achievementViewController, true, null);
        }
    }
}
