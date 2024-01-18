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

namespace RezzoCrypt.Xeggex
{
    public class XeggexExchange(XeggexConnection connection)
    {
        private readonly XeggexConnection _connection = connection;

        public IEnumerable<XeggexOrder> AllOrders(string primaryAsset, string secondaryAsset, DateTime startDate, DateTime endDate, int limit = 50) =>
            new[] { "active", "filled", "cancelled" }
            .SelectMany(status => _connection.GetUrlResult<XeggexOrder[]>("/getorders", new
            {
                symbol = $"{primaryAsset}_{secondaryAsset}",
                status,
                limit,
                skip = 0,
            }, secure: true));
        public IEnumerable<XeggexOrder> OpenedOrders(string primaryAsset, string secondaryAsset, int limit = 500) =>
            _connection.GetUrlResult<XeggexOrder[]>("/getorders", new
            {
                symbol = $"{primaryAsset}/{secondaryAsset}",
                status = "active",
                limit,
                skip = 0,
            }, secure: true);
        public XeggexOrder PlaceOrder(string primaryAsset, string secondaryAsset, double price, double qty, OrderSide side = OrderSide.BUY, OrderType type = OrderType.LIMIT) =>
            _connection.GetUrlResult<XeggexOrder>("/createorder", new
            {
                symbol = $"{primaryAsset}/{secondaryAsset}",
                side = side.ToString(),
                type = type.ToString(),
                quantity = qty,
                price,
            }, XeggexConnection.Method.Post, secure: true);
        public string CancelOrder(string orderId)
            => _connection.GetUrlResult<string>("/cancelorder", new { id = orderId }, XeggexConnection.Method.Post, secure: true);
    }
}
