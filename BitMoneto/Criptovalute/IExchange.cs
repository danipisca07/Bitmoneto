﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Criptovalute
{
    public interface IExchange
    {
        String Nome { get; }
        String ChiavePubblica { get; }
        String ChiavePrivata { get; }
        List<Fondo> Fondi { get; }
        Task<List<Fondo>> ScaricaFondi();
    }
}
