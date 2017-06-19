## UnityIPABuilder

1. 首先配置好BuildScript目录下的xcode_basic.cfg配置文件

2. 脚本打包，根据自己传入的参数打包。例如:

```
./cmd_start_unity_ipa_builder.sh -v 1.0.0 -b 100 -c QA

```
执行完毕之后，ipa文件和dSYM文件存放在项目目录下的 **iOSBuild/build/Release-iphoneos**目录下。日志文件存放在**iOSBuild/build/log**目录下。

3. Editor模式下打包，打开 Menu [Window] -> [Package Build Window] 选择好自己需要配置的参数，然后点击*build*即可build生成ipa包，输出文件的存放路径同2。