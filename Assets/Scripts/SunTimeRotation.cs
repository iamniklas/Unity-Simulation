using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Basiert auf Sonnenrotier-Script aus dem Unterricht von Ralf Hüwe, SAE München
public class SunTimeRotation : MonoBehaviour
{
    [SerializeField] float mRotationSpeed = 0.25f;

    [SerializeField] float mYRotation = -20.0f;

    int mAngleChangePerHour = 15;
    int mHoursPerDay = 24;

    public void SetStartRotationWithSeed(int _seed)
    {
        System.Random rand = new System.Random(_seed);
        int randomNumber = rand.Next() % mHoursPerDay;
        transform.rotation = 
            Quaternion.Euler(randomNumber * mAngleChangePerHour, mYRotation, 0.0f);
    }
    
    void Update()
    {
        transform.Rotate(Vector3.right * mRotationSpeed * Time.deltaTime);
    }
}
