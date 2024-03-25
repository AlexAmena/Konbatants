using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatController : MonoBehaviour
{
    // Start is called before the first frame update
    private Text buffObj;
    private GameObject barObj;
    public PlayerController player;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "vitbar")
                barObj = transform.GetChild(i).gameObject;
            if (transform.GetChild(i).name == "buff")
                buffObj = transform.GetChild(i).gameObject.GetComponent<Text>();
        }
    }
    public void setPlayer(PlayerController player)
    {
        this.player = player;
        
        transform.Translate(0,-50*(player.playerID-1),0);
        
       
    }
    // Update is called once per frame
    void Update()
    {
        barObj.GetComponent<Image>().color = player.playerColor;
        if (player == null){//PLAYER IS DEAD
             buffObj.text = "Dead";
            barObj.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20);
        }
        else {
            buffObj.text =player.getBuffName();
            barObj.GetComponent<RectTransform>().sizeDelta = new Vector2(player.getActualVit(), 20);
        }
    }
}
