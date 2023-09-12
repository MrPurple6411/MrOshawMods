﻿#if SUBNAUTICA
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.Subnautica;
#endif

#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Pets.BelowZero;
#endif

using DaftAppleGames.SubnauticaPets.MonoBehaviours.Console;
using DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils;
using DaftAppleGames.SubnauticaPets.Utils;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using rail;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.CustomObjects
{
    /// <summary>
    /// This static class provides methods to create our "Buildable Pets", via the
    /// new Pet Fabricator.
    /// </summary>
    public static class PetBuildablePrefab
    {
        /// <summary>
        /// Initialise all Pet buildables
        /// </summary>
        public static void InitPetBuildables()
        {
#if SUBNAUTICA
            CaveCrawlerBuildable.Register();
            BloodCrawlerBuildable.Register();
            CrabSquidBuildable.Register();
            AlienRobotBuildable.Register();
#endif
#if SUBNAUTICAZERO
            PenglingBabyBuildable.Register();
            PenglingAdultBuildable.Register();
            SnowStalkerBabyBuildable.Register();
            PinnicaridBuildable.Register();
            TrivalveYellowBuildable.Register();
            TrivalveBluePetBuildable.Register();
#endif
        }

        /// <summary>
        /// Generic method to return the PrefabInfo for a given set of pet details
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public static PrefabInfo CreateBuildablePrefabInfo(string classId, string displayName, string description, string textureName)
        {
            Log.LogDebug($"PetBuildablePrefab: CreateBuildablePrefabInfo with classId {classId}.");
            PrefabInfo prefabInfo = PrefabInfo
                .WithTechType(classId, displayName, description)
                // Set the icon
                .WithIcon(ModUtils.GetSpriteFromAssetBundle(textureName));

            return prefabInfo;
        }

        /// <summary>
        /// Generic method to register PetInfo
        /// </summary>
        /// <param name="prefabInfo"></param>
        /// <param name="cloneTemplateClassGuid"></param>
        /// <param name="modelGameObjectName"></param>
        /// <param name="recipe"></param>
        /// <param name="modelScale"></param>
        /// <param name="vfxMinOffset"></param>
        /// <param name="vfxMaxOffset"></param>
        /// <returns></returns>
        public static void RegisterPrefabInfo(PrefabInfo prefabInfo, string cloneTemplateClassGuid,
            string modelGameObjectName,
            RecipeData recipe, Vector3 modelScale,
            float vfxMinOffset, float vfxMaxOffset)
        {
            // Create prefab
            CustomPrefab prefab = new CustomPrefab(prefabInfo);
            // Copy the prefab model
            CloneTemplate cloneTemplate = new CloneTemplate(prefabInfo, cloneTemplateClassGuid);
            // modify the cloned model:
            cloneTemplate.ModifyPrefab += obj =>
            {
                // Set constructable flags
                ConstructableFlags constructableFlags = ConstructableFlags.Inside;

                // Find the object that holds the model for the fabricator
                GameObject modelGameObject = null;

                Log.LogDebug($"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {obj.name}...");
                // First, find the Animator
                Animator animator = obj.GetComponentInChildren<Animator>(true);
                if (animator == null)
                {
                    Log.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find Animator in {obj.name}"!);
                }
                else
                {
                    GameObject animatorGameObject = animator.gameObject;
                    modelGameObject = animatorGameObject;

                    // Set the model scale when it's used by the fabricator
                    ScaleOnStart scaleOnStart = modelGameObject.AddComponent<ScaleOnStart>();
                    scaleOnStart.Scale = modelScale;

                    // Add Fabricator VFX
                    VFXFabricating fabVfx = modelGameObject.AddComponent<VFXFabricating>();
                    fabVfx.localMinY = vfxMinOffset; // -0.2f
                    fabVfx.localMaxY = vfxMaxOffset; // 0.5f
                    fabVfx.posOffset = new Vector3(0.0f, 0.0f, 0.0f);
                    fabVfx.eulerOffset = new Vector3(0.0f, 0.0f, 0.0f);
                    fabVfx.scaleFactor = 1.0f;
                    // We'll take this opportunity to disable this for the ghost. We'll re-enable when spawned
                    animator.enabled = false;

                    /*
                    // Look for the first MeshRender in the child - this will work as the model
                    SkinnedMeshRenderer renderer = animatorGameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                    if (renderer == null)
                    {
                        Log.LogError($"PetBuildableInfo: RegisterPrefabInfo can't find SkinnedMeshRenderer in {animatorGameObject.name}"!);
                    }
                    else
                    {
                        Log.LogDebug($"PetBuildablePrefab: RegisterPrefabInfo looking for prefab model for {prefabInfo.TechType}. Found {modelGameObject.name}. Done.");
                        modelGameObject = renderer.gameObject;
                    }
                    */
                }

                /*
                GameObject model = obj.transform.Find(modelGameObjectName).gameObject;
                if (!model)
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo cannot find object model {modelGameObjectName} in prefab!");
                }
                else
                {
                    Log.LogDebug($"PetBuildableUtils: RegisterPrefabInfo {modelGameObjectName} found model on {model.name}");
                }
                */

                // add all components necessary for it to be built:
                PrefabUtils.AddConstructable(obj, prefabInfo.TechType, constructableFlags, modelGameObject);
            };
            // Assign the created clone model to the prefab itself:
            prefab.SetGameObject(cloneTemplate);
            // Set recipe
            prefab.SetRecipe(recipe);
            // Register it into the game:
            prefab.Register();
        }

#if SUBNAUTICA
        /// <summary>
        /// Cave Crawler Buildable
        /// </summary>
        public static class CaveCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CaveCrawlerPet.ClassId, null, null, CaveCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, CaveCrawlerPet.PrefabGuid, CaveCrawlerPet.ModelName, CaveCrawlerPet.GetRecipeData(),
                    CaveCrawlerPet.ModelScale, CaveCrawlerPet.VfxMinOffset, CaveCrawlerPet.VfxMaxOffset);
                CaveCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Blood Crawler Buildable
        /// </summary>
        public static class BloodCrawlerBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(BloodCrawlerPet.ClassId, null, null, BloodCrawlerPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, BloodCrawlerPet.PrefabGuid, BloodCrawlerPet.ModelName, BloodCrawlerPet.GetRecipeData(),
                    BloodCrawlerPet.ModelScale, BloodCrawlerPet.VfxMinOffset, BloodCrawlerPet.VfxMaxOffset);
                BloodCrawlerPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Crab Squid Buildable
        /// </summary>
        public static class CrabSquidBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(CrabSquidPet.ClassId, null, null, CrabSquidPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, CrabSquidPet.PrefabGuid, CrabSquidPet.ModelName, CrabSquidPet.GetRecipeData(),
                    CrabSquidPet.ModelScale, CrabSquidPet.VfxMinOffset, CrabSquidPet.VfxMaxOffset);
                CrabSquidPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Alien Robot Buildable
        /// </summary>
        public static class AlienRobotBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(AlienRobotPet.ClassId, null, null, AlienRobotPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, AlienRobotPet.PrefabGuid, AlienRobotPet.ModelName, AlienRobotPet.GetRecipeData(),
                    AlienRobotPet.ModelScale, AlienRobotPet.VfxMinOffset, AlienRobotPet.VfxMaxOffset);
                AlienRobotPet.BuildablePrefabInfo = Info;
            }
        }

