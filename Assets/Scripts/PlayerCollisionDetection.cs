using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interaktion mit Collider des PlayerControllers
public class PlayerCollisionDetection : MonoBehaviour
{
    Inventory mInventory = null;

    private void Start()
    {
        mInventory = transform.GetComponentInChildren<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case NameAndTagHolder.M_TAG_TARGETZONE:
                if(mInventory.HasItemOfType<Book>())
                {
                    //Wechsel zum Hauptmenü, wenn Sicherheitszone betreten wurde
                    //und Bücher im Inventar sind.
                    GameObject.Find(NameAndTagHolder.M_NAME_CANVAS)
                        .GetComponent<GameCanvas>().ShowFinalPanel();
                }
                break;
        }
    }
}
