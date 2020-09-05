using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CCTV : MonoBehaviour
{
    //Renderer des Indikator-Lichts
    [SerializeField] Renderer mLight = null;

    //Start-Rotation der Kamera
    Vector3 mStartRotation = Vector3.zero;

    //Maximale Änderung der Rotation bei Generierung
    [SerializeField] float mRotationChange = 90.0f;

    //Rotationsvektor
    Vector3 mRotationVector = Vector3.zero;

    //Lichtstatus und Farben
    int mLightState = 0;
    [SerializeField] Color mLightOff = Color.black;
    [SerializeField] Color mLightOn = Color.black;

    //Spieler
    Transform mPlayer = null;

    //Wachen-Parent
    Transform mPatrols = null;
    
    //Winkel zum Spieler
    float mAngleToPlayer = 0.0f;

    //Sichtwinkel
    [SerializeField] float mMaxSightAngle = 45.0f;
    //Sichtweite
    [SerializeField] float mMaxSightDistance = 20.0f;

    //Erkennungszeit und Timer
    [SerializeField] float mDetectionTime = 2.0f;
    float mDetectionTimer = 0.0f;

    //Kamerazustand
    public bool isObserving { get; private set; } = true;
    float mLightStartTime = 0f;
    float mLightTime = 1f;

    //RaycastHit des Kamera-Raycasts
    RaycastHit mHit = default;

    //Alarmzustand der Kamera
    public bool mAlarmed = false;

    //Alarmquelle der Kamera
    AudioSource mAudioSource = null;

    void Start()
    {
        mPlayer = 
            GameObject.FindGameObjectWithTag(NameAndTagHolder.M_TAG_PLAYER).transform;

        mPatrols = GameObject.Find(NameAndTagHolder.M_NAME_PATROLS).transform;

        mStartRotation = transform.eulerAngles;
        
        mRotationVector = transform.up;

        mAudioSource = GetComponent<AudioSource>();

        //Invoke für rotes Licht
        InvokeRepeating("ToggleLight", mLightStartTime, mLightTime);
    }

    //Rotierung der Kamera
    public void SetRandomRotation(int _direction)
    {
        //Drei Varianten
        switch (_direction)
        {
            case 0:
                //Rechts drehen
                transform.Rotate(Vector3.up, mRotationChange, Space.World);
                break;

            case 1:
                //Links drehen
                transform.Rotate(Vector3.up, -mRotationChange, Space.World);
                break;

            default:
                //Nicht drehen
                break; 
        }
    }
    
    void Update()
    {
        //Ist Kamera aktiv
        if (isObserving)
        {
            //Vektor zum Spieler
            Vector3 toPlayer = 
                (mPlayer.position - transform.position).normalized;
            
            //Winkel zum Spieler
            mAngleToPlayer = Vector3.Angle(transform.forward, toPlayer);

            //Ist der Spieler im Sichtwinkel und trifft der Raycast etwas
            if(mAngleToPlayer < mMaxSightAngle && 
                Physics.Raycast(transform.position, 
                                mPlayer.position - transform.position, 
                                out mHit, 
                                mMaxSightDistance)
                                )
            {
                switch (mHit.collider.tag)
                {       
                    //Trifft Raycast den Spieler oder die Spielerkamera
                    case NameAndTagHolder.M_TAG_PLAYER:
                    case NameAndTagHolder.M_TAG_MAIN_CAMERA:
                        Debug.Log(mHit.collider.gameObject.name);
                        if(mDetectionTimer <= mDetectionTime)
                        {
                            //Erhöhe Timer um 1 pro Sekunde
                            mDetectionTimer += Time.deltaTime;
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //Andernfalls, verringe Timer um 1 pro Sekunde
                if(mDetectionTimer > 0.0f)
                {
                    mDetectionTimer -= Time.deltaTime;
                }
            }

            //Ist Erkennungs-Timer größer als Erkennungszeit
            if(mDetectionTimer >= mDetectionTime)
            {
                //1. Löse Alarm aus, wenn nicht bereits ausgelöst wurde
                //2. Starte Alarmton
                //3. Rufe Wachen
                if(!mAlarmed)
                {
                    mAlarmed = true;
                    mAudioSource.loop = true;
                    mAudioSource.Play();
                    CallPatrols(mHit.point);
                }
            }
        }
    }
    
    void CallPatrols(Vector3 _position)
    {
        //Iteration über Children des Patrol-Parents
        for (int i = 0; i < mPatrols.childCount; i++)
        {
            //Rufe alle Wachen zur Position des Kameras und 
            //übergebe eigene Kamerakomponente (zum Abschalten des Alarms)
            mPatrols.GetChild(i).GetComponent<Patrol>()
                .GoToCameraAlarm(_position, GetComponent<CCTV>());
        }
    }
    
    void ToggleLight()
    {
        //Ändere Lichtfarbe zur anderen Farbe
        switch (mLightState)
        {
            case 0:
                mLight.material.color = mLightOff;
                mLightState = 1;
                break;

            case 1:
                mLight.material.color = mLightOn;
                mLightState = 0;
                break;
        }
    }

    //Kamera deaktivieren (durch Sicherheitsraum)
    public void DisableCamera()
    {
        isObserving = false;
        mLight.material.color = mLightOff;
        CancelInvoke("ToggleLight");
    }

    //Kameraalarm deaktivieren
    public void ResetCall()
    {
        mAudioSource.loop = false;
        mDetectionTimer = 0.0f;
        mAlarmed = false;
    }
}