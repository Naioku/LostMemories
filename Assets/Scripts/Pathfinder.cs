using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private PathController pathPrefab;

    private GhostController _ghostController;
    private List<Transform> _waypoints;
    private int _waypointIndex;
    private Rigidbody2D _rigidbody2D;
    private bool _enabledPathfinding;

    private void Awake()
    {
        _ghostController = GetComponent<GhostController>();
    }

    private void Start()
    {
        _waypoints = pathPrefab.GetWaypoints();
        transform.position = _waypoints[_waypointIndex].position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_enabledPathfinding) return;
        
        if (_waypointIndex < _waypoints.Count)
        {
            Vector2 targetPosition = _waypoints[_waypointIndex].position;
            
            if (Vector2.Distance(transform.position, targetPosition) < _ghostController.GetPositionRadius())
            {
                _waypointIndex++;
                return;
            }

            Utils.MoveTowardsPosition(
                targetPosition,
                _ghostController.GetMovementSpeed(),
                transform,
                _rigidbody2D);
        }
        else
        {
            _waypointIndex = 0;
        }
    }

    public void EnablePathfinder()
    {
        _enabledPathfinding = true;
    }

    public void DisablePathfinder()
    {
        _enabledPathfinding = false;
    }
}
