using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProjetIA
{
    /*Ity ny classe implication miheriter ny interface ILogique*/
    class Implication : ILogique
    {
        /*attribut nom no asina ny string implication "-" */
        public string nom { get; set; }

        public Implication()
        {
            /*Initialisation any nom ao @ Constructeur*/
            nom = "-";
        }

        //Fonction implementena avy ao @ interface IData mba ahafana afficher hoe inona le nom an le classe
        public void Affichage()
        {
            Console.Write(this.nom);
        }
    }
}
