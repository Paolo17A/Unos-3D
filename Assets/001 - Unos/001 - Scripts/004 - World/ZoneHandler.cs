using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneHandler : MonoBehaviour
{
    //=============================================================================================
    public QuestData QuestData;
    [SerializeField] private WorldCore WorldCore;
    //=============================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if (QuestData.IsAccomplised) return;

        WorldCore.EnteredZone = this;
        WorldCore.DisplayZonePopUp();
    }

    private void OnTriggerExit(Collider other)
    {
        if (QuestData.IsAccomplised) return;

        WorldCore.EnteredZone = null;
        WorldCore.HideZonePopUp();
    }
}
