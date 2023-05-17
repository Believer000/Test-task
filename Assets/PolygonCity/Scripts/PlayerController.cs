using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WaypointData[] waypoints;
    public Animation animationComponent;
    public float walkSpeed = 3.0f;
    public bool isClamp;

    private float _minDistanceToSwitchWaypoint;
    private int _currentWaypointIndex;

    private void Awake()
    {
        if(waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints isn't assigned!");
            Destroy(this);
            return;
        }

        animationComponent.clip = waypoints[0].animationClip;
        animationComponent.Play();
    }

    private void Update()
    {
        var currentWaypoint = waypoints[_currentWaypointIndex].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, walkSpeed * Time.deltaTime);
        transform.LookAt(currentWaypoint, transform.up);

        if(Vector3.Distance(transform.position, currentWaypoint) > _minDistanceToSwitchWaypoint)
        {
            return;
        }

        _currentWaypointIndex++;

        if(_currentWaypointIndex >= waypoints.Length)
        {
            if(isClamp)
            {
                _currentWaypointIndex = 0;
            }
            else
            {
                animationComponent.Stop();
                Destroy(this);
                return;
            }
        }

        animationComponent.clip = waypoints[_currentWaypointIndex].animationClip;
        animationComponent.Play();
    }
}

[Serializable]
public class WaypointData
{
    public Transform transform;
    public AnimationClip animationClip;
}