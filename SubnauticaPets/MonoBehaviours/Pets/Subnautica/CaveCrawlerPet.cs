﻿#if SUBNAUTICA
using Nautilus.Assets;
using Nautilus.Crafting;
using UnityEngine;
using static LootDistributionData;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica
{
    /// <summary>
    /// Implements CaveCrawler specific Pet functionality
    /// </summary>
    internal class CaveCrawlerPet : Pet
    {
        // Configuration required to build and spawn

        // Pet
        public static string ClassId = "CaveCrawlerPet";
        public static string TextureName = "CaveCrawlerTexture";
        public static PrefabInfo BuildablePrefabInfo;
        public static string PrefabGuid = "3e0a11f1-e2b2-4c4f-9a8e-0b0a77dcc065"; // https://github.com/LeeTwentyThree/Nautilus/blob/master/Nautilus/Documentation/resources/SN1-PrefabPaths.json
        public static string ModelName = "cave_crawler_01"; // Animator on "cave_crawler_01" parent
        public static Vector3 ModelScale = new Vector3(1, 1, 1);
        public static float VfxMinOffset = -0.2f;
        public static float VfxMaxOffset = 0.5f;

        // Pet DNA
        public static string DnaClassId = "CaveCrawlerPetDna";
        public static string DnaTextureName = "CaveCrawlerDnaStrandTexture";
        public static PrefabInfo DnaBuildablePrefabInfo;

        // Random DNA collectible distribution biome data
        public static BiomeData[] LootDistributionBiomeData = new LootDistributionData.BiomeData[] {
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Barrier, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.Kelp_TechSite_Scattered, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.CrashZone_Sand, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite_Barrier, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.GrassyPlateaus_TechSite_Scattered, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Barrier, count = 1, probability = 0.9f},
            new LootDistributionData.BiomeData { biome = BiomeType.SafeShallows_TechSite_Scattered, count = 1, probability = 0.9f},
        };

        public static Color PetObjectColor = Color.cyan;

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

        // Cave Crawler scale factor
        public override float ScaleFactor => 1.0f;
    }
}
#endif