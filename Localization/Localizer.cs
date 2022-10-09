using NorskaLib.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NorskaLib.Localization
{
	public abstract class Localizer : MonoBehaviour
	{
        [Dependency] protected LocalizationManager LocalizationManager;

        [SerializeField] string key;

        private Language language = Language.UNIDENTIFIED;

        protected virtual void Awake()
        {
            LocalizationManager = DependencyContainer.Instance.Resolve<LocalizationManager>();
        }

        protected virtual void OnEnable()
        {
            if (language != LocalizationManager.CurrentLanguage)
                OnLocalizationChanged(LocalizationManager.CurrentLanguage);

            LocalizationManager.onLanguageChanged += OnLocalizationChanged;
        }

        protected virtual void OnDisable()
        {
            LocalizationManager.onLanguageChanged -= OnLocalizationChanged;
        }

        void OnLocalizationChanged(Language language)
        {
            SetText(LocalizationManager.GetText(key));

            this.language = language;
        }

        protected abstract void SetText(string localizedText);
    }
}