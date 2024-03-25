using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : Buff
{
    Vector3 initialLocalPosition;
    // Start is called before the first frame update
    void Start()
    {
        damage = 25;
        pushForceHorizontal = 5;
        pushForceVertical = 0;
    }

    public override void use()
    {
        StartCoroutine("Fall");
        transform.position = player.transform.position + new Vector3(0, 39, 0);
    }
    IEnumerator Fall()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Rigidbody>().velocity = new Vector3(0,-100,0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Split("r")[0] == "playe")
            this.PlayerDamageAndImpulse(collision.gameObject);
        if (collision.gameObject.tag == "ground")
            StartCoroutine("timeToDissapear");
    }
    IEnumerator timeToDissapear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
