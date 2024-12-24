using Microsoft.AspNetCore.Mvc;
using network_monitor.Services;
using network_monitor.Models;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;

namespace network_monitor.Controllers
{
    public class PingResultsController : Controller
    {
        private readonly PingResultService _service;

        public PingResultsController()
        {
            _service = new PingResultService();
        }

        [HttpPost]
        public IActionResult AddPingResult(PingResultModel pingResult)
        {
            _service.SavePingResult(pingResult);
            return Ok("Ping result saved.");
        }

        [HttpGet]
        public IActionResult GetPingResults()
        {
            var results = _service.GetPingResults();
            return Json(results);
        }
    }
}
