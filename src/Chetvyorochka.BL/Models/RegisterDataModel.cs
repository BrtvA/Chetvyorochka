namespace Chetvyorochka.BL.Models
{
    public class RegisterDataModel
    {
        public string Login { get; set; } = null!;
        public string FistName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Password { get; set; } = null!;
    }
}
