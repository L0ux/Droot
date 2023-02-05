using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cameraman : MonoBehaviour
{
    // Start is called before the first frame update
    public void onEndTravellingStart()
    {
        GameManager.instance.myCameraLarge.Follow = null;
        GameManager.instance.cameraAnimation = false;
        StartCoroutine(GameManager.instance.zoomCamera());
    }
}
