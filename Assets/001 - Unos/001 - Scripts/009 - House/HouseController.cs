using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }
}
