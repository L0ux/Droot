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
        createNewBout();

    }

    public void onDeathBout(List<Vector2> points = null)
    {
        currentBout = null;
        createRacine(points);
        createNewBout();
    }

    public void createNewBout()
    {
        if (currentBout != null)
            return;
        currentBout = Instantiate(boutPrefab, Vector3.zero, Quaternion.identity);
        myCamera.Follow = currentBout.transform;
    }
    
    public void OnStartBout()
    {
        if (currentBout == null)
            return;
        this.currentBout.startToMove();
    }
    public void createRacine(List<Vector2> points = null)
    {
        if (points == null)
            return;
        Racine racine = Instantiate(racinePrefab, Vector3.zero, Quaternion.identity);
        racine.createCollider(points);
    }


    public void reachWater()
    {
        Debug.Log("C'est GAGNE");
    }
}
