﻿#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements CrabSquid specific Pet functionality
    /// </summary>
    internal class CrabSquidPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "CrabSquidPet";
        public static string TextureName = "CrabSquidTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "4c2808fe-e051-44d2-8e64-120ddcdc8abb"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json
        public static string ModelName = "Crab_Squid"; // Anim on "Crab_Squid"
        public static Vector3 ModelScale = new Vector3(0.07f, 0.07f, 0.07f);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 1.2f;

        // Pet DNA
        public static string DnaClassId = "CrabSquidPetDna";
        public static string DnaTextureName = "CrabSquidDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 4, probability = 0.6f},
            new LootDistributionData.BiomeData { biome = BiomeType.KooshZone_TechSite, count = 5, probability = 0.8f},
            new LootDistributionData.BiomeData { biome = BiomeType.Mountains_TechSite, count = 5, probability = 0.4f},
            new LootDistributionData.BiomeData { biome = BiomeType.SparseReef_Techsite, count = 8, probability = 0.5f},
            new LootDistributionData.BiomeData { biome = BiomeType.UnderwaterIslands_TechSite, count = 6, probability = 0.8f},
        };

        public static Color PetObjectColor = Color.blue;

        /// <summary>
        /// Defines the Recipe for fabricating the Pet
        /// </summary>
        /// <returns></returns>
        public static RecipeData GetRecipeData()
        {
            RecipeData recipe = new RecipeData(
                new CraftData.Ingredient(TechType.Gold, 3),
                new CraftData.Ingredient(DnaBuildablePrefabInfo.TechType, 5));
            return recipe;
        }

        // Crab Squid scale factor
        public override float ScaleFactor => 0.07f;

        /// <summary>
        /// Add Creature specific components
        /// </summary>
        public override void AddComponents()
        {

            base.AddComponents();
        }

        /// <summary>
        /// Remove Creature specific components
        /// </summary>
        public override void RemoveComponents()
        {
            Log.LogDebug("CrabSquidPet: Destroying components...");
            ModUtils.DestroyComponentsInChildren<EMPAttack>(gameObject);
            ModUtils.DestroyComponentsInChildren<AttackLastTarget>(gameObject);
            // ModUtils.DestroyComponentsInChildren<SwimBehaviour>(gameObject);
            Log.LogDebug("CrabSquidPet: Destroying components... Done."); 
            base.RemoveComponents();
        }

        /// <summary>
        /// Update Creature specific components
        /// </summary>
        public override void UpdateComponents()
        {
            base.UpdateComponents();
        }
    }
}
#endif