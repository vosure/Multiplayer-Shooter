using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string name)
    {
        this.roomName = name;
    }

    public void CreateRoom()
    {
        if (roomName != "" && roomName != null)
        {
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, string.Empty, string.Empty, string.Empty, 0, 0, networkManager.OnMatchCreate);
        }
    }
}
