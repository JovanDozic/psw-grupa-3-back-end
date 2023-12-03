﻿using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Encounters.API.Enums;

namespace Explorer.Encounters.Core.Domain
{
    public class Encounter: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public int Experience { get; set; }
        public EncounterStatus Status { get; set; }
        public EncounterType Type { get; init; }
        public int Radius { get; init; }
        public int[] Participants { get; set; } = Array.Empty<int>();
        public Encounter() {}

        public Encounter(string name, string description, Location location, int experience, EncounterStatus status, EncounterType type, int radius)
        {
            Name = name;
            Description = description;
            Location = location;
            Status = status;
            Type = type;
            Experience = experience;
            Radius = radius;
        }

        public bool Activate(int personId, double longitude, double latitude)
        {
            throw new NotImplementedException();
        }
    }
}
