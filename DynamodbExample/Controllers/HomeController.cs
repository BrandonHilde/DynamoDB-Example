using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DynamodbExample.Models;
using Microsoft.Extensions.Options;

namespace DynamodbExample.Controllers
{
    
    public class HomeController : Controller
    {
        ConfigSettings.DynamoDBSettings _DynamoSettings = new ConfigSettings.DynamoDBSettings();

        public HomeController(IOptions<ConfigSettings.DynamoDBSettings> dbSettings)
        {
            _DynamoSettings = dbSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(ContactModel model)
        {
            DynamoManager dm = new DynamoManager(_DynamoSettings, Amazon.RegionEndpoint.USEast2);

            if(model != null)
            {
               var result = await dm.PutContactModel(model);
            }

            return View();
        }
    }
}