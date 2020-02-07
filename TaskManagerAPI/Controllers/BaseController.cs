using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    public class BaseController : ApiController
    {
        protected ApplicationDbContext context;

        public BaseController()
        {
           if(context == null)
                context = new ApplicationDbContext();
        }
    }
}