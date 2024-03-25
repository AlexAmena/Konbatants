using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Buff
{
    private GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
        pushForceHorizontal = 30;
        pushForceVertical = 30;
        magnitude = 2000;

        explosion = FindObjectOfType<Explosion>().gameObject;
    }
    public override void use()
    {
        //la siguiente funci�n es necesaria para que la fuerza resultatnte del lanzamiento sea igual independientemente de la direcci�n
        //del personaje.
        //Es facil editar la distancia cambiando las variables magnitude y yForce.
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(player.transform.position.x + player.getPlayerDirection().x * 3, player.transform.position.y + 3, player.transform.position.z + player.getPlayerDirection().z * 3);

        Vector3 direction = player.playerDirection;
        float xForce = 0;
        float yForce = 1500;
        float zForce = 0;
        
        float total = Mathf.Abs(direction.x) + Mathf.Abs(direction.z);
        float xP = Mathf.Abs(direction.x) / total;
        float zP = Mathf.Abs(direction.z) / total;
        xForce = Mathf.Sqrt((Mathf.Pow(magnitude, 2) - Mathf.Pow(yForce, 2)) * xP);
        if (direction.x < 0)
            xForce = -xForce;
        zForce = Mathf.Sqrt((Mathf.Pow(magnitude, 2) - Mathf.Pow(yForce, 2)) * zP);
        if (direction.z < 0)
            zForce = -zForce;

        Vector3 throwForce = new Vector3(xForce, yForce, zForce);
        Debug.Log(throwForce);
        Debug.Log(throwForce.magnitude);
        GetComponent<Rigidbody>().AddForce(throwForce);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag.Split('r')[0] == "playe")
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GameObject exp = Instantiate(explosion);
            exp.transform.position = transform.position;
            exp.GetComponent<Explosion>().initiate();
            Destroy(gameObject);
        }
    }
}
