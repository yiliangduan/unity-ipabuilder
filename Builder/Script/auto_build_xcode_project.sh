#!/bin/sh

#Created by elang on 2017/6/19.

#-------------使用说明--------------

usage() 
{ 
	echo "Usage: $0 [-v <string>] [-b <number>] [-c] <Debug|QA|Release>" 

	echo "[-v] Version    eg: 1.0.0"
  	echo "[-b] Build      eg: 100"
    echo "[-p] Xcode project absolute path"

    echo "[-m] Build Mode  Options: 1.Debug, 2.Release"

  	echo "[-c] Channel    Options: 1.Debug, 2.QA, 3.Release"

exit 1;}



unity_project_path=`pwd`

build_script_path=$unity_project_path"/Builder/Script/"
project_xcode_path=$unity_project_path"/Builder/iOSBuild/"
product_path=$unity_project_path"/Builder/Product/"

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
        *)
            #usage
            ;;
    esac
done
shift $((OPTIND-1))

if [ -z "${v}" ] || [ -z "${b}" ] || [ -z "${c}" ]; then
    usage
fi

build_version=${v}

build_number=${b}

build_channel=${c}

build_mode=${m}

if [ -z "${m}" ]; then 
    build_mode="Release" 
fi


echo "[version:"$build_version" build:"$build_number" channel:"$build_channel" mode:"$build_mode"]"


#--------------读取xcode_basic配置--------------

while read line; do

    key=`echo $line|awk -F '=' '{print $1}'`

    value=`echo $line|awk -F '=' '{print $2}'`

    case $key in

        "channel")
            channel=$value
        ;;

        "code_signing_identity")
            
            if [ $channel == $build_channel ]; then 
                code_signing_identity=$value
            fi
        ;;

        "provisioning_profile")
            if [ $channel == $build_channel ]; then
                provisioning_profile=$value
            fi
        ;;

        "bundle_identity")
            if [ $channel == $build_channel ]; then
                bundle_identity=$value
            fi
        ;;

        "display_name")
            if [ $channel == $build_channel ]; then
                display_name=$value
            fi
        ;;

        *)
        ;;
    esac

done < $build_script_path""xcode_basic.cfg

if [ -z "${channel}" ] ; then
    echo "The "$build_channel" channel for config does not contain the [channel] field!"
    exit 1;
fi

if [ -z "${code_signing_identity}" ]; then
    echo "The "$build_channel" channel for config does not contain the [code_signing_identity] field!"
    exit 1;
fi

if [ -z "${bundle_identity}" ]; then
    echo "The "$build_channel" channel for config does not contain the [bundle_identity] field!"
    exit 1;
fi

echo "[channel:"$build_channel"  code_signing_identity:"$code_signing_identity" bundle_identity:"$bundle_identity"]"

echo "[-------check environment complete------]"



#---------------构建Xcode工程---------------



product_name=${bundle_identity##*.}

project_target="Unity-iPhone"

xcode_project_filepath=$project_xcode_path$project_target".xcodeproj"
package_out_path=$project_xcode_path"build/Release-iphoneos/"

app_package_path=$package_out_path$product_name".app"

if [ ! -d "$app_package_path" ]; then
    rm -rf $app_package_path
fi


#xcodebuild  clean

xcodebuild  -project $xcode_project_filepath \
            -configuration $build_mode \
            -target $project_target \
            PROVISIONING_PROFILE="$provisioning_profile" \
            CODE_SIGN_IDENTITY="$code_signing_identity" \
            PRODUCT_BUNDLE_IDENTIFIER=$bundle_identity \
            PRODUCT_NAME=$product_name \
            DISPLAY_NAME=$display_name


echo "[-------build project complete------]"



#---------把Xcode生成的app包压缩成ipa包-----------

cur_folder_path=`pwd`

cur_time=`date "+%Y%m%d%H%M"`


package_unique_flag=$display_name"_"$build_version"_"$build_number"_"$cur_time
package_unique_path=$package_out_path$package_unique_flag".ipa"

xcrun -sdk iphoneos PackageApplication -v $app_package_path -o $package_unique_path

mv -v $package_unique_path $product_path"ipa/"$package_unique_flag".ipa"

mv -v $app_package_path".dSYM" $product_path"dSYM/"$package_unique_flag".app.dSYM"

echo "remove "$app_package_path
rm -rf $app_package_path

cd $pre_compile_path

echo "[ output => "$package_unique_path"]"

echo "[-------app compress to ipa complete------]"
