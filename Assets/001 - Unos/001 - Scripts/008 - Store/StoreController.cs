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
        GameManager.Instance.ProgressContainer.SetActive(true);
        StoreCore.InitializeStore();
    }

    private void Update()
    {
        if(Application.isEditor)
            HandleEditorInput();
        else
            HandleAndroidInput();
    }

    private void HandleEditorInput()
    {
        if (StoreCore.IsShowingShoppingList()) return;

        if (Mouse.current.leftButton.isPressed)
            StoreCore.HandleInput(Input.mousePosition);
        else if (StoreCore.isDragging)
        {
            StoreCore.CheckAndDestroyDraggable();
            StoreCore.isDragging = false;
        }
    }

    private void HandleAndroidInput()
    {
        if (StoreCore.IsShowingShoppingList()) return;

        if (Touchscreen.current.primaryTouch.press.isPressed)
            StoreCore.HandleInput(Touchscreen.current.primaryTouch.position.ReadValue());
        else if (StoreCore.isDragging)
        {
            StoreCore.CheckAndDestroyDraggable();
            StoreCore.isDragging = false;
        }
    }

}
