using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LaboInter.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private Clients? connected_client;

        public ShopController(AppDbContext context) => _context = context;
        public IActionResult Index()
        {
            string? sessionConnection = HttpContext.Session.GetString("conn");
            if (sessionConnection is not null) connected_client = JsonSerializer.Deserialize<Clients>(sessionConnection);
            else connected_client = null;

            ViewBag.Client = connected_client;
            List<(Coffrets, string?)> filmListe = [];
            List<Genres> genresList = _context.Genre.AsEnumerable().ToList();

            foreach(Coffrets x in _context.Coffret.AsEnumerable())
            {
                string? coffGenre = genresList.Where(y => y.Id == x.Genre).First().Label;
                filmListe.Add((x, coffGenre));
            }

            ViewBag.Coffret = filmListe;

            return View();
        }

        public IActionResult Detail(Coffrets coffret)
        {
            return View(coffret);
        }
    }
}
