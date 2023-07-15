using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using Photon.Pun;

public class Player : MonoBehaviour
{
    //public List<Color> Color;
    // Start is called before the first frame update
    [SerializeField]
    private GameManager playerManager;
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private GameObject coinsMetter;
    [SerializeField]
    private Transform healthTransform;
    public float health = 100;
    [SerializeField]
    private float moveSpeed = 2.0f;
    public GameObject bullet;
    public Transform shootPlace;
    private PlayerInputControl inputControl;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Transform playerSpriteTransform;
    public Teams team = Teams.Team1;
    public float bulletForce = 20f;
    public List<GameObject> coins;
    public int coinsCollected = 0;
    public PhotonView view;

    void Awake()
    {
        inputControl = new PlayerInputControl();
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }


    private void Start()
    {
        
    }

    private Vector2 playerVelocity;

    void Update()
    {
        if (GameManager.AllowToMove)
        {
            playerVelocity = inputControl.Joystick.Move.ReadValue<Vector2>();

            if (inputControl.Joystick.Attack.triggered)
            {
                Shoot();
            }
        }
    }
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (playerVelocity != Vector2.zero)
            {
                rb.MovePosition(rb.position + playerVelocity * moveSpeed * Time.fixedDeltaTime);
                float angle = Mathf.Atan2(playerVelocity.y, playerVelocity.x) * Mathf.Rad2Deg - 180f;
                playerSpriteTransform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    public Color PlayerSpriteColor
    {
        get { return playerSprite.color; }
        set { playerSprite.color = value; }
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (collision.gameObject.GetComponent<Bullet>().team != team)
            {
                TakeDamage(10);
                Destroy(collision.gameObject);
            }
        }
        if (collision.tag == "Coin")
        {
            TakeCoin(collision.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthTransform.localScale = new Vector3(health / 100, healthTransform.localScale.y, healthTransform.localScale.z);
    }

    public void Shoot()
    {
        //print("shooted");
        GameObject bullet = Instantiate(this.bullet, shootPlace.transform.position, playerSpriteTransform.rotation,null);
        bullet.GetComponent<Bullet>().team = team;
        bullet.GetComponent<Rigidbody2D>().AddForce(shootPlace.up * bulletForce, ForceMode2D.Impulse);
    }
    public void TakeCoin(GameObject coin)
    {
        coinsCollected++;
        if (coinsCollected < coins.Count)
        {
            coins[coinsCollected].SetActive(true);
        }
        else
        {
            GameManager.Win(team);
        }
        Destroy(coin);
    }
    
}
