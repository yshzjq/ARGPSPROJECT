using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapViewManager : MonoBehaviour
{
    public RawImage mapImage; // UI에 배치한 RawImage
    private string apiKey = "AIzaSyBBvtvqRKiE736jth34T558YPwAHmXrSJ8"; // 자신의 구글 맵 API 키 입력
    private float userLatitude = 37.5665f; // 예: 서울 위도
    private float userLongitude = 126.978f; // 예: 서울 경도
    private float destinationLatitude = 37.5805f; // 목적지 위도
    private float destinationLongitude = 126.995f; // 목적지 경도

    private Texture2D[][] textures;
    public int textureIdx = 3;
    public int posIdx = 0;
    private int MaxIdx = 7;
    //private int MaxPosIdx = 2;

    public GameObject LoadingObject;

    

    void Start()
    {
        userLatitude = PlayerPrefs.GetFloat("DestinationLatitude");
        userLongitude = PlayerPrefs.GetFloat("DestinationLongitude");
        destinationLatitude = PlayerPrefs.GetFloat("Latitude");
        destinationLongitude = PlayerPrefs.GetFloat("Longitude");


        textures = new Texture2D[2][];
        textures[0] = new Texture2D[8];
        textures[1] = new Texture2D[8];
        StartCoroutine("GetMapImage");
    }

    private IEnumerator GetMapImage()
    {
        // 구글 맵 Static API URL
        string[][] urls = new string[2][];
        urls[0] = new string[8];
        urls[1] = new string[8];
        
        int zoom = 7;

        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 2; j++)
            {

                if (j == 0)
                {
                    urls[j][i] = "https://maps.googleapis.com/maps/api/staticmap?"
                           + "center=" + destinationLatitude + "," + destinationLongitude
                           + "&zoom=" + zoom
                           + "&size=1000x1000"
                           + "&markers=color:blue%7C" + userLatitude + "," + userLongitude
                           + "&markers=color:red%7C" + destinationLatitude + "," + destinationLongitude
                           + "&key=" + UnityWebRequest.EscapeURL(apiKey);
                }
                else
                {
                    urls[j][i] = "https://maps.googleapis.com/maps/api/staticmap?"
                           + "center=" + userLatitude + "," + userLongitude
                           + "&zoom=" + zoom
                           + "&size=1000x1000"
                           + "&markers=color:blue%7C" + userLatitude + "," + userLongitude
                           + "&markers=color:red%7C" + destinationLatitude + "," + destinationLongitude
                           + "&key=" + UnityWebRequest.EscapeURL(apiKey);
                }

                using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(urls[j][i]))
                {
                    yield return webRequest.SendWebRequest();

                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                        textures[j][i] = texture;
                    }
                    else
                    {
                        Debug.LogError("Error fetching map: " + webRequest.error);
                    }
                }
            }
            zoom += 2;
        }

        mapImage.texture = textures[posIdx][textureIdx];

        LoadingObject.SetActive(false);
        
    }

    public void AddBtn()
    {
        if (textureIdx + 1 >= MaxIdx) return;

        textureIdx++;
        mapImage.texture = textures[posIdx][textureIdx];
    }

    public void SubTractBtn()
    {
        if (textureIdx - 1 < 0) return;

        textureIdx--;
        mapImage.texture = textures[posIdx][textureIdx];
    }

    public void DesPosBtn()
    {
        posIdx = 0;

        mapImage.texture = textures[posIdx][textureIdx];
    }

    public void MyPosBtn()
    {
        posIdx = 1;

        mapImage.texture = textures[posIdx][textureIdx];
    }

    public void CancelAndExitBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}