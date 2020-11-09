using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using ToDo.Core;
using ToDo.UI;
using UIKit;

namespace ToDo.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<Setup, App, FormsApp>
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            return base.FinishedLaunching(application, launchOptions);
        }
    }
}


