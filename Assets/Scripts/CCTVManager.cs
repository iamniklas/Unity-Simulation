using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVManager : MonoBehaviour
{
    //Liste aller Kameras
    List<CCTV> mCameras = new List<CCTV>();    

    //Liste aller Kameras aktualisieren (nach Festlegung der aktiven Kameras)
    public void UpdateCameraList()
    {
        mCameras.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            mCameras.Add(transform
                        .GetChild(i)
                        .GetChild(1)
                        .GetComponent<CCTV>());
        }
    }

    //Kamera mit Index rotieren
    public void RotateCamera(int _index, int _rotation)
    {
        mCameras[_index].SetRandomRotation(_rotation);
    }

    //Alle Kameras deaktivieren
    public void DisableCameras()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<CCTV>().DisableCamera();
        }
    }
}