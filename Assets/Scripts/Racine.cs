using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racine : MonoBehaviour
{

    PolygonCollider2D polygon;
    List<Vector2> liste;
      
    // Start is called before the first frame update
    void Start()
    {
        liste = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createCollider(List<Vector2> points)
    {
        polygon = GetComponent<PolygonCollider2D>();
        liste = points;
        polygon.SetPath(0, points.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(liste != null)
        {
            liste.ForEach(p => Gizmos.DrawSphere(p, 0.05f));
        }
    }


}
