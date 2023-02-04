using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racine : MonoBehaviour
{

    EdgeCollider2D collider;
      
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createCollider(List<Vector2> points)
    {
        collider = GetComponent<EdgeCollider2D>();
        collider.points = points.ToArray();
    }

  


}
