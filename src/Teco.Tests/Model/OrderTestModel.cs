#region License
// Copyright (c) 2019 Evgenii Zeiler, https://github.com/XMypuK
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Linq;

namespace Teco.Tests.Model {
    public class OrderTestModel {
        public String Customer { get; private set; }
        public OrderItem[] OrderList { get; private set; }
        public Double OrderTotal { get; private set; }
        public Double Discount { get; private set; }
        public Double OrderTotalWithDiscount { get; private set; }

        public OrderTestModel() {
            this.Customer = "John Smith";
            this.OrderList = new OrderItem[] {
                new OrderItem("Smartphone SuperMegaXPhone", 1023.0, 2),
                new OrderItem("Protecting Glass", 23.0, 2),
                new OrderItem("Headphones", 56.0, 1)
            };
            this.OrderTotal = this.OrderList.Sum(item => item.Total);
            this.Discount = 10.0;
            this.OrderTotalWithDiscount = this.OrderTotal * (100.0 - this.Discount) / 100.0;
        }
    }


    public class OrderItem {
        public String Title { get; private set; }
        public Double Price { get; private set; }
        public Int32 Quantity { get; private set; }
        public Double Total { get; private set; }

        public OrderItem(String title, Double price, Int32 quantity) {
            this.Title = title;
            this.Price = price;
            this.Quantity = quantity;
            this.Total = price * quantity;
        }
    }
}
