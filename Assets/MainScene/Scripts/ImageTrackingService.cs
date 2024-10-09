using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ImageTrackingService : MonoBehaviour
{
    public ARTrackedImageManager manager;

    public MainManager mainManager;



    // 상태
    public bool isNoticeMessageAppear = false;

    // 스캔 정보
    string[] scanInfo; // 0 이름 1 경도 2 위도 순

    private void OnEnable()
    {
        manager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
       manager.trackedImagesChanged -= OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage t in eventArgs.added)
        {
            HandleTrackedImage(t);
        }
        foreach (ARTrackedImage t in eventArgs.updated)
        {
            HandleTrackedImage(t);
        }

    }

    void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        // 이미지가 현재 Tracking 상태인지 확인
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (isNoticeMessageAppear == false)
            {
                isNoticeMessageAppear=true;
                // 이미지를 인식하면 처리 (여기서 메시지 표시)
                string[] scanInfo = trackedImage.referenceImage.name.Split(',');


                MainManager.instance.AppearNoticeMessage(scanInfo[0] + "\n위치를 보시겠습니까?");

                PlayerPrefs.SetFloat("Latitude", float.Parse(scanInfo[1]));
                PlayerPrefs.SetFloat("Longitude", float.Parse(scanInfo[2]));

            }
        }
    }

    public void PushYesBtn()
    {
        // 지도를 보여준다.
    }

    public void PushNoBtn()
    {
        isNoticeMessageAppear = false;
    }


}
