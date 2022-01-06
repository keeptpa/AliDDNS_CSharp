using System;
using System.Collections.Generic;
using System.Text;

using Tea;
using Tea.Utils;
using AlibabaCloud.SDK.Alidns20150109;
using AlibabaCloud.SDK.Alidns20150109.Models;

namespace AliDDNS
{
    class QueryModule
    {
        private static Client CreateClient(string accessKeyId, string accessKeySecret)
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

            return new AlibabaCloud.SDK.Alidns20150109.Client(config);
        }

        public static DescribeDomainRecordsResponse Query(string domain,string key,string secret)
        {
            Client client = CreateClient(key,secret);

            DescribeDomainRecordsRequest describeDomainRecordsRequest = new DescribeDomainRecordsRequest
            {
                DomainName = domain,
            };
            DescribeDomainRecordsResponse resp = client.DescribeDomainRecords(describeDomainRecordsRequest);

            //resp.Body.
            return (resp);

            //AlibabaCloud.TeaConsole.Client.Log();
        }
    }
}
