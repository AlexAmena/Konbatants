using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GameController : MonoBehaviour
{
    public GameObject ground;

    private ArrayList players;
   
    private int playerCount;

    [SerializeField]
    private GameObject buff;

    private const float phaseEndTime = 30f;
    private const int phaseAmount = 7;

    private float timer = 0f;
    private float phaseTimer = 0f;
    private int gamePhase = 1;

    private float timerEnd = (phaseAmount-1)*phaseEndTime;

    public Text timerText;
    public Text suddenText;
    public Text deathText;

    private int timeToNextBuff;

    private GameObject ost;

    private GameObject leftLimiter;
    private GameObject rightLimiter;
    private GameObject frontLimiter;
    private GameObject backLimiter;

    private int targetFrameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("WaitToSpawn");

        // Get the amount of players from PlayerPrefs
        int amountPlayers = PlayerPrefs.GetInt("amountPlayers", 4);

        List<GameObject> players = new List<GameObject>();
        foreach (PlayerController p in GameObject.FindObjectsOfType<PlayerController>())
            players.Add(p.gameObject);

        // Disable player objects that exceed the desired amount
        for (int i = amountPlayers; i < players.Count; i++)
            players[i].SetActive(false);

        playerCount = amountPlayers;
        Debug.Log("playerCount: " + playerCount);

        timeToNextBuff = 5;
        ost = GameObject.FindGameObjectWithTag("ost");
        nextOst();

        leftLimiter = GameObject.FindGameObjectWithTag("limiters").transform.GetChild(0).gameObject;
        rightLimiter = GameObject.FindGameObjectWithTag("limiters").transform.GetChild(1).gameObject;
        backLimiter = GameObject.FindGameObjectWithTag("limiters").transform.GetChild(2).gameObject;
        frontLimiter = GameObject.FindGameObjectWithTag("limiters").transform.GetChild(3).gameObject;

        Application.targetFrameRate = targetFrameRate;
    }


    // Update is called once per frame
    void Update()
    {
        if(Application.targetFrameRate!=targetFrameRate)
            Application.targetFrameRate=targetFrameRate;
        timer += Time.deltaTime;
        setUItexttimer();
        phaseTimer += Time.deltaTime;
        if (phaseTimer >= phaseEndTime)
        {
            gamePhase++;
            phaseTimer = phaseTimer - phaseEndTime;
            nextOst();
        }
        if (gamePhase == phaseAmount)
        {
            gamePhase++;
            StartCoroutine("DwarfPlayGround");
        }
    }

    /**
     * BuffManager (another class????)
     */
    void spawnBuff(GameObject buff)
    {
        GameObject instance = Instantiate(buff);
        instance.SetActive(true);
        Vector3 position = calculatePosition();

        instance.transform.position = position;
    }

    Vector3 calculatePosition()
    {
        System.Random rnd = new System.Random();
        int x = rnd.Next((int)leftLimiter.transform.position.x,(int)rightLimiter.transform.position.x);
        int z = rnd.Next((int)frontLimiter.transform.position.z,(int)backLimiter.transform.position.z);
        Vector3 position = new Vector3(x, 30f, z);

        return position;
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(timeToNextBuff);
        spawnBuff(buff);
        StartCoroutine("WaitToSpawn");
    }

    private void nextOst()
    {
        switch (gamePhase)
        {
            case 1:
                activateAudioSource(0);
                activateAudioSource(1);
                break;
            case 2:
                
                activateAudioSource(2);
                activateAudioSource(3);
                break;
            case 3:
                activateAudioSource(4);
                activateAudioSource(5);
                break;
            case 4:
                activateAudioSource(6);
                activateAudioSource(7);
                break;
            case 5:
                activateAudioSource(8);
                activateAudioSource(9);
                break;
            case 6:
                activateAudioSource(10);
                activateAudioSource(11);
                break;
            case 7: 
                activateAudioSource(12);
                activateAudioSource(13);
                suddenDeath();
                break;
        }
    }
    private void activateAudioSource(int i)
    {
        ost.transform.GetChild(i).GetComponent<AudioSource>().mute = false;
    }

    private void setUItexttimer(){
        if (timer<=timerEnd){
            int minutes = Mathf.FloorToInt((timerEnd-timer) / 60F);
            int seconds = Mathf.FloorToInt((timerEnd-timer) % 60F);
            timerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");    
        }
    }
    private void suddenDeath()
    {
        timerText.text="";
        deathText.GetComponent<Animator>().SetBool("active", true);
        suddenText.GetComponent<Animator>().SetBool("active", true);
    }
    IEnumerator DwarfPlayGround()
    {
        yield return new WaitForSeconds(0.05f);
        ground.transform.localScale -= new Vector3(0.001f, 0, 0.001f); 
        StartCoroutine("DwarfPlayGround");
    }
}
