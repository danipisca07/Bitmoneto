﻿using Criptovalute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace GestoriAPI
{
    public class CryptoCompareConvertitore : IConvertitore
    {
        private static readonly String INDIRIZZO = "https://min-api.cryptocompare.com/data/";
        private dynamic _metadatiSimboli = null;
        public string[] SimboliConversioni { get; }

        public CryptoCompareConvertitore()
        {
            SimboliConversioni = new String[] { "BTC", "ETH", "EUR", "USD" };
        }

        public CryptoCompareConvertitore(String[] simboliConversioni)
        {
            SimboliConversioni = simboliConversioni;
        }

        public async Task<Dictionary<String, decimal>> ScaricaCambi(String simboloValuta)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(INDIRIZZO)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            String listaConversioni = SimboliConversioni.Select(a => a.ToString()).Aggregate((i, j) => i + "," + j);
            HttpResponseMessage risposta = await client.GetAsync("price?tsyms="+listaConversioni+"&fsym="+ simboloValuta);
            if (risposta.IsSuccessStatusCode)
            {
                Dictionary<String, decimal> cambi = new Dictionary<string, decimal>();
                String contenuto = await risposta.Content.ReadAsStringAsync();
                var json = Json.Decode(contenuto);
                
                foreach(String valuta in SimboliConversioni)
                {
                    if (!Decimal.TryParse(json[valuta].ToString(),out decimal cambio))
                    {
                        if (!Decimal.TryParse(json[valuta].ToString(), System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent, new CultureInfo(""), out cambio))
                        {
                            throw new EccezioneApi("CryptoCompareConvertitore(ScaricaCambi()): Valore non valido");
                        }
                    }
                    cambi.Add(valuta, cambio);
                }

                return cambi;
            }
            else
            {
                throw new EccezioneApi("CryptoCompareConvertitore(ScaricaCambi()):Errore chiamata API, codice:" + risposta.StatusCode);
            }
        }

        private async Task ScaricaSimboli()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(INDIRIZZO);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage risposta = await client.GetAsync("all/coinlist");
                if (risposta.IsSuccessStatusCode)
                {
                    String contenuto = await risposta.Content.ReadAsStringAsync();
                    var json = Json.Decode(contenuto);

                    _metadatiSimboli = json["Data"];
                }
                else
                {
                    throw new EccezioneApi("CryptoCompareConvertitore(ScaricaSimboli()):Errore chiamata API, codice:" + risposta.StatusCode);
                }
            }
        }

        public async Task<String> NomeValutaDaSimbolo(String simbolo)
        {
            if (_metadatiSimboli == null)
                await ScaricaSimboli();
            dynamic metadata = _metadatiSimboli[simbolo];

            //A volte il simbolo delle criptovalute anziche di 4 lettere è stato tagliato a 3 per rispettare lo standard ISO, quindi se non trovo corrispondenza con il simbolo intero provo a ridurlo a tre lettere
            if (metadata == null && simbolo.Length > 3 && _metadatiSimboli[simbolo.Substring(0, 3)] != null)
                metadata = _metadatiSimboli[simbolo.Substring(0, 3)];

            if (metadata == null)
                throw new EccezioneApi("CryptoCompareConvertitore(NomeValutaDaSimbolo()):Errore: valuta sconosciuta");

            return metadata["CoinName"];
        }


    }
}
