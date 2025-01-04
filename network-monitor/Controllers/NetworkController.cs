using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using network_monitor.Helpers;
using network_monitor.Services;
using network_monitor.Models;
using System.Runtime.CompilerServices;

namespace network_monitor.Controllers
{
    public class NetworkController : Controller
    {
        private readonly SubnetCalcService _service;
        public NetworkController()
        {
            _service = new SubnetCalcService();
        }
        public ActionResult ArpResults()
        {
            // Call the utility method to get arp -a results
            string arpResults = NetworkUtils.ExecuteArpCommand();

            // Pass the results to the view
            ViewBag.ArpResults = arpResults;

            return View();
        }

        public ActionResult Ping()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PingResult(string ipAddress)
        {
            var model = new PingResultModel { IPAddress = ipAddress };

            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(ipAddress);

                    model.Success = reply.Status == IPStatus.Success;
                    model.Status = reply.Status.ToString();
                    model.RoundTripTime = reply.RoundtripTime;
                    model.ErrorMessage = "";
                }
            }
            catch (Exception ex)
            {
                model.Success = false;
                model.Status = "Error";
                model.ErrorMessage = ex.Message;
            }

            var pingResultsToDB = new PingResultsController { };
            pingResultsToDB.AddPingResult(model);
            return View(model);
        }

        public ActionResult SubnetCalc()
        {
            return View();
        }

  

        [HttpPost]
        public ActionResult SubnetCalcResult(string subnetMask, string ipAddress)
        {
            var model = new SubnetCalcModel { IPAddress = ipAddress, SubnetMask = subnetMask };
            try
            {
                model = _service.Calculate(model);
            }
            catch (Exception ex) {
                model = null;
            }
            
            return View(model);
        }
    }

}
