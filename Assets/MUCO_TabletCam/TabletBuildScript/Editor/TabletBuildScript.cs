using Unity.XR.Oculus;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.Management;

namespace MUCO.Build.Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.XR;
    using UnityEngine.XR.Management;


    public static class TabletBuildScript
    {
        // Perhaps by looking into this, we can have a single build for both tablet and VR headset.
        // But for now we go with special build for tablets.
        ///https://docs.unity3d.com/Packages/com.unity.xr.management@3.2/manual/EndUser.html
        /// snatched from https://stackoverflow.com/a/69245435
        public static void DisableXRLoaders(out List<string> removedLoadersList, out bool prevAutoLoader)
        {
            removedLoadersList = new List<string>();
            prevAutoLoader = false;

            XRGeneralSettingsPerBuildTarget buildTargetSettings;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
            {
                return;
            }

            var buildTargetGroup = BuildTargetGroup.Android;
            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(buildTargetGroup);
            if (settings == null)
            {
                return;
            }

            XRManagerSettings loaderManager = settings.AssignedSettings;

            if (loaderManager == null)
            {
                return;
            }

            prevAutoLoader = loaderManager.automaticLoading;
            loaderManager.automaticLoading = false;
            Debug.Log("Set auto loading to false");

            var loaders = loaderManager.activeLoaders;

            // If there are no loaders present in the current manager instance, then the settings will not be included in the current build.
            if (loaders.Count == 0)
            {
                Debug.Log("XR Loaders count == 0. Sad!");

                return;
            }

            var rl = loaders.ToList();
            removedLoadersList.AddRange(loaders.Select(l => l.name));

            foreach (var loader in rl)
            {
                Debug.Log("Removing loader " + loader.name + "; ");

                loaderManager.TryRemoveLoader(loader);
            }
        }

        // Undoes what DisableXRLoaders does.
        public static void ReenableXRLoaders(List<string> loaders, bool prevAutoLoader)
        {
            XRGeneralSettingsPerBuildTarget buildTargetSettings;
            EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);
            if (buildTargetSettings == null)
            {
                return;
            }

            var buildTargetGroup = BuildTargetGroup.Android;
            XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(buildTargetGroup);
            if (settings == null)
            {
                return;
            }

            XRManagerSettings loaderManager = settings.AssignedSettings;

            if (loaderManager == null)
            {
                return;
            }

            foreach (var loader in loaders)
            {
                Debug.Log("Re-adding loader " + loader + "; ");
                if (loader.ToLower().Contains("oculus"))
                {
                    // how to load the oculus loader???!?!?! find the scriptableobject???
                    var loaderz = AssetDatabase.FindAssets("Oculus Loader");
                    var guid = loaderz.FirstOrDefault();
                    if (guid != null)
                    {
                        var assetatpath = AssetDatabase.GUIDToAssetPath(loaderz.FirstOrDefault());
                        var ocuLoaderFile =
                            (OculusLoader) AssetDatabase.LoadAssetAtPath(assetatpath, typeof(OculusLoader));
                        Debug.Log("Adding " + assetatpath + " ehhh: " + ocuLoaderFile);
                        var oculusLoaderino = ocuLoaderFile;

                        loaderManager.TryAddLoader(oculusLoaderino);
                    }
                }
            }

            loaderManager.automaticLoading = prevAutoLoader;
            Debug.Log("Setting auto loading to true");
        }

        public static void Build(string fullPath, bool andRun)
        {
            BuildOptions buildOptions = BuildOptions.ShowBuiltPlayer;
            if (andRun)
            {
                buildOptions = buildOptions | BuildOptions.AutoRunPlayer;
            }

            // disable VR shitz
            DisableXRLoaders(out var loaders, out var prevAutoLoader);

            // manifest shenanz
            //TabletManifestoWriter.WriteTabletCamStuffToTheManifest(out var oldMan);

            Debug.Log("Building to: " + fullPath);

            BuildPipeline.BuildPlayer(new BuildPlayerOptions()
            {
                scenes = SelectScenes(),
                locationPathName = fullPath,
                target = BuildTarget.Android,
                options = buildOptions
            });

            // revert manifest shenanz
            //TabletManifestoWriter.RevertManifest(oldMan);

            // reenable VR shitz
            ReenableXRLoaders(loaders, prevAutoLoader);

            Debug.Log("TabletCam build completed");
        }


        private static string GetPath(string buildFolder, string filename)
        {
            // format:
            var fileWithExtension = string.Concat(filename, ".", GetExtension(BuildTarget.Android));
            return Path.Combine(buildFolder, fileWithExtension);
        }

        public static string GetExtension(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                {
                    return "exe";
                }

#pragma warning disable 0618
                case BuildTarget.StandaloneLinux:
                {
                    return "x86";
                }

                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                {
                    return "x86_64";
                }
#pragma warning restore 0618
                case BuildTarget.Android:
                {
                    return "apk";
                }
            }

            return string.Empty;
        }

        private static string[] SelectScenes()
        {
            return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        }
    }
}