using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public List<Transform> GetWaypoints()
    {
        var waypoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        return waypoints;
    }
}
