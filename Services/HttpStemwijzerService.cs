using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;
using mauistemwijzer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using System.Text.Json;

namespace mauistemwijzer.Services;

public partial class HttpStemwijzerService : IStemwijzerService
{
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private List<Party>? _cachedParties;
    private List<int>? _cachedQuestions;
    private ResultaatResponse? _cachedResultaat;

    public HttpStemwijzerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // alle partijen
    public async Task<List<Party>> GetPartiesAsync()
    {
        if (_cachedParties != null)
            return _cachedParties;

        await _lock.WaitAsync();
        try
        {
            if (_cachedParties != null)
                return _cachedParties;

            _cachedParties = await GetFromApiAsync<List<Party>>("http://localhost/programming/stemwijzer/backend/index.php?action=partijen") ?? [];
        }
        finally
        {
            _lock.Release();
        }

        return _cachedParties;
    }

    // all questions
    public async Task<List<int>> GetQuestionsAsync()
    {
        await _lock.WaitAsync();
        try
        {
            _cachedQuestions = await GetFromApiAsync<List<int>>("http://localhost/programming/stemwijzer/backend/index.php?action=vragen");
        }
        finally
        {
            _lock.Release();
        }

        return _cachedQuestions ?? [];
    }

    public async Task<Question?> GetQuestionByIdAsync(int id)
    {
        var data = await GetFromApiAsync<QuestionApiResponse>("http://localhost/programming/stemwijzer/backend/?action=vragen&id=" + id);
        Debug.WriteLine("data is " + data);
        return data?.Vraag;
    }

    // Resultaat API (partijposities en gebruikerpositie op de politieke roos)
    public async Task<ResultaatResponse?> GetResultaatAsync(List<UserAnswer>? answers = null)
    {
        string endpoint = "http://localhost/programming/stemwijzer/backend/index.php?action=resultaat";

        try
        {
            if (answers != null && answers.Count > 0)
            {
                var payload = new
                {
                    answers = answers.Select(a => new
                    {
                        questionId = a.QuestionId,
                        answer = a.Answer.ToString()
                    }).ToList()
                };

                Debug.WriteLine(JsonSerializer.Serialize(payload));

                var response = await _httpClient.PostAsJsonAsync(endpoint, payload);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResultaatResponse>();
                    if (result != null)
                    {
                        _cachedResultaat = result;
                        return result;
                    }
                }
            }

            var getResult = await GetFromApiAsync<ResultaatResponse>(endpoint);
            _cachedResultaat = getResult;
            return getResult;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[HttpStemwijzerService] Error fetching resultaat: {ex.Message}");
            return await GetFromApiAsync<ResultaatResponse>(endpoint);
        }
    }

    // clear cache
    public async Task RefreshAsync()
    {
        await _lock.WaitAsync();
        try
        {
            _cachedParties = null;
            _cachedQuestions = null;
            _cachedResultaat = null;
        }
        finally
        {
            _lock.Release();
        }
    }

    // api generic helperfunc
    private async Task<T?> GetFromApiAsync<T>(string endpoint) where T : class
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<T>(endpoint);
            if (result == null)
            {
                throw new InvalidOperationException($"De API gaf een leeg antwoord terug ({endpoint}).");
            }
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[HttpStemwijzerService] Error fetching REST API from {endpoint}: {ex}");
            throw new HttpRequestException($"Fout bij het ophalen van gegevens van de API ({endpoint}): {ex.Message}", ex);
        }
    }
}
