using UnityEngine;

using TextField = TMPro.TMP_Text;

namespace Shooter
{
    [RequireComponent(typeof(TextField)), DisallowMultipleComponent]
    public class LocalizerTMP : MonoBehaviour
    {
        //[SerializeField] string key;

        //private ILocalization Localization;

        //private TextField textField;

        //private void CheckInitTextField() {
        //    if (textField == null) {
        //        textField = GetComponent<TextField>();
        //    }
        //}

        //protected virtual void Awake() {
        //    Localization = ContainerHolder.Container.Resolve<ILocalization>();
        //    CheckInitTextField();

        //    Localization.OnChangeLocale += OnChangeLocale;
        //    OnChangeLocale();
        //}

        //protected virtual void OnChangeLocale() {
        //    if (!string.IsNullOrEmpty(key)) {
        //        textField.text = Localization.Get(key);
        //    }
        //}

        //protected virtual void OnDestroy() {
        //    if (Localization != null) {
        //        Localization.OnChangeLocale -= OnChangeLocale;
        //    }
        //}

        //#if UNITY_EDITOR
        //protected virtual void OnValidate() {
        //    CheckInitTextField();
        //}
        //#endif
    }
}
