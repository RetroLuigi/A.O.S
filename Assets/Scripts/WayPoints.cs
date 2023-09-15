using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    public Vector3 nextPos;
    public List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            points.Add(child);
        }
        foreach (Transform child in transform)
        {
            positions.Add(child.position);
        }
    }

    // Update is called once per frame
    void Update() { }
}
