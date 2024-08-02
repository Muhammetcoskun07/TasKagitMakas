using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using TasKagitMakas.Models;
using Dapper;

namespace TasKagitMakas.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Server=45.84.189.34\\MSSQLSERVER2019;Initial Catalog=muham128_TkmOyun;User Id=muham128_TkmOyundb;Password=522848Aa.;TrustServerCertificate=True";


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ScoreModel GetGameScore()
        {
            using var connection = new SqlConnection(connectionString);
            var score = connection.QueryFirstOrDefault<ScoreModel>("SELECT * FROM Tkm");
            return score;
        }

        public IActionResult Index()
        {
            return View(GetGameScore());
        }

        public IActionResult Oyna(int id)
        {
            string[] hamleler = ["taþ", "kaðýt", "makas"];

            Random random = new Random();
            int cpuHamle = random.Next(0, hamleler.Length);

            var score = GetGameScore();

            ViewBag.Cpu = hamleler[cpuHamle];
            ViewBag.Oyuncu = hamleler[id];
            if(id == cpuHamle)
            {
                ViewBag.Kazanan = "berabere";
            } else if(
                    (id == 0 && cpuHamle == 2) ||
                    (id == 2 && cpuHamle == 1) ||
                    (id == 1 && cpuHamle == 0) 
                )
            {
                score.PlayerScore++;
                ViewBag.Kazanan = "oyuncu";
            }
            else
            {
                score.CpuScore++;
                ViewBag.Kazanan = "cpu";
            }

            using var connection = new SqlConnection(connectionString);
            connection.Execute("UPDATE Tkm SET PlayerScore = @PlayerScore, CpuScore = @CpuScore", score);

            return View("index", score);
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
