/**
 * Created by elang on 2017/6/19.
 */

using UnityEngine;

namespace XcodeBuilder
{
	public class ProjectBasicConfig
	{
		public string CodeSigningIdentify;

		public string ProvisioningProfile;

		public string ProvisioningProfileSpecifier;

		public string BundleIdentity;
		public string DisplayName;

		public string TeamId;

		public void Dump()
		{
			Debug.Log("CodeSigningIdentify: " + CodeSigningIdentify +
			          "\nProvisioningProfile: " + ProvisioningProfile + 
			          "\nProvisioningProfileSpecifier: " + ProvisioningProfileSpecifier +
			          "\nBundleIdentity: " + BundleIdentity + 
			          "\nDisplayName: " + DisplayName +
			          "\nTeamId: " + TeamId);
		}
	}
	
}