# 关于Ligg.EasyWinApp
English | [简体中文](./README.zh-CN.md)
- History[History](https://www.cnblogs.com/liggin2019/p/11780431.html)
- Over all introduction[Over all introduction](https://www.cnblogs.com/liggin2019/p/11824064.html)
- Dev. and Application guide, based on version 3.5.2, currently under construction..[guide,](https://liggin2019.gitee.io/projguide)
- Current version: 3.0.2, will be upgraded to version 3.5.2. Version 3.5.2 has greatly enhanced the reusability of the interface and can realize all levels（View/Area/Zone/Control）
- 3.5.2 will be a long-term stable release.


## Introduction
### This solution  is a Windows application programming framework and UI library. By this framework, never need any code, only by XML file
- to build any complex Windows winform GUI,  console program input/output user interface;
- to implement basic process control (value assignment, conditional judgment, loop, jump, etc) in an Execel formular like manner; 
- to implement  basic functionalities (string/file basic functionality, logic judgment, mathematical operation, data input/output and input verification, data conversion, encryption/decryption, form field validation, etc.) in an Execel formular like manner; 
- to process Windows script and Python script (new feature in  version 3.52)
- to achieve specific business logic processing function.  by dynamically loading 'Plug and Play' .Net component or COM component (CBPL DLL) to 
- to supports multi-threading and multi-language.：

### Ligg.EasyWinform
Ligg.EasyWinform Is a Winform programming framework and UI library. It can excellent imitate the UI of  SAP GUI, 360 security guard software and Symantec endpoint client.

###  Ligg.EasyWinConsole
Ligg.EasyWinConsole is a Windows console programming framework, can be called by easywinform or used alone. It can be used for automation device debugging, software testing, configuration deployment of IT operation and maintenance , server of instant messaging / message queu, etc.
- It is not uploaded in this version, but will be uploaded in version 3.5.2.

## Development environment
- Microsoft Visual Studio 2017, version: 15.8.9
- Microsoft .NET Framework version: 4.6.01586

## Development/Test
#### Please go to the demo folder and run each case against the source code. As shown in the figure below
![case](https://liggin2019.gitee.io/Static/images/EasyWinApp/cases.png)

## Usage：
1. For automation equipment development, debugging, operation and maintenance, that is so called 'host computer development', which is also the starting point of this framework. 
-- For Embedded / hardware engineers, you do not need to write human-computer interaction code, only need to configure XML, through the built-in SerialConnector /SocketConnector /WebSocketConnector /OpcConnector /OpcUaConnector /MqttConnector interfaces to call the corresponding hardware control program (cbpl DLL), you can create a beautiful equipment operation and maintenance management system. 

2. For testing or prototype design in the process of software development.
-- No need to write test case code , only use the built-in HttpClientHandler for server-side test, and use the built-in JobScheduler&ThreadDispatcher carries out stress / robustness test; 
-- Generate prototype interface by configuration , to carry out deep communication and interaction among project manager, product manager, architect, user and programmer in requirements analysis stage, outline design stage, detailed design stage and development stage.

3. For configuration, deployment, monitoring of IT operation and maintenance automation, of course, is only for windows. For the  windows + UNIX, we have a solution based on zabbix/granfana.

4. For  tool software especially software with high security requirements
It can overcome the security weakness of browser-based front-end  (browser client is non blind), and realize various verification and encryption --by lisense dog, by permitting specific hosts/OS to run, by limitting running in specific LAN , by authorizing Windows user to run , request-response data encryption, etc.

5. As web client
-- never need any code, you can  customiz interface and form to  communication with server by Restful protocol through the encapsulation of httpclient
-- It is especially suitable for the front end such as MES or WMS, which needs to be connected to the device. After all, WinForm is easier to connect to the device than the browser based front end.


## License
[MIT](https://github.com/Liggin2019/Ligg.EasyWinApp/blob/master/LICENSE) license.
Copyright (c) 2019-present Liggin2019

## Running snapshot
#### login
![login](https://liggin2019.gitee.io/Static/images/EasyWinApp/login-en.png)
#### software cover
![software cover](https://liggin2019.gitee.io/Static/images/EasyWinApp/software-cover-en.png)
#### about
![about](https://liggin2019.gitee.io/Static/images/EasyWinApp/about-en.png)
#### main UI
![main UI](https://liggin2019.gitee.io/Static/images/EasyWinApp/main-ui-en.png)  
#### main UI 
![main UI1](https://liggin2019.gitee.io/Static/images/EasyWinApp/main-ui1-en.png)  