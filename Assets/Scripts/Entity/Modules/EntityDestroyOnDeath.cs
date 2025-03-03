using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityDestroyOnDeath : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Entity>().OnDeath += () => Destroy(gameObject);
    }
}
