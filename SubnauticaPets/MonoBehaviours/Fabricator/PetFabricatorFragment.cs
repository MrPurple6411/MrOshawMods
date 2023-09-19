﻿using DaftAppleGames.SubnauticaPets.Utils;
using UnityEngine;
using static DaftAppleGames.SubnauticaPets.SubnauticaPetsPlugin;

namespace DaftAppleGames.SubnauticaPets.MonoBehaviours.Fabricator
{
    /// <summary>
    /// MonoBehaviour class to support the Pet Fabricator Fragment
    /// </summary>
    internal class PetFabricatorFragment : BaseFragment
    {
        // Fragment specific collider dimensions
        public override Vector3 ColliderCenter => new Vector3(0.0f, 0.61f, 0.24f);
        public override Vector3 ColliderSize => new Vector3(1.02f, 1.2f, 0.52f);

        public const string DamagedTexture = "PetFabricatorDamagedTexture";
        public const string MeshTextureName = "submarine_Workbench";
        /// <summary>
        /// Don't need any of the base class Awake
        /// </summary>
        public override void Awake()
        {
            MatUtils.SetMaterialTexture(gameObject, MeshTextureName, DamagedTexture);
            base.AddComponents();
        }

        /// <summary>
        /// Don't need any of the base class Start
        /// </summary>
        public override void Start()
        {

        }
    }
}
