/**
 * Created by elang on 2017/6/19.
 */


using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR


namespace XcodeBuilder
{
    public static class ProjectBuilder {

        public static ProjectBuildData BuildData;

        private static System.Diagnostics.Process m_Process;

        public static void BuildForiOS(ProjectBuildData data)
        {
            ExportForiOS(data);
        
            BuildiOSPorject();
        }

        public static void ExportForiOS(ProjectBuildData data)
        {
            if (Directory.Exists(BuildPaths.ProjectPath))
            {
                Directory.Delete(BuildPaths.ProjectPath, true);
            }

            if (data.ScriptingImpl == ScriptingImpl.Mono2x)
            {
                BuildForiOSBaseOnMono();
            }
            else
            {
                if (data.Architecture == ArchitectureType.ARM64)
                {
                    BuildForiOSBaseOnIL2CPP64();
                }
                else
                {
                    BuildForiOSBaseOnIL2CPP32_64();
                }
            }

            XCodePostProcess.OnPostProcessBuild(BuildTarget.iOS, BuildPaths.ProjectPath, data.Channel, data);
        }

        private static void BuildForiOSBaseOnMono()
        {
            BuildOptions buildOptions = BuildOptions.None;

            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);

            ExportIOSProjectInteral(buildOptions);
        }

        private static void BuildForiOSBaseOnIL2CPP64()
        {
            BuildOptions buildOptions = BuildOptions.None;

            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            // 0 - None, 1 - ARM64, 2 - Universal
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 1);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            ExportIOSProjectInteral(buildOptions);
        }

        private static void BuildForiOSBaseOnIL2CPP32_64()
        {
            BuildOptions buildOptions = BuildOptions.None;

            buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            // 0 - None, 1 - ARM64, 2 - Universal
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, 2);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);

            ExportIOSProjectInteral(buildOptions);
        }

        private static void ExportIOSProjectInteral(BuildOptions options)
        {
            options |= BuildOptions.SymlinkLibraries;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = GetBuildScenes(),
                locationPathName = BuildPaths.ProjectFolderName,
                target = BuildTarget.iOS,
                options = options
            };


            BuildStaticData.Step = BuildStepDesc.ExportXcodeProject;

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        private static string[] GetBuildScenes()
        {
            int buildScenesLength = EditorBuildSettings.scenes.Length;
            string[] buildScenes = new string[buildScenesLength];

            for (int i=0; i<buildScenesLength; ++i)
            {
                buildScenes[i] = EditorBuildSettings.scenes[i].path;
            }

            return buildScenes;
        }

        public static void BuildiOSPorject()
        {
            BuildStaticData.Step = BuildStepDesc.BuildXcodeProject;

            if(File.Exists(BuildPaths.ProjectBuildScriptPath))
            {
                RunShellScript(BuildPaths.ProjectBuildScriptPath, "1.0.0", 100, "QA");
            }
            else
            {
                Debug.LogWarning("not found [" + BuildPaths.ProjectBuildScriptPath + "] file!");
            }
        }

        private static void RunShellScript(string scriptFileName, string version, int build, string channel)
        {
            string argument = string.Format("-v {0} -b {1} -c {2}", version, build, channel);

            m_Process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = scriptFileName,
                    Arguments = argument,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            m_Process.Start ();

            while (!m_Process.StandardOutput.EndOfStream)
            {
                Debug.Log(m_Process.StandardOutput.ReadLine());
            }

            m_Process.WaitForExit();
            m_Process.Close();
        }
    }
}

#endif
