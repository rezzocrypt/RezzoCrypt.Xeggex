/*
 *   Copyright (c) 2024 Alexey Vinogradov
 *   All rights reserved.

 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 */

using Flurl;
using Flurl.Http;
using RezzoCrypt.Xeggex.APIs;
using RezzoCrypt.Xeggex.Objects;
using System.Text;

namespace RezzoCrypt.Xeggex
{
    public class XeggexConnection(string apiKey = "", string apiSecret = "")
    {
        internal readonly string _baseUrl = "https://api.xeggex.com/api/v2";
        internal string _apiKey = apiKey;
        internal string _apiSecret = apiSecret;

        #region Вспомогательные

        internal enum Method
        {
            Get,
            Post,
            Delete
        }

        internal string SecretHash => Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKey}:{_apiSecret}"));

        internal T GetUrlResult<T>(string url, object? data = null, Method method = Method.Get, bool secure = false)
            where T : class
        {
            var currentRequest = _baseUrl
                .AppendPathSegment(url)
                .WithHeader("Content-Type", "application/json")
                .SetQueryParams(data);

            if (secure)
            {
                currentRequest = currentRequest
                    .WithHeader("Authorization", "Basic " + SecretHash);
            }

            try
            {
                var responseResult = method switch
                {
                    Method.Post => currentRequest.PostAsync().Result,
                    Method.Delete => currentRequest.DeleteAsync().Result,
                    _ => currentRequest.GetAsync().Result,
                };
                return typeof(T) == typeof(string)
                    ? (T)(responseResult.GetStringAsync().Result as object)
                    : responseResult.GetJsonAsync<T>().Result;
            }
            catch (Exception ex)
            {
                if (ex is FlurlHttpException fhttpex)
                {
                    string serverErrorMessage = string.Empty;
                    try
                    {
                        serverErrorMessage = $"url: {url}, data: {currentRequest.Url.Query}, error: {fhttpex.Call.Response.GetStringAsync().Result}";
                    }
                    catch
                    {
                        // Could not extract server side error , just continue with original exception.
                    }

                    if (!string.IsNullOrEmpty(serverErrorMessage))
                    {
                        throw new Exception(serverErrorMessage);
                    }
                }
                throw;
            }
        }

        #endregion

        public XeggexAccount Account => new(this);

        public XeggexExchange Exchange => new(this);

        public XeggexExchangeData Data => new(this);

        public XeggexAccountInfo Ping() => GetUrlResult<XeggexAccountInfo>("/info");
    }
}