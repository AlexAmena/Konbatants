using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : Buff
{
    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
        pushForceHorizontal = 20;
        pushForceVertical = 0;
        magnitude = 2500;
    }
    public override void use()
    {
        //la siguiente función es necesaria para que la fuerza resultatnte del lanzamiento sea igual independientemente de la dirección
        //del personaje.
        //Es facil editar la distancia cambiando las variables magnitude y yForce.
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(player.transform.position.x + player.getPlayerDirection().x * 3, player.transform.position.y + 2, player.transform.position.z + player.getPlayerDirection().z * 3);

        Vector3 direction = player.playerDirection;
        float xForce = 0;
        float yForce = 0;
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
}
