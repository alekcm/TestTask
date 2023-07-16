using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
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
    public bool shoot = false;

    void Awake()
    {
        inputControl = new PlayerInputControl();
        playerManager = FindObjectOfType<GameManager>();
        PlayerSpriteColor = playerManager.teamColors[(int)team].teamColor;
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    private Vector2 playerVelocity;

    void Update()
    {
        if (view.IsMine)
        {
            if (GameManager.AllowToMove)
            {
                playerVelocity = inputControl.Joystick.Move.ReadValue<Vector2>();

                if (inputControl.Joystick.Attack.triggered)
                {
                    Shoot();
                    shoot = true;
                }
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
        UpdateHealthBar();
        if (health <= 0) 
        {
            Player[] players = FindObjectsOfType<Player>();
            print(players.Length);
            if (players.Length == 2)
            {
                if (players[0] != this)
                    playerManager.Win(players[0]);
                else
                    playerManager.Win(players[1]);
            }
            Destroy(gameObject);
        }
    }
    public void UpdateHealthBar()
    {
        healthTransform.localScale = new Vector3(health / 100, healthTransform.localScale.y, healthTransform.localScale.z);
    }
    public void Shoot()
    {
        GameObject bullet = Instantiate(this.bullet, shootPlace.transform.position, playerSpriteTransform.rotation, null);
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
            //playerManager.Win(team);
        }
        Destroy(coin);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(shoot);
            stream.SendNext(health);
            stream.SendNext((int)team);
            if (shoot)
            {
                shoot = false;
            }
        }
        else
        {
            if (info.Sender == view.Owner)
            {
                if ((bool)stream.ReceiveNext())
                {
                    Shoot();
                }
                float newHealth = (float)stream.ReceiveNext();
                if (health != newHealth)
                {
                    health = newHealth;
                    UpdateHealthBar();
                }
                int newTeam = (int)stream.ReceiveNext();
                if ((int)team != newTeam)
                {
                    PlayerSpriteColor = playerManager.teamColors[newTeam].teamColor;
                    team = (Teams)newTeam;
                }
            }
        }
    }
}
