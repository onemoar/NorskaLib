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
    }
}
