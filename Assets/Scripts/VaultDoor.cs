using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    //Material Unlit-Grün
    [SerializeField] Material mMaterialUnlocked = null;

    //Renderer der Statusleuchte
    [SerializeField] Renderer mDoorLight = null;

    //Türstatus
    bool mDoorIsUnlocked = false;
    [SerializeField] string mDoorTrigger = "toggledoor";

    //Tür entriegeln und öffnen
    public void UnlockDoor()
    {
        if(!mDoorIsUnlocked)
        {
            mDoorLight.material = mMaterialUnlocked;
            GetComponent<Animator>().SetTrigger(mDoorTrigger);
        }
        mDoorIsUnlocked = true;
    }
}