# 关于Ligg.EasyWinApp
简体中文 | [English](./README.md)
- [开发过程](https://www.cnblogs.com/liggin2019/p/11780431.html)
- [整体介绍](https://www.cnblogs.com/liggin2019/p/11824064.html)
- [开发应用手册](https://liggin2019.gitee.io/projguide)（基于3.5.2版本，目前正在建设中...）
- 当前版本: 3.2.1，即将升级至3.5.2版本，3.5.2版本在界面的重用性方面有重大改善，可以实现各个层次（View/Area/Zone/Controlle）的重用。
- 3.5.2将是一个长期稳定版本。
## 介绍
### 本解决方案是一个Windows应用编程框架和UI库，包括2部分Ligg.EasyWinform和Ligg.EasyWinConsole，通过该框架，不需任何代码，仅通过XML配置文件，可以：
- 搭建任意复杂的Windows图形应用界面，控制台界面的输入输出界面；
- 以类似Execel公式的方式实现基本的过程控制(赋值、条件判断、循环、跳转等)；
- 以类似Execel公式的方式实现基本功能(字符/文件处理、逻辑运算、数学运算、数据输入输出、数据格式转换、加密解密、表单字段验证等)；
- Windows脚本执行、Python脚本执行(在version 3.5.2实现)
- 动态加载“即插即用“的.Net组件或COM组件(CBPL DLL)实现特定的业务处理功能；
- 支持多线程、多语言。

### Ligg.EasyWinForm
Ligg.EasyWinForm是一个Winform应用编程框架和UI库。完全可以高仿SAP GUI，360安全卫士系列软件，Symantec Endpoint 客户端的界面。

###  Ligg.EasyWinConsole
Ligg.EasyWinConsole是一个Windows控制台应用编程框架，可以由EasyWinform调用或单独使用，可用于设备调试、软件测试、IT 运维配置部署、即时通讯/消息队列的服务端等。
- 在本版本没有上传，将在3.5.2版本统一上传。

## 开发环境
- Microsoft Visual Studio 2017, version: 15.8.9
- Microsoft .NET Framework version: 4.6.01586

## 开发/测试
#### 请至Demo文件夹，对照源码，运行各用例。如下图
![用例](https://liggin2019.gitee.io/Static/images/EasyWinApp/cases.png)

## 用途：
1. 自动化设备开发、调试、运维，上位机开发，这也是本框架的始发点。嵌入式/硬件工程师不需编写人机交互代码，只需配置Xml，通过内置的SerialConnector/SocketConnector/WebSocketConnector/OpcConnector/OpcUaConnector/MqttConnector接口调用对应的硬件控制程序(CBPL DLL)，可以创建界面精美的设备运维管理系统。

2. 软件开发过程中的测试和原型设计。不需编写测试用例代码，利用内置的HttpClientHandler进行服务器端测试；利用内置的jobScheduler / ThreadDispatcher进行压力/鲁棒测试；配置产生原型界面在需求分析阶段、概要设计阶段、详细设计阶段、开发阶段在项目经理、产品经理、架构师、用户、程序员之间深度沟通和交互。

3.  IT运维自动化的配置部署监控，当然只是对windows, 对于全系统Windows+unix， 有一套基于zabbix/granfana 的方案。

4. 工具类软件，安全性要求高的软件，利用对操作系统资源调用方便，克服服浏览器前端安全性方面的弱点（浏览器客户端是非盲视的），实现各种验证、加密（软件狗、限定特定主机、限定局域网运行、限定特定Windows用户运行、request-reponse的数据加密等）。

5. web客户端，通过封装的HttpClient，不需任何代码，实现定制界面和表单与Restful接口通讯。尤其适用MES或WMS这类需要连接设备的系统的前端，毕竟Winform与基于浏览器的前端相比更加容易连接设备。

## License
[MIT](https://github.com/Liggin2019/Ligg.EasyWinApp/blob/master/LICENSE) license.
Copyright (c) 2019-present Liggin2019

## 示例截图
#### 登录
![登录](https://liggin2019.gitee.io/Static/images/EasyWinApp/login-cn.png)
#### 软件封面
![软件封面](https://liggin2019.gitee.io/Static/images/EasyWinApp/software-cover-cn.png)
#### 关于
![软件封面](https://liggin2019.gitee.io/Static/images/EasyWinApp/about-cn.png)
#### 主界面
![登录](https://liggin2019.gitee.io/Static/images/EasyWinApp/main-ui-cn.png)  
#### 主界面1
![登录](https://liggin2019.gitee.io/Static/images/EasyWinApp/main-ui1-cn.png)  