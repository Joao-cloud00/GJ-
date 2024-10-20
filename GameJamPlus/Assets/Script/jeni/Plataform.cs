using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB; 
    private Transform currentPoint; 
    public float speed = 2f; 

    void Start()
    {
        currentPoint = pointB.transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
        {
            currentPoint = currentPoint == pointA.transform ? pointB.transform : pointA.transform;
        }
    }
}
