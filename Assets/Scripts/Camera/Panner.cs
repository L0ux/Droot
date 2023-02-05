using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Panner : MonoBehaviour
{

    [SerializeField] private float panSpeed ;
    private CinemachineInputProvider myInputProvider;
    private CinemachineVirtualCamera virtualCamera;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        myInputProvider = GetComponent<CinemachineInputProvider>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cameraTransform = GetComponent<Transform>();

    }

    private void Update()
    {
        float x = myInputProvider.GetAxisValue(0);
        float y = myInputProvider.GetAxisValue(1);
        float z = myInputProvider.GetAxisValue(2);
        if(y != 0 && virtualCamera.Priority >= 10)
        {
            panScreen(x, y);
        }

    }
    public Vector2 panDirection(float x , float y)
    {
        Vector2 direction = Vector2.zero;
        if(y >= Screen.height * 0.7f)
        {
            direction.y += 1; 
        }else if (y <= Screen.height * 0.3f)
        {
            direction.y -= 1;
        }

        return direction;

    }
    public void panScreen(float x, float y)
    {
        Vector2 direction = panDirection(x, y);
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
            cameraTransform.position + (Vector3) direction * panSpeed,
            Time.deltaTime); 
    }
}
