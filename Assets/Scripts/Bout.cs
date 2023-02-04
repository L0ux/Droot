using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem;
using System;

public class Bout : MonoBehaviour
{
    
    public GameObject mask;

    private Transform target;


    /*0 is straight down*/
    public float angleRotation  = 0;

    private float speed = 0;
    
    private LineRenderer lineRenderer;
    private List<Vector2> points;
    private float pointSpacing = 0.05f;


    private bool isMoving = false ;



    /*Verify*/
    Vector2 spawnLocation;
    private const float distanceInsensibleOtherRoot = 0.8f;


    /*GestionBourgeons*/
    private Vector2 positionLastBourgon ;
    public const int BourgonEveryNbUnit = 3;
    private float distanceBeforeNextBourgeon;

    

    private void Start()
    {
        target = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>(); 
        this.points = new List<Vector2>();
        positionLastBourgon = this.transform.position;
        distanceBeforeNextBourgeon = BourgonEveryNbUnit + UnityEngine.Random.Range(-1, 1);
        lineRenderer.SetPosition(0, target.position);
        points.Add(target.position);
        spawnLocation = target.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 distanceToMouse = mousePosition - (Vector2) target.position;

        /*****ANGLE****/
        float angle = Vector2.SignedAngle(Vector2.down, distanceToMouse);

        /*REMAP angle */
        float max = 79;
        float min = -79;

        angle = Mathf.Min(angle, max);
        angle = Mathf.Max(angle, min);

        /*****SPEED******/
        /*Si on bouge ET souris sous la racine*/
        if (this.isMoving)
            this.speed = Vector2.Distance(target.position, mousePosition) / 4;
        else
            speed = 0f;


        
        /*A REFACTO */
        bool angleTooMUCH = angle > 80 || angle < -80;
        bool mouseAboveBout = (mousePosition.y >target.position.y);

        if (mouseAboveBout)
            speed = speed / 2;
        if(angleTooMUCH)
            speed = speed / 2;        



        this.angleRotation = angle;

        /*AddNoise � l'angle a refaire 
         * A REFAIRE pour plus consistant*/
        this.angleRotation += UnityEngine.Random.Range(-60, 60);


        /****** MOVE *******/
        Vector2 move = Quaternion.AngleAxis(angleRotation, new Vector3(0, 0, 1)) * Vector2.down * speed;
        Vector2 destination = (Vector2)target.position + move ;
        

        target.DOMove(destination,1);


        drawLine();
        spawnBourgeon();

  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle" || other.tag == "Border")
        {
            die();
        }

        if(other.tag == "Root" &&  Vector2.Distance(target.position,spawnLocation)>distanceInsensibleOtherRoot)
        {
            die();
        }

        if(other.tag == "Eau")
        {
            GameManager.instance.reachWater();
            die();
        }
        
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Root" && Vector2.Distance(target.position, spawnLocation) > distanceInsensibleOtherRoot)
        {
            die();
        }
    }

    void drawLine()
    {
        /****DRAW LINE*******/
        if (Vector2.Distance(target.position, points.Last()) > pointSpacing)
        {
            points.Add(target.position);
            lineRenderer.positionCount = points.Count();
            lineRenderer.SetPosition(points.Count - 1, target.position);
        }

    }


    private void spawnBourgeon()
    {
        /*InitBourgeon*/
        if (Vector2.Distance(target.position, positionLastBourgon) > distanceBeforeNextBourgeon)
        {
            GameManager.instance.createBourgeon(target.position);
            positionLastBourgon = target.position;
            distanceBeforeNextBourgeon = BourgonEveryNbUnit + UnityEngine.Random.Range(-1, 1);

        }
    }
    public void die()
    {
        GameManager.instance.onDeathBout(points);
        mask.SetActive(true);
        target.DOMove(new Vector2(target.position.x, target.position.y), 1);
        Destroy(this);
    }

    public void startToMove()
    {
        this.isMoving = true;        
    }

}
