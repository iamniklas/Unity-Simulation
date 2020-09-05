using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    private Transform mWaypointGroup { get; set; } = null;

    Animator mAnimator = null;

    [Header("BlendTree Idle-Run Value Name")]
    [SerializeField] string mBlendTreeValueName = "Speed";

    NavMeshAgent mAgent = null;
    
    int mTargetWaypoint = -1;

    Vector3 mTargetPosition = Vector3.zero;

    float mDistanceToWayPoint;

    [SerializeField] float mTargetMaxChangeValue = 2;

    [SerializeField] Transform player = null;

    [SerializeField] float crossResult;
    [SerializeField] float mMaxAngleSight = 45;

    RaycastHit mHit = default;

    [SerializeField] Vector3 mRayOffset = new Vector3(0, 2, 0);

    [SerializeField] float mAngleToPlayer = 45.0f;

    [SerializeField] Text mRecognitionIndicator = null;
    [SerializeField] string mHeadTextSuspicious = "?";
    [SerializeField] Color mHeadColorSuspicious = Color.magenta;
    [SerializeField] string mHeadTextAlarmed = "!";
    [SerializeField] Color mHeadColorAlarmed = Color.magenta;

    public int mPathId { private get; set; } = 0;

    [SerializeField] float mRecognitionDistance = 20.0f;

    float mRecognitionTimer = 0.0f;
    [SerializeField] float mRecognitionTime = 2.0f;

    bool mFollowingPlayer = false;

    //States
    const int M_PATROULLING = 0;    
    const int M_FOLLOWING_PLAYER = 1;
    const int M_CHECKING_SUSPICIOUS_LOCATION = 2;
    const int M_CHECKING_CAMERA_CALL = 3;

    //Aktueller State
    int mActiveState = 0;

    bool hasReachedTarget = false;

    float mWaypointWaitingTime = 5.0f;

    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();

        mWaypointGroup = GameObject.Find(NameAndTagHolder.M_NAME_PATROL_PATHS).transform;
        player = GameObject.Find(NameAndTagHolder.M_NAME_PLAYER).transform.GetChild(0);

        GoToNextWaypoint();

        mActiveState = M_PATROULLING;
    }
    
    void Update()
    {
        Vector3 forward = transform.forward;

        Vector3 toPlayer = (player.position - transform.position).normalized;

        //Winkel zum Spieler
        mAngleToPlayer = Vector3.Angle(forward, toPlayer);
        
        Vector3 raystart = transform.position + mRayOffset;

        //State-Switch
        switch (mActiveState)
        {
            //Spieler verfolgen
            case M_FOLLOWING_PLAYER:
                mAgent.speed = 6;
                mAgent.destination = 
                    new Vector3(player.position.x,
                                transform.position.y, 
                                player.position.z);

                if (Physics.Raycast(raystart, 
                                    player.position - raystart, 
                                    out mHit, 
                                    Mathf.Infinity))
                {
                    switch (mHit.collider.tag)
                    {
                        case NameAndTagHolder.M_TAG_PLAYER:
                            if (mRecognitionTimer <= mRecognitionTime)
                            {
                                mRecognitionTimer += Time.deltaTime;
                            }
                            break;

                        default:
                            mRecognitionTimer -= Time.deltaTime;
                            break;
                    }
                }
                else
                {
                    if (mRecognitionTimer > 0.0f)
                    {
                        //mRecognitionIndicator.text = "";
                        mRecognitionTimer -= Time.deltaTime;
                    }
                    else
                        mRecognitionTimer = 0.0f;
                }

                //Kehre zurück zur Patroullie, wenn Timer kleiner gleich 0
                if(mRecognitionTimer <= 0.0f)
                {
                    mActiveState = M_PATROULLING;
                }
                break;

            //Patroullieren
            case M_PATROULLING:

                if (mAngleToPlayer < mMaxAngleSight && 
                    Physics.Raycast(raystart, 
                                    player.position - raystart, 
                                    out mHit, 
                                    mRecognitionDistance))
                {
                    switch (mHit.collider.tag)
                    {
                        case NameAndTagHolder.M_TAG_PLAYER:
                            if (mRecognitionTimer <= mRecognitionTime)
                            {
                                //Erhöhe Timer, wenn kleiner als Erkennungszeit
                                mRecognitionTimer += Time.deltaTime;
                            }
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    if (mRecognitionTimer > 0.0f)
                    {
                        mRecognitionTimer -= 1.5f * Time.deltaTime;
                    }
                    else
                        mRecognitionTimer = 0.0f;
                }

                if (mRecognitionTimer >= mRecognitionTime)
                {
                    if (!mFollowingPlayer)
                    {
                        mRecognitionIndicator.text = mHeadTextAlarmed;
                        mRecognitionIndicator.color = mHeadColorAlarmed;
                        mActiveState = M_FOLLOWING_PLAYER;
                    }
                }
                else
                if (mRecognitionTimer > 0.0f)
                {
                    //Zeige Alarmbereitschaft
                    mRecognitionIndicator.text = mHeadTextSuspicious;
                    mRecognitionIndicator.color = mHeadColorSuspicious;
                    mRecognitionIndicator.transform.LookAt(player);
                }
                else
                {
                    //mActiveState = M_PATROULLING;
                    mRecognitionIndicator.text = "";
                }
                
                mAgent.speed = 5;
                GoToWaypoint(mTargetWaypoint);
                //Wenn nahe am aktuellen Wegpunkt
                mDistanceToWayPoint = 
                    (mWaypointGroup
                    .GetChild(mPathId)
                    .GetChild(mTargetWaypoint)
                    .position - transform.position).magnitude;

                if (mDistanceToWayPoint <= mTargetMaxChangeValue)
                {
                    if (!mFollowingPlayer && !hasReachedTarget)
                    {
                        hasReachedTarget = true;
                        Invoke("GoToNextWaypoint", mWaypointWaitingTime);
                    }
                }

                break;

            default:
                break;
        }

        mAnimator.SetFloat(mBlendTreeValueName, 
                           mAgent.velocity.magnitude / mAgent.speed);
    }

    //Gehe zu einem bestimmten Wegpunkt
    void GoToWaypoint(int _waypoint)
    {
        mTargetWaypoint = _waypoint;
        mAgent.destination = 
            mWaypointGroup.GetChild(mPathId).GetChild(mTargetWaypoint).position;
    }

    //Gehe zum nächsten Wegpunkt
    void GoToNextWaypoint()
    {
        if(mTargetWaypoint == mWaypointGroup.GetChild(mPathId).childCount - 1)
            mTargetWaypoint = 0;
        else
            mTargetWaypoint++; 

        mAgent.destination = 
            mWaypointGroup.GetChild(mPathId).GetChild(mTargetWaypoint).position;

        hasReachedTarget = false;
    }

    //Methode, um von außerhalb die Coroutine zu starten
    public void GoToCameraAlarm(Vector3 _target, CCTV _callingCamera)
    {
        StartCoroutine(CameraAlarmCoroutine(_target, _callingCamera));
    }

    IEnumerator CameraAlarmCoroutine(Vector3 _target, CCTV _callingCamera)
    {
        float distanceToTarget = 0.0f;
        hasReachedTarget = false;
        mActiveState = M_CHECKING_CAMERA_CALL;

        mAgent.destination = _target;
        mAgent.speed = 7;

        //Solange nicht das Ziel erreicht wurde
        while(!hasReachedTarget)
        {
            distanceToTarget = (_target - transform.position).magnitude;
            //Prüfe, ob aktuelle Distanz zum Punkt kleiner ist 
            //als die maximale Wegpunktsentfernung
            if (distanceToTarget < mTargetMaxChangeValue)
            {
                //Wenn das so ist, dann ist das Ziel erreicht
                hasReachedTarget = true;
            }
            yield return new WaitForEndOfFrame();
        }
        //Warte 5 Sekunden
        yield return new WaitForSeconds(mWaypointWaitingTime);
        //Gehe wieder zurück auf Patroullie
        hasReachedTarget = false;
        distanceToTarget = 0.0f;
        mActiveState = M_PATROULLING;
        //Setze Kamera zurück
        _callingCamera.ResetCall();
        _callingCamera.mAlarmed = false;
        yield return null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case NameAndTagHolder.M_TAG_PLAYER:
                //Gegner hat Spieler gefangen
                //Wechsle zur Menü-Szene
                GameObject.Find(NameAndTagHolder.M_NAME_CANVAS)
                    .GetComponent<GameCanvas>().ShowFinalPanel();
                break;
                
            default:
                break;
        }
    }
}