using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace WebKH2024.Helpers
{
    public class MoMoPaymentHelper
    {
        public static string GenerateSignature(string rawData, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
        public static string CreatePaymentRequest(string orderId, string requestId, long amount, string orderInfo, string returnUrl, string notifyUrl)
        {
            var partnerCode = System.Configuration.ConfigurationManager.AppSettings["MoMo_PartnerCode"];
            var accessKey = System.Configuration.ConfigurationManager.AppSettings["MoMo_AccessKey"];
            var secretKey = System.Configuration.ConfigurationManager.AppSettings["MoMo_SecretKey"];
            var endpoint = System.Configuration.ConfigurationManager.AppSettings["MoMo_Endpoint"];
            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType=captureWallet";

            var signature = GenerateSignature(rawHash, secretKey);

            var requestBody = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount,
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData = "",
                requestType = "captureWallet",
                signature
            };
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"MoMo API trả về lỗi: {response.StatusCode}. Chi tiết: {response.Content.ReadAsStringAsync().Result}";
                    throw new Exception(errorMessage);
                }
                var responseContent = response.Content.ReadAsStringAsync().Result;
                dynamic result = JsonConvert.DeserializeObject(responseContent);
                if (result == null || result.payUrl == null)
                {
                    string errorMessage = "Không nhận được URL thanh toán từ MoMo.";
                    throw new Exception(errorMessage);
                }

                return result.payUrl;
            }

        }
    }
}
