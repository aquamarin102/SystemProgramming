using Characters;
using Mirror;
using UnityEngine;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        //[SerializeField] private string _playerName;
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            //player.GetComponent<ShipController>().PlayerName = _playerName;
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}