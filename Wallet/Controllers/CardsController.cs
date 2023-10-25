using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Wallet.Models;

namespace Wallet.Controllers
{
    public class CardsController : Controller
    {
        const string server = "localhost";
        const string database = "Wallet";
        private string connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True", server, database);

        [HttpPost]
        public ActionResult Save(string Bank, string Brand, string Name, string CardNumber, int CVV, DateTime ExpirationDate)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@Bank",Bank),
                new SqlParameter("@Brand",Brand),
                new SqlParameter("@Name", Name),
                new SqlParameter("@CardNumber", CardNumber),
                new SqlParameter("@CVV", CVV),
                new SqlParameter("@ExpirationDate", ExpirationDate)

            };
            Database.DatabaseHelper.ExecuteNonQuery("SaveCard", param);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Edit(int id)
        {
            // Retrieve the card by its ID from your data source
            Card card = GetCard(id);// Implement the logic to retrieve the card

            if (card == null)
            {
                // Handle the case where the card with the provided ID was not found
                return NotFound();
            }

            Card model = new Card();
            model.Bank = "BAC.png";

            return View("Edit", card);
        }


        [HttpPost]
        public ActionResult Update(int id, string Bank, string Brand, string Name, string CardNumber, int CVV, DateTime ExpirationDate)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@ID",id),
                new SqlParameter("@Bank",Bank),
                new SqlParameter("@Brand",Brand),
                new SqlParameter("@Name",Name),
                new SqlParameter("@CardNumber",CardNumber),
                new SqlParameter("@CVV",CVV),
                new SqlParameter("@ExpirationDate",ExpirationDate)

            };
            Database.DatabaseHelper.ExecuteNonQuery("UpdateCard", param);


            return RedirectToAction("Index", "Home");

        }

        public Card GetCard(int Id)
        {
            Card card = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Cards WHERE ID = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", Id));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            card = new Card
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Bank = reader["bank"].ToString(),
                                Brand = reader["Brand"].ToString(),
                                Name = reader["Name"].ToString(),
                                CardNumber = Convert.ToInt64(reader["CardNumber"]),
                                CVV = Convert.ToInt32(reader["cvv"]),
                                ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"])
                            };
                        }
                    }
                }
            }

            return card;

        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {

            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("ID",Id)
            };

            Database.DatabaseHelper.ExecuteNonQuery("DeleteCard", param);

            return RedirectToAction("Index", "Home");

        }
        public IActionResult Add()
        {
            return View();
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
