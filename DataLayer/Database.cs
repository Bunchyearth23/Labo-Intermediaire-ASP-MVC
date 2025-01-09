using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace DataLayer
{    
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Clients> Client { get; set; }
        public DbSet<Genres> Genre { get; set; }
        public DbSet<Coffrets> Coffret { get; set; }
        public DbSet<Historiques> Historique { get; set; }
    }
    
    public class Clients 
    {
        public required int Id { get; set; }
        [DisplayName("Nom: ")]
        [Required(ErrorMessage = "Le nom est obligatoire !")]
        public required string Nom { get; set; }
        [DisplayName("Prénom: ")]
        [Required(ErrorMessage = "Le prénom est obligatoire !")]
        public required string Prenom { get; set; }
        [DisplayName("Adresse: ")]
        [Required(ErrorMessage = "L'adresse est obligatoire !")]
        public required string Adresse { get; set; }
        [DisplayName("Email: ")]
        [Required(ErrorMessage = "L'email est obligatoire !")]
        [EmailAddress(ErrorMessage = "Email invalide !")]
        public required string Email { get; set; }
        [DisplayName("Password: ")]
        [Required(ErrorMessage = "Un mot de passe est obligatoire !")]
        public required string Password { get; set; }
        public required bool IsAdmin { get; set; }
    }
    public class Genres 
    { 
        public int Id { get; set; }
        public string? Label { get; set; }
        public Genres() { }
    }
    public class Coffrets
    {
        public required int Id { get; set; }
        [DisplayName("Titre: ")]
        [Required(ErrorMessage = "Titre Requis")]
        public required string Titre { get; set; }
        [DisplayName("Bonus: ")]
        public string? Bonus { get; set; }
        [DisplayName("Prix: ")]
        [Required(ErrorMessage = "Prix Requis")]
        public required decimal Prix { get; set; }
        [DisplayName("Quantité: ")]
        [Required(ErrorMessage = "Quantitée Requise")]
        public required int Quantité { get; set; }
        [DisplayName("Synopsis: ")]
        [Required(ErrorMessage = "Synopsis Requis")]
        public required string Synopsis { get; set; }
        [DisplayName("Genre: ")]
        [Required(ErrorMessage = "Genre Requis")]
        public required int Genre { get; set; }
        [DisplayName("URL Affiche: ")]
        [Required(ErrorMessage = "Affiche Requise")]
        public required string Affiche { get; set; }
    }

    public class Historiques
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CoffretId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Historiques(Coffrets coffret, Clients client)
        {
            ClientId = client.Id;
            CoffretId = coffret.Id;
            Amount = coffret.Prix;
            Date = DateTime.Now;
        }

        public Historiques() { }
    }
}
