using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generierung der Spielwelt
public class Game : MonoBehaviour
{
    int mSeed = 0;
    //Kamera-Parent
    [SerializeField] Transform mCameras = null;
    //Wachen-Parent
    [SerializeField] Transform mPatrols = null;
    //Wachen-Pfad-Parent
    [SerializeField] Transform mPatrolRoutes = null;
    //Boxen-Parent
    [SerializeField] Transform mBoxes = null;

    //Spawnpunkte der Schlüsselkarte(n)
    [SerializeField] Transform mKeycardSpawnPoints = null;

    //Parent der Varianten des Sicherheitsraums
    [SerializeField] Transform[] mSecurityRooms = new Transform[2];
    //Parent der Varianten des Tresoreingangs
    [SerializeField] Transform[] mVaultEntries = new Transform[2];

    //Prefab der Wache
    [SerializeField] GameObject mPatrolPrefab = null;
    //Prefab der Schlüsselkarte
    [SerializeField] GameObject mCardPrefab = null;

    System.Random mRandom = null;

    int mRandomNumber = 0;

    void Start()
    {
        //Random Generation

        int mSeedInput = GameInfo.instance.mSeed;

        //Ist Seed nicht 0, dann nutze Seed aus GameInfo Singleton
        //Ansonsten Zufallswert festlegen
        mSeed = mSeedInput != 0 ? 
            mSeedInput : UnityEngine.Random.Range(0, int.MaxValue);

        //Random Objekt mit Seed
        mRandom = new System.Random(mSeed);
        

        //Entscheidung der Position des Sicherheitsraums (Delete-Methode)


        mRandomNumber = mRandom.Next();

        //Generierte Zufallszahl auf 0 oder 1 setzen, je nach der Zahl
        int whichRoom = mRandomNumber < (int.MaxValue / 2) ? 0 : 1;       

        switch (whichRoom)
        {
            case 0:
                Destroy(mSecurityRooms[0].GetChild(0).gameObject);
                Destroy(mSecurityRooms[1].GetChild(1).gameObject);
                break;

            case 1:
                Destroy(mSecurityRooms[0].GetChild(1).gameObject);
                Destroy(mSecurityRooms[1].GetChild(0).gameObject);
                break;
        }


        //Entscheidung der Position des Eingangs zum Tresor (Delete-Methode)
        
        ReinitializeRandomObject();

        mRandomNumber = mRandom.Next();

        //Generierte Zufallszahl auf 0 oder 1 setzen, je nach der Zahl
        whichRoom = mRandomNumber < (int.MaxValue / 2) ? 0 : 1;

        switch (whichRoom)
        {
            case 0:
                Destroy(mVaultEntries[0].gameObject);
                break;

            case 1:
                Destroy(mVaultEntries[1].gameObject);
                break;
        }

        //Patrol + Path Decision (Create-Methode)

        ReinitializeRandomObject();

        int patrols = GameInfo.instance.mPatrols;

        List<int> patrolWaypointIndicies = new List<int>();
        List<GameObject> patrolObjects = new List<GameObject>();

        //Erzeuge Wachen
        for (int i = 0; i < patrols; i++)
        {
            GameObject tempPatrol = Instantiate(mPatrolPrefab, mPatrols);
            patrolObjects.Add(tempPatrol);
        }

        //Füge jeder Wache einen Pfad hinzu, der nicht bereits vergeben wurde
        while (patrolWaypointIndicies.Count < patrols)
        {
            mRandomNumber = mRandom.Next() % GameInfo.mTotalPatrolPaths;
            if(!patrolWaypointIndicies.Contains(mRandomNumber))
            {
                patrolWaypointIndicies.Add(mRandomNumber);
                patrolObjects[patrolWaypointIndicies.Count - 1]
                    .GetComponent<Patrol>().mPathId = mRandomNumber;

                patrolObjects[patrolWaypointIndicies.Count - 1].transform.position =
                    mPatrolRoutes.GetChild(patrolWaypointIndicies.Count - 1)
                    .GetChild(0).position;
            }
        }

        //Entscheidung der Sonnenrotation

        ReinitializeRandomObject();
        
        GameObject.Find(NameAndTagHolder.M_NAME_DIRECIONAL_LIGHT)
            .GetComponent<SunTimeRotation>()
            .SetStartRotationWithSeed(mRandom.Next());

        //Keycard Positions (Create-Methode)

        ReinitializeRandomObject();

        int keycards = GameInfo.instance.mKeycards;

        int currentKeycards = 0;

        List<int> keycardPositionIndicies = new List<int>();

        //Erzeuge Keycards und setze an eine noch nicht belegte Spawn-Position
        while(currentKeycards < keycards)
        {
            mRandomNumber = mRandom.Next() % GameInfo.mTotalKeycards;
            if(!keycardPositionIndicies.Contains(mRandomNumber))
            {
                Instantiate(mCardPrefab, 
                            mKeycardSpawnPoints.GetChild(mRandomNumber));
                keycardPositionIndicies.Add(mRandomNumber);
                currentKeycards++;
            }
        }
        
        //Camera Decision (Delete-Methode)

        ReinitializeRandomObject();

        int cameras = GameInfo.mTotalCameras - GameInfo.instance.mCameras;

        int currentCameras = 0;

        List<int> camIndicies = new List<int>();

        //Zerstöre Kameras
        while (currentCameras < cameras)
        {
            mRandomNumber = mRandom.Next();
            int nextIndex = mRandomNumber % GameInfo.mTotalCameras;
            if (!camIndicies.Contains(nextIndex))
            {
                camIndicies.Add(nextIndex);
                Destroy(mCameras.GetChild(nextIndex).gameObject);
                currentCameras++;
            }
        }

        mCameras.GetComponent<CCTVManager>().UpdateCameraList();

        //Rotation der Kameras
        for (int i = 0; i < GameInfo.instance.mCameras; i++)
        {
            mRandomNumber = mRandom.Next() % GameInfo.mTotalCameraRotations;
            mCameras.GetComponent<CCTVManager>().RotateCamera(i, mRandomNumber);
        }

        //Box Decision (Delete-Methode)

        ReinitializeRandomObject();

        //Zu löschende Boxen
        int boxes = GameInfo.mTotalBoxes - GameInfo.instance.mBoxes;

        int currentBoxes = 0;

        //Liste mit Indicies, um Duplikate zu verhindern
        List<int> boxIndicies = new List<int>();        

        while (currentBoxes < boxes)
        {
            mRandomNumber = mRandom.Next();
            int nextIndex = mRandomNumber % GameInfo.mTotalBoxes;
            if (!boxIndicies.Contains(nextIndex))
            {
                boxIndicies.Add(nextIndex);
                Destroy(mBoxes.GetChild(nextIndex).gameObject);
                currentBoxes++;
            }
        }
    }

    void ReinitializeRandomObject()
    {
        //Random neu initialisieren, 
        //um Abhängigkeiten von anderen Parametern zu verhindern
        mRandom = new System.Random(mSeed);
    }
}