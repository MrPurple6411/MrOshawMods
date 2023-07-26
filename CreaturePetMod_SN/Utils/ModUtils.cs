﻿using System.Text.RegularExpressions;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours;
using DaftAppleGames.CreaturePetModSn.MonoBehaviours.Pets;
using UnityEngine;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;
using Object = UnityEngine.Object;

namespace DaftAppleGames.CreaturePetModSn.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    internal static class ModUtils
    {
        /// <summary>
        /// Example static method to return Players current location / transform
        /// </summary>
        /// <returns></returns>
        internal static Transform GetPlayerTransform()
        {
            return Player.main.transform;
        }

        /// <summary>
        /// Updates the InputManager Spawn shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateSpawnKeyboardShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.SpawnKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Updates the InputManager Spawn Modifier shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateSpawnKeyboardModifierShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.SpawnModifierKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }


        /// <summary>
        /// Updates the InputManager Kill All shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateKillAllKeyboardShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.KillAllKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Updates the InputManager Spawn Modifier shortcut key
        /// </summary>
        /// <param name="newKeyCode"></param>
        internal static void UpdateKillAllKeyboardModifierShortcut(KeyCode newKeyCode)
        {
            ModInputManager inputManager = Object.FindObjectOfType<ModInputManager>();
            if (inputManager != null)
            {
                inputManager.KillAllModifierKeyCode = newKeyCode;
            }
            else
            {
                Log.LogDebug("UpdateSpawnKeyboardShortcut: Didn't find a ModInputManager");
            }
        }

        /// <summary>
        /// Update the PetSpawner Pet Type
        /// </summary>
        /// <param name="newPetType"></param>
        internal static void UpdatePetType(PetCreatureType newPetType)
        {
            PetSpawner petSpawner = Object.FindObjectOfType<PetSpawner>();
            if (petSpawner != null)
            {
                petSpawner.PetCreatureType = newPetType;
            }
            else
            {
                Log.LogDebug("UpdatePetType: Didn't find a PetSpawner");
            }
        }

        /// <summary>
        /// Update the PetSpawner Pet Name
        /// </summary>
        /// <param name="newPetName"></param>
        internal static void UpdatePetName(PetName newPetName)
        {
            PetSpawner petSpawner = Object.FindObjectOfType<PetSpawner>();
            if (petSpawner != null)
            {
                petSpawner.PetName = newPetName;
            }
            else
            {
                Log.LogDebug("UpdatePetName: Didn't find a PetSpawner");
            }
        }

        /// <summary>
        /// Destroys all child components of a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void DestroyComponentsInChildren<T>(GameObject gameObject)
        {
            Log.LogDebug($"ModUtils: Destroying all components of type: {typeof(T)}");
            var components = gameObject.GetComponentsInChildren<T>(true);

            Log.LogDebug($"ModUtils: Found {components.Length} instances to destroy");

            // Iterate through all child components and destroy them
            foreach (var component in components)
            {
                GameObject.Destroy(component as MonoBehaviour);
                Log.LogDebug($"ModUtils: Destroyed: {component.GetType()}");
            }
            Log.LogDebug($"ModUtils: Destroying all components of type: {typeof(T)}. Done.");
        }

        /// <summary>
        /// Adds spaces in CaselCase strings.
        /// So the above becomes "Camel Case".
        /// Used to "prettify" enum strings, for example.
        /// </summary>
        /// <param name="enumString"></param>
        /// <returns></returns>
        internal static string AddSpacesInCamelCase(string enumString)
        {
            return Regex.Replace(enumString, "([A-Z])", " $1").Trim();
        }
    }
}
