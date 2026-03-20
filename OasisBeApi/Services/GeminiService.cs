// Services/GeminiService.cs
using System.Text;
using System.Text.Json;
using Google.GenAI;
using Google.GenAI.Types;
using Oasis.DTOs.Activity;
using Oasis.Services.Interfaces;

namespace Oasis.Services;

public class GeminiService : IGeminiService {
    private const string Model = "gemini-2.5-flash-lite";

    private readonly ILogger<GeminiService> _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ActivitySuggestionHistory _history;

    public GeminiService(
        ILogger<GeminiService> logger,
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        ActivitySuggestionHistory history) {
        _logger = logger;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _history = history;
    }

    public async Task<ActivityDto> PromptAsync(AiPromptRequestDto request) {
        try {
            var apiKey = _config["GEMINI_API_KEY"] ?? throw new InvalidOperationException("GEMINI_API_KEY is not set.");

            var client = new Client(apiKey: apiKey);

            var contextBuilder = new StringBuilder();

            var now = DateTime.Now;
            contextBuilder.AppendLine($"Current time: {now:HH:mm} ({GetTimeOfDay(now)}) on {now:dddd, MMMM d yyyy}");
            contextBuilder.AppendLine($"Season: {GetSeason(now)}");

            if (!string.IsNullOrWhiteSpace(request.Location)) {
                contextBuilder.AppendLine($"Location: {request.Location}");

                if (request.IncludeWeather) {
                    var weather = await FetchWeatherAsync(request.Location);
                    if (weather != null)
                        contextBuilder.AppendLine($"Current weather: {weather}");
                }
            }

            if (request.TeamSize.HasValue)
                contextBuilder.AppendLine($"Team size: {request.TeamSize} people");

            // Add a random seed so responses vary between calls
            var seed = Random.Shared.Next(1, 10000);
            contextBuilder.AppendLine($"Variation seed (ignore, just ensures unique response): {seed}");

            var recentTitles = _history.GetRecent().ToList();
            if (recentTitles.Count > 0) {
                contextBuilder.AppendLine();
                contextBuilder.AppendLine("Recently suggested activities (DO NOT suggest these or anything similar):");
                foreach (var title in recentTitles)
                    contextBuilder.AppendLine($"- {title}");
            }

            var fullPrompt = $$"""
            You are a creative team activity suggester. You must respond ONLY with a single valid JSON object, no markdown, no explanation.

            Context:
            {{contextBuilder}}

            Your task is to suggest **one fun and unique activity** for the team. Mix **large group activities** (like going to parks, escape rooms, bowling alleys, restaurants) **with smaller, creative, or low-cost activities** (like board game tournaments, mini scavenger hunts, team storytelling, painting together, group meditation, yoga, skill-sharing workshops, cooking challenges, trivia games, or photography challenges). Avoid repeating any recently suggested activities.

            Here are some example activities you could take inspiration from:
            - Outdoor: picnic in a park, nature walk, urban scavenger hunt, bike ride, beach clean-up
            - Social: trivia night, karaoke, team dinner, networking café, mini treasure hunt
            - Creative: group painting, storytelling circle, DIY craft session, team video challenge, photography scavenger hunt
            - Sport/Physical: friendly soccer match, relay races, yoga/stretch session, indoor climbing, dance challenge
            - Relaxation/Mindfulness: meditation, breathing exercises, guided visualization, journal reflection circle
            - Skill/Personal Growth: cooking class, workshop on a new skill, coding mini-challenge, problem-solving game, debate or discussion session

            Guidelines:
            - Prioritize creativity and variety.
            - Consider time of day, season, weather, and team size.
            - Real venues are optional—smaller, low-key activities are equally valid.
            - Make the activity appropriate for the given team size.
            - Never suggest the same activity twice.

            You DO NOT necesseraly need to use any of the above examples. They are just to give you an idea of the type of activities we are looking for. Be creative and suggest something fun and unique!

            Respond with exactly this JSON structure:
            {
                "title": "max 50 chars",
                "description": "max 255 chars, max 3 sentences",
                "minMember": 2,
                "maxMember": 10,
                "experience": 20-100,
                "categoryName": "max 20 chars"
            }

            Rules:
            - experience: 20 = very easy, 100 = very challenging
            - categoryName examples: "Outdoor", "Creative", "Sport", "Relaxation", "Social", "Mindfulness", "Skill"
            - minMember must be > 0
            - Return raw JSON only, no backticks
            """;

            var response = await client.Models.GenerateContentAsync(
                model: Model,
                contents: fullPrompt,
                config: new GenerateContentConfig {
                    Tools = [new Tool { GoogleSearch = new GoogleSearch() }]
                }
            );

            var text = response.Candidates?[0].Content?.Parts?[0].Text
                ?? throw new InvalidOperationException("Empty response from Gemini.");

            text = text.Trim();
            if (text.StartsWith("```"))
                text = string.Join("\n", text.Split('\n').Skip(1).SkipLast(1));

            var activity = JsonSerializer.Deserialize<ActivityDto>(text, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Failed to deserialize Gemini response.");

            if (activity.Title != null)
                _history.Add(activity.Title);

            return activity;

        } catch (Exception ex) {
            _logger.LogError(ex, "Error calling Gemini API");
            throw;
        }
    }

    private static string GetTimeOfDay(DateTime dt) => dt.Hour switch {
        < 6  => "night",
        < 12 => "morning",
        < 17 => "afternoon",
        < 21 => "evening",
        _    => "night"
    };

    private static string GetSeason(DateTime dt) => dt.Month switch {
        12 or 1 or 2 => "Winter",
        3 or 4 or 5  => "Spring",
        6 or 7 or 8  => "Summer",
        _            => "Autumn"
    };

    private async Task<string?> FetchWeatherAsync(string location) {
        try {
            var http = _httpClientFactory.CreateClient();

            var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(location)}&count=1";
            var geoResponse = await http.GetFromJsonAsync<GeoResponse>(geoUrl);
            var place = geoResponse?.Results?.FirstOrDefault();
            if (place == null) return null;

            var weatherUrl = $"https://api.open-meteo.com/v1/forecast" +
                $"?latitude={place.Latitude}&longitude={place.Longitude}" +
                $"&current=temperature_2m,weathercode,windspeed_10m" +
                $"&temperature_unit=celsius&windspeed_unit=kmh&timezone=auto";

            var weatherResponse = await http.GetFromJsonAsync<WeatherResponse>(weatherUrl);
            var current = weatherResponse?.Current;
            if (current == null) return null;

            return $"{GetWeatherCondition(current.Weathercode)}, {current.Temperature2m}°C, wind {current.Windspeed10m} km/h";
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Could not fetch weather for {Location}", location);
            return null;
        }
    }

    private static string GetWeatherCondition(int code) => code switch {
        0              => "Clear sky",
        1 or 2 or 3    => "Partly cloudy",
        45 or 48       => "Foggy",
        51 or 53 or 55 => "Drizzle",
        61 or 63 or 65 => "Rainy",
        71 or 73 or 75 => "Snowy",
        80 or 81 or 82 => "Rain showers",
        95             => "Thunderstorm",
        _              => "Cloudy"
    };

    private record GeoResponse(List<GeoResult>? Results);
    private record GeoResult(double Latitude, double Longitude);
    private record WeatherResponse(CurrentWeather? Current);
    private record CurrentWeather(double Temperature2m, int Weathercode, double Windspeed10m);
}