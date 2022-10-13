using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.UI
{
    public static class Events
    {
        public static Action<string> onWidgetClick = (id) => { };

        public static Action<Screen> onScreenShown = (screen) => { };
        public static Action<Screen> onScreenHidden = (screen) => { };
        public static Action<Screen> onScreenDestroyed = (screen) => { };
    }
}
