﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitfinex;
using Criptovalute;

namespace GestoriAPI
{
    public class BitfinexExchange : Criptovalute.IExchange
    {
        private readonly BitfinexApi _apiClient;
        private readonly ValutaFactory _factory;
        private List<Fondo> _fondi = null;

        public string Nome { get { return "Bitfinex"; } }

        public string ChiavePubblica { get; }

        public string ChiavePrivata { get; }

        public List<Fondo> Fondi { get { return _fondi; } }

        public BitfinexExchange(String publicKey, String privateKey, ValutaFactory factory)
        {
            if (publicKey == null || publicKey == "" || privateKey == null || privateKey == "")
                throw new ArgumentException("chiavi non valide!");
            _apiClient = new BitfinexApi(publicKey, privateKey);
            ChiavePubblica = publicKey;
            ChiavePrivata = privateKey;
            _factory = factory ?? throw new ArgumentException("factory non può essere null!");
        }
        
        public async Task<List<Fondo>> ScaricaFondi()
        {
            _fondi = new List<Fondo>();
            //L'implementazione delle API utilizzata non è asincrona quindi la rendo asincrona inserendola in un Task
            await Task.Run(() =>
            {
                try
                {
                    var risArray = _apiClient.GetBalances().ToArray();

                    for (int i = 0; i < risArray.Length; i++)
                    {
                        if (risArray[i].Amount != 0)
                        {
                            decimal quantità = Convert.ToDecimal(risArray[i].Amount);
                            Valuta valuta = _factory.OttieniValuta(risArray[i].Currency);
                            Fondo fondo = new Fondo(valuta, quantità);
                            _fondi.Add(fondo);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new EccezioneApi("BitfinexExchange(ScaricaFondi()): Errore durante il collegamento: " + e.Message);
                }
            });
            return _fondi;
        }

        public override bool Equals(object obj)
        {
            var exchange = obj as BitfinexExchange;
            return exchange != null &&
                  _apiClient.Equals(exchange._apiClient) &&
                  _factory.Equals(exchange._factory);
        }

        public override int GetHashCode()
        {
            var hashCode = -1016245487;
            hashCode = hashCode * -1521134295 + _apiClient.GetHashCode();
            hashCode = hashCode * -1521134295 + _factory.GetHashCode();
            return hashCode;
        }


    }
}
