using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [SerializeField] private Bout boutPrefab;
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
    public void launchBout()
    {
        currentBout = Instantiate(boutPrefab,Vector3.zero,Quaternion.identity);
        myCamera.Follow = currentBout.transform;
    }
    
    public void OnStartBout()
    {
        Debug.Log("OnStarBout");
        this.currentBout.startToMove();
    }
}
