# AliDDNS
---  
____Archived____  
由于跨平台问题，考虑: [相同Java实现](https://github.com/keeptpa/AliDDNS_Java)
---


可以自动将解析地址指向软件所在主机的外网地址。  

---  
__USAGE__  

第一次运行会问询Access， 目前是明文存储的，这实在是太蠢了，一定会在不远的将来被修复。  
会生成两个配置文件，其中appConfigs储存了软件自身的设置，clientConfigs储存了解析记录的设置，单个设置如下：  

```
{
    "inControlling": true,            //是否由软件控制，默认值为false  
    "RR": "test",                     //RR，即主域名前缀  
    "RecordId": "73xxxxxxxx0029824", //该记录在阿里云的ID，请不要修改，除非你想得到如盘丝洞一样混乱的解析记录  
    "type": "A",                      //解析记录类型  
    "currentValue": "12.34.56.78",    //目前阿里云解析设置的解析目标
    "expectedValue": "98.76.54.32"    //期望的目标，请不要修改，不过改了也没用的
  }
```  
  
若要软件帮助你控制一条记录，则将`inControlling`置为true即可，不需其他设置。 

---
__WARNING__  
获取外部IP的方式是访问IP查询网址，因此请注意 __不要使用代理__，或在代理中放行`http://ipv4.icanhazip.com/`  
当然你也可以使用服务器搭建自己的查询response  
