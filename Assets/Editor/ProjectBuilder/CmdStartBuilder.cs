using UnityEngine;
using System;
using XcodeBuilder;

public class CmdStartBuilder {

    public static void PreformBuild()
    {
        string commandLine = Environment.CommandLine;
        string[] commandLineArgs = Environment.GetCommandLineArgs();

        ProjectBuildData buildData = new ProjectBuildData();

        for (int i=0; i<commandLineArgs.Length; ++i)
        {
            if (commandLineArgs[i].Contains("="))
            {
                string[] argument = commandLineArgs[i].Split('=');

                string argumentName = argument[0];
                string argumentValue = argument[1];

                if (argumentName.Contains("version"))
                {
                    buildData.IdentityVersion = argumentValue;
                }

                if (argumentName.Contains("build"))
                {
                    buildData.IdentityBuild = argumentValue;
                }

                if (argumentName.Contains("channel"))
                {
                    buildData.Channel = PackageChannelStrToEnum (argumentValue);
                }

                if (argumentName.Contains("architecture"))
                {
                    buildData.Architecture = ArchitectureStrToEnum (argumentValue);
                }

                if (argumentName.Contains("scripting_implementation"))
                {
                    buildData.ScriptingImpl = ScriptingImplementationStrToEnum (argumentValue);
                }
            }
        }

        if (buildData.Integrity())
        {
            ProjectBuilder.BuildForiOS(buildData);
        }
    }

    private static PackageChannel PackageChannelStrToEnum(string channel)
    {
        if (channel == PackageChannel.DEBUG.ToString())
        {
            return PackageChannel.DEBUG;
        }
        else if (channel == PackageChannel.QA.ToString())
        {
            return PackageChannel.QA;
        }
        else if (channel == PackageChannel.Release.ToString())
        {
           return PackageChannel.Release;
        }
        else
        {
            Debug.LogError("channel value invalid!");
            return PackageChannel.DEBUG;
        }
    }

    private static ArchitectureType ArchitectureStrToEnum(string architecture)
    {
        if (architecture == ArchitectureType.None.ToString())
        {
            return ArchitectureType.None;
        }
        else if (architecture == ArchitectureType.ARM64.ToString())
        {
            return ArchitectureType.ARM64;
        }
        else if (architecture == ArchitectureType.Universal.ToString())
        {
            return ArchitectureType.Universal;
        }
        else 
        {
            Debug.LogError("Architecture type invalid!");
            return ArchitectureType.None;
        }
    }

    private static ScriptingImpl ScriptingImplementationStrToEnum(string scriptingImpl)
    {
        if (scriptingImpl == ScriptingImpl.Mono2x.ToString())
        {
            return ScriptingImpl.Mono2x;
        }
        else if (scriptingImpl == ScriptingImpl.IL2CPP.ToString())
        {
            return ScriptingImpl.IL2CPP;
        }
        else
        {
            Debug.Log("ScriptingImplementation invalid!");
            return ScriptingImpl.Mono2x;
        }
    }
}