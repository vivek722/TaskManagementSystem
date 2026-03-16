using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.interfaces;

namespace TaskManagement.Domain.services;
public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AiService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"];
    }
    public async Task<string> CategorizeTask(string text)
    {
        var prompt = $@"
                Classify this task into one category:
                Bug, Feature, Improvement, Documentation.

                Task: {text}
                ";

        return await SendPrompt(prompt);
    }

    public async Task<string> Chat(string promptText)
    {
        var prompt = $@"
            You are an assistant for a task management system.
            User question:
            {promptText}
            ";

        return await SendPrompt(prompt);
    }

    public async Task<string> DetectPriority(string text)
    {
                    var prompt = $@"
                            Determine priority for this task.
                            Return only one word: High, Medium, or Low.

                            Task: {text}
                            ";

        return await SendPrompt(prompt);
    }

    public async Task<string> GenerateSummary(string text)
    {
        var prompt = $"Summarize this task in one short sentence:\n{text}";
        return await SendPrompt(prompt);
    }

    public async Task<List<string>> GenerateTasks(string promptText)
    {
                    var prompt = $@"
                        Generate task list for this project.

                        Project: {promptText}

                        Return tasks as numbered list.
                        ";

        var result = await SendPrompt(prompt);

        return result.Split("\n").ToList();
    }

    private async Task<string> SendPrompt(string prompt)
    {
        var request = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

        var response = await _httpClient.PostAsJsonAsync(
            "https://api.openai.com/v1/chat/completions",
            request);

        dynamic result = await response.Content.ReadFromJsonAsync<dynamic>();

        return result.choices[0].message.content;
    }
}
