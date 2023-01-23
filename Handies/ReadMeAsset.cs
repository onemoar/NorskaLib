using UnityEngine;

namespace NorskaLib.Handies
{
    [CreateAssetMenu(fileName = "ReadMe", menuName = "Editor/ReadMe", order = 1)]
    public sealed class ReadMeAsset : ScriptableObject
    {
        [TextArea]
        [SerializeField] string text;
    }
}
