using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public List<Color> playerColors;
    public List<Team> teamColors;
    public GameObject playerPrefab;
    public static bool AllowToMove = false;
    public TMPro.TextMeshProUGUI winnerNameText;
    public GameObject winnerPopup;
    public enum Teams
    {
        Team1, Team2
    }
    public void Start()
    {
        StartGame();
        SpawnPlayer();
    }
    void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            CoinSpawner.SpawnCoins();
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            AllowToMove = true;
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            StartGame();
    }
    public void Win(Player player)
    {
        winnerPopup.SetActive(true);
        winnerNameText.text = $"Победил игрок {(int)player.team}. \nСобрано монет: {player.coinsCollected}";
        AllowToMove = false;
    }
    public void SpawnPlayer()
    {
        int currentTeam = PhotonNetwork.CurrentRoom.PlayerCount-1;
        Player player = PhotonNetwork.Instantiate(playerPrefab.name, teamColors[currentTeam].spawnPlace.position, Quaternion.identity).GetComponent<Player>();
        player.team = teamColors[currentTeam].team;
        player.PlayerSpriteColor = teamColors[currentTeam].teamColor;
    }
}