#endif

#if SUBNAUTICAZERO
        /// <summary>
        /// Baby Pengling Buildable
        /// </summary>
        public static class PenglingBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingBabyPet.ClassId, null, null, PenglingBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PenglingBabyPet.PrefabGuid, PenglingBabyPet.ModelName, PenglingBabyPet.GetRecipeData(),
                    PenglingBabyPet.ModelScale, PenglingBabyPet.VfxMinOffset, PenglingBabyPet.VfxMaxOffset);
                PenglingBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pengling Adult Buildable
        /// </summary>
        public static class PenglingAdultBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PenglingAdultPet.ClassId, null, null, PenglingAdultPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PenglingAdultPet.PrefabGuid, PenglingAdultPet.ModelName, PenglingAdultPet.GetRecipeData(),
                    PenglingAdultPet.ModelScale, PenglingAdultPet.VfxMinOffset, PenglingAdultPet.VfxMaxOffset);
                PenglingAdultPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Snow Stalker Baby Buildable
        /// </summary>
        public static class SnowStalkerBabyBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(SnowStalkerBabyPet.ClassId, null, null, SnowStalkerBabyPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, SnowStalkerBabyPet.PrefabGuid, SnowStalkerBabyPet.ModelName, SnowStalkerBabyPet.GetRecipeData(),
                    SnowStalkerBabyPet.ModelScale, SnowStalkerBabyPet.VfxMinOffset, SnowStalkerBabyPet.VfxMaxOffset);
                SnowStalkerBabyPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Pinnicarid Buildable
        /// </summary>
        public static class PinnicaridBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(PinnicaridPet.ClassId, null, null, PinnicaridPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, PinnicaridPet.PrefabGuid, PinnicaridPet.ModelName, PinnicaridPet.GetRecipeData(),
                    PinnicaridPet.ModelScale, PinnicaridPet.VfxMinOffset, PinnicaridPet.VfxMaxOffset);
                PinnicaridPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Yellow Trivalve Buildable
        /// </summary>
        public static class TrivalveYellowBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(TrivalveYellowPet.ClassId, null, null, TrivalveYellowPet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, TrivalveYellowPet.PrefabGuid, TrivalveYellowPet.ModelName, TrivalveYellowPet.GetRecipeData(),
                    TrivalveYellowPet.ModelScale, TrivalveYellowPet.VfxMinOffset, TrivalveYellowPet.VfxMaxOffset);
                TrivalveYellowPet.BuildablePrefabInfo = Info;
            }
        }

        /// <summary>
        /// Blue Trivalve Buildable
        /// </summary>
        public static class TrivalveBluePetBuildable
        {
            // Init PrefabInfo
            public static PrefabInfo Info { get; } = CreateBuildablePrefabInfo(TrivalveBluePet.ClassId,
                null, null, TrivalveBluePet.TextureName);

            // Register with the game
            public static void Register()
            {
                RegisterPrefabInfo(Info, TrivalveBluePet.PrefabGuid,
                    TrivalveBluePet.ModelName, TrivalveBluePet.GetRecipeData(), TrivalveBluePet.ModelScale,
                    TrivalveBluePet.VfxMinOffset, TrivalveBluePet.VfxMaxOffset);
                TrivalveBluePet.BuildablePrefabInfo = Info;
            }
        }
#endif
    }
}