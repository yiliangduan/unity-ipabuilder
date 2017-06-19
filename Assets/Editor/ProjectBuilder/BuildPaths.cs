/**
 * Created by elang on 2017/6/19.
 */

using UnityEngine;

namespace XcodeBuilder
{
    public class BuildPaths
    {
        public static readonly string ProjectPath = Application.dataPath + "/../Builder/iOSBuild/";

        public static readonly string ProjectBuildScriptPath = Application.dataPath + "/../Builder/Script/auto_build_xcode_project.sh";

        public static readonly string ProjectFolderName = "Builder/iOSBuild";

        public static readonly string ProjectBasicConfigFilePath =  Application.dataPath+"/../Builder/Script/xcode_basic.cfg";
    }

}