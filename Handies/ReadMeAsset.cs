using UnityEngine;

namespace NorskaLib.Handies
{
    [CreateAssetMenu(fileName = "ReadMe", menuName = "Editor/ReadMe", order = 1)]
    public class ReadMeAsset : ScriptableObject
    {
        [TextArea]
        [SerializeField] string text;
    }
}
