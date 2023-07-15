using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public List<Color> playerColors;
    public List<Team> teamColors;
    public GameObject playerPrefab;
    public static bool AllowToMove = false;
    public enum Teams
    {
        Team1, Team2
    }
    public int currentTeam = 0;
    public void Start()
    {
        StartGame();
    }

    public void Update()
    {
        if (currentTeam == 0)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                StartGame();
        }
    }
    void StartGame()
    {
        currentTeam = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        if (currentTeam == 1)
            CoinSpawner.SpawnCoins();
        if (currentTeam > 0)
            AllowToMove = true;
        SpawnPlayer();
    }

    public static void Win(Teams team)
    {

    }
    public void SpawnPlayer()
    {
        Player player = PhotonNetwork.Instantiate(playerPrefab.name, teamColors[currentTeam].spawnPlace.position, Quaternion.identity).GetComponent<Player>();
        player.PlayerSpriteColor = teamColors[currentTeam].teamColor;
        player.team = teamColors[currentTeam].team;
        currentTeam++;
    }
}
