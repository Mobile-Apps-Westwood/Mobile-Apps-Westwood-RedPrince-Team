using SQLite;

namespace RedPrince.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Username { get; set; }

        public string Password { get; set; }

        public string Hint { get; set; }

        public int Money { get; set; }
    }
}
