using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Linq;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [SerializeField] private Bout boutPrefab;
    [SerializeField] private Racine racinePrefab;
    [SerializeField] private GameObject bourgeonPrefab;
    [SerializeField] private GameObject bourgeonContainer;

    [SerializeField] private float sizeCameraOnZoom;
    [SerializeField] private float sizeCameraOnDezoom;

    [SerializeField] private GameObject startPoint;

    private List<GameObject> bourgeons;

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
        bourgeons = new List<GameObject>();
        myCamera.m_Lens.OrthographicSize = sizeCameraOnDezoom;

        createBourgeon(startPoint.transform.position);

    }

    public void onDeathBout(List<Vector2> points = null)
    {
        currentBout = null;
        createRacine(points);
        /*Dezoom*/
        myCamera.m_Lens.OrthographicSize = sizeCameraOnDezoom;
        

    }

    private void createNewBout(Vector2 position)
    {
        if (currentBout != null)
            return;
        myCamera.m_Lens.OrthographicSize = sizeCameraOnZoom;

        currentBout = Instantiate(boutPrefab, position, Quaternion.identity);
        myCamera.Follow = currentBout.transform;
    }
    
    private void OnClick()
    {
        if (currentBout == null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Transform closestBourgeon =  bourgeonContainer.GetComponentsInChildren<Transform>().OrderBy(trsf => Vector2.Distance(trsf.position,mousePosition) ).First();
            createNewBout(closestBourgeon.position);
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

    public void createBourgeon(Vector2 positionBourgeon )
    {
        GameObject newBourgeon = Instantiate(bourgeonPrefab, bourgeonContainer.transform);
        newBourgeon.transform.position = positionBourgeon;
    }

    public void reachWater()
    {
        Debug.Log("C'est GAGNE");
    }
}
