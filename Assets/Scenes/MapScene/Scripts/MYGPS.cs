using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MYGPS : MonoBehaviour
{

    public float latitude = 0f;
    public float longitude = 0f;

    public MapViewManager mapViewManager;

    IEnumerator Start()
    {
        
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
            
        // Starts the location service.
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
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
            yield break;
        }
        else
        {

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;

            PlayerPrefs.SetFloat("DestinationLatitude",latitude);
            PlayerPrefs.SetFloat("DestinationLongitude", longitude);
        }

        mapViewManager.enabled = true;
    }

   
}
