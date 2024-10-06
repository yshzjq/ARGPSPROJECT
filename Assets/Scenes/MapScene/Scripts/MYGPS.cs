using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MYGPS : MonoBehaviour
{

    public double latitude;
    public double longitude;

    public MapViewManager mapViewManager;

    IEnumerator Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;
        // Starts the location service.
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status ==
        LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;  

            mapViewManager.enabled = true;
        }
    }
}
