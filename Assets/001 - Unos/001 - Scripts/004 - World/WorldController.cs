using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private WorldCore WorldCore;

    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }
}
