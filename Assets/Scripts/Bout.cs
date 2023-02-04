using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.InputSystem;

public class Bout : MonoBehaviour
{
    private Transform target;
    private LineRenderer lineRenderer;    
    private List<Vector2> points;
    private float pointSpacing = 0.05f;
    private bool move = false ;
    public const float ignoreCollisionTime = 0.5f;
    private float lifeTime = .0f;

    private void Start()
    {
        target = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>(); 
        this.points = new List<Vector2>();
        points.Add(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 destination =  (Vector2)target.position + ((mousePosition -(Vector2)target.position )/10) ;

        /*if(mousePosition.y > target.position.y){
            target.DOMove(target.position,1);
        }else{
            target.DOMove(new Vector2(destination.x,mousePosition.y),1);
        }*/


        if (!this.move)
            return;

        lifeTime += Time.deltaTime;

        Debug.Log(lifeTime);


       target.DOMove(new Vector2(destination.x,mousePosition.y),1);
        
    

        if( Vector2.Distance(target.position,points.Last()) > pointSpacing){
            points.Add(target.position);        
            lineRenderer.positionCount = points.Count();
            lineRenderer.SetPosition(points.Count -1,target.position);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Obstacle")
        {
            GameManager.instance.launchBout(points);
            target.DOMove(new Vector2(target.position.x, target.position.y), 1);
            Destroy(this);
        }

        if(other.tag == "Root" && lifeTime > ignoreCollisionTime)
        {
            GameManager.instance.launchBout(points);
            target.DOMove(new Vector2(target.position.x, target.position.y), 1);
            Destroy(this);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Root" && lifeTime > ignoreCollisionTime)
        {
            GameManager.instance.launchBout(points);
            target.DOMove(new Vector2(target.position.x, target.position.y), 1);
            Destroy(this);
        }
    }


    public void startToMove()
    {
        this.move = true;
    }

}
