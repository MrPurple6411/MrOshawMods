﻿using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using PlasticGui;
using Debug = UnityEngine.Debug;

namespace DaftAppleGames.SubnauticaModsProject.Editor
{
    public static class ModEditorUtils
    {
        private const string ModFolder = "E:\\Dev\\DAG\\Subnautica Mods\\";
        private const string BundleAssetFolder = "Assets/AssetBundles";

        public static void BuildAssetBundle(string bundleName, string modProjectFolderName)
        {
            string bundleDeployTargetFolder  = $"{ModFolder}\\{modProjectFolderName}\\Assets";

            if (!Directory.Exists(BundleAssetFolder))
            {
                Directory.CreateDirectory(BundleAssetFolder);
            }

            UnityEngine.Debug.Log("Building asset bundles...");
            AssetBundleManifest manifest = BuildAssetBundleByName(bundleName, BundleAssetFolder);
            UnityEngine.Debug.Log("Building asset bundle... Done.");

            UnityEngine.Debug.Log("Deploying asset bundle...");
            // Deploy the Asset Bundle
            string from = $"{Application.dataPath}\\AssetBundles\\{bundleName}";
            string fromManifest = $"{from}.manifest";
            string to = $"{bundleDeployTargetFolder}\\{bundleName}";
            string toManifest = $"{bundleDeployTargetFolder}\\{bundleName}.manifest";

            UnityEngine.Debug.Log($"Copy from: {from} to {to}.");
            File.Copy(from, to, true);
            File.Copy(fromManifest, toManifest, true);
            UnityEngine.Debug.Log("Deploying asset bundle... Done.");
        }

        /// <summary>
        /// Builds only the specific Asset Bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="outputPath"></param>
        private static AssetBundleManifest BuildAssetBundleByName(string assetBundleName, string outputPath)
        {
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();

            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);

            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = assetBundleName;
            build.assetNames = assetPaths;

            builds.Add(build);

            return BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
    }
}