using DataLayer;
using LaboInter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static Utils.Utils;

namespace LaboInter.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private Clients? connected_client;
        public HomeController(AppDbContext context) => _context = context;
        public IActionResult Index()
        {
            string? buffer = HttpContext.Session.GetString("conn");

            if (buffer is not null) connected_client = JsonSerializer.Deserialize<Clients>(buffer);
            else connected_client = null;

            if (connected_client is null)
            {
                ViewBag.Client = null;
            }
            else
            {
                ViewBag.Client = connected_client;
                ViewBag.AsHistorique = _context.Historique.Where(x => x.ClientId == connected_client.Id).IsNullOrEmpty();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Connection()
        {
            return View();
        }

        public IActionResult Deconnection()
        {
            HttpContext.Session.Remove("conn");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Administration()
        {
            ViewBag.Genre = _context.Genre.AsEnumerable();
            return View();
        }
        [HttpPost]
        public IActionResult Administration(Coffrets coffret)
        {
            ViewBag.Genre = _context.Genre.AsEnumerable();
            if(!ModelState.IsValid)
            {
                return View(coffret);
            }

            if(coffret.Bonus is null)
            {
                coffret.Bonus = "Aucun";
            }

            _context.Coffret.Add(coffret);
            _context.SaveChanges();
            ViewBag.Message = "Film Ajouté !";
            return View(coffret);
        }
        [HttpPost]
        public IActionResult deleteCoffret(int? id)
        {
            ViewBag.Genre = _context.Genre.AsEnumerable();
            Coffrets? coffret = null;

            if (id is not null)
            {
                 coffret = _context.Coffret
                    .Where(x => x.Id == id)
                    .First();
            } 
            else
            {
                ViewBag.MessageDelete = "Aucun Id spécifié";
                return View(nameof(Administration));
            }

            if(coffret is null)
            {
                ViewBag.MessageDelete = "Aucun coffret avec cet Id";
                return View(nameof(Administration));
            }

            _context.Coffret.Remove(coffret);
            _context.SaveChanges();
            ViewBag.MessageDelete = "Coffret Supprimé !";
            return View(nameof(Administration));
        }
        [HttpPost]
        public IActionResult Inscription(Clients client)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            bool IsAvailable = _context.Client
                .Where(x => x.Email == client.Email)
                .FirstOrDefault() is null ? true : false;

            if(!IsAvailable)
            {
                ViewBag.Error = "Cet email est déjà utilisé !";
                return View(client);
            }

            client.Password = PasswordHash(client.Password);

            _context.Client.Add(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Connection(ConnectionModel client)
        {
            if(!ModelState.IsValid)
            {
                return View(client);
            }

            string input_password = PasswordHash(client.Password);

            connected_client = _context.Client
                .Where(x => x.Email == client.Email && x.Password == input_password)
                .FirstOrDefault();

            if (connected_client is not null)
            {
                HttpContext.Session.SetString("conn", JsonSerializer.Serialize<Clients>(connected_client));
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                ViewBag.Error = "Ce compte n'existe pas";
                return View(client);
            }
        }

        public IActionResult addGenre(string? label)
        {
            ViewBag.Genre = _context.Genre.AsEnumerable();

            if (label is not null)
            {
                Genres newGenre = new();
                newGenre.Label = label;

                _context.Genre.Add(newGenre);
                _context.SaveChanges();
                ViewBag.MessageGenre = "Genre ajouté !";
                return View(nameof(Administration));
            }

            ViewBag.MessageGenre = "Manque texte";
            return View(nameof(Administration));
        }

        [HttpGet]
        public IActionResult GetFilm(int id)
        {
            ViewBag.Genre = _context.Genre.AsEnumerable();
            Coffrets? Coffret = _context.Coffret.Where(x => x.Id == id).FirstOrDefault();

            if (Coffret is null)
            {
                ViewBag.ModifyError = "Aucun coffret avec cet Id";
                return View(nameof(Administration));
            }
            ViewBag.Modify = true;
            ViewBag.CoffretModif = Coffret;
            return View(nameof(Administration));
        }

        [HttpPost]
        public IActionResult Modify(Coffrets modif)
        {
            _context.Coffret.Update(modif);
            _context.SaveChanges();
            return RedirectToAction(nameof(Administration));
        }
    }
}
