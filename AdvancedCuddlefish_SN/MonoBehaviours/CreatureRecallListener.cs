﻿using System;
using UnityEngine;
using static Oculus.Platform.Models.Product;

namespace DaftAppleGames.CuddlefishRecall_SN.MonoBehaviours
{
    internal class CreatureRecallListener : MonoBehaviour
    {
        private const string CantRecallMessage = "Cannot recall Cuddlefish to this location!";
        private const string RecallSuccessfulMessage = "Cuddlefish recalled!";
        private const string RecallEnRoute = "Cuddlefish is en-route!";
        private SwimBehaviour _swimBehaviour;
        private SwimRandom _swimRandom;

        // Parameters for the "Swim To" function
        private Vector3 _currentTarget;
        private bool _isBeingRecalled = false;

        /// <summary>
        /// Initialise the component
        /// </summary>
        public void Start()
        {
            CuddlefishRecallPlugin.Log.LogDebug("Finding SwimBehaviour...");
            _swimBehaviour = GetComponent<SwimBehaviour>();
            _swimRandom = GetComponent<SwimRandom>();

        }

        /// <summary>
        /// Used to call the SwimTo behaviour, if enabled
        /// </summary>
        public void Update()
        {
            if (!_isBeingRecalled)
            {
                return;
            }
            // Check to see if we've arrived
            if (Vector3.Distance(transform.position, Player.main.transform.position) < 2.0f)
            {
                ErrorMessage.AddMessage("Cuddlefish has arrived!");
                _isBeingRecalled = false;
                return;
            }

            // Swim to target
            _swimBehaviour.SwimTo(Player.main.transform.position, CuddlefishRecallPlugin.RecallSwimVelocity.Value);
        }

        /// <summary>
        /// Public method to recall the creature to the target transform
        /// </summary>
        /// <param name="targetPosition"></param>
        public void RecallCreature(float buffer)
        {
            // Teleport method
            if (CuddlefishRecallPlugin.RecallMethod.Value == RecallMoveMethod.Teleport)
            {
                Vector3 targetPosition = Player.main.transform.position + (Vector3.forward * buffer);
                CuddlefishRecallPlugin.Log.LogDebug($"Teleporting GameObject to: {targetPosition}");
 
                if (Player.main.GetBiomeString().StartsWith("precursor", StringComparison.OrdinalIgnoreCase))
                {
                    ErrorMessage.AddMessage(CantRecallMessage);
                    return;
                }

                int num = UWE.Utils.OverlapSphereIntoSharedBuffer(transform.position, 5f, -1, QueryTriggerInteraction.UseGlobal);
                for (int i = 0; i < num; i++)
                {
                    if (UWE.Utils.sharedColliderBuffer[i].GetComponentInParent<SubRoot>())
                    {
                        ErrorMessage.AddMessage(CantRecallMessage);
                        return;
                    }
                }

                gameObject.transform.position = targetPosition;
                ErrorMessage.AddMessage(RecallSuccessfulMessage);
                CuddlefishRecallPlugin.Log.LogDebug("GameObject teleported.");
            }

            // Swim to method
            if (CuddlefishRecallPlugin.RecallMethod.Value == RecallMoveMethod.SwimTo)
            {
                CuddlefishRecallPlugin.Log.LogDebug($"Swimming to Player position");
                _isBeingRecalled = true;
                CuddlefishRecallPlugin.Log.LogDebug("Swimming to player in progress...");
                ErrorMessage.AddMessage(RecallEnRoute);
            }
        }
    }
}
