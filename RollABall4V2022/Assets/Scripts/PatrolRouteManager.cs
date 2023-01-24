using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRouteManager : MonoBehaviour
{
    public List<Transform> patrolRoutePoints;

    // Start is called before the first frame update
    void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        for (var index = 1; index < children.Length; index++)
        {
            Transform childTransform = children[index];
            patrolRoutePoints.Add(childTransform);
        }
    }

    private void OnDrawGizmos()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        
        for (int i = 1; i < children.Length; i++)
        {
            if(i<children.Length-1)
                Debug.DrawLine(children[i].position, children[i+1].position, Color.red);
            else
                Debug.DrawLine(children[i].position, children[1].position, Color.red); 
        }
    }
}
