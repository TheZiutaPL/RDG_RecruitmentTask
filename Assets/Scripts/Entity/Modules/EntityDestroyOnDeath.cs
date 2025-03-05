using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(Entity))]
public class EntityDestroyOnDeath : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 2;

    private void Start()
    {
        GetComponent<Entity>().OnDeath += () => HandleDestroy();
    }

    private async void HandleDestroy()
    {
        await Task.Delay((int)(destroyAfterSeconds * 1000));
        Destroy(gameObject);
    }
}
