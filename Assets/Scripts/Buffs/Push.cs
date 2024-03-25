using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : Buff
{
    private Vector3 direction = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        magnitude = 4300;
        damage = 0;
    }
    public override void use()
    {
        //la siguiente funci�n es necesaria para que la fuerza resultatnte del lanzamiento sea igual independientemente de la direcci�n
        //del personaje.
        //Es facil editar la distancia cambiando las variables magnitude y yForce.
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //transform.position = player.transform.position + new Vector3(player.getPlayerDirection().x * 10, 2, player.getPlayerDirection().z * 10);
        transform.position = player.transform.position + (player.transform.forward) * 4;

        direction = player.playerDirection;
        gameObject.transform.forward = direction;
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

//        transform.position = player.transform.position + new Vector3(direction.x, 0, direction.z) * 10;
        StartCoroutine("StartToMove", throwForce);
    }
    IEnumerator StartToMove(Vector3 throwForce)
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.Impulse);
        StartCoroutine("StartToMove", throwForce);
    }
}
