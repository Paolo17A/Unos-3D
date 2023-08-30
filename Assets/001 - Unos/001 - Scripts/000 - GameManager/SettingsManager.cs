using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    public void Start()
    {
        BGMSlider.value = GameManager.Instance.AudioManager.GetBGMVolume();
        SFXSlider.value = GameManager.Instance.AudioManager.GetSFXVolume();
    }

    public void SetBGMVolume()
    {
        GameManager.Instance.AudioManager.SetBGMVolume(BGMSlider.value);
    }

    public void SetSFXVolume()
    {
        GameManager.Instance.AudioManager.SetSFXVolume(SFXSlider.value);
    }
}
