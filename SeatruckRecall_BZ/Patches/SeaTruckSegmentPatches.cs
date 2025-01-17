﻿using HarmonyLib;
using DaftAppleGames.SeatruckRecall_BZ.AutoPilot;
using DaftAppleGames.SeatruckRecall_BZ.Navigation;

namespace DaftAppleGames.SeatruckRecall_BZ.Patches
{
    /// <summary>
    /// Harmony patches for the Seatruck
    /// </summary>
    [HarmonyPatch(typeof(SeaTruckSegment))]
    internal class SeaTruckSegmentPatches
    {
        /// <summary>
        /// Patch the Start method, to add the instance
        /// to the static global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.Start))]
        [HarmonyPostfix]
        internal static void StartPostfix(SeaTruckSegment __instance)
        {
            // Check to see if this is the "main" truck segment
            if (__instance.isMainCab)
            {
                // Add the SeatruckRecallListener component
                if (!__instance.gameObject.GetComponent<SeaTruckAutoPilot>())
                {
                    // Add the new AutoPilot component
                    SeaTruckDockRecallPlugin.Log.LogInfo("Adding SeaTruckAutopilot component...");
                    SeaTruckAutoPilot newAutoPilot = __instance.gameObject.AddComponent<SeaTruckAutoPilot>();
                    SeaTruckDockRecallPlugin.Log.LogInfo(
                    $"Added SeaTruckAutopilot component to {__instance.gameObject.name}!");

                    // Register the new AutoPilot component with all registered Dock Recallers
                    SeaTruckDockRecallPlugin.RegisterAutoPilot(newAutoPilot);

                    // Add the Waypoint Nav component
                    SeaTruckDockRecallPlugin.Log.LogInfo("Adding WaypointNavigation component...");
                    WaypointNavigation waypointNav = __instance.gameObject.AddComponent<WaypointNavigation>();
                    SeaTruckDockRecallPlugin.Log.LogInfo(
                        $"Added WaypointNavigation component to {__instance.gameObject.name}!");

                    // Add the RigidBody based NavMovement component
                    SeaTruckDockRecallPlugin.Log.LogInfo("Adding NavMovement component...");
                    INavMovement newNavMovement = __instance.gameObject.AddComponent<RigidbodyNavMovement>();
                    // INavMovement newNavMovement = __instance.gameObject.AddComponent<TeleportNavMovement>();
                    // INavMovement newNavMovement = __instance.gameObject.AddComponent<TransformNavMovement>();
                    SeaTruckDockRecallPlugin.Log.LogInfo(
                        $"Added NavMovement component to {__instance.gameObject.name}!");
                }
            }
        }

        /// <summary>
        /// Patch the OnDestroy method, to remove
        /// the instance from the global list
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.OnDestroy))]
        [HarmonyPostfix]
        internal static void OnDestroyPostfix(SeaTruckSegment __instance)
        {
            SeaTruckAutoPilot autoPilot = __instance.GetComponent<SeaTruckAutoPilot>();
            if (autoPilot)
            {
                SeaTruckDockRecallPlugin.UnRegisterAutoPilot(autoPilot);
            }
        }

        /// <summary>
        /// Patch UpdateKinematicState, to prevent the Rigidbody being disabled
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(SeaTruckSegment.UpdateKinematicState))]
        [HarmonyPrefix]
        internal static bool UpdateKinematicStatePrefix(SeaTruckSegment __instance)
        {
            SeaTruckDockRecallPlugin.Log.LogInfo($"In UpdateKinematicState....{__instance.gameObject.name}");

            SeaTruckSegment firstSegment = __instance.GetFirstSegment();
            SeaTruckAutoPilot autoPilot = firstSegment.GetComponent<SeaTruckAutoPilot>();
            if (autoPilot)
            {
                if (autoPilot.AutoPilotEnabled)
                {
                    SeaTruckDockRecallPlugin.Log.LogInfo("Overriding UpdateKinematicState....");
                    UWE.Utils.SetIsKinematicAndUpdateInterpolation(firstSegment.rb, false, false);
                    return false;
                }
            }

            return true;
        }
    }
}
