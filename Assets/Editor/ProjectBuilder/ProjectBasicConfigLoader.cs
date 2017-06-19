/**
 * Created by elang on 2017/6/19.
 */

using System;
using System.IO;
using UnityEngine;

namespace XcodeBuilder
{
	public class ProjectBasicConfigLoader
	{
		private static string ChannelToStr(PackageChannel channel)
		{
			switch (channel)
			{
				case PackageChannel.DEBUG:
					return "Debug";
				case PackageChannel.QA:
					return "QA";
				case PackageChannel.Release:
					return "Release";
				default:
					throw new ArgumentOutOfRangeException("channel", channel, null);
			}
		}

		private static string ClipConfigValue(string wrapConfig)
		{
			return wrapConfig.Split('=')[1];
		}

		public ProjectBasicConfig GetConfig(PackageChannel channel)
		{
			if (!File.Exists(BuildPaths.ProjectBasicConfigFilePath))
			{
				Debug.LogError("xcode basic config not found!");
				return null;
			}
		
			ProjectBasicConfig config = new ProjectBasicConfig();
		
			string[] signingConfig = File.ReadAllLines(BuildPaths.ProjectBasicConfigFilePath);

			string channelFlag = "channel="+ChannelToStr(channel);

			for (int i=0; i<signingConfig.Length; ++i)
			{
				string lineContent = signingConfig[i];

				if (!lineContent.Contains(channelFlag)) continue;
			
				while (!signingConfig[++i].Contains("code_signing_identity")){}
				config.CodeSigningIdentify = ClipConfigValue(signingConfig[i].Trim());

				while (!signingConfig[++i].Contains("provisioning_profile")) {}
				config.ProvisioningProfile = ClipConfigValue(signingConfig[i].Trim());

				while (!signingConfig[++i].Contains("provisioning_profile_specifier")){}
				config.ProvisioningProfileSpecifier = ClipConfigValue(signingConfig[i].Trim());

				while (!signingConfig[++i].Contains("team_id")) {}
				config.TeamId = ClipConfigValue(signingConfig[i].Trim());

				while (!signingConfig[++i].Contains("bundle_identity")) {}
				config.BundleIdentity = ClipConfigValue(signingConfig[i].Trim());
					
				while (!signingConfig[++i].Contains("display_name")) {}
				config.DisplayName = ClipConfigValue(signingConfig[i].Trim());
				
				config.Dump();

				return config;
			}

			return null;
		}
	}
}