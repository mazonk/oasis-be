using Oasis.Models;
using System.Collections.Generic;

namespace Oasis.Data.Seed;

public static class LevelSeed {
    public static List<Level> GetLevels() {
        return new List<Level> {
            // Tier 1: Getting Started
            new Level { LevelId = 1, Name = "Getting Started", MinExp = 0 },
            new Level { LevelId = 2, Name = "Showing Up", MinExp = 50 },
            new Level { LevelId = 3, Name = "First Step", MinExp = 100 },
            new Level { LevelId = 4, Name = "Warming Up", MinExp = 150 },
            new Level { LevelId = 5, Name = "Finding Your Rhythm", MinExp = 200 },

            // Tier 2: Social Spark
            new Level { LevelId = 6, Name = "Coffee Buddy", MinExp = 300 },
            new Level { LevelId = 7, Name = "Lunch Breaker", MinExp = 400 },
            new Level { LevelId = 8, Name = "Small Talk Pro", MinExp = 500 },
            new Level { LevelId = 9, Name = "Social Spark", MinExp = 600 },
            new Level { LevelId = 10, Name = "Conversation Starter", MinExp = 700 },

            // Tier 3: Active Together
            new Level { LevelId = 11, Name = "Walk & Talk", MinExp = 850 },
            new Level { LevelId = 12, Name = "Step Outside", MinExp = 1000 },
            new Level { LevelId = 13, Name = "Fresh Air Advocate", MinExp = 1200 },
            new Level { LevelId = 14, Name = "Movement Maker", MinExp = 1400 },
            new Level { LevelId = 15, Name = "Active Ally", MinExp = 1600 },

            // Tier 4: Building Bonds
            new Level { LevelId = 16, Name = "Friendly Face", MinExp = 1800 },
            new Level { LevelId = 17, Name = "Trusted Teammate", MinExp = 2000 },
            new Level { LevelId = 18, Name = "Bond Builder", MinExp = 2300 },
            new Level { LevelId = 19, Name = "Connection Keeper", MinExp = 2600 },
            new Level { LevelId = 20, Name = "Team Player", MinExp = 3000 },

            // Tier 5: Positive Influence
            new Level { LevelId = 21, Name = "Good Vibes", MinExp = 3400 },
            new Level { LevelId = 22, Name = "Energy Booster", MinExp = 3800 },
            new Level { LevelId = 23, Name = "Morale Builder", MinExp = 4200 },
            new Level { LevelId = 24, Name = "Uplift Creator", MinExp = 4600 },
            new Level { LevelId = 25, Name = "Positive Force", MinExp = 5000 },

            // Tier 6: Balanced Mind
            new Level { LevelId = 26, Name = "Mindful Worker", MinExp = 5500 },
            new Level { LevelId = 27, Name = "Balance Seeker", MinExp = 6000 },
            new Level { LevelId = 28, Name = "Stress Navigator", MinExp = 6500 },
            new Level { LevelId = 29, Name = "Calm Presence", MinExp = 7000 },
            new Level { LevelId = 30, Name = "Wellbeing Guardian", MinExp = 7600 },

            // Tier 7: Community Builder
            new Level { LevelId = 31, Name = "Community Minded", MinExp = 8200 },
            new Level { LevelId = 32, Name = "Connector", MinExp = 8800 },
            new Level { LevelId = 33, Name = "Bridge Builder", MinExp = 9400 },
            new Level { LevelId = 34, Name = "Culture Creator", MinExp = 10000 },
            new Level { LevelId = 35, Name = "Team Catalyst", MinExp = 11000 },

            // Tier 8: Engagement Leader
            new Level { LevelId = 36, Name = "Initiative Taker", MinExp = 12000 },
            new Level { LevelId = 37, Name = "Event Starter", MinExp = 13000 },
            new Level { LevelId = 38, Name = "Momentum Builder", MinExp = 14000 },
            new Level { LevelId = 39, Name = "Engagement Leader", MinExp = 15000 },
            new Level { LevelId = 40, Name = "Inspiration Source", MinExp = 16500 },

            // Tier 9: Culture Champion
            new Level { LevelId = 41, Name = "Culture Advocate", MinExp = 18000 },
            new Level { LevelId = 42, Name = "Team Champion", MinExp = 19500 },
            new Level { LevelId = 43, Name = "Connection Master", MinExp = 21000 },
            new Level { LevelId = 44, Name = "Workplace Hero", MinExp = 23000 },
            new Level { LevelId = 45, Name = "Culture Champion", MinExp = 25000 },

            // Tier 10: Unplug Legend
            new Level { LevelId = 46, Name = "Wellbeing Leader", MinExp = 27000 },
            new Level { LevelId = 47, Name = "Human Connector", MinExp = 29000 },
            new Level { LevelId = 48, Name = "Unplug Advocate", MinExp = 32000 },
            new Level { LevelId = 49, Name = "Burnout Breaker", MinExp = 35000 },
            new Level { LevelId = 50, Name = "Unplug Legend", MinExp = 40000 },
        };
    }
}