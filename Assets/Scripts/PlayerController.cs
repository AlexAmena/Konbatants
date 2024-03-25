using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;

    //player stats and data
    public int playerID;
    private int vit;
    public int actualVit;
    public GameObject vitbar;


    //color
    public Color playerColor;

    //movement an direction
    public Vector3 playerDirection;
    private Vector3 movement;
    private int movementSpeed;
    
    //rigidbody
    private Rigidbody rb;
    private CustomGravity gS;

    //jump attributes
    public bool canJump;
    public int jumpForce;
    private RaycastHit floorHit;

    //Buffs
    public Buff buff;

    //normalAttack
    private GameObject attackObject;
    private bool canAttack;
    //private float attackTimer; ???

    public bool isStunned = false;
    //ignore raycast is used to say to ignore raycast for some time
    public bool ignoreRaycast = false;

    //Amount of players

    void Start()
    {
        playerID = int.Parse(gameObject.tag.Split('r')[1]);
        vit = 100;
        actualVit = 100;
        
        playerDirection = new Vector3(0.2f, 0, 0);
        movementSpeed = 10;

        rb = GetComponent<Rigidbody>();
        gS = FindObjectOfType<CustomGravity>();

        canJump = true;
        jumpForce = 5;
        
        //attack = 
        canAttack = true;
        
        //Find normalAttack gameObject in player children
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "attack")
                attackObject = transform.GetChild(i).gameObject;
            if (transform.GetChild(i).name == "Vitbar")
                vitbar = transform.GetChild(i).gameObject;
        }
        attackObject.GetComponent<Buff>().setPlayer(this);
        attackObject.SetActive(false);

        if(playerID!=1) InstantiateHUDComponent(); //player1 HUD is created at match start
        anim = GetComponent<Animator>();
        //StartCoroutine("ForceCanJumpTrue");
    }

    // Update is called once per frame
    void Update()
    {
        vitbar.transform.rotation = Quaternion.identity;
        if (!isStunned)
        {
            setMovement();
            setJump();
            attackButton();
            buffButton();
        }
        showStats();
    }
    private void FixedUpdate()
    {
        if (!isStunned)
        {
            movePlayer(movement);
            //jump();
        }
        isJumping();
    }
    public Vector3 getPlayerDirection()
    {
        return playerDirection;
    }
    public Buff getBuff()
    {
        return buff;
    }

    private void setMovement()
    {
        float h = Input.GetAxis("Player"+ playerID +"H");
        float v = Input.GetAxis("Player" + playerID + "V");

        //Set the direction that player is looking and rotate
        if (h >= 0.01f || h <= -0.01f || v >= 0.01f || v <= -0.01f)
        {
            playerDirection = new Vector3(h, 0, v);
            transform.forward = playerDirection;
        }
        walkingAndIdleAnimation();

        movement = new Vector3(h * movementSpeed * 15, rb.velocity.y, v * movementSpeed * 15);
    }
    private void walkingAndIdleAnimation()
    {
        if (Mathf.Abs(playerDirection.x) < 0.1f)
            anim.SetFloat("SpeedX", 0f);
        else
            anim.SetFloat("SpeedX", playerDirection.x);

        if (Mathf.Abs(playerDirection.z) < 0.1f)
            anim.SetFloat("SpeedY", 0f);
        else
            anim.SetFloat("SpeedY", playerDirection.z);
    }

    private void movePlayer(Vector3 movement)
    {
        //rb.velocity=movement;
        rb.AddForce(movement, ForceMode.Force);
        int maximumSpeed = 11;
        if (rb.velocity.x > maximumSpeed) rb.velocity = new Vector3(maximumSpeed, rb.velocity.y, rb.velocity.z);
        if (rb.velocity.z > maximumSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maximumSpeed);
        if(rb.velocity.x < -maximumSpeed) rb.velocity = new Vector3(-maximumSpeed, rb.velocity.y, rb.velocity.z);
        if(rb.velocity.z < -maximumSpeed) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maximumSpeed);

        float total = Mathf.Abs(movement.x) + Mathf.Abs(movement.z);
        float xP = Mathf.Abs(movement.x) / total;
        float zP = Mathf.Abs(movement.z) / total;
        float yForce = 0;
        float xForce = Mathf.Sqrt((Mathf.Pow(maximumSpeed, 2) - Mathf.Pow(yForce, 2)) * xP);
        if (xForce.Equals(float.NaN)) xForce = 0;
        if (transform.forward.x < 0) xForce = -xForce;
        float zForce = Mathf.Sqrt((Mathf.Pow(maximumSpeed, 2) - Mathf.Pow(yForce, 2)) * zP);
        if (zForce.Equals(float.NaN)) zForce = 0;
        if (transform.forward.z < 0) zForce = -zForce;

        rb.velocity = new Vector3(xForce, rb.velocity.y, zForce);
    }
    private void setJump()
    {
        if (Input.GetButtonDown("Jump" + playerID) && canJump)
        {
            canJump = false;
            jumpForce = 30;
            jump();
            Debug.Log("jump imput");
        }
    }
    private void jump()
    {
        //Debug.Log("JumpForce:" + this.jumpForce);
        if (this.jumpForce>0)
        {
            Debug.Log(jumpForce);
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            //this.jumpForce = 0;
            Debug.Log("Jumped");
        }
    }
    private void isJumping()
    {
        /*if (Physics.Raycast(transform.position, new Vector3(0, -0.1f, 0), out floorHit, 0.01f))
        {
            Debug.Log("Hitting floor" + playerID);
            Debug.DrawRay(transform.position, new Vector3(0, -0.1f, 0), Color.red, floorHit.distance);
            Debug.Log("setting canjump true" + playerID);
            if (!ignoreRaycast)
            {
                Debug.Log("can jump true" + playerID);
                canJump = true;
                isStunned = false;
            }
        }*/
    }
    //detect attack button
    private void attackButton()
    {
        if(Input.GetButton("Player" + playerID + "Hit") && canAttack)
        {
            attackObject.GetComponent<Buff>().use();
            canAttack = false;
            StartCoroutine("CanAttackTrue");
            //isStunned = true;
            //StartCoroutine("SetStunnedFalse");
        }
    }
    /**
     * If the player has a buff, then use it. 
     * Buff-s player attribute is seted in this moment?
     */
    private void buffButton()
    { 
        if (Input.GetButton("BuffUse" + playerID) && buff != null || Input.GetKey(KeyCode.C) && buff!= null)
        {
            buff.player = GetComponent<PlayerController>();
            buff.use();
            buff = null;
        }
    }

    private void showStats() 
    { 
        if(Input.GetButton("SeeStats" + playerID))
            ActiveVitbar();
    }

    //if player touches an object of the type Buff and receives damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "deathZone")
        {
            Die();
        }
        else
        {
            Buff b = other.gameObject.GetComponent<Buff>();
            if (b != null && ( b.player == null ||  gameObject.tag != b.player.tag))
            {
                isStunned = true;
                ignoreRaycast = true;
                ApplyImpulse(b);
                //Withouth ignore raycast isStunned is set false instantly and the impulse is stopped
                StartCoroutine("SetIgnoreRaycastFalse");
                TakeDamage(b);

                //hau pixket kutrie da
                if (b.GetType().Name == "NormalAttack")
                    other.gameObject.SetActive(false);
                else
                    Destroy(other.gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //this will be executed if the collider is a player
        if (collision.gameObject.tag == "ground")
        {
            canJump = true;
        }
    }
    public void ApplyImpulse(Buff b)
    {
        Vector3 attackVelocity = b.GetComponent<Rigidbody>().velocity;

        if (Vector3.Magnitude(attackVelocity) > 0) //if attack is something thrown 
            rb.AddForce(new Vector3(attackVelocity.x, b.getPushForceVertical(), attackVelocity.z), ForceMode.Impulse);

        else //normalAttack, swordAttack... attacks that don't have velocity
        {
            Vector3 attackDirection = transform.position - b.player.transform.position;
            rb.AddForce(attackDirection * 15, ForceMode.Impulse);
        }
    }
    private void TakeDamage(Buff b)
    {
        this.actualVit -= b.getDamage();
        if(actualVit <= 0)
            Die();
        //audio mario ostiaka hildakuen
        ActiveVitbar();
    }
    public void ActiveVitbar()
    {
        vitbar.transform.localScale = new Vector3(0.25f * actualVit / vit, 1, 0.05f);
        vitbar.SetActive(true);
        StartCoroutine("showVitBar");
    }
    IEnumerator showVitBar()
    {
        yield return new WaitForSeconds(4);
        vitbar.SetActive(false);
    }

    public void Die()
    {
        Destroy(gameObject);
        PlayerPrefs.SetInt("amountPlayers", PlayerPrefs.GetInt("amountPlayers") - 1);
        if(PlayerPrefs.GetInt("amountPlayers") < 2) //End
            SceneManager.LoadScene("Menu");
    }
    
    IEnumerator SetIgnoreRaycastFalse()
    {
        Debug.Log("sisi");
        yield return new WaitForSeconds(0.5f);
        ignoreRaycast = false;
        isStunned = false;
    }
    IEnumerator SetStunnedFalse()
    {
        yield return new WaitForSeconds(0.5f);
        isStunned = false;
    }

    public int getMovementSpeed()
    {
        return this.movementSpeed;
    }
    public void setMovementSpeed(int speed)
    {
        this.movementSpeed = speed;
    }
    public int getVit()
    {
        return this.vit;
    }
    public string getBuffName()
    {
        if(buff != null)
        return buff.getName();
        else
            return "No buff";
    }
    public int getActualVit()
    {
        return this.actualVit;
    }
    public void setActualVit(int vit)
    {
        this.actualVit = vit;
    }

    IEnumerator ForceCanJumpTrue()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (Physics.Raycast(transform.position, new Vector3(0, -0.1f, 0), out floorHit, 0.01f))
        {
            canJump = true;
            //StartCoroutine("ForceCanJumpTrue");
        }
    }
    IEnumerator CanAttackTrue()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        canAttack=true;
    }

    public void InstantiateHUDComponent()
    {
        PlayerStatController psc = Instantiate(GameObject.FindGameObjectWithTag("playerStats"),GameObject.FindObjectOfType<Canvas>().transform).GetComponent<PlayerStatController>();
        psc.setPlayer(this);
    }
}