using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProjetIA
{
    /*Ito no interface heriten'ny interface logique, classe Proposition sy GrandProposition*/
    public interface IData
    {
        string nom { get; set; }
        void Affichage();
    }
}
