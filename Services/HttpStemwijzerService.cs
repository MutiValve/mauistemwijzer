using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;
using mauistemwijzer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace mauistemwijzer.Services;

public partial class HttpStemwijzerService : IStemwijzerService
{
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private List<Party>? _cachedParties;
    private List<int>? _cachedQuestions;
    private List<Coordinate>? _cachedCoordinates;

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

            _cachedParties = await GetFromApiAsync<List<Party>>("http://localhost/programming/stemwijzer/backend/?action=partijen") ?? [];
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
        //if (_cachedQuestions != null)
        //    return _cachedQuestions;

        await _lock.WaitAsync();
        try
        {
            //if (_cachedQuestions != null)
            //    return _cachedQuestions;

            _cachedQuestions = await GetFromApiAsync<List<int>>("http://localhost/programming/stemwijzer/backend/index.php?action=vragen");
        }
        finally
        {
            _lock.Release();
        }

        return _cachedQuestions;
    }

    public async Task<Question?> GetQuestionByIdAsync(int id)
    {
        var data = await GetFromApiAsync<QuestionApiResponse>("http://localhost/programming/stemwijzer/backend/?action=vragen&id=" + id);
        //Console.log


        return data?.Vraag;
    }

    //coordinaten
    public async Task<List<Coordinate>> GetAllCoordinatesAsync()
    {
        if (_cachedCoordinates != null)
            return _cachedCoordinates;

        _cachedCoordinates = await GetFromApiAsync<List<Coordinate>>("rekenshit.php") ?? [];
        return _cachedCoordinates;
    }

    // clear cache
    public async Task RefreshAsync()
    {
        await _lock.WaitAsync();
        try
        {
            _cachedParties = null;
            _cachedQuestions = null;
            _cachedCoordinates = null;
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
            var killme = await _httpClient.GetAsync(endpoint);
            string responseBody = await killme.Content.ReadAsStringAsync();

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

    // (Of gebruik je bestaande datastructuur uit je eerdere code voor de database-velden)
}