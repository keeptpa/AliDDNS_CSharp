using System;
using System.Collections.Generic;
using AlibabaCloud.SDK.Alidns20150109;
using AlibabaCloud.SDK.Alidns20150109.Models;

namespace AliDDNS
{
    class ModifyModule
    {
        private Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 您的AccessKey ID
                AccessKeyId = accessKeyId,
                // 您的AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "alidns.cn-hongkong.aliyuncs.com";
            return new Client(config);
        }

        public void Modify(string accessKeyId, string accessKeySecret,List<DNSConfig> config)
        {
            Client client = CreateClient(accessKeyId, accessKeySecret);

            foreach(var item in config)
            {
                if (item.inControlling && item.currentValue != item.expectedValue)
                {
                    UpdateDomainRecordRequest updateDomainRecordRequest = new UpdateDomainRecordRequest
                    {
                        RecordId = item.RecordId,
                        RR = item.RR,
                        Type = item.type,
                        Value = item.expectedValue,
                    };

                    UpdateDomainRecordResponse res = client.UpdateDomainRecord(updateDomainRecordRequest);
                    LogModule.Log(string.Format("RecordID:{0} has been changed from {1} to {2}, type {3}", item.RecordId,item.currentValue,item.expectedValue,item.type));
                }
            }
            
        }

    }
}
