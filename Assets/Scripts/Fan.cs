using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    //Rotiergeschwindigkeit in Grad pro Sekunde
    [SerializeField] float mRotationSpeed = 360.0f;

    //Rotationsvektor
    Vector3 mRotationVector = Vector3.up;

    void Update()
    {
        //Rotieren
        transform.Rotate(mRotationVector * (mRotationSpeed * Time.deltaTime));
    }
}
