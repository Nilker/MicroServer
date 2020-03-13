using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Abc.ServiceInstance.Ext
{
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            var consulClient = new ConsulClient(s =>
            {
                s.Address=new Uri("http://localhost:8500/");
                s.Datacenter = "dc1";
            });

            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);

            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "service"+Guid.NewGuid(),
                Name = "AbcService",
                Address = ip,
                Port = port,
                Tags = new string[] {weight.ToString()},
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12), //间隔12秒
                    HTTP = $"http://{ip}:{port}/api/health/index",
                    Timeout = TimeSpan.FromSeconds(5),   //检查等待时间
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)  //失败多久后移除
                }
            });

            Console.WriteLine($"注册啦--ip:{ip}---port:{port}");
        }
    }
}
