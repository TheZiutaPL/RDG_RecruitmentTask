using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void SetProjectile(Vector3 direction, float speed, float lifetime)
    {
        this.direction = direction;
        this.speed = speed;

        //Makes projectile face its direction
        transform.rotation = Quaternion.LookRotation(direction);


        //Destroys projectile after some time
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += speed * Time.fixedDeltaTime * direction;
    }
}
