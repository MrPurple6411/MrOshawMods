﻿using DaftAppleGames.SubnauticaPets.MonoBehaviours.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours
{
    /// <summary>
    /// Base MonoBehaviour class for new Fragments
    /// </summary>
    internal abstract class BaseFragment : MonoBehaviour
    {
        public abstract Vector3 ColliderSize { get; }
        public abstract Vector3 ColliderCenter { get; }

        private int _numCalls = 0;

        /// <summary>
        /// Add new components
        /// </summary>
        public virtual void Awake()
        {
            _numCalls++;
            AddComponents();
            RemoveOldModel();
        }

        /// <summary>
        /// Reconfigure any components before start
        /// </summary>
        public virtual void Start()
        {
            ResizeCollider();
            UpdateComponents();
        }

        /// <summary>
        /// Add and configure components
        /// </summary>
        private void AddComponents()
        {
            Log.LogDebug($"Adding Components to {gameObject.name}");

            // Sky Applier
            SkyApplier skyApplier;
            skyApplier = gameObject.GetComponent<SkyApplier>();
            if (skyApplier == null)
            {
                skyApplier = gameObject.AddComponent<SkyApplier>();
            }
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            skyApplier.renderers = renderers;

            // AlignToFloor
            AlignToFloorOnStart alignToFloor;
            alignToFloor = gameObject.GetComponent<AlignToFloorOnStart>();
            if (alignToFloor == null)
            {
                // alignToFloor = gameObject.AddComponent<AlignToFloorOnStart>();
            }

            // FreezeOnSettle
            FreezeOnSettle freeze;
            freeze = gameObject.GetComponent<FreezeOnSettle>();
            if (freeze == null)
            {
                freeze = gameObject.AddComponent<FreezeOnSettle>();
            }
        }

        /// <summary>
        /// Update some of the cloned components
        /// </summary>
        private void UpdateComponents()
        {
            // Prevent fragments from being phsyically picked up
            Pickupable pickupable = GetComponent<Pickupable>();
            if (pickupable)
            {
                pickupable.isPickupable = false;
            }

            // Prevent fragments from being knocked around, but allow them to sink.
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            if (rigidbody)
            {
                // rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }


        /// <summary>
        /// Resize the box collider
        /// </summary>
        private void ResizeCollider()
        {
            BoxCollider collider = GetComponentInChildren<BoxCollider>(true);
            if (collider)
            {
                collider.center = ColliderCenter;
                collider.size = ColliderSize;
            }
        }

        /// <summary>
        /// Deletes the old model
        /// </summary>
        private void RemoveOldModel()
        {
            GameObject oldModelGameObject = gameObject.FindChild("model");
            if (oldModelGameObject != null)
            {
                Log.LogDebug($"BaseFragment: Destroying old model...");
                Object.Destroy(oldModelGameObject);
            }
        }
    }
}
