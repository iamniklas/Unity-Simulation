using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecuritySystem : MonoBehaviour
{
    [SerializeField] CCTVManager mCCTVManager = null;

    public CCTVManager CCTVManager
    {
        get
        {
            return mCCTVManager;
        }
    }

    //Status der Sicherheitselemnte
    public bool mCamerasActive = true;
    public bool mVaultLocked = true;

    //UI-Text des Computers
    [SerializeField] Text mComputerScreenText = null;

    //Text-Elemente
    //Headline
    [SerializeField] [TextArea] string mStringHeader = 
        "Security System Status \n";
    //Status-Texte des Kamerasystems
    [SerializeField] [TextArea] string mStringCamerasOnline = 
        "CAMERAS: ONLINE \n";
    [SerializeField] [TextArea] string mStringCamerasOffline = 
        "CAMERAS: OFFLINE \n";
    //Status-Texte des Tresorraums
    [SerializeField] [TextArea] string mStringVaultLocked = 
        "VAULT: LOCKED \n";
    [SerializeField] [TextArea] string mStringVaultUnlocked = 
        "VAULT: OPENED \n";    

    //Anpassen des Textes auf aktuellen Status des Sicherheitssystems
    public void AdaptText()
    {
        string result = "";
        result += mStringHeader;
        result += mCamerasActive ? mStringCamerasOnline : mStringCamerasOffline;
        result += mVaultLocked ? mStringVaultLocked : mStringVaultUnlocked;
        mComputerScreenText.text = result;
    }
}
