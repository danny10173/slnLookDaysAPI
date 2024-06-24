using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using LookDaysAPI.Models;
using Microsoft.Extensions.Configuration;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Models
{
    public class ECPayService
    {
        private readonly string _merchantID;
        private readonly string _hashKey;
        private readonly string _hashIV;
        private readonly string _paymentGatewayUrl;

        public ECPayService(IConfiguration configuration)
        {
            _merchantID = configuration["ECPay:MerchantID"];
            _hashKey = configuration["ECPay:HashKey"];
            _hashIV = configuration["ECPay:HashIV"];
            _paymentGatewayUrl = configuration["ECPay:ServiceURL"];
        }

        public string CreatePaymentRequest(List<Booking> bookings, decimal totalAmount)
        {
            int roundedTotalAmount = (int)Math.Round(totalAmount);
            var itemNames = string.Join("#", bookings.Select(b => b.Activity.Name));

            var parameters = new Dictionary<string, string>
            {
                { "MerchantID", _merchantID },
                { "MerchantTradeNo", bookings.First().MerchantTradeNo },
                { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
                { "PaymentType", "aio" },
                { "TotalAmount", roundedTotalAmount.ToString() },
                { "TradeDesc", "Order Payment" },
                { "ItemName", itemNames },
                { "ReturnURL", "https://localhost:7148/api/Payment/Return" },
                //{ "ReturnURL", "https://eogpfwxnqo2n3f2.m.pipedream.net" },
                { "ClientBackURL", "http://localhost:5173/payment-success"},
                { "ChoosePayment", "Credit" },
                { "EncryptType", "1" }//請固定填入1，使用SHA256加密。
            };


            parameters["CheckMacValue"] = GenerateCheckMacValue(parameters);
            
            var formBuilder = new StringBuilder();
            formBuilder.Append("<form id='ecpay_form' action='" + _paymentGatewayUrl + "' method='POST'>");

            foreach (var param in parameters)
            {
                formBuilder.Append("<input type='hidden' name='" + param.Key + "' value='" + param.Value + "' />");
            }

            formBuilder.Append("<input type='submit' value='Submit' style='display:none;'></form>");
            formBuilder.Append("<script>document.getElementById('ecpay_form').submit();</script>");

            return formBuilder.ToString();
        }

        public string GenerateCheckMacValue(Dictionary<string, string> parameters)
        {
            // 將參數字典按鍵名排序
            var orderedParams = parameters.OrderBy(p => p.Key);
             Console.WriteLine(orderedParams);

            // 組合參數為查詢字符串
            var queryString = new StringBuilder();
            queryString.Append($"HashKey={_hashKey}&");
            foreach (var param in orderedParams)
            {
                queryString.Append($"{param.Key}={param.Value}&");
            }
            queryString.Append($"HashIV={_hashIV}");
            Console.WriteLine(queryString);

            // 使用 HttpUtility.UrlEncode 並轉換為小寫
            string urlEncodedString = HttpUtility.UrlEncode(queryString.ToString()).ToLower();
            Console.WriteLine(urlEncodedString);

            // 使用 SHA256 生成哈希值
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(urlEncodedString));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

                Console.WriteLine(hashString);
                // 返回生成的檢查碼
                return hashString;
            }
        }
    }
}