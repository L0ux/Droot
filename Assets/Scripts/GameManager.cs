using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [SerializeField] private Bout boutPrefab;
    [SerializeField] private Racine racinePrefab;
    [SerializeField] private CinemachineVirtualCamera myCamera;
    Bout currentBout;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }




    public void Start()
    {
        launchBout();
    }

    public void launchBout(List<Vector2> points = null)
    {
        if(points != null)
        {
            Racine racine = Instantiate(racinePrefab, Vector3.zero, Quaternion.identity);
            racine.createCollider(points);
        }
        currentBout = Instantiate(boutPrefab,Vector3.zero,Quaternion.identity);
        myCamera.Follow = currentBout.transform;
    }
    
    public void OnStartBout()
    {
        this.currentBout.startToMove();
    }
}
