using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : Buff
{
    public PlayerController p;
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
    }
    public override void use()
    {
        p = player;
        int initialMovementSpeed = player.getMovementSpeed();
        player.setMovementSpeed(initialMovementSpeed+damage);
        StartCoroutine("ReturnVelocityToBase", initialMovementSpeed);
    }
    IEnumerator ReturnVelocityToBase(int speed)
    {
        yield return new WaitForSeconds(5f);
        p.setMovementSpeed(speed);
        Destroy(gameObject);
    }
}
