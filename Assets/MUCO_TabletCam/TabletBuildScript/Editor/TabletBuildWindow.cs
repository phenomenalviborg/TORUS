namespace MUCO.Build.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// A window that can help build a project to a new platform, bypassing the extra click for "Switch Platforms".
    /// It can be also used as a reference for how to do build scripts, and if you clone it you can extend with your own pre-build operations,
    ///     like disabling some game objects, generating stuff before build, enabling scenes, or whatever.
    ///     
    /// made by @horatiu665 and Rami Ahmed Bock cca 2018
    /// </summary>
    public class TabletBuildWindow : EditorWindow
    {
        [SerializeField]
        private string oldManifestoSave;

        //[MenuItem("Tools/TabletCam Build", priority = 66)]
        public static void BuildForTablet()
        {
            var buildPath = TabletBuildScriptPrefs.GetBuildPath();
            if (!File.Exists(buildPath))
            {
                buildPath = SetBuildPath();
            }

            TabletBuildScript.Build(buildPath, false);
        }

        [MenuItem("Tools/TabletCam Build And Run", priority = 67)]
        public static void BuildAndRunForTablet()
        {
            var buildPath = TabletBuildScriptPrefs.GetBuildPath();
            if (!File.Exists(buildPath))
            {
                buildPath = SetBuildPath();
            }

            TabletBuildScript.Build(buildPath, true);
        }

        // folder prompt, gets saved in the editor prefs.
        private static string SetBuildPath()
        {
            var fullPath = EditorUtility.SaveFilePanel("Build Folder",
                Application.dataPath.Replace("Assets", string.Empty),
                "build",
                TabletBuildScript.GetExtension(BuildTarget.Android));
            if (!string.IsNullOrEmpty(fullPath))
            {
                Debug.Log("Saving to " + fullPath);
                TabletBuildScriptPrefs.SetBuildPath(fullPath);
            }

            return fullPath;
        }

        private static void ResetTabletCamPrefs()
        {
            TabletBuildScriptPrefs.ResetTabletCamPrefs();
        }


        // OLD SHIT STARTS HERE ----- useful to see how the editor window can be done
        private const string BUILD_WINDOW_TITLE = "TabletCam Build";

        private Vector2 _scroll;

        private BuildTarget _buildTarget = BuildTarget.Android;
        private bool _buildAndRun = true;
        private bool _devBuild = false;
        private string _buildFolder = string.Empty;

        // opens build window.
        [MenuItem("Tools/TabletCam Build Window")]
        public static void OpenBuildWindow()
        {
            // try to dock next to Game window as I like it /H
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            var gameWindow = windows.FirstOrDefault(e => e.titleContent.text.Contains("Game"));

            TabletBuildWindow bsw;
            if (gameWindow != null)
            {
                bsw = GetWindow<TabletBuildWindow>(BUILD_WINDOW_TITLE, true, gameWindow.GetType());
            }
            else
            {
                bsw = GetWindow<TabletBuildWindow>(BUILD_WINDOW_TITLE, true);
            }

            bsw.Show();
        }

        private void OnEnable()
        {
            this.minSize = new Vector2(200, 17);

            LoadPrefs();
        }

        private void OnFocus()
        {
            LoadPrefs();
        }

        public void LoadPrefs()
        {
            if (string.IsNullOrWhiteSpace(_buildFolder))
            {
                _buildFolder = TabletBuildScriptPrefs.GetBuildPath();
            }

            _buildAndRun = TabletBuildScriptPrefs.GetBuildAndRun();
            _devBuild = TabletBuildScriptPrefs.GetDevelopmentBuild();
            _buildTarget = TabletBuildScriptPrefs.GetBuildTarget();
        }

        private void OnGUI()
        {
            _scroll = GUILayout.BeginScrollView(_scroll);

            // Build and run GUI, options buttons etc.
            //OnGUI_RenderBuildAndRunning();
            OnGUI_RenderSimpleOptions();

            EditorGUILayout.Separator();

            GUILayout.EndScrollView();
        }

        private void OnGUI_RenderSimpleOptions()
        {
            EditorGUILayout.LabelField(
                new GUIContent("Welcome to TabletCam Build Window, the best build tool in the world"),
                EditorStyles.centeredGreyMiniLabel);

            if (GUILayout.Button("Build For Tablet"))
            {
                BuildForTablet();
            }

            if (GUILayout.Button("Reset TabletCam Prefs"))
            {
                ResetTabletCamPrefs();
            }

            if (false)
                if (GUILayout.Button("WriteManifest Test"))
                {
                    TabletManifestoWriter.WriteTabletCamStuffToTheManifest(out var oldMan);
                    oldManifestoSave = oldMan;
                }

            if (false)
                if (GUILayout.Button("Revert Manifest"))
                {
                    TabletManifestoWriter.RevertManifest(oldManifestoSave);
                }

            //GUILayout.Space(4);

            if (false)
                if (GUILayout.Button("Manifest find it"))
                {
                    var p = TabletManifestoWriter.FindTabletManifestFile();
                    Debug.Log("Found " + p);
                }
        }


        // the gui for the advanced build options
        private void OnGUI_RenderBuildAndRunning()
        {
            EditorGUILayout.LabelField(
                new GUIContent("Building & Running", "Building and optionally auto-running builds."),
                EditorStyles.centeredGreyMiniLabel);

            EditorGUI.BeginChangeCheck();

            _buildTarget =
                (BuildTarget) EditorGUILayout.EnumPopup(new GUIContent("Build Target", "Which platform to build for."),
                    _buildTarget);

            if (EditorGUI.EndChangeCheck())
            {
                TabletBuildScriptPrefs.SetBuildTarget(_buildTarget);
            }

            EditorGUI.BeginChangeCheck();

            _devBuild = EditorGUILayout.Toggle(
                new GUIContent("Development Build", "Toggle to build as development build."), _devBuild);

            if (EditorGUI.EndChangeCheck())
            {
                TabletBuildScriptPrefs.SetDevelopmentBuild(_devBuild);
            }

            EditorGUI.BeginChangeCheck();
            _buildAndRun =
                EditorGUILayout.Toggle(
                    new GUIContent("Build and Run?", "Whether to auto-run the game after building, if successful."),
                    _buildAndRun);

            if (EditorGUI.EndChangeCheck())
            {
                TabletBuildScriptPrefs.SetBuildAndRun(_buildAndRun);
            }


            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Build"))
            {
                BuildButton_Singleplayer();
            }

            if (GUILayout.Button("Run"))
            {
                RunAsSingleplayer();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void BuildButton_Singleplayer()
        {
            if (!Directory.Exists(_buildFolder))
            {
                _buildFolder = SetBuildPath();
                base.Repaint();
            }

            if (_buildAndRun)
            {
                TabletBuildScriptOLD.BuildAndRunSingleplayer64("", _buildFolder, _devBuild, _buildTarget);
            }
            else
            {
                TabletBuildScriptOLD.BuildSingleplayer64("", _buildFolder, _devBuild, _buildTarget);
            }

            GUIUtility.ExitGUI();
        }

        private void RunAsSingleplayer()
        {
            TabletBuildScriptOLD.PlayAsSingleplayer("");
            GUIUtility.ExitGUI();
        }
    }
}