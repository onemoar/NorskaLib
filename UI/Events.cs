using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.UI
{
    public static class Events
    {
        /// <summary>
        /// Params: string - widget id
        /// </summary>
        public static Action<string> onWidgetClick;

        /// <summary>
        /// Params: screen - source object, int - new order
        /// </summary>
        public static Action<Window, int> onWindowOrderChanged;
        public static Action<Window> onWindowShown;
        public static Action<Window> onWindowHidden;
        public static Action<Window> onWindowDestroyed;
    }
}
