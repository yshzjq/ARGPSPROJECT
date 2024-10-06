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

    public string destinationLatitude = "34.0522";  // 목표 위치 위도 (예시: 로스앤젤레스)
    public string destinationLongitude = "-118.2437";  // 목표 위치 경도

    private string apiKey = "AIzaSyBBvtvqRKiE736jth34T558YPwAHmXrSJ8";  // Google Cloud API 키


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
        //string originLatitude = (mygps.latitude).ToString();  // 예: 샌프란시스코
        //string originLongitude = (mygps.longitude).ToString();

        // Debug.Log(originLongitude + "  " + originLatitude);

        string originLatitude = "37.280270";
        string originLongitude = "126.83260";

        string destinationLatitude = "37.280766";  // 예: 로스앤젤레스
        string destinationLongitude = "126.829231";

        string origin = originLatitude + "," + originLongitude;
        string destination = destinationLatitude + "," + destinationLongitude;



        // Directions API URL 생성
        string url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + origin +
            "&destination=" + destination + "&key=" + apiKey;

        // Directions API 요청
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("1");
            // 결과를 JSON으로 파싱
            string jsonResult = request.downloadHandler.text;
            JObject jsonData = JObject.Parse(jsonResult);

            // 경로 추출
            var routes = jsonData["routes"];
            if (routes != null && routes.HasValues)
            {
                // 경로를 가져옴
                var route = routes[0];
                var overviewPolyline = route["overview_polyline"]["points"].ToString();

                // 지도에 경로를 그리기 위해 이 데이터를 사용
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
        // Static Maps API URL 생성
        string baseURL = "https://maps.googleapis.com/maps/api/staticmap?";
        string path = "path=enc:" + polyline;  // Directions API에서 받은 polyline 경로를 사용
        string url = baseURL + "size=600x400&" + path +
            "&markers=color:blue%7C" + origin +
            "&markers=color:red%7C" + destination +
            "&key=" + apiKey;

        // 지도 이미지 요청
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Texture2D를 UI에 적용
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
            mapImage.texture = texture;  // 지도 이미지를 RawImage에 적용
        }
        else
        {
            Debug.LogError("Failed to load map: " + request.error);
        }
    }
}

