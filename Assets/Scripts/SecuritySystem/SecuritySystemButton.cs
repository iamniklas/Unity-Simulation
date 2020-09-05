using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Button für Interaktion mit Sicherheitssystem
public class SecuritySystemButton : MonoBehaviour
{
    [SerializeField] SecuritySystem mSecuritySystem = null;
    
    //Enum der möglichen Typen des Buttons
    public enum SecurityType { CAMERAS, VAULT};

    //Typ des Button-Objekts
    [SerializeField] SecurityType mSecurityType = 0;

    //Eigener Renderer zum Ändern der Farbe
    Renderer mRenderer = null;

    //Material: "Unlit-Rot"
    [SerializeField] Material mMaterialDeactivated = null;

    void Start()
    {
        mRenderer = GetComponent<Renderer>();
    }

    //Reagieren auf Klicken des Buttons
    public void ButtonClick()
    {
        //Unterscheidung nach Button-Typ
        switch (mSecurityType)
        {
            case SecurityType.CAMERAS:
                //Kameras deaktivieren und UI-Text updaten
                mSecuritySystem.mCamerasActive = false;
                mSecuritySystem.CCTVManager.DisableCameras();
                mSecuritySystem.AdaptText();
                break;

            case SecurityType.VAULT:
                //Tresor öffnen und UI-Text updaten
                mSecuritySystem.mVaultLocked = false;                
                GameObject.FindGameObjectWithTag(NameAndTagHolder.M_TAG_TOPSECUREDDOOR)
                    .GetComponent<VaultDoor>().UnlockDoor();
                mSecuritySystem.AdaptText();
                break;
        }
        //Button Farbe auf Rot
        mRenderer.material = mMaterialDeactivated;
    }
}