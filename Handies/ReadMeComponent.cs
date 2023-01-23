using UnityEngine;

namespace NorskaLib.Handies
{
    public sealed class ReadMeComponent : MonoBehaviour
    {
        [TextArea]
        [SerializeField] string text;
    }
}
