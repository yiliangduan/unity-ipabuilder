

namespace XcodeBuilder
{
    public static class BuildStaticData
    {
        public static string Step = BuildStepDesc.Ready;
    }

    [System.Serializable]
    public enum ArchitectureType
    {
        None,
        ARM64,
        Universal
    }


    [System.Serializable]
    public enum ScriptingImpl
    {
        Mono2x,
        IL2CPP
    }

    [System.Serializable]
    public class ProjectBuildData {

        public ScriptingImpl ScriptingImpl = ScriptingImpl.Mono2x;

        public ArchitectureType Architecture = ArchitectureType.None;

        public string IdentityVersion;

        public string IdentityBuild;

        public PackageChannel Channel;
    }
}