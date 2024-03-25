using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected int damage;
    protected int pushForceHorizontal;
    protected int pushForceVertical;
    //some buffs can be use more tha once
    protected int uses;

    //when the buff is throwable
    protected int magnitude;
    public PlayerController player;

    public abstract void use();

    public int getDamage() { return damage; }
    public int getPushForceHorizontal() { return pushForceHorizontal; }
    public int getPushForceVertical() { return pushForceVertical; }
    public int getUses() { return uses; }

    public void throwBuffInDirection()
    {
        ;
    }
    public PlayerController getPlayer()
    {
        return player;
    }
    public void setPlayer(PlayerController player)
    {
        this.player = player;
    }
    public Vector3 calculateInitialPos(){
        return player.transform.position + (player.transform.forward) * 4;
    }
    public string getName()
    {
        return this.GetType().Name;
    }

    public void PlayerDamageAndImpulse(GameObject g)
    {
        PlayerController p = g.GetComponent<PlayerController>();
        p.isStunned = true;
        p.ignoreRaycast = true;

        Vector3 distance = g.transform.position - transform.position;

        float pX = Mathf.Abs(distance.x) * 100 / (Mathf.Abs(distance.x) + Mathf.Abs(distance.z));
        float pZ = Mathf.Abs(distance.z) * 100 / (Mathf.Abs(distance.x) + Mathf.Abs(distance.z));

        float xForce = Mathf.Sqrt((Mathf.Pow(pushForceHorizontal, 2) - Mathf.Pow(0, 2)) * pX);
        if (distance.x < 0)
            xForce = -xForce;
        float zForce = Mathf.Sqrt((Mathf.Pow(pushForceHorizontal, 2) - Mathf.Pow(0, 2)) * pZ);
        if (distance.z < 0)
            zForce = -zForce;
        float yForce = pushForceVertical;

        g.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, zForce), ForceMode.Impulse);
        p.StartCoroutine("SetIgnoreRaycastFalse");

        p.actualVit -= damage;
        if (p.actualVit <= 0)
            p.Die();
        //audio mario ostiaka hildakuen
        p.ActiveVitbar();
    }
}
