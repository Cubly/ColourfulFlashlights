using HarmonyLib;
using System;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ColourfulFlashlights
{
    [HarmonyPatch]
    public class NetworkObjectManager
    {
        public static GameObject networkHandlerHost;
        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), "Start")]
        public static void Init()
        {
            if (networkPrefab != null)
                return;

            networkPrefab = (GameObject)Assets.MainAssetBundle.LoadAsset("CFNetwork");
            networkPrefab.AddComponent<NetworkHandler>();

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), "Awake")]
        static void SpawnNetworkHandler()
        {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
            {
                networkHandlerHost = Object.Instantiate(networkPrefab);
                networkHandlerHost.GetComponent<NetworkObject>().Spawn(true);
            }
        }

        static GameObject networkPrefab;
    }
}
