using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Unos/Data/QuestData")]
public class QuestData : ScriptableObject
{
    public enum Items { NONE, WRENCH, BATTERY, FLASHLIGHT, PAINT, ROPE, SAW, KNIFE, WHISTLE, BISCUIT, WATER, BREAD, BROCOLLI, CAN, CHICKEN, MEAT, MILK, AGUA, BANDAGE, FIRST_AID, INSULIN, LOTION, MEDICINE, SOAP, STEHOSCOPE }
    private List<Items> DefaultHardwareStoreItems = new List<Items> {Items.BATTERY, Items.FLASHLIGHT, Items.WHISTLE};
    private List<Items> DefaultMarketItems = new List<Items> { Items.WATER, Items.CAN, Items.BISCUIT, Items.BREAD, };
    private List<Items> DefaultDrugstoreItems = new List<Items> { Items.AGUA, Items.BANDAGE, Items.FIRST_AID, Items.INSULIN, Items.MEDICINE };
    [field: SerializeField] public bool IsAccomplised { get; set; }

    [field:Header("ZONE VARIABLES")]
    [field: SerializeField] public string ZoneName { get;set; }
    [field: SerializeField] public string SceneName { get; set; }
    [field: SerializeField] public List<Items> ItemsToGet { get; set; }

    public void SetItemsToGet()
    {
        switch(SceneName)
        {
            case "HardwareStoreScene":
                ItemsToGet = DefaultHardwareStoreItems;
                break;
            case "MarketScene":
                ItemsToGet = DefaultMarketItems;
                int rand = Random.Range(0, 3);
                for(int i = 0; i < rand; i++)
                    ItemsToGet.Add(Items.CAN);
                break;
            case "DrugstoreScene":
                ItemsToGet = DefaultDrugstoreItems;
                break;
        }
    }
}