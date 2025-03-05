using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigationAI : FollowAI
{
    private NavMeshAgent agent;
    [SerializeField] private float minDistanceMargin = 0.25f;

    protected override Vector2 GetMovementDirection() => agent.velocity;

    protected override void Setup()
    {
        base.Setup();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.acceleration = aiAcceleration;
        agent.speed = aiSpeed;
        agent.stoppingDistance = Mathf.Max(targetReachDistance - minDistanceMargin, 0);

        OnRoamingPositionChanged += SetDestination;
        OnOverrideBehavior += (x) => SetDestination(transform.position);
    }

    //There is no logic needed | it is handled by OnRoamingPositionChanged event
    protected override void HandleFreeRoam() { }

    protected override void HandleChase()
    {
        SetDestination(playerTransform.position);
    }

    private void SetDestination(Vector3 position) 
    {
        if (agent == null || !agent.enabled)
            return;

        agent.SetDestination(position); 
    }
}
