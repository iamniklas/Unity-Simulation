using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Normale Türe ohne Sicherheitsvorkehrungen
public class Door : MonoBehaviour
{
    //Zustand der Türbewegung
    bool mIsMoving = false;

    string mAnimationTrigger = "toggledoor";

    //Tür öffnen und schließen
    public void ToggleDoor()
    {
        //Tür muss ganz offen oder zu sein, um zu interagieren
        if (mIsMoving) return;

        GetComponent<Animator>().SetTrigger(mAnimationTrigger);
        mIsMoving = true;
    }

    //Türbewegung wieder freigeben (Animation Event)
    public void ReleaseDoor()
    {
        mIsMoving = false;
    }
}
