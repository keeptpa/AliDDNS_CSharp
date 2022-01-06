using AlibabaCloud.SDK.Alidns20150109;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliDDNS
{
    class ConfigModule
    {
        internal AppConfig aconfig;


        public static Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config cconfig = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 您的AccessKey ID
                AccessKeyId = accessKeyId,
                // 您的AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            cconfig.Endpoint = "alidns.cn-hongkong.aliyuncs.com";

            return new Client(cconfig);
        }

    }

    class AppConfig
    {
        public AppConfig(string apppath)
        {
            this.appPath = apppath;
        }
        internal string appPath;
        internal string configPath;
        [JsonProperty]
        internal string accessKey;
        [JsonProperty]
        internal string accessSecret;
        [JsonProperty]
        internal string domainName;
    }
    
    //每一对解析的配置类，会被序列化
    class DNSConfig
    {
        //是否由程序管理
        [JsonProperty]
        internal bool inControlling = false;
        //储存解析记录ID
        [JsonProperty]
        internal string RR;
        [JsonProperty]
        internal string RecordId;
        [JsonProperty]
        internal string type;
        //目前与将设置的值，若不由程序管理，则将设置的值为空
        [JsonProperty]
        internal string currentValue;
        [JsonProperty]
        internal string expectedValue;
        
    }


}
