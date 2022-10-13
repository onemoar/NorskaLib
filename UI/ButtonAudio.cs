using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DarkTonic.MasterAudio;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour
{
    [SerializeField] string soundEffectKey = "BigButtonClick";

    void Awake()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        //MasterAudio.PlaySoundAndForget(soundEffectKey);
    }
}
