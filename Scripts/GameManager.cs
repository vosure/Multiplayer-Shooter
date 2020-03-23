﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.Log("More than one GameManager in scene");
        }
        else
        {
            singleton = this;
        }
    }

    public void SetSceneCameraActice(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void DeRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string playerID in players.Keys)
    //    {
    //        GUILayout.Label(playerID + " - " + players[playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    #endregion
}
