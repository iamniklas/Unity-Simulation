using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton mit allen im Hauptmenü eingestellten Parametern
public class GameInfo : MonoBehaviour
{
    //Statische Instanz
    public static GameInfo instance = null;

    //Festgelegte Parameter
    public int mPatrols { get; private set; }
    public int mKeycards { get; private set; }
    public int mCameras { get; private set; }
    public int mBoxes { get; private set; }
    public int mSeed { get; private set; }

    //Maximale Anzahl 
    public static int mTotalBoxes { get; private set; } = 11;
    public static int mTotalCameras { get; private set; } = 5;
    public static int mTotalCameraRotations { get; private set; } = 3;
    public static int mTotalKeycards { get; private set; } = 8;
    public static int mTotalPatrolPaths { get; private set; } = 5;

    void Awake()
    {
        //Sicherstellen, dass nur 1 Objekt vorhanden ist
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        //Szenenübergreifendes GameObject
        DontDestroyOnLoad(this);
    }
    
    //Parameter setzen (durch Klicken auf "Starten")
    public void SetData(int _patrols, int _keycards, int _cameras, int _boxes, int _seed)
    {
        instance.mPatrols = _patrols;
        instance.mKeycards = _keycards;
        instance.mCameras = _cameras;
        instance.mBoxes = _boxes;
        instance.mSeed = _seed;
    }
}