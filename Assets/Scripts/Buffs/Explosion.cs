using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float timer;
    private int damage;
    private List<GameObject> collisionedPlayers;
   
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.25f;
        damage = 3;
        collisionedPlayers= new List<GameObject>();
    }    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Split('r')[0] == "playe")
        {
            bool finded = false;
            if (collisionedPlayers.Count > 0)
                for (int i = 0; i < collisionedPlayers.Count; i++)
                    if (collisionedPlayers[i].tag == other.gameObject.tag)
                        finded = true;

            if (!finded) //withouth this player can be hitted two times by the same explosion gameobject
            {
                collisionedPlayers.Add(other.gameObject);

                PlayerController p = other.gameObject.GetComponent<PlayerController>();
                p.isStunned = true;
                p.ignoreRaycast = true;

                float impulseForce = 1f / timer;
                Debug.Log(impulseForce);
                Vector3 distance = other.gameObject.transform.position - transform.position;

                float pX = Mathf.Abs(distance.x) * 100 / (Mathf.Abs(distance.x) + Mathf.Abs(distance.z));
                float pZ = Mathf.Abs(distance.z) * 100 / (Mathf.Abs(distance.x) + Mathf.Abs(distance.z));

                float xForce = Mathf.Sqrt((Mathf.Pow(impulseForce, 2) - Mathf.Pow(0, 2)) * pX);
                if (distance.x < 0)
                    xForce = -xForce;
                float zForce = Mathf.Sqrt((Mathf.Pow(impulseForce, 2) - Mathf.Pow(0, 2)) * pZ);
                if (distance.z < 0)
                    zForce = -zForce;
                float yForce = 20;

                other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(xForce, yForce, zForce), ForceMode.Impulse);
                p.StartCoroutine("SetIgnoreRaycastFalse");

                Debug.Log("total damage:" + (int)(damage / timer));
                p.actualVit -= (int)(damage / timer);
                if (p.actualVit <= 0)
                    p.Die();
                //audio mario ostiaka hildakuen
                p.ActiveVitbar();
            }
        }
    }
    public void initiate()
    {
        StartCoroutine("Explode");
    }
    IEnumerator Explode()
    {
        timer += 0.05f;
        yield return new WaitForSeconds(0.05f);
        transform.localScale += new Vector3(2.4f, 3.2f, 2.4f);
        
        if(timer<0.75f)
            StartCoroutine("Explode");
        else
            Destroy(gameObject);
    }
}
