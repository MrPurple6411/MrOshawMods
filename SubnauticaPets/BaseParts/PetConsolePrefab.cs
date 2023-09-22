﻿
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using UnityEngine;
using static CraftData;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;
using Nautilus.Handlers;

namespace DaftAppleGames.SubnauticaPets.BaseParts
{
    /// <summary>
    /// Static class for creating the Pet Console prefab
    /// </summary>
    internal static class PetConsolePrefab
    {
        // Prefab class Id
        private const string PrefabClassId = "PetConsole";

        // Asset Bundle refs for Databank
        private static readonly string PetConsoleMainImageTexture = "PetConsoleDataBankMainImageTexture";
        private static readonly string PetConsolePopupImageTexture = "PetConsoleDataBankPopupImageTexture";

        // Databank key constants
        private const string PetConsoleEncyPath = "Tech/Habitats";
        private const string PetConsoleEncyKey = "PetConsole";

        /// <summary>
        /// Makes the new Pet Console available for use.
        /// </summary>
        public static void InitPetConsole()
        {
            PrefabInfo consolePrefabInfo = PrefabInfo.WithTechType(PrefabClassId, null, null, unlockAtStart: false);
            CustomPrefab consolePrefab = new CustomPrefab(consolePrefabInfo);

            /*
            // Create our custom prefab
            CustomPrefab consolePrefab = new CustomPrefab(PrefabClassId,
                null,
                null,
                ModUtils.GetSpriteFromAssetBundle(PetConsole.PetConsoleIconTexture));
            */

            // We'll use the PictureFrame as a template
            PrefabTemplate consoleTemplate = new CloneTemplate(consolePrefab.Info, TechType.PictureFrame)
            {
                // Reconfigure the prefab, once it's been created
                ModifyPrefab = ConfigureConsoleComponents
            };
            consolePrefab.SetGameObject(consoleTemplate);

            // Define the recipe for the new Console
            RecipeData recipe = new RecipeData
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 3),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.CopperWire, 2),
                    new Ingredient(TechType.Glass, 1)
                }
            };

            // Set the recipe.
            consolePrefab.SetRecipe(recipe);

            // Set up Databank
            SetupDatabank();

            // Set up the scanning and fragment unlocks
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry...");
            consolePrefab.SetUnlock(PetConsoleFragmentPrefab.PrefabInfo.TechType, 3)
                .WithScannerEntry(5f, false, PetConsoleEncyKey, true)
                .WithPdaGroupCategory(TechGroup.InteriorModules, TechCategory.InteriorModule)
                .WithAnalysisTech(ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture));
            Log.LogDebug("PetConsoleFragmentPrefab: Setting up scanner entry... Done.");

            consolePrefab.Register();

            PetConsole.PrefabInfo = consolePrefab.Info;
        }

        /// <summary>
        /// Set up Databank Entries
        /// </summary>
        private static void SetupDatabank()
        {
                // Set up Databank
                Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry...");
                PDAHandler.AddEncyclopediaEntry(PetConsoleEncyKey, PetConsoleEncyPath, null, null,
                    ModUtils.GetTexture2DFromAssetBundle(PetConsoleMainImageTexture),
                    ModUtils.GetSpriteFromAssetBundle(PetConsolePopupImageTexture));
                Log.LogDebug("PetConsoleFragmentPrefab: Setting up Databank entry... Done.");
        }

        /// <summary>
        /// Set up our custom "Console" component, replacing the default one
        /// </summary>
        /// <param name="consoleGameObject"></param>
        private static void ConfigureConsoleComponents(GameObject consoleGameObject)
        {
            // Add the PetConsole component, which will provision the UI
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component...");
            PetConsole petConsole = consoleGameObject.AddComponent<PetConsole>();
            Log.LogDebug("PetConsolePrefab: Adding PetConsole component... Done.");
        }
    }
}
