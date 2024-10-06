using System;
using System.Collections;   
using UnityEngine;
using UnityEngine.Android;


public class GPSlocation : MonoBehaviour
{



    [HideInInspector]
    public double latitude = 0;
    [HideInInspector]
    public double longitude = 0;

    [HideInInspector]
    public float delay;
    [HideInInspector]
    public float maxtime = 5.0f;

    [HideInInspector]
    public bool receiveGPS = false;

    //double detailed_num = 1.0;

    private void Start()
    {
        StartCoroutine(Gps_man());
        Input.compass.enabled = true;
    }

    IEnumerator Gps_man()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) // 권한 요청하기  // GPS 요청 
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        Input.location.Start(0.1f, 0.1f); ;
        while (Input.location.status == LocationServiceStatus.Initializing && delay < maxtime)
        {
            yield return new WaitForSeconds(1);
            delay++;
        }

        receiveGPS = true;

        while (receiveGPS)
        {
            
            latitude = MathF.Truncate(Input.location.lastData.latitude*100000) / 100000f;
            longitude = MathF.Truncate(Input.location.lastData.longitude * 100000 ) / 100000f;

            yield return new WaitForSeconds(0.2f);
        }
    }
}

