using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProjetIA
{
    /*Ity ny classe Proposition miheriter interface IData. Raha ohatra ka proposition irary
     ihany no ao anatiny de Proposition no apesaina*/
    class Proposition : IData
    {
        
        //attribut asina ny nom an'ilay proposition ex:p1
        public string nom { get; set; }
        
        //attribut asina ny valeur bool ana negation rehefa true izy dia tsisy negation fa rehefa false dia misy
        public bool isTrue { get; set; }
        
        /*attribut anasivana ny Proposition precedente satria mizarazara 
         branche fille le proposition rehefa be dia be dia mila alaina ko ny branche parent mba 
         mba ahafahana miverina any @ racine ohatra*/
        public Proposition precedentProposition { get; set; }
        
        //Constructeur proposition raha tsy asina proposition precedente
        public Proposition(string nom, bool istrue)
        {
            this.nom = nom;
            this.isTrue = istrue;
            //atao null le precedenteProposition raha ohatra ka tsis branche mere intsony
            precedentProposition = null;
        }

        //Constructeur proposition raha asina proposition precedente ao @ parametre
        public Proposition(string nom, bool istrue,ref Proposition precedente)
        {
            this.nom = nom;
            this.isTrue = istrue;
            precedentProposition = precedente;
        }

        //Fonction ao @ classe IData reimplemeter ato @ classe Proposition affichena ny nom an'ilay classe
        public virtual void Affichage()
        {
            if (this.isTrue)
                Console.Write(this.nom);
            else
                Console.Write("/"+this.nom);
        }

        /*Fonction ahafahana manao parcours ny proposition sy logique rehetra ao anaty hypothese.
         Misy virtual satria ny GrandProposition miheriter classe Proposition donc avadika polymorphisme
         reccursif*/
        public virtual string Parcourir()
        {
            if (this.isTrue)
                return this.nom;
            else
                return "/" + this.nom;
        }

        //Regle de Negation
        public virtual void Negation()
        {
            if (isTrue)
                isTrue = false;
            else
                isTrue = true;
        }

        //Ty tsy mande fa avelako eo ihany aloha sao dia ilaina
        /*public virtual Proposition ParcoursPrincipeDeRobinson(string proposition)
        {
            if (Parcourir() == proposition)
            {
                Console.WriteLine("Coucou "+this.Parcourir());
                if (((GrandProposition)precedentProposition).propositionGauche.Parcourir() == this.Parcourir())
                {
                    precedentProposition = ((GrandProposition)precedentProposition).propositionDroite;
                    return this;
                }
                else if (((GrandProposition)precedentProposition).propositionDroite.Parcourir() == this.Parcourir())
                {
                    precedentProposition = ((GrandProposition)precedentProposition).propositionGauche;
                    return this;
                }
            }
            return null;
        }*/

        //Initialisation an'ny Proposition precedente rehefa tsy any @ constructeur no initialisena ilay izy
        public void InitialisationPropositionPrecedente(Proposition precedente)
        {
            precedentProposition = precedente;
        }

        /*Ity Fonction ahafahana miverina any @ Proposition Principale miverina any @ branche mere izany.
         Classe GrandProposition miheriter an'ity fonction ity izay no mapatonga anazy misy virtual
         Sady @ zay lasa fonction polymorphisme reccursive le izy aveo*/
        public virtual Proposition RetourVersLaRacine()
        {
            if (precedentProposition != null)
                return precedentProposition.RetourVersLaRacine();
            else
                return this;
        }
    }
}
