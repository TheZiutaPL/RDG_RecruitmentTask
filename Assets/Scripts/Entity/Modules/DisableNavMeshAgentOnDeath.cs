using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(NavMeshAgent))]
public class DisableNavMeshAgentOnDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Entity>().OnDeath += () => GetComponent<NavMeshAgent>().enabled = false;
    }
}
