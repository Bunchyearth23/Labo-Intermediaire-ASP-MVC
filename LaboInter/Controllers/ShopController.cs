using DataLayer;
using LaboInter.Tools;
using Microsoft.AspNetCore.Mvc;

namespace LaboInter.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private SessionManager _sessionManager;

        public ShopController(AppDbContext context, SessionManager manager) {
            _context = context;
            _sessionManager = manager;
        }
        public IActionResult Index()
        {
            ViewBag.Client = _sessionManager.CurrentUser;
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

        public IActionResult Detail(Coffrets coffretin)
        {
            ViewBag.Client = _sessionManager.CurrentUser;
            ViewBag.Coffret = _context.Coffret.Where(x => x.Id == coffretin.Id).First();
            return View();
        }

        public IActionResult Transaction(Coffrets coffretIn)
        {
            if (_sessionManager.CurrentUser is not null)
            {
                Historiques newHistorique = new(coffretIn, _sessionManager.CurrentUser);
                _context.Historique.Add(newHistorique);

                coffretIn.Quantité -= 1;
                _context.Coffret.Update(coffretIn);
                _context.SaveChanges();
            }

            return View();
        }
    }
}
