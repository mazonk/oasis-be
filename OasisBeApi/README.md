# Oasis Backend API

ASP.NET Core Web API for the Oasis team wellness platform.

## Tech Stack

- **Framework**: ASP.NET Core (.NET 8)
- **ORM**: Entity Framework Core
- **Database**: PostgreSQL
- **Auth**: JWT Bearer tokens
- **AI**: Google Gemini API (`gemini-2.5-flash-lite`) via `Google.GenAI`
- **Weather**: Open-Meteo (free, no API key required)

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL running locally or remotely

### Setup

1. Clone the repo and navigate to the project:
   ```bash
   cd OasisBeApi
   ```

2. Set your environment variables (do **not** put secrets in `appsettings.json`):
   ```bash
   # Option A — environment variable
   export GEMINI_API_KEY="your-api-key-here"

   # Option B — dotnet user secrets (recommended for local dev)
   dotnet user-secrets set "GEMINI_API_KEY" "your-api-key-here"
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=oasis;Username=postgres;Password=yourpassword"
   ```

3. Apply migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` by default.

---

## Project Structure

```
OasisBeApi/
├── Controllers/          # HTTP endpoints
├── Services/             # Business logic
│   └── Interfaces/
├── Repositories/         # Data access layer
│   └── Interfaces/
├── Models/               # EF Core entity models
├── DTOs/                 # Request/response shapes
│   ├── Activity/
│   ├── Member/
│   └── Team/
├── Data/                 # AppDbContext
└── Migrations/
```

---

## API Overview

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new member |
| POST | `/api/auth/login` | Login, returns JWT |

### Member
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/member` | Get current user (from JWT) |
| GET | `/api/member/{id}` | Get member by ID |
| PUT | `/api/member` | Update current user |

### Team
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/team` | Create a team |
| GET | `/api/team` | Get all teams |
| GET | `/api/team/{id}` | Get team by ID |
| GET | `/api/team/member/{memberId}` | Get team by member ID |

### Activity
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/activity` | Get all activities |
| GET | `/api/activity/{id}` | Get activity by ID |
| POST | `/api/activity/{id}/complete` | Complete an activity (awards XP) |

### AI
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/ai/prompt` | Generate an activity suggestion via Gemini |

### Level
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/level` | Get all levels |
| GET | `/api/level/{id}` | Get level by ID |

### Team Invitation
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/teaminvitation` | Invite a member by email |
| GET | `/api/teaminvitation/active?email=` | Get active invitations for an email |
| POST | `/api/teaminvitation/respond` | Accept or decline an invitation |

---

## AI Activity Suggestion

`POST /api/ai/prompt`

```json
{
  "location": "Copenhagen, Denmark",
  "includeWeather": true,
  "teamSize": 8
}
```

- Calls Gemini with time, season, weather, and team size as context
- Uses Google Search grounding to find real local venues
- Keeps a history of recent suggestions to avoid repetition
- Returns a structured `ActivityDto`

---

## XP & Leveling

- Members earn XP by completing activities (`activity.experience` value)
- Teams also earn XP per completed activity
- After each XP gain, the member is automatically leveled up to the highest `Level` where `member.experience >= level.minExp`
- Seed levels via EF migrations — example:

```csharp
new Level { LevelId = 1, Name = "Rookie", MinExp = 0 },
new Level { LevelId = 2, Name = "Bronze", MinExp = 100 },
new Level { LevelId = 3, Name = "Silver", MinExp = 500 },
new Level { LevelId = 4, Name = "Gold", MinExp = 1000 }
```

---

## Notes

- All protected endpoints require a valid JWT in the `Authorization: Bearer <token>` header
- The `GEMINI_API_KEY` uses the free tier of Gemini 2.5 Flash Lite — ~1,000 requests/day
- Google Search grounding costs ~$0.035/request on paid tier
- Weather is fetched from Open-Meteo — completely free, no key needed
