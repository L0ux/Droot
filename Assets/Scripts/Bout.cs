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

    GameObject[] waterPOints;
    private Transform target;
    
    /*0 is straight down*/
    private float angleRotation  = 0; 
    private float speed = 0;
    
    private LineRenderer lineRenderer;
    private List<Vector2> points;

    private bool isMoving = false ;

    private float pointSpacing = 0.05f;
    public const float ignoreCollisionTime = 0.5f;
    private float lifeTime = .0f;


    /*GestionBourgeons*/
    private Vector2 positionLastBourgon ;
    public const int BourgonEveryNbUnit = 3;

    private void Start()
    {
        target = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>(); 
        this.points = new List<Vector2>();
        positionLastBourgon = this.transform.position;
        lineRenderer.SetPosition(0, target.position);
        points.Add(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 distanceToMouse = mousePosition - (Vector2) target.position;

        /*****SPEED******/
       
        /*Si on bouge ET souris sous la racine*/
        if (this.isMoving && target.position.y > mousePosition.y)
            this.speed = (target.position.y - mousePosition.y) / 4;
        else
            speed = 0f;

        /*****ANGLE****/
        this.angleRotation = Vector2.SignedAngle(Vector2.down, distanceToMouse);
        /*AddNoise � l'angle a refaire 
         * A REFAIRE pour plus consistant*/
        this.angleRotation += UnityEngine.Random.Range(-60, 60);


        /****** MOVE *******/
        Vector2 move = Quaternion.AngleAxis(angleRotation, new Vector3(0, 0, 1)) * Vector2.down * speed;
        Vector2 destination = (Vector2)target.position + move ;        
        

        target.DOMove(destination,1);


        
    
        /****DRAW LINE*******/
        if( Vector2.Distance(target.position,points.Last()) > pointSpacing){
            points.Add(target.position);
            lineRenderer.positionCount = points.Count();
            lineRenderer.SetPosition(points.Count -1,target.position);
        }

        if(Math.Abs(target.position.y- positionLastBourgon.y) > BourgonEveryNbUnit)
        {
            GameManager.instance.createBourgeon(target.position);
            positionLastBourgon = target.position;
        }


        if (isMoving && speed > 0f)
            lifeTime += Time.deltaTime;        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle" || other.tag == "Border")
        {
            die();
        }

        if(other.tag == "Root" && lifeTime > ignoreCollisionTime)
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
        if (collision.tag == "Root" && lifeTime > ignoreCollisionTime)
        {
            die();
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
