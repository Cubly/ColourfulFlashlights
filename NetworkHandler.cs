using Unity.Netcode;
using static ColourfulFlashlights.Helpers;

namespace ColourfulFlashlights
{
    public class NetworkHandler : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public static NetworkHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateColourData(int playerId, string colour, bool justSpawned = false)
        {
            if (IsOwner)
            {
                UpdateColourDataClientRpc(playerId, colour);
            }
            else
            {
                UpdateColourDataServerRpc(playerId, colour, justSpawned);
            }
        }

        [ClientRpc]
        public void UpdateColourDataClientRpc(int playerId, string colour)
        {
            Plugin.playerData[playerId] = colour;
            Plugin.mls.LogInfo($"CLIENTRPC - UPDATING {playerId} COLOUR TO {colour}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateColourDataServerRpc(int playerId, string colour, bool justSpawned = true)
        {
            UpdateColourDataClientRpc(playerId, colour);
            Plugin.mls.LogInfo($"SERVERRPC - UPDATING {playerId} COLOUR TO {colour}");
            if (justSpawned)
            {
                ClientRpcParams clientRpcParams = new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { (ulong)playerId }
                    }
                };
                string serializedData = Helpers.SerializeDict(Plugin.playerData);
                GetColourDataClientRpc(playerId, serializedData, clientRpcParams);
                Plugin.mls.LogInfo($"SERVERRPC - GETTING PLAYERDATA FOR {playerId}");
            }
        }

        [ClientRpc]
        public void GetColourDataClientRpc(int playerId, string serializedData, ClientRpcParams clientRpcParams = default)
        {
            Plugin.playerData = Helpers.DeserializeDict(serializedData);
            if (!Plugin.playerData.ContainsKey(Plugin.currentPlayerId))
            {
                Plugin.playerData[Plugin.currentPlayerId] = ColorToHexString(Plugin.activeColour);
            }
            Plugin.mls.LogInfo($"CLIENTRPC - SENT PLAYERDATA TO {playerId}");
        }
    }
}
