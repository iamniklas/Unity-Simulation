using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    RaycastHit mHit = default;

    [SerializeField] KeyCode mInteractionKey = KeyCode.F;

    [SerializeField] float mMaxInteractionDistance = 2f;

    [SerializeField] string[] mInteractiveTags = 
    {
        NameAndTagHolder.M_TAG_DOOR,
        NameAndTagHolder.M_TAG_KEYCARD,
        NameAndTagHolder.M_TAG_KEYREADER,
        NameAndTagHolder.M_TAG_BUTTON
    };

    [SerializeField] GameObject mInteractionHintText = null;

    [SerializeField] GameObject mCardIndicatorIcon = null;

    Transform mPlayerRaycastStart = null;
    
    void Update()
    {
        mPlayerRaycastStart = Camera.main.transform;

        //Raycast für Interaktionshinweis
        if (Physics.Raycast(mPlayerRaycastStart.position, 
                            transform.forward, 
                            out mHit) 
            && mHit.distance < mMaxInteractionDistance)
        {
            //Überprüfung, ob aktuelles Objekt interaktiv ist
            if (Array.Exists(mInteractiveTags, 
                element => element == mHit.collider.tag))
            {
                if(!(mHit.collider.tag.Equals(NameAndTagHolder.M_TAG_KEYREADER)
                    && !GetComponent<Inventory>().HasItemOfType<Keycard>()))
                {
                    //Zeige Hinweis, wenn aktuelles Objekt interaktiv ist
                    //und beim Keyreader nur, wenn die Keycard im Inventar ist
                    mInteractionHintText.SetActive(true);
                }
            }
            else
            {
                mInteractionHintText.SetActive(false);
            }
        }
        else
        {
            mInteractionHintText.SetActive(false);
        }

        if (Input.GetKeyDown(mInteractionKey))
        {
            if(Physics.Raycast(transform.position, 
                               transform.forward, 
                               out mHit) 
               && mHit.distance < mMaxInteractionDistance)
            {
                //Unterscheidung je nach Objekt-Tag bei Klick der Interaktionstaste
                switch (mHit.collider.gameObject.tag)
                {
                    case NameAndTagHolder.M_TAG_DOOR:
                        mHit.transform.parent.GetComponent<Door>().ToggleDoor();
                        break;

                    case NameAndTagHolder.M_TAG_KEYCARD:
                        GetComponent<Inventory>().AddItem(new Keycard());
                        Destroy(mHit.collider.gameObject);

                        mCardIndicatorIcon.SetActive(true);
                        break;

                    case NameAndTagHolder.M_TAG_KEYREADER:
                        if (GetComponent<Inventory>().HasItemOfType<Keycard>())
                        {
                            mHit.transform.parent.GetComponent<SecuredDoor>()
                                .GrantAccess();
                        }
                        break;

                    case NameAndTagHolder.M_TAG_BUTTON:
                        mHit.collider.GetComponent<SecuritySystemButton>()
                            .ButtonClick();
                        break;

                    case NameAndTagHolder.M_TAG_TARGET:
                        GetComponent<Inventory>().AddItem(new Book());
                        Destroy(mHit.collider.gameObject);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case NameAndTagHolder.M_TAG_TARGETZONE:
                if(GetComponent<Inventory>().HasItemOfType<Book>())
                {
                    GameObject.Find(NameAndTagHolder.M_NAME_CANVAS)
                        .GetComponent<GameCanvas>().ShowFinalPanel();
                    //SceneManager.LoadScene(mMenuSceneIndex);
                }
                break;

            default:
                break;
        }
    }
}