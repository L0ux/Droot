using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] int nbRacineMaxForLvl;
    [SerializeField] List<GameObject> allWater;
    private List<GameObject> activatedWater;
    private int nbBoutDead;

    [SerializeField] TextMeshProUGUI textEau;
    [SerializeField] TextMeshProUGUI textRacine;


    public static GameManager instance;
    [SerializeField] private Bout boutPrefab;
    [SerializeField] private Racine racinePrefab;
    [SerializeField] private GameObject bourgeonPrefab;
    [SerializeField] private GameObject bourgeonContainer;

    [SerializeField] private float sizeCameraOnZoom;
    [SerializeField] private float sizeCameraOnDezoom;

    [SerializeField] private GameObject startPoint;

    [SerializeField] public CinemachineVirtualCamera myCameraLarge;
    [SerializeField] public CinemachineVirtualCamera myCameraSerre;


    Bout currentBout;

    Bourgeon currentSelectedBourgeon;
    bool isWinning = false;
    public bool cameraAnimation = true;


    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        activatedWater = new List<GameObject>();
        nbBoutDead = 0;
        textRacine.text = (nbRacineMaxForLvl - nbBoutDead ) + " <sprite name=Racine>";
        textEau.text = activatedWater.Count() + "/" + allWater.Count() + " <sprite name=GoutteEau>";
        myCameraSerre.Follow=startPoint.transform;

        createBourgeon(startPoint.transform.position,90);
        selectBourgeon(bourgeonContainer.GetComponentInChildren<Bourgeon>());
    }

    public void Update()
    {
        if (currentBout == null)
            waitForSelectionBourgeon();
    }

    private void waitForSelectionBourgeon()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Bourgeon[] allBourgeons = bourgeonContainer.GetComponentsInChildren<Bourgeon>().ToArray();
        if (allBourgeons.Count() == 0)
            return;
        Bourgeon closestBourgeon =  bourgeonContainer.GetComponentsInChildren<Bourgeon>().OrderBy(brg => Vector2.Distance(brg.transform.position, mousePosition)).First();

        selectBourgeon(closestBourgeon);
    }

    private void selectBourgeon(Bourgeon bourgeon)
    {
        if (currentSelectedBourgeon == bourgeon)
            return;
        
        deselectBourgeon();
        currentSelectedBourgeon = bourgeon;
        currentSelectedBourgeon.Select();

    }
    private void deselectBourgeon()
    {
        if (currentSelectedBourgeon == null)
            return;
        currentSelectedBourgeon.UnSelect();
        currentSelectedBourgeon = null;
    }
    public void onDeathBout(List<Vector2> points = null)
    {


        nbBoutDead++;
        textRacine.text = (nbRacineMaxForLvl - nbBoutDead) + " <sprite name=Racine>";

        currentBout = null;
        createRacine(points);
        myCameraLarge.transform.position = new Vector3(points.Last().x, points.Last().y+10, -10);

        /*Dezoom*/
        StartCoroutine(dezoomCamera());
    }

    private void createNewBout(Vector2 position)
    {
        if (currentBout != null)
            return;

        deselectBourgeon();
        currentBout = Instantiate(boutPrefab, position, Quaternion.identity);
        StartCoroutine(zoomCamera());
        myCameraSerre.Follow = currentBout.transform;
    }
    
    private void OnClick()
    {

        if (isWinning || cameraAnimation)
            return;

        if (currentBout == null)
        {
            createNewBout(currentSelectedBourgeon.transform.position);
        }
        else
            this.currentBout.startToMove();
    }

    private void createRacine(List<Vector2> points = null)
    {
        if (points == null)
            return;
        Racine racine = Instantiate(racinePrefab, Vector3.zero, Quaternion.identity);
        racine.createCollider(points);
    }

    public void createBourgeon(Vector2 positionBourgeon, float yrotation = 0 )
    {
        GameObject newBourgeon = Instantiate(bourgeonPrefab, bourgeonContainer.transform);
        if(yrotation == 0)
        {
            int random = UnityEngine.Random.Range(0, 10);
            Debug.Log(random);
            if (random > 5)
            {
                yrotation = UnityEngine.Random.Range(-230, -270);
            }
            else
            {
                yrotation = UnityEngine.Random.Range(-20, -115);
            }
        }

        Vector3 desiredRotation = new Vector3(0, 0, yrotation);
        newBourgeon.transform.DORotate(desiredRotation,0);
        newBourgeon.transform.position = positionBourgeon;
    }

    public bool onReachWater(GameObject eau, Bout picker)
    {
        if (!activatedWater.Contains(eau))
        {
            myCameraSerre.Follow = eau.transform;
            eau.GetComponent<Eau>().pickWater(picker);
            eau.GetComponent<PolygonCollider2D>().enabled = false;
            activatedWater.Add(eau);
            textEau.text = activatedWater.Count() + "/" + allWater.Count() + " <sprite name=GoutteEau>";
            
            if(activatedWater.Count() == allWater.Count())
            {
                onWin();
            }
        }
        return false;
    }

    private void onWin()
    {
        isWinning = true;
        Debug.Log("C'est gagn?");
    }

    public IEnumerator zoomCamera()
    {
        myCameraLarge.Priority = 1;
        myCameraSerre.Priority = 10;

        yield return null;
        /*while( myCamera.m_Lens.OrthographicSize > sizeCameraOnZoom)
        {
            myCamera.m_Lens.OrthographicSize -= 0.1f;
            yield return null;
        }*/
    }

    public IEnumerator dezoomCamera()
    {
        myCameraLarge.Priority = 10;
        myCameraSerre.Priority = 1;

        yield return null;
        /*while (myCamera.m_Lens.OrthographicSize < sizeCameraOnDezoom)
        {
            myCamera.m_Lens.OrthographicSize += 0.1f;
            yield return null;
        }*/
    }

}
