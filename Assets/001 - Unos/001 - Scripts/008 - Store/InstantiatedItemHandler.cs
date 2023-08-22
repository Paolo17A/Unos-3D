using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatedItemHandler : MonoBehaviour
{
    //====================================================================================================================
    public QuestData.Items ThisItem;
    [ReadOnly] public bool newlySpawned;

    /*[Header("DEBUGGER")]
    [SerializeField][ReadOnly] private bool isDragged = false;
    [SerializeField][ReadOnly] private Vector3 mouseDragStartPos;
    [SerializeField][ReadOnly] private Vector3 spriteDragStartPos;*/
    //====================================================================================================================

    /*private void OnMouseDown()
    {
        isDragged = true;
        mouseDragStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15));
        spriteDragStartPos = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        if (isDragged)
        {
            transform.localPosition = spriteDragStartPos + (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15)) - mouseDragStartPos);
        }
    }

    private void OnMouseUp()
    {
        isDragged = false;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);

        foreach (Collider2D collider in colliders)
        {
            DisplayedItemHandler displayedItemHandler = collider.GetComponent<DisplayedItemHandler>();
            if (displayedItemHandler != null && displayedItemHandler.ThisItem == ThisItem)
            {
                Destroy(gameObject);
                break;
            }
            else
                transform.localPosition = spriteDragStartPos;
        }
    }*/
}
