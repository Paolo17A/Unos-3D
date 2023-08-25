using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI OptionTMP;
    [SerializeField] private int OptionIndex;
    [SerializeField] private Toggle toggle;

    private Toggle.ToggleEvent toggleEvent;

    private void Start()
    {
        toggleEvent = toggle.onValueChanged;
    }
    public void SetOptionText(string option)
    {
        OptionTMP.text = option;
        gameObject.SetActive(true);
    }
}
