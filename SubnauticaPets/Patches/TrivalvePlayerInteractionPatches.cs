﻿#if SUBNAUTICAZERO
using DaftAppleGames.SubnauticaPets.Mono.Pets;
using HarmonyLib;
using Oculus.Platform;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(Player))]
    internal class TrivalvePlayerInteractionPatches
    {
        internal class TrivalvePlayerInteractionPatch
        {
            [HarmonyPatch(nameof(TrivalvePlayerInteraction.OnHandHover))]
            [HarmonyPrefix]
            public static bool OnHandHover_Prefix(TrivalvePlayerInteraction __instance, GUIHand hand)
            {
                // CreaturePetPlugin_BZ.Log.LogDebug("In TrivalvePlayerInteraction.OnHandOver");
                Pet pet = __instance.GetComponent<Pet>();
                // Call original class method if not dealing with a Pet
                if (!pet)
                {
                    return true;
                }

                if (AllowedToInteract(__instance.trivalve.swimWalkController.state, pet))
                {
                    return false;
                }

                if (Player.main.GetRightHandDown())
                {
                    __instance.Invoke("PlayCommandAnimation", 0f);
                    return false;
                }

                // Configure cursor
                string text = __instance.trivalve.followingPlayer ? "FishStopFollow" : "FishStartFollow";
                HandReticle.main.SetText(HandReticle.TextType.Hand, $"Play with {pet.PetName}", false,
                    GameInput.Button.LeftHand);
                HandReticle.main.SetText(HandReticle.TextType.HandSubscript, text, true, GameInput.Button.RightHand);
                HandReticle.main.SetIcon(HandReticle.IconType.Hand);
                return false;
            }

            [HarmonyPatch(nameof(TrivalvePlayerInteraction.OnHandClick))]
            [HarmonyPrefix]
            public static bool OnHandClick_Prefix(TrivalvePlayerInteraction __instance, GUIHand hand)
            {
                Pet pet = __instance.GetComponent<Pet>();
                // Call original class method if not dealing with a Pet
                if (!pet)
                {
                    Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: not a pet Trivalve!");
                    return true;
                }

                SwimWalkCreatureController.State state = __instance.trivalve.swimWalkController.state;
                if (!AllowedToInteract(state, pet))
                {
                    Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Pet Trivalve, not allowed to interact!");
                    return false;
                }

                if (state == SwimWalkCreatureController.State.Swim)
                {
                    Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Swimming state!");
                    __instance.PrepareWaterCinematic(hand.player);
                    return false;
                }

                Log.LogDebug("TrivalvePlayerInteraction.OnHandClick: Land state!");
                __instance.PrepareLandCinematic(hand.player);
                return false;
            }

            /// <summary>
            /// Overrides the TrivalvePlayerInteraction "IsAllowedToInteract" method
            /// </summary>
            /// <param name="swimWalkState"></param>
            /// <returns></returns>
            public static bool AllowedToInteract(SwimWalkCreatureController.State swimWalkState, Pet pet)
            {
                TrivalvePlayerInteraction _trivalvePlayerInteraction = pet.GetComponent<TrivalvePlayerInteraction>();

                if (_trivalvePlayerInteraction.state != TrivalvePlayerInteraction.State.None)
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: State is not None!");
                    return false;
                }

                if (PlayerCinematicController.cinematicModeCount > 0)
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Player cinematic count is not 0!");
                    return false;
                }

                if (!_trivalvePlayerInteraction.liveMixin.IsAlive())
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivalve is not alive!");
                    return false;
                }

                Player localPlayerComp = Player.main;
                if (localPlayerComp == null)
                {
                    // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Can't find LocalPlayerComp!");
                    return false;
                }

                if (swimWalkState == SwimWalkCreatureController.State.Swim)
                {
                    if (!localPlayerComp.IsSwimming())
                    {
                        // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivale is swimming and Player is not!!");
                        return false;
                    }
                }
                else
                {
                    if (!_trivalvePlayerInteraction.trivalve.onSurfaceTracker.onSurface)
                    {
                        // CreaturePetPlugin_BZ.Log.LogDebug( $"CreaturePet.AllowedToInteract: Trivalve not on surface!");
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
#endif