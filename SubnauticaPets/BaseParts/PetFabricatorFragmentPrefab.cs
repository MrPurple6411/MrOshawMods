﻿using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using System.Collections.Generic;
using Nautilus.Assets.Gadgets;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetFabricatorFragmentPrefab
    {
        // Public PrefabInfo, for anything that wants to lookup TechType etc.
        public static PrefabInfo PrefabInfo;

        // Class Id TechType
        private static readonly string PrefabClassId = "PetFabricatorFragment";

        // Defines spawn locations for instances
#if SUBNAUTICA
        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-172.27f,-43.07f, -234.29f), new Vector3(0f, 0f, 0f))
        };

        /*
            new Vector3(-172.27f,-43.07f, -234.29f) /*,
            new Vector3(-385.8968f,-118.473f, 623.9641f),
            new Vector3(-1603.463f,-342.9196f, 79.65623f),
            new Vector3(-773.6248f,-212.8109f, -729.6456f),
            new Vector3(-31.82343f,-24.19011f, -417.7448f),
            new Vector3(12.00361f,-25.82509f, -243.0502f),
            new Vector3(76.30077f,-24.12408f, -88.81075f),
            new Vector3(82.54358f,-37.00394f, 117.0964f),
            new Vector3(376.3995f,-25.73f, -209.09f) 
        */
#endif
#if SUBNAUTICAZERO
        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-171.25f,-41.34f, -234.25f), new Vector3(0f, 0f, 0f))
        };
#endif

        /// <summary>
        /// Initialise the Pet Fabricator fragment prefab
        /// </summary>
        public static void InitPrefab()
        {
            Log.LogDebug("PetFabricatorFragmentPrefab: InitPrefab...");
            PrefabInfo fabricatorFragmentInfo = PrefabInfo.WithTechType(PrefabClassId, null, null);
            CustomPrefab fabricatorFragmentPrefab = new CustomPrefab(fabricatorFragmentInfo);

            PrefabTemplate cloneTemplate = new CloneTemplate(fabricatorFragmentInfo, TechType.BioreactorFragment)
            {
                ModifyPrefab = prefab =>
                {
                    // Add component
                    Log.LogDebug("PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component...");
                    prefab.AddComponent<PetFabricatorFragment>();
                    Log.LogDebug(
                        "PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component... Done.");
                }
            };
            fabricatorFragmentPrefab.SetGameObject(cloneTemplate);
            ModUtils.SetupCoordinatedSpawn(fabricatorFragmentPrefab, SpawnLocations);
            Log.LogDebug($"PetFabricatorFragmentPrefab: Registering {PrefabClassId}...");
            fabricatorFragmentPrefab.Register();
            Log.LogDebug($"PetFabricatorFragmentPrefab: Init Prefab for {PrefabClassId}. Done.");
            PrefabInfo = fabricatorFragmentPrefab.Info;
        }
    }
}
