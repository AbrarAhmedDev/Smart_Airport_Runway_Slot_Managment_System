namespace BlazorApp1.Services
{
    public class AirportService
    {
        public bool IsRunwayOpen { get; set; } = true;
        public string WeatherStatus { get; set; } = "Clear Skies";

        // NEW: System Audit Log
        public List<string> SystemLogs { get; set; } = new() { "System Initialized: Runway Active" };

        public event Action? OnStateChanged;

        public void ToggleRunway(string adminName)
        {
            IsRunwayOpen = !IsRunwayOpen;
            AddLog($"ATC {adminName} {(IsRunwayOpen ? "OPENED" : "CLOSED")} the runway.");
            OnStateChanged?.Invoke();
        }

        public void SetWeather(string weather, string adminName)
        {
            WeatherStatus = weather;
            AddLog($"ATC {adminName} updated weather to: {weather}");
            OnStateChanged?.Invoke();
        }

        public void AddLog(string message)
        {
            SystemLogs.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}");
            if (SystemLogs.Count > 10) SystemLogs.RemoveAt(10); // Keep only last 10
            OnStateChanged?.Invoke();
        }
    }
}