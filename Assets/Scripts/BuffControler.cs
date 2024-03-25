using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Linq;
using System.Threading;


public class BuffControler : MonoBehaviour
{
    public List<GameObject> buffList = new List<GameObject>();

    //private string buffToActive;
    private GameObject buffToActive;

    public GameObject[] buffProb = new GameObject[100];

    // Start is called before the first frame update
    void Start()
    {
        GameObject allBuffs = GameObject.FindGameObjectWithTag("buffObjects");

        for(int i= 0; i<allBuffs.transform.childCount; i++)
            buffList.Add(allBuffs.transform.GetChild(i).gameObject);

        randomBuff();
        
        for (int i = 0; i< buffProb.Length; i+=10)
        {
            buffProb[i] = buffList[0];
            buffProb[i+1] = buffList[1];
            buffProb[i+2] = buffList[2];
            buffProb[i+3] = buffList[3];
            buffProb[i+4] = buffList[4];
            buffProb[i + 5] = buffList[5];
            buffProb[i + 6] = buffList[0];
            buffProb[i + 7] = buffList[1];
            buffProb[i + 8] = buffList[2];
            buffProb[i + 9] = buffList[3];
        }
    }
    private void randomBuff()
    {
        System.Random rnd= new System.Random();
        int number = rnd.Next(0, 99);
        buffToActive = buffProb[number];
    }

    private void OnCollisionEnter(Collision collision)
    {
        //this will be executed if the collider is a player
        if (collision.gameObject.tag.Split('r')[0] == "playe")
        {
            PlayerController p = (PlayerController)collision.gameObject.GetComponent<PlayerController>();
            if (p.buff == null) {
                p.buff = Instantiate(buffToActive).GetComponent<Buff>();
                Destroy(gameObject);
            }
        }
    }
}
