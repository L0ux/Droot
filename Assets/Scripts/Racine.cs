using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racine : MonoBehaviour
{

    EdgeCollider2D edgeCollider;
      
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
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.points = points.ToArray();
    }

  


}
