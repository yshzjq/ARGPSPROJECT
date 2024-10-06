using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Android;
using Newtonsoft.Json.Linq;

public class MapManager : MonoBehaviour
{
    public RawImage mapImage;
    public MYGPS mygps;

    public string destinationLatitude = "34.0522";  // ��ǥ ��ġ ���� (����: �ν���������)
    public string destinationLongitude = "-118.2437";  // ��ǥ ��ġ �浵

    private string apiKey = "AIzaSyBBvtvqRKiE736jth34T558YPwAHmXrSJ8";  // Google Cloud API Ű


    // 
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
        StartCoroutine("GetDirections");
    }



    IEnumerator GetDirections()
    {
        //string originLatitude = (mygps.latitude).ToString();  // ��: �������ý���
        //string originLongitude = (mygps.longitude).ToString();

        // Debug.Log(originLongitude + "  " + originLatitude);

        string originLatitude = "37.280270";
        string originLongitude = "126.83260";

        string destinationLatitude = "37.280766";  // ��: �ν���������
        string destinationLongitude = "126.829231";

        string origin = originLatitude + "," + originLongitude;
        string destination = destinationLatitude + "," + destinationLongitude;



        // Directions API URL ����
        string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +
            "&destination=" + destination + "&key=" + apiKey;

        // Directions API ��û
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("1");
            // ����� JSON���� �Ľ�
            string jsonResult = request.downloadHandler.text;
            JObject jsonData = JObject.Parse(jsonResult);

            // ��� ����
            var routes = jsonData["routes"];
            if (routes != null && routes.HasValues)
            {
                // ��θ� ������
                var route = routes[0];
                var overviewPolyline = route["overview_polyline"]["points"].ToString();

                // ������ ��θ� �׸��� ���� �� �����͸� ���
                StartCoroutine(DisplayRouteOnMap(overviewPolyline, origin, destination));
            }
            else
            {
                Debug.LogError("No routes found.");
            }
        }
        else
        {
            Debug.LogError("Error in fetching directions: " + request.error);
        }
    }

    IEnumerator DisplayRouteOnMap(string polyline, string origin, string destination)
    {
        Debug.Log("1");
        // Static Maps API URL ����
        string baseURL = "https://maps.googleapis.com/maps/api/staticmap?";
        string path = "path=enc:" + polyline;  // Directions API���� ���� polyline ��θ� ���
        string url = baseURL + "size=600x400&" + path +
            "&markers=color:blue%7C" + origin +
            "&markers=color:red%7C" + destination +
            "&key=" + apiKey;

        // ���� �̹��� ��û
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Texture2D�� UI�� ����
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
            mapImage.texture = texture;  // ���� �̹����� RawImage�� ����
        }
        else
        {
            Debug.LogError("Failed to load map: " + request.error);
        }
    }
}

