using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VisionMask : MonoBehaviour
{
    new Transform transform;

    private void OnEnable()
    {
        transform = GetComponent<Transform>();
        Vector3 desiredScale = new Vector3(30, 15, 0);
        transform.DOScale(desiredScale, 2);
    }
}
