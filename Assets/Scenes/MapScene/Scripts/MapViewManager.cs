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

    private Texture2D[] textures;
    public int textureIdx = 2;
    private int MaxIdx = 5;

    void Start()
    {
        textures = new Texture2D[5];
        StartCoroutine("GetMapImage");
    }

    private IEnumerator GetMapImage()
    {
        // 구글 맵 Static API URL
        string[] urls = new string[5];
        
        int zoom = 6;

        for (int i = 0; i < 5; i++)
        {
            urls[i] = "https://maps.googleapis.com/maps/api/staticmap?"
                   + "center=" + userLatitude + "," + userLongitude
                   + "&zoom=" + zoom
                   + "&size=1000x1000"
                   + "&markers=color:blue%7C" + userLatitude + "," + userLongitude
                   + "&markers=color:red%7C" + destinationLatitude + "," + destinationLongitude
                   + "&key=" + UnityWebRequest.EscapeURL(apiKey);

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(urls[i]))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    textures[i] = texture;
                }
                else
                {
                    Debug.LogError("Error fetching map: " + webRequest.error);
                }
            }

            zoom += 3;
        }

        mapImage.texture = textures[textureIdx];
        
    }

    public void AddBtn()
    {
        if (textureIdx + 1 >= MaxIdx) return;

        textureIdx++;
        mapImage.texture = textures[textureIdx];
    }

    public void SubTractBtn()
    {
        if (textureIdx - 1 < 0) return;

        textureIdx--;
        mapImage.texture = textures[textureIdx];
    }

    public void CancelAndExitBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}