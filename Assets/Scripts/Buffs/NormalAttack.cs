using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : Buff
{
    Vector3 initialLocalPosition;
    // Start is called before the first frame update
    void Start()
    {
        damage = 2;
        pushForceHorizontal = 200;
        pushForceVertical = 0;
        initialLocalPosition = new Vector3(-0.1566f, 2.14f, 2.11f);
    }
    public override void use()
    {
        transform.localPosition = initialLocalPosition;
        gameObject.SetActive(true);
        StartCoroutine("timeToDissapear");
    }
    IEnumerator timeToDissapear()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
}
