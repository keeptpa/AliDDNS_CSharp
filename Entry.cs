using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using static AlibabaCloud.SDK.Alidns20150109.Models.DescribeDomainRecordsResponseBody.DescribeDomainRecordsResponseBodyDomainRecords;

namespace AliDDNS
{
    class Entry
    {
        static void Main()
        {
            //设定目录环境
            ConfigModule cm = new ConfigModule();
            cm.aconfig = new AppConfig(AppDomain.CurrentDomain.BaseDirectory);
            cm.aconfig.configPath = cm.aconfig.appPath + "config\\";
            if (!Directory.Exists(cm.aconfig.configPath))
            {
                Directory.CreateDirectory(cm.aconfig.configPath);
            }


            //查询本机外部IP
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead("http://ipv4.icanhazip.com/");
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();

            string externalIP = strHTML.Substring(0, strHTML.IndexOf("\n"));

            //打开Log
            LogModule.Log("Application trigged running ..");


            //如果没有就添加配置文件
            if (!File.Exists(cm.aconfig.configPath + "appSettings.json"))
            {
                LogModule.Log("App config not found, create one.");
                
                    

                //问询用户，添加配置文件
                Console.WriteLine("Please input your accessKey:");
                cm.aconfig.accessKey = Console.ReadLine();
                Console.WriteLine("Please input your accessSecretKey:");
                cm.aconfig.accessSecret = Console.ReadLine();
                Console.WriteLine("Please input the domain name you want application to control:");
                cm.aconfig.domainName = Console.ReadLine();
                //日后这里要加密
                string point = JsonConvert.SerializeObject(cm.aconfig, Formatting.Indented);

                using (File.Create(cm.aconfig.configPath + "appSettings.json"))
                {

                }
                StreamWriter sw = new StreamWriter(cm.aconfig.configPath + "appSettings.json", true);
                sw.WriteLine(point);
                sw.Flush();
                sw.Dispose();
                LogModule.Log("App settings created.");

                //查询一次后退出程序，以留下json让用户更改
                using (File.Create(cm.aconfig.configPath + "clientSettings.json")) {

                }
                
                List<DescribeDomainRecordsResponseBodyDomainRecordsRecord> queryAnswer =  QueryModule.Query(cm.aconfig.domainName, cm.aconfig.accessKey, cm.aconfig.accessSecret).Body.DomainRecords.Record;
                List<DNSConfig> dnsList = new List<DNSConfig>();
                foreach(var item in queryAnswer)
                {
                    DNSConfig temp = new DNSConfig
                    {
                        currentValue = item.Value,
                        RR = item.RR,
                        RecordId = item.RecordId,
                        type = item.Type
                    };
                    dnsList.Add(temp);
                }
                StreamWriter sw2 = new StreamWriter(cm.aconfig.configPath + "clientSettings.json", true);
                sw2.WriteLine(JsonConvert.SerializeObject(dnsList,Formatting.Indented));
                sw2.Flush();
                sw2.Dispose();
                LogModule.Log("Client settings created.");
                Process.GetCurrentProcess().Kill();
            }

            //读取配置文件

            string config = File.ReadAllText(cm.aconfig.configPath + "appSettings.json");


            //日后加密了，这里则需要解密
            cm.aconfig = JsonConvert.DeserializeObject<AppConfig>(config);
            cm.aconfig .appPath = AppDomain.CurrentDomain.BaseDirectory;
            cm.aconfig.configPath = cm.aconfig.appPath + "config\\";

            if (cm.aconfig == null)
            {
                Console.WriteLine("WARN: App configs corrputed.");
                File.Delete(cm.aconfig.configPath + "clientSettings.json");
                Process.GetCurrentProcess().Kill();
            }

            LogModule.Log("App config found.");

            List<DescribeDomainRecordsResponseBodyDomainRecordsRecord> queryAnswer2 = QueryModule.Query(cm.aconfig.domainName, cm.aconfig.accessKey, cm.aconfig.accessSecret).Body.DomainRecords.Record;
            //这是本次查询到的解析记录
            List<DNSConfig> dnsList2 = new List<DNSConfig>();
            //这是本地保存的解析记录，包含是否控制的布尔值
            List<DNSConfig> dnsList3 = new List<DNSConfig>();

            //读取本地解析记录
            string dnsconfig = File.ReadAllText(cm.aconfig.configPath + "clientSettings.json");
            dnsList3 = JsonConvert.DeserializeObject<List<DNSConfig>>(dnsconfig);

            foreach (var item in queryAnswer2)
            {
                DNSConfig temp = new DNSConfig
                {
                    currentValue = item.Value,
                    RR = item.RR,
                    RecordId = item.RecordId,
                    type = item.Type
                };

                foreach(var dns in dnsList3)
                {
                    if(dns.RecordId == temp.RecordId)
                    {
                        temp.inControlling = dns.inControlling;
                        //如果是由程序控制，那么开始查询此刻的本机外部地址填入
                        if (dns.inControlling)
                        {
                            temp.expectedValue = externalIP;
                        }
                        
                    }
                }

                dnsList2.Add(temp);

            }



            //覆盖clientConfig
            StreamWriter sw3 = new StreamWriter(cm.aconfig.configPath + "clientSettings.json", false);
            sw3.WriteLine(JsonConvert.SerializeObject(dnsList2, Formatting.Indented));
            sw3.Flush();
            sw3.Dispose();
            LogModule.Log("Replaced client config.");



            //开始更改解析
            ModifyModule ml = new ModifyModule();
            ml.Modify(cm.aconfig.accessKey,cm.aconfig.accessSecret,dnsList2);


        }
    }
}
