using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityDestroyOnDeath : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 2;

    private void Start()
    {
        GetComponent<Entity>().OnDeath += () => StartCoroutine(HandleDestroy());
    }

    private IEnumerator HandleDestroy()
    {
        yield return new WaitForSeconds(destroyAfterSeconds);

        if(this != null)
            Destroy(gameObject);
    }
}
