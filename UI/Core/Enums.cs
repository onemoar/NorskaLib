using System.Collections;
using System.Collections.Generic;

namespace NorskaLib.UI
{
    public enum ShowScreenMode
    {
        /// <summary>
        /// Show this screen and hide all others.
        /// </summary>
        Single,
        /// <summary>
        /// Show this screen, other remain their shown/hidden state.
        /// </summary>
        Additive,
        /// <summary>
        /// Show this screen and hide all other screens with same order value.
        /// </summary>
        SoloInLayer
    }
}