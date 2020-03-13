using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Abc.ClientDemo.Models;
using Abc.Interface;
using Abc.Model;
using Consul;

namespace Abc.ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUserService _iUserService;

        public HomeController(ILogger<HomeController> logger, IUserService iUserService)
        {
            _logger = logger;
            _iUserService = iUserService;
        }

        public IActionResult Index()
        {
            //优化
            //ViewBag.Users = _iUserService.UserAll();

            //优化2
            //var url = "http://localhost:5726/api/users/all";
            //ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(InvokeApi(url));

            //优化3

            var url = "http://AbcService/api/users/all";
            Uri uri = new Uri(url);
            string groupName = uri.Host;

            ConsulClient clinet = new ConsulClient(s =>
              {
                  s.Address = new Uri("http://localhost:8500/");
                  s.Datacenter = "dc1";
              });

            var response = clinet.Agent.Services().Result.Response;


            //foreach (var item in response.Where(s=>s.Value.Service.Equals(groupName,StringComparison.OrdinalIgnoreCase)))
            //{
            //    Console.WriteLine($"{item.Key}-------{item.Value}-----{Newtonsoft.Json.JsonConvert.SerializeObject(item)}");
            //}

            var dic = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

            //负载均衡策略
            //1.随机访问
            var agentSer = dic[new Random(DateTime.Now.Millisecond).Next(0, dic.Count())].Value;

            //2.轮询策略
            agentSer = dic[iSeed++ % dic.Length].Value;

            //3.权重策略
            var pairList = new List<KeyValuePair<string, AgentService>>();
            foreach (var pair in dic)
            {
                int count = int.Parse(pair.Value.Tags?[0]);
                for (int i = 0; i < count; i++)
                {
                    pairList.Add(pair);
                }
            }

            agentSer = pairList.ToArray()[new Random(iSeed++).Next(0, pairList.Count())].Value;

            url = $"{uri.Scheme}://{agentSer.Address}:{agentSer.Port}/api/users/all";


            ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(InvokeApi(url));
            return View();
        }

        private static int iSeed = 0;
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestMessage = new HttpRequestMessage();
                requestMessage.Method = HttpMethod.Get;
                requestMessage.RequestUri = new Uri(url);

                var result = httpClient.SendAsync(requestMessage).Result.Content.ReadAsStringAsync().Result;
                return result;

            }
        }
    }
}
