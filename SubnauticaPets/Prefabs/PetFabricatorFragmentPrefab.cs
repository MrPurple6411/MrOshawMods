﻿using DaftAppleGames.SubnauticaPets.Mono.BaseParts;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Utility;

namespace DaftAppleGames.SubnauticaPets.Prefabs
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class PetFabricatorFragmentPrefab
    {
        // Public PrefabInfo, for anything that needs it
        public static PrefabInfo PrefabInfo;

        // Prefab Class Id
        private const string PrefabClassId = "PetFabricatorFragment";

        // Asset bundle references
        private const string PetFabricatorPopupImageTexture = "PetFabricatorDataBankPopupImageTexture";


#if SUBNAUTICA
        private static readonly SpawnLocation[] SpawnLocations =
        {
            new SpawnLocation(new Vector3(-172.27f,-43.07f, -234.29f), new Vector3(346.22f, 345.14f, 8.72f)),
            new SpawnLocation(new Vector3(-385.88f,-124.79f, 623.95f), new Vector3(8.71f, 0.62f, 2.86f)),
            new SpawnLocation(new Vector3(-1603.49f,-355.97f, 79.63f), new Vector3(5.06f, 2.32f, 354.08f)),
            new SpawnLocation(new Vector3(-773.64f,-224.83f, -729.66f), new Vector3(13.85f, 0.46f, 354.31f)),
            new SpawnLocation(new Vector3(-31.72f,-32.77f, -418.56f), new Vector3(348.20f, 355.20f, 347.04f)),
            new SpawnLocation(new Vector3(12.01f,-28.85f, -243.06f), new Vector3(11.23f, 0.61f, 1.94f)),
            new SpawnLocation(new Vector3(76.28f,-30.01f, -88.79f), new Vector3(290.52f, 172.81f, 186.37f)),
            new SpawnLocation(new Vector3(82.50f,-40.76f, 117.07f), new Vector3(3.60f, 357.73f, 330.99f)),
            new SpawnLocation(new Vector3(376.36f,-26.60f, -209.09f), new Vector3(292.15f, 131.21f, 226.02f)),

            /*
               warp -172.27 -43.07 -234.29
               warp -385.8968 -118.473 623.9641
               warp -1603.463 -342.9196 79.65623
               warp -773.6248 -212.8109 -729.6456
               warp -31.82343 -24.19011 -417.7448
               warp 12.00361 -25.82509 -243.0502
               warp 76.30077 -24.12408 -88.81075
               warp 82.54358 -37.00394 117.0964
               warp 376.3995 -25.73 -209.09
             *
             */
        };


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
            PrefabInfo fabricatorPrefabInfo = PrefabInfo
                .WithTechType(PrefabClassId, null, null, unlockAtStart: false);

            CustomPrefab fabricatorFragmentPrefab = new CustomPrefab(fabricatorPrefabInfo);

            PrefabTemplate cloneTemplate = new CloneTemplate(fabricatorFragmentPrefab.Info, TechType.GravSphereFragment)

            // PrefabTemplate cloneTemplate = new CloneTemplate(fabricatorFragmentPrefab.Info, "8029a9ce-ab75-46d0-a8ab-63138f6f83e4")
            {
                ModifyPrefab = prefab =>
                {
                    // Add components
                    Log.LogDebug("PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component...");
                    PrefabUtils.AddBasicComponents(prefab, PrefabClassId, fabricatorPrefabInfo.TechType, LargeWorldEntity.CellLevel.Medium);
                    PrefabUtils.AddResourceTracker(prefab, TechType.Fragment);
                    prefab.AddComponent<PetFabricatorFragment>();
                    Log.LogDebug(
                        "PetFabricatorFragmentPrefab: InitPrefab adding PetFabricatorFragment component... Done.");
                }
            };
            
            fabricatorFragmentPrefab.SetGameObject(cloneTemplate);
            fabricatorFragmentPrefab.SetSpawns(SpawnLocations);

            // Set up scannable
            fabricatorFragmentPrefab.SetUnlock(fabricatorPrefabInfo.TechType)
                .WithScannerEntry(fabricatorPrefabInfo.TechType, 5f, true, PetFabricatorPrefab.PetFabricatorEncyKey, true)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetFabricatorPopupImageTexture), null, null);

            Log.LogDebug($"PetFabricatorFragmentPrefab: Registering {PrefabClassId}...");
            fabricatorFragmentPrefab.Register();
            Log.LogDebug($"PetFabricatorFragmentPrefab: Init Prefab for {PrefabClassId}. Done.");
            PrefabInfo = fabricatorFragmentPrefab.Info;
        }
    }
}
