#!/bin/sh

#Created by elang on 2017/6/19.

#-------------使用说明--------------

usage() 
{ 
	echo "Usage: $0 [-v] <string> [-b] <number> [-c] <Debug|QA|Release> -[p Option] <path> [-m Option] <Mode> [-a Option] <Architecture> [-s Option] <ScriptingImplementation>" 

	echo "[-v] Version    eg: 1.0.0"
  	echo "[-b] Build      eg: 100"
    echo "[-p] Xcode project absolute path. default is \"pwd/../\" "

    echo "[-c] Channel    Options: 1.Debug, 2.QA, 3.Release"

    echo "[-m] Build Mode  Options: 1.Debug, 2.Release"

  	echo "[-a] Architecture Options: 1.None, 2.ARM64 3.Universal; (None is use to Mono2x platform)"

  	echo "[-s] ScriptingImplementation Options: 1.Mono2x, 2.IL2CPP"

exit 1;}


#-----------读取命令参数-------------

while getopts ":v:b:c:p:m:" o; do
    case "${o}" in
        v)
            v=${OPTARG}
            
            ;;
        b)
            b=${OPTARG}
            ;;
        c)
			c=${OPTARG}

			((c == "Debug" || c == "QA" || c == "Release" )) || usage
			;;
        p)
            p=${OPTARG}
            ;;
        m)
            m=${OPTARG}
            ;;
        a)
			a=${OPTARG}
			;;
		s)
			s=${OPTARG}
			;;
        *)
            #usage
            ;;
    esac
done
shift $((OPTIND-1))

if [ -z "${v}" ] || [ -z "${b}" ] || [ -z "${c}" ]; then
    usage
    exit 1
fi

if [ -z "${p}" ]; then
	project_path=`pwd`"/../../"
else
	project_path=${p}
fi

if [ -z "${m}" ]; then
	build_mode=Debug
else
	build_mode=${m}
fi

if [ -z "${a}" ]; then
	architecture=None
fi

if [ -z "${s}" ]; then
	scripting_implementation=Mono2x
	architecture=None
fi

#---静默启动Unity，调用我们的打包的方法----

log_file_path=../Product/log/

if [ ! -d $log_file_path ]; then
	mkdir -p $log_file_path
fi

log_file_path=$log_file_path"build_ipa_log_"`date "+%Y%m%d%H%M"`".txt"

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	 -batchmode \
	 -projectPath $project_path \
	 -nographics \
	 -executeMethod CmdStartBuilder.PreformBuild version=${v} build=${b} channel=${c} mode=$build_mode architecture=$architecture scripting_implementation=$scripting_implementation \
	 -logFile $log_file_path \
	 -quit 


#-------end-------