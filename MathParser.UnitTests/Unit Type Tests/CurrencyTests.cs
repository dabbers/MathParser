using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using dab.Library.MathParser;
using System.Net;
using System.Text;

namespace MathParserUnitTest.Unit_Type_Tests
{
    [TestClass]
    public class CurrencyTests
    {
        MathParser mp = new MathParser("http://localhost:8813/rates/", ".");
        static HttpListener hl;


        static string jsonResp = @"{
  ""timestamp"": 1414274440,
  ""base"": ""USD"",
  ""rates"": {
    ""CAD"": 2,
    ""EUR"": 3,
    ""GBP"": 4,
    ""BTC"": 5,
    ""USD"": 1
}}";

        [ClassInitialize]
        public static void CreateWebServer(TestContext ctx)
        {
            if (!HttpListener.IsSupported)
            {
                throw new Exception("HttpListener is not supported but is required");
            }

            hl = new HttpListener();
            hl.Prefixes.Add("http://localhost:8813/rates/");
            hl.Start();
            hl.BeginGetContext(ListenerCallback, hl);
        }

        [ClassCleanup]
        public static void DestroyWebServer()
        {
            hl.Stop();
        }

        public static void ListenerCallback(IAsyncResult result)
        {

            // Get the context
            var context = hl.EndGetContext(result);
            // listen for the next request
            hl.BeginGetContext(ListenerCallback, null);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonResp);
            // and send it
            var response = context.Response;
            response.ContentType = "text/json";
            response.ContentLength64 = buffer.Length;
            response.StatusCode = 200;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        [TestMethod]
        public void USDtoCAD()
        {
            var result = mp.Evaluate("1 usd to cad");
            Assert.AreEqual(1, result.Value);
            Assert.AreEqual(CurrencyUnits.CAD, result.DesiredUnit);
        }

        [TestMethod]
        public void BTCtoUSD()
        {
            var result = mp.Evaluate("5 BTC to USD");
            Assert.AreEqual(1, result.Value);
            Assert.AreEqual(CurrencyUnits.USD, result.DesiredUnit);
            Assert.AreEqual("1 USD", result.ToString());
        }

        [TestMethod]
        public void BTCtoCAD()
        {
            var result = mp.Evaluate("5 BTC to CAD");
            Assert.AreEqual(1, result.Value);
            Assert.AreEqual(CurrencyUnits.CAD, result.DesiredUnit);
        }
    }
}
