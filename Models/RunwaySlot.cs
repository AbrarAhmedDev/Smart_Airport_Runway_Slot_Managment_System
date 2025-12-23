namespace BlazorApp1.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Pilot";
    }

    public class RunwaySlot
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime ScheduledTime { get; set; }
        public string PilotName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Type { get; set; } = "Arrival";
    }
}