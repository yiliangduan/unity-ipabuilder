
using UnityEngine;

namespace XcodeBuilder
{
    public class BuildPaths
    {
        public static readonly string ProjectPath = Application.dataPath + "/../iOSBuild/";

        public static readonly string ProjectBuildScriptPath = Application.dataPath + "/../BuildScript/auto_build_xcode_project.sh";

        public static readonly string ProjectFolderName = "iOSBuild";

        public static readonly string ProjectBasicConfigFilePath =  Application.dataPath+"/../BuildScript/xcode_basic.cfg";
    }

}