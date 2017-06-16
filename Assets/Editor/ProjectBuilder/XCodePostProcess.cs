using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XCodeEditor;
#endif
using System.IO;

namespace XcodeBuilder
{
	public static class XCodePostProcess
	{

#if UNITY_EDITOR
		public static void OnPostProcessBuild(BuildTarget target, string xcodeProjectPath, PackageChannel channel, ProjectBuildData data)
		{
			if (target != BuildTarget.iOS) {
				Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
				return;
			}

			// Create a new project object from build target
			XCProject project = new XCProject(xcodeProjectPath + "Unity-iPhone.xcodeproj");

			// Find and run through all projmods files to patch the project.
			// Please pay attention that ALL projmods files in your project folder will be excuted!
			string[] files = Directory.GetFiles( Application.dataPath, "*.projmods", SearchOption.AllDirectories );
			foreach( string file in files ) {
				UnityEngine.Debug.Log("ProjMod File: "+file);
				project.ApplyMod( file );
			}

			ProjectBasicConfig config = new ProjectBasicConfigLoader().GetConfig(channel);
		
			OverWriteBuildSetting(config, project);
		
			OverWriteInfoPlist(config, xcodeProjectPath, data);
		
			project.overwriteBuildSetting ("ENABLE_BITCODE","NO");

			// Finally save the xcode project
			project.Save();
		}


		//TODO implement generic settings as a module option
		private static void OverWriteBuildSetting(ProjectBasicConfig config, XCProject project)
		{
			project.overwriteBuildSetting("CODE_SIGN_IDENTITY", config.CodeSigningIdentify, "Debug");
			project.overwriteBuildSetting("CODE_SIGN_IDENTITY", config.CodeSigningIdentify, "Release");
		
			project.overwriteBuildSetting("PROVISIONING_PROFILE", config.ProvisioningProfile, "Release");
			project.overwriteBuildSetting("PROVISIONING_PROFILE", config.ProvisioningProfile, "Debug");
			project.overwriteBuildSetting("PROVISIONING_PROFILE_SPECIFIER", config.ProvisioningProfileSpecifier, "Debug");
			project.overwriteBuildSetting("PROVISIONING_PROFILE_SPECIFIER", config.ProvisioningProfileSpecifier, "Release");

			project.overwriteBuildSetting("DEVELOPMENT_TEAM", config.TeamId, "Release");
			project.overwriteBuildSetting("DEVELOPMENT_TEAM", config.TeamId, "Debug");

			project.overwriteBuildSetting("PRODUCT_BUNDLE_IDENTIFIER", config.BundleIdentity);

			string[] bundleIdentifyParts = config.BundleIdentity.Split('.');
			project.overwriteBuildSetting("PRODUCT_NAME", bundleIdentifyParts[bundleIdentifyParts.Length-1]);

			project.overwriteBuildSetting ("ENABLE_BITCODE","NO");
		}


		private static void OverWriteInfoPlist(ProjectBasicConfig config, string xcodeProjectPath, ProjectBuildData data)
		{
			XCPlist plist = new XCPlist(xcodeProjectPath + "Info.plist");

			Dictionary<string, object> configItems = new Dictionary<string, object>
			{
				{XCPlist.CFBundleDisplayName, config.DisplayName},
				{XCPlist.CFBundleShortVersionString, data.IdentityVersion},
				{XCPlist.CFBundleVersion, data.IdentityBuild}
			};

			plist.OverWriteItems(configItems);
		}
#endif
	}
}