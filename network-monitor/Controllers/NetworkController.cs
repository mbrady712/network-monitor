using Microsoft.AspNetCore.Mvc;
using network_monitor.Helpers;

namespace network_monitor.Controllers
{
    public class NetworkController : Controller
    {
        public ActionResult ArpResults()
        {
            // Call the utility method to get arp -a results
            string arpResults = NetworkUtils.ExecuteArpCommand();

            // Pass the results to the view
            ViewBag.ArpResults = arpResults;

            return View();
        }
    }

}
