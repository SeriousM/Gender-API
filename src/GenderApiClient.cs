﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MicroKnights.Gender_API
{
    public class GenderApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GenderApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        protected virtual async Task<TResponse> ExecuteRequest<TResponse>(string url) where TResponse : GenderApiResponse
        {
            try
            {
                using (var client = _httpClientFactory.CreateClient("GenderAPI"))
                {

                    var key = client.DefaultRequestHeaders.Authorization.Parameter;
                    var jsonResult = await client.GetStringAsync($"{url}&key={key}");
                    if (jsonResult.IndexOf("errno", StringComparison.InvariantCultureIgnoreCase) > 0)
                    {
                        var error = JsonConvert.DeserializeObject<GenderApiErrorResponse>(jsonResult);
                        return (TResponse)Activator.CreateInstance(typeof(TResponse),new GenderApiException(error.ErrorCode,error.ErrorMessage));
                    }
                    return JsonConvert.DeserializeObject<TResponse>(jsonResult);
                }

            }
            catch (Exception ex)
            {
                return (TResponse)Activator.CreateInstance(typeof(TResponse), new GenderApiException(ex));
            }
        }


        public Task<GenderApiNameResponse> GetByNameAndCountry2Alpha(string name, string country2AlphaCode)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?name={name}&country={country2AlphaCode}");
        }

        public Task<GenderApiNameResponse> GetByNameAndCountryType(string name, CountryType countryType)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?name={name}&country={countryType.Alpha2Code}");
        }

        public Task<GenderApiNameResponse> GetByNameAndIp(string name, string ipAddresss)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?name={name}&ip={ipAddresss}");
        }

        public Task<GenderApiNameResponse> GetByNameAndBrowserLanguageLocale(string name, string browserLanguageLocale)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?name={name}&locale={browserLanguageLocale}");
        }

        public Task<GenderApiNameResponse> GetByEmail(string email)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?email={email}");
        }

        public Task<GenderApiNameResponse> GetByEmailandCountryType(string email, CountryType countryType)
        {
            return ExecuteRequest<GenderApiNameResponse>($"get?email={email}&country={countryType.Alpha2Code}");
        }

        public Task<GenderApiResponse> GetByEmailandCountry2Alpha(string email, string country2AlphaCode)
        {
            return ExecuteRequest<GenderApiResponse>($"get?email={email}&country={country2AlphaCode}");
        }
    }
}