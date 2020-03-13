using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abc.Interface;
using Abc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Abc.ServiceInstance.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;

        private readonly IUserService _iUserService;


        private IConfiguration Configuration { get; }
        public UsersController(ILogger<UsersController> logger, IUserService iUserService, IConfiguration configuration)
        {
            _logger = logger;
            _iUserService = iUserService;
            Configuration = configuration;
        }

        /// <summary>
        /// 获取一个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            return _iUserService.FindUser(id);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IEnumerable<User> Get()
        {
            Console.WriteLine(Configuration["ttt"]);
            var tt= _iUserService.UserAll().ToList();

            tt.ForEach(s=>s.Role= Configuration["ttt"]);

            return tt;
        }
    }
}