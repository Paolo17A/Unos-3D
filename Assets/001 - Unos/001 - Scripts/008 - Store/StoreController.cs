using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StoreController : MonoBehaviour
{
    [SerializeField] private StoreCore StoreCore;
    private void Awake()
    {
        GameManager.Instance.SceneController.ActionPass = true;
    }

    private void Start()
    {
        StoreCore.InitializeStore();
    }

    private void Update()
    {
        if (StoreCore.IsShowingShoppingList()) return;
        if (Mouse.current.leftButton.IsPressed())

            StoreCore.HandleInput(Input.mousePosition);

        else if (StoreCore.isDragging)
        {
            StoreCore.CheckAndDestroyDraggable();
            StoreCore.isDragging = false;
        }
    }
}
