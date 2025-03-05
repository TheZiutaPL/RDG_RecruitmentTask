using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(FollowAI))]
public class OverrideAIBehaviorOnDeath : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Entity>().OnDeath += () => GetComponent<FollowAI>().ToggleAI(false);
    }
}
