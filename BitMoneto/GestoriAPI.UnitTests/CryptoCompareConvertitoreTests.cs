﻿using System;
using System.Collections.Generic;
using GestoriAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bitmoneto.UnitTests
{
    [TestClass]
    public class CryptoCompareConvertitoreTests
    {
        [TestMethod]
        public void ScaricaCambi_BTC_OK()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            Dictionary<String, decimal> cambi = conv.ScaricaCambi("BTC").Result;
            Assert.AreEqual<int>(cambi.Count, 4);
            bool ok = cambi.TryGetValue("BTC", out decimal ris);
            Assert.IsTrue(ok);
            Assert.AreEqual<Decimal>(1, ris);
            ok = cambi.TryGetValue("ETH", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("EUR", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("USD", out ris);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void ScaricaCambi_ETH_OK()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            Dictionary<String, decimal> cambi = conv.ScaricaCambi("ETH").Result;
            Assert.AreEqual<int>(cambi.Count, 4);
            bool ok = cambi.TryGetValue("ETH", out decimal ris);
            Assert.IsTrue(ok);
            Assert.AreEqual<Decimal>(1, ris);
            ok = cambi.TryGetValue("BTC", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("EUR", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("USD", out ris);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void ScaricaCambi_EUR_OK()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            Dictionary<String, decimal> cambi = conv.ScaricaCambi("EUR").Result;
            Assert.AreEqual<int>(cambi.Count, 4);
            bool ok = cambi.TryGetValue("EUR", out decimal ris);
            Assert.IsTrue(ok);
            Assert.AreEqual<Decimal>(1, ris);
            ok = cambi.TryGetValue("BTC", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("ETH", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("USD", out ris);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void ScaricaCambi_USD_OK()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            Dictionary<String, decimal> cambi = conv.ScaricaCambi("USD").Result;
            Assert.AreEqual<int>(cambi.Count, 4);
            bool ok = cambi.TryGetValue("USD", out decimal ris);
            Assert.IsTrue(ok);
            Assert.AreEqual<Decimal>(1, ris);
            ok = cambi.TryGetValue("BTC", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("ETH", out ris);
            Assert.IsTrue(ok);
            ok = cambi.TryGetValue("EUR", out ris);
            Assert.IsTrue(ok);
        }        

        [TestMethod]
        public void NomeValutaDaSimbolo_BTC_Bitcoin()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            String nome = conv.NomeValutaDaSimbolo("BTC").Result;
            Assert.AreEqual(nome, "Bitcoin");
        }

        [TestMethod]
        public void NomeValutaDaSimbolo_ETH_Ethereum()
        {
            CryptoCompareConvertitore conv = new CryptoCompareConvertitore();
            String nome = conv.NomeValutaDaSimbolo("ETH").Result;
            Assert.AreEqual(nome, "Ethereum");
        }


    }
}
