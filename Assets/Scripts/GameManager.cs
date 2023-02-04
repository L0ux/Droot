using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

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

    [SerializeField] private CinemachineVirtualCamera myCamera;
    Bout currentBout;

    Bourgeon currentSelectedBourgeon;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        myCamera.m_Lens.OrthographicSize = sizeCameraOnDezoom;
        createBourgeon(startPoint.transform.position);
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
        currentSelectedBourgeon.GetComponent<SpriteRenderer>().color = Color.yellow;

    }
    private void deselectBourgeon()
    {
        if (currentSelectedBourgeon == null)
            return;
        currentSelectedBourgeon.GetComponent<SpriteRenderer>().color = Color.white;
        currentSelectedBourgeon = null;
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

        deselectBourgeon();
        myCamera.m_Lens.OrthographicSize = sizeCameraOnZoom;

        currentBout = Instantiate(boutPrefab, position, Quaternion.identity);
        myCamera.Follow = currentBout.transform;
    }
    
    private void OnClick()
    {
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
