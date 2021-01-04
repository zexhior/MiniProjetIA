using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProjetIA
{
    /*Ity ny classe Ou miheriter an'ny interface ILogique*/
    class Et : ILogique
    {
        /*attribut nom no asina ny string implication "&" */
        public string nom { get; set; }
        public Et()
        {
            /*Initialisation any nom ao @ Constructeur*/
            this.nom = "&";
        }
        //Fonction implementena avy ao @ interface IData mba ahafana afficher hoe inona le nom an le classe
        public void Affichage()
        {
            Console.Write(this.nom);
        }
    }
}
