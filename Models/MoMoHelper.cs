using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SachOnline.Models
{
    public class MoMoSecurity
    {
        public string signSHA256(string rawData, string secretKey)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(rawData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
    }

    public class PaymentRequest
    {
        public static string sendPaymentRequest(string endpoint, string postJson)
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);
            byte[] data = Encoding.UTF8.GetBytes(postJson);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";
            httpWReq.ContentLength = data.Length;

            using (Stream stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            WebResponse response = httpWReq.GetResponse();
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            return responseString;
        }
    }

    public class Result
    {
        public string message { get; set; }
        public string orderId { get; set; }
        public string errorCode { get; set; }
    }
}
