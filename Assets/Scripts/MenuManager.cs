using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static bool first = true;
    [SerializeField]
    private GameObject startMenu;

    [SerializeField]
    private GameObject finalMenu;
        

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("amountPlayers", 2);
        if (first){
            startMenu.SetActive(true);
            finalMenu.SetActive(false);
            first = false;
        }else{
            finalMenu.SetActive(true);
            startMenu.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
