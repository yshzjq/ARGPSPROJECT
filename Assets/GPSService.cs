using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSService : MonoBehaviour
{
    public Text textMsg;

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
            textMsg.text = "Timed out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
        else
        {
            while (true)
            {
                textMsg.text = "À§Ä¡: "
                + Input.location.lastData.latitude + " "
                + Input.location.lastData.longitude + " "
                + Input.location.lastData.horizontalAccuracy;
                yield return new WaitForSeconds(1);
            }
        }

        // Input.location.Stop();
    }
}
