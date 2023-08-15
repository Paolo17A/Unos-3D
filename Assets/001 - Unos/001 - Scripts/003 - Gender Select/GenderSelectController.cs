using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenderSelectController : MonoBehaviour
{
    [SerializeField] private GenderSelectCore GenderSelectCore;
    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        GenderSelectCore.SelectMaleCharacter();
    }
}
