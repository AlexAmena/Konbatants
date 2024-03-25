using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Buff
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void use()
    {
        gameObject.SetActive(true);
        player.isStunned = true;
        transform.position = new Vector3(player.transform.position.x + player.transform.forward.x * 3, 
                                        player.transform.position.y + 3,
                                        player.transform.position.z + player.transform.forward.z * 3);
        transform.forward = player.transform.forward;
        StartCoroutine("SetStunnedFalse");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Buff>() != null)
        {
            DestoryObjects(collision.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Buff>() != null)
        {
            DestoryObjects(other.gameObject);
        }
    }
    public void DestoryObjects(GameObject g){
            Destroy(g);
            Destroy(gameObject);
            player.isStunned=false;
    }
    IEnumerator SetStunnedFalse()
    {
        yield return new WaitForSeconds(1.5f);
        player.isStunned = false;
        Destroy(this.gameObject);
    }
}
