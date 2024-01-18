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

using RezzoCrypt.Xeggex.Objects;
using RezzoCrypt.Xeggex.Objects.Enums;

namespace RezzoCrypt.Xeggex.APIs
{
    public class XeggexExchangeData(XeggexConnection connection)
    {
        private readonly XeggexConnection _connection = connection;

        public XeggexBidsAsks ExchangePositions(string primaryAsset, string secondaryAsset, int limit = 10)
            => _connection.GetUrlResult<XeggexBidsAsks>("/orderbook", new
            {
                ticker_id = $"{primaryAsset}/{secondaryAsset}",
                depth = limit
            });

        public IEnumerable<XeggexExchangeKline> Kline(string primaryAsset, string secondaryAsset, DateTime startDate, DateTime endDate, KlinePeriod period)
            => _connection.GetUrlResult<XeggexExchangeKline[]>($"/market/getsymbol/{primaryAsset}_{secondaryAsset}", new { });
    }
}
