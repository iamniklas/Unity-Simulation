using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tür des Sicherheitsraums
public class SecuredDoor : MonoBehaviour
{
    //Renderer des Keyreaders
    [SerializeField] Renderer mKeyreaderRenderer = null;
    
    //Material "Unlit-Grün"
    [SerializeField] Material mCardGranted = null;

    //Name des Animator-Triggers
    [SerializeField] string mAnimatorTriggerName = "toggledoor";

    //Animator zum Öffnen der Tür
    Animator mDoorAnimator = null;

    //Türstatus: Is geöffnet?
    bool mDoorIsUnlocked = false;

    void Start()
    {
        mDoorAnimator = GetComponent<Animator>();
    }

    //Gebe Zugang zum Raum frei
    public void GrantAccess()
    {
        if (!mDoorIsUnlocked)
        {
            //Keycardreader Material ändern auf grün
            mKeyreaderRenderer.material = mCardGranted;
            //Tür öffnen
            mDoorAnimator.SetTrigger(mAnimatorTriggerName);

            mDoorIsUnlocked = true;
        }
    }
}
