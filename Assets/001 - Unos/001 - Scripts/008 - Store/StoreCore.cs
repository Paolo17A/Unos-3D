using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StoreCore : MonoBehaviour
{
    //======================================================================================================================
    public Collider2D colliderZone;

    [Header("INITIALIZATION VARIABLES")]
    [SerializeField] TextMeshProUGUI StoreNameTMP;

    [Header("SHOPPING LIST VARIABLES")]
    [SerializeField] private GameObject ShoppingListContainer;
    [SerializeField] private TextMeshProUGUI ShoppingListTMP;

    [Header("QUEST VARIABLES")]
    [SerializeField][ReadOnly] private List<QuestData.Items> CurrentItems;
    [SerializeField] private Button ProceedBtn;

    [Header("DRAG VARIABLES")]
    [ReadOnly] public bool isDragging = false;
    [SerializeField][ReadOnly] private GameObject currentDraggable;
    [SerializeField][ReadOnly] private Vector3 startingMouseDragPos;
    [SerializeField][ReadOnly] private Vector3 startingSpriteDragPos;

    [Header("DEBUGGER")]
    Ray ray;
    RaycastHit2D hit;
    //======================================================================================================================

    #region INITIALIZATION
    public void InitializeStore()
    {
        StoreNameTMP.text = GameManager.Instance.CurrentQuest.ZoneName;
        GameManager.Instance.CurrentQuest.SetItemsToGet();
        DisplayShoppingList();
        ProceedBtn.gameObject.SetActive(false);
        ShoppingListTMP.text = CreateShoppingList(CountElements(GameManager.Instance.CurrentQuest.ItemsToGet));
    }
    #endregion

    public void DisplayShoppingList()
    {
        ShoppingListContainer.SetActive(true);
    }

    public void HideShoppingList()
    {
        ShoppingListContainer.SetActive(false);
    }

    public bool IsShowingShoppingList()
    {
        return ShoppingListContainer.activeInHierarchy;
    }

    #region DRAG
    public void HandleInput(Vector3 inputPosition)
    {
        //  OnMouseDown
        if (!isDragging)
        {
            ray = Camera.main.ScreenPointToRay(inputPosition);
            hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider == null) return;

            if (hit.collider.GetComponent<DisplayedItemHandler>() != null)
            {
                Debug.Log("clicked on displayed item");
                isDragging = true;
                startingMouseDragPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 15));
                startingSpriteDragPos = hit.collider.transform.position;  

                // Instantiate the prefab at the stored click position
                currentDraggable = Instantiate(hit.collider.GetComponent<DisplayedItemHandler>().InstantiatedItemPrefab, startingSpriteDragPos, Quaternion.identity);
                currentDraggable.GetComponent<InstantiatedItemHandler>().newlySpawned = true;
            }
            else if (hit.collider.GetComponent<InstantiatedItemHandler>() != null)
            {
                Debug.Log("clicked on instantiated item");
                isDragging = true;
                startingMouseDragPos = Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 15));
                startingSpriteDragPos = hit.collider.transform.position;

                currentDraggable = hit.collider.gameObject;
            }
        }

        //  OnMouseDrag
        if (isDragging && currentDraggable != null)
        {
            currentDraggable.transform.position = startingSpriteDragPos + (Camera.main.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 15)) - startingMouseDragPos);
            currentDraggable.transform.position = new Vector3(currentDraggable.transform.position.x, currentDraggable.transform.position.y, 0);
        }
    }

    public void CheckAndDestroyDraggable()
    {
        if (currentDraggable == null) return;

        if (currentDraggable.GetComponent<InstantiatedItemHandler>().newlySpawned)
        {
            if (CalculateOverlapPercentage(currentDraggable.GetComponent<Collider2D>(), colliderZone) >= 0.6f)
            {
                // Keep the draggable within the collider zone
                currentDraggable.transform.position = new Vector3(
                    Mathf.Clamp(currentDraggable.transform.position.x, colliderZone.bounds.min.x, colliderZone.bounds.max.x),
                    Mathf.Clamp(currentDraggable.transform.position.y, colliderZone.bounds.min.y, colliderZone.bounds.max.y),
                    currentDraggable.transform.position.z
                );

                AddItemAndProcessQuest();
            }
            else
            {
                Destroy(currentDraggable);
                currentDraggable = null;
            }
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(currentDraggable.transform.position, currentDraggable.transform.localScale, 0);
            foreach(Collider2D collider in colliders)
            {
                DisplayedItemHandler displayedItemHandler = collider.GetComponent<DisplayedItemHandler>();

                //  Destroy the instantiated item if it collides with a displayed item
                if (displayedItemHandler != null)
                {
                    RemoveItemandProcessQuest();
                    break;
                }
            }

            if (CalculateOverlapPercentage(currentDraggable.GetComponent<Collider2D>(), colliderZone) < 0.6f)
                // Keep the draggable within the collider zone
                currentDraggable.transform.position = new Vector3(
                Mathf.Clamp(currentDraggable.transform.position.x, colliderZone.bounds.min.x, colliderZone.bounds.max.x),
                Mathf.Clamp(currentDraggable.transform.position.y, colliderZone.bounds.min.y, colliderZone.bounds.max.y),
                currentDraggable.transform.position.z
            );
        }
    }
    
    #endregion

    #region QUEST
    private void AddItemAndProcessQuest()
    {
        //  Add selected item to list
        CurrentItems.Add(currentDraggable.GetComponent<InstantiatedItemHandler>().ThisItem);
        currentDraggable.GetComponent<InstantiatedItemHandler>().newlySpawned = false;
        currentDraggable = null;

        if (AreEnumListsEqual(CurrentItems, GameManager.Instance.CurrentQuest.ItemsToGet))
            ProceedBtn.gameObject.SetActive(true);
        else
            ProceedBtn.gameObject.SetActive(false);
    }

    private void RemoveItemandProcessQuest()
    {
        CurrentItems.Remove(currentDraggable.GetComponent<InstantiatedItemHandler>().ThisItem);
        Destroy(currentDraggable);
        currentDraggable = null;

        if (AreEnumListsEqual(CurrentItems, GameManager.Instance.CurrentQuest.ItemsToGet))
            ProceedBtn.gameObject.SetActive(true);
        else
            ProceedBtn.gameObject.SetActive(false);
    }

    public void ProceedQuest()
    {
        GameManager.Instance.CurrentQuest.IsAccomplised = true;
        GameManager.Instance.CurrentQuest = null;
        GameManager.Instance.SceneController.CurrentScene = "WorldScene";
    }
    #endregion

    #region UTILITY
    private bool AreEnumListsEqual<T>(List<T> list1, List<T> list2) where T : Enum
    {
        Dictionary<T, int> dict1 = CountElements(list1);
        Dictionary<T, int> dict2 = CountElements(list2);

        return AreDictionariesEqual(dict1, dict2);
    }

    private Dictionary<T, int> CountElements<T>(List<T> list) where T : Enum
    {
        Dictionary<T, int> dict = new Dictionary<T, int>();
        foreach (T item in list)
        {
            if (dict.ContainsKey(item))
            {
                dict[item]++;
            }
            else
            {
                dict[item] = 1;
            }
        }
        return dict;
    }

    private string CreateShoppingList<T>(Dictionary<T, int> itemCounts) where T : Enum
    {
        return string.Join(Environment.NewLine, itemCounts.Select(kv => $"-{kv.Key} ({kv.Value})\n"));
    }

    private bool AreDictionariesEqual<T>(Dictionary<T, int> dict1, Dictionary<T, int> dict2)
    {
        if (dict1.Count != dict2.Count)
            return false;

        foreach (var kvp in dict1)
        {
            if (!dict2.ContainsKey(kvp.Key) || dict2[kvp.Key] != kvp.Value)
                return false;
        }

        return true;
    }

    private float CalculateOverlapPercentage(Collider2D draggableCollider, Collider2D zoneCollider)
    {
        Bounds draggableBounds = draggableCollider.bounds;
        Bounds zoneBounds = zoneCollider.bounds;

        float overlapX = Mathf.Max(0, Mathf.Min(draggableBounds.max.x, zoneBounds.max.x) - Mathf.Max(draggableBounds.min.x, zoneBounds.min.x));
        float overlapY = Mathf.Max(0, Mathf.Min(draggableBounds.max.y, zoneBounds.max.y) - Mathf.Max(draggableBounds.min.y, zoneBounds.min.y));

        float overlapArea = overlapX * overlapY;
        float draggableArea = draggableBounds.size.x * draggableBounds.size.y;

        Debug.Log("Overlap Percentage: " + (overlapArea / draggableArea));
        return overlapArea / draggableArea;
    }
    #endregion
}
