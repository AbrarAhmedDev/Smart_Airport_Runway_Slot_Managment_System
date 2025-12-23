using BlazorApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp1.Services
{
    public class SlotService
    {
        public User? CurrentUser { get; private set; }
        private List<User> _users = new() { new User { Username = "admin", Password = "123", Role = "Admin" } };
        private List<RunwaySlot> _slots = new();

        public DateTime StartTime { get; set; } = DateTime.Today.AddHours(8);
        public DateTime EndTime { get; set; } = DateTime.Today.AddHours(20);

        // --- MANDATORY REQUIREMENT: STATE MANAGEMENT ---
        // This event notifies all components when data changes
        public event Action? OnStateChanged;
        private void NotifyStateChanged() => OnStateChanged?.Invoke();

        public bool Login(string user, string pass)
        {
            var found = _users.FirstOrDefault(u => u.Username == user && u.Password == pass);
            if (found != null) { CurrentUser = found; return true; }
            return false;
        }

        public void Signup(string user, string pass, string role)
        {
            if (!_users.Any(u => u.Username == user))
            {
                _users.Add(new User { Username = user, Password = pass, Role = role });
            }
        }

        public void Logout() => CurrentUser = null;

        // --- ADVANCED FUNCTIONALITY: SEARCH & FILTER ---
        public List<RunwaySlot> GetSlots(string searchTerm = "")
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return _slots;
            return _slots.Where(s => s.FlightNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public void RemoveSlot(int id)
        {
            _slots.RemoveAll(s => s.Id == id);
            NotifyStateChanged();
        }

        // --- NEW ADMIN FUNCTIONALITY: APPROVAL ---
        public void ApproveSlot(int id)
        {
            var slot = _slots.FirstOrDefault(s => s.Id == id);
            if (slot != null)
            {
                slot.Status = "Approved";
                NotifyStateChanged();
            }
        }

        public string BookSlot(string flight, DateTime requestedTime, string type)
        {
            if (requestedTime.TimeOfDay < StartTime.TimeOfDay || requestedTime.TimeOfDay > EndTime.TimeOfDay)
                return $"Runway is closed. Operating hours: {StartTime:HH:mm} - {EndTime:HH:mm}";

            // SMART LOGIC: Conflict check (Within 60 mins)
            bool isTaken = _slots.Any(s => Math.Abs((s.ScheduledTime - requestedTime).TotalMinutes) < 60);

            if (isTaken) return "Time conflict. Please allow a 1-hour gap.";

            _slots.Add(new RunwaySlot
            {
                Id = _slots.Count + 1,
                FlightNumber = flight,
                ScheduledTime = requestedTime,
                Type = type,
                PilotName = CurrentUser?.Username ?? "Unknown",
                Status = "Pending" // Starts as pending for Admin approval
            });

            NotifyStateChanged(); // Update UI everywhere
            return "Success";
        }
    }
}