using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBtnFunction : MonoBehaviour
{
    public ImageTrackingService its;

    public void WindowOutNotice()
    {
        its.isNoticeMessageAppear = false;
    }
}
