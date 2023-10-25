using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Wallet.Database;
using Wallet.Models;

namespace Wallet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var currentDate = DateTime.Now;
            var threeMonthsFromNow = currentDate.AddMonths(3);

            var cards = GetCards(); // Replace with your card retrieval logic

            foreach (var card in cards)
            {
                // Check if the ExpirationDate is within 3 months from the current date
                card.ShowBadge = card.ExpirationDate.HasValue && card.ExpirationDate.Value <= threeMonthsFromNow;
            }

            ViewBag.CardList = cards;

            return View();
        }
        public List<Card> GetCards()
        {
            List<Card> CardList = new List<Card>();
            DataTable ds = Database.DatabaseHelper.ExecuteQuery("GetCards", null);

            foreach (DataRow dr in ds.Rows)
            {
                CardList.Add(new Card
                {
                    Id = Convert.ToInt32(dr["ID"]),
                    Bank = dr["bank"].ToString(),
                    Brand = dr["Brand"].ToString(),
                    Name = dr["Name"].ToString(),
                    CardNumber = Convert.ToInt64(dr["CardNumber"]),
                    CVV = Convert.ToInt32(dr["cvv"]),
                    ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"])
                });
            }

            return CardList;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}