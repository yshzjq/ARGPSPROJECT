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



    // ����
    public bool isNoticeMessageAppear = false;

    // ��ĵ ����
    string[] scanInfo; // 0 �̸� 1 �浵 2 ���� ��

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
        // �̹����� ���� Tracking �������� Ȯ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (isNoticeMessageAppear == false)
            {
                isNoticeMessageAppear=true;
                // �̹����� �ν��ϸ� ó�� (���⼭ �޽��� ǥ��)
                string[] scanInfo = trackedImage.referenceImage.name.Split(',');


                MainManager.instance.AppearNoticeMessage(scanInfo[0] + "\n��ġ�� ���ðڽ��ϱ�?");

                PlayerPrefs.SetFloat("Latitude", float.Parse(scanInfo[1]));
                PlayerPrefs.SetFloat("Longitude", float.Parse(scanInfo[2]));

            }
        }
    }

    public void PushYesBtn()
    {
        // ������ �����ش�.
    }

    public void PushNoBtn()
    {
        isNoticeMessageAppear = false;
    }


}
