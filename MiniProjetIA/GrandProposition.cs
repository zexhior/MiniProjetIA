using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProjetIA
{
    //Classe GrandProposition miheriter an'ny Classe Proposition
    class GrandProposition : Proposition
    {
        //Ex: p1|p2
        
        //Proposition Gauche no anasivana ny proposition izay a gauche, @ Ex izao p1 ny proposition Gauche
        public Proposition propositionGauche { get; set; }
        
        //logique anasivana ny logique entre an'ilay proposition roa
        public ILogique logique { get; set; }
        
        //Proposition Droite no anasivana ny proposition izay ao @ droite, @ Ex izao p2 ny proposition Droite
        public Proposition propositionDroite { get; set; }
        
        //ListeProposition dia anasivana ny proposition rehetra ao anatiny ny hypothese 
        //ato zao ohatra ny proposition mipetraka ao anaty listeProposition dia p1 sy p2 fa string ireo fa tsy classe
        public List<String> listeProposition { get; set; }
        
        //Constructeur an'ny classe GrandProposition raha tsy asina proposition precedente mitovy @
        //ao @ classe Proposition
        public GrandProposition(string nom, bool istrue,Proposition propositionGauche,ILogique logique,
            Proposition propositionDroite) : base(nom, istrue)
        {
            this.propositionGauche = propositionGauche;
            this.logique = logique;
            this.propositionDroite = propositionDroite;
            listeProposition = new List<String>();
        }

        //Constructeur an'ny classe GrandPrososition raha asina proposition precedente
        public GrandProposition(string nom, bool istrue, ref Proposition precedente, Proposition propositionGauche, ILogique logique,
            Proposition propositionDroite) : base(nom, istrue, ref precedente)
        {
            this.propositionGauche = propositionGauche;
            this.logique = logique;
            this.propositionDroite = propositionDroite;
            listeProposition = new List<String>();
        }

        //Constructeur an'ny classe GrandProposition raha tsy initialisena ny proposition precedente,
        //ny propositionGauche sy Droite ary ny Logique
        public GrandProposition(string nom, bool istrue):base(nom, istrue)
        {
            listeProposition = new List<String>();
        }
        //Constructeur an'ny classe GrandProposition raha tsy initialisena ny propositionGauche sy
        //Droite ary ny Logique
        public GrandProposition(string nom, bool istrue, ref Proposition precedente) : base(nom, istrue, ref precedente)
        {
            listeProposition = new List<String>();
        }

        //Ato no initialisena ny listeDesPropositions
        public void InitialiserLaListeDesPropositions()
        {
            //Fafana aloha raha ohatra ka nisy modification ohatra hoe voafafa ny proposition sasany ao anatin'ilay
            //hypothese de mifafa dia averenina amboarina indray ny liste de proposition ao anaty hypothese
            listeProposition.Clear();

            //hypothese asina ny version string le hypothese
            string hypothese = this.Parcourir();

            //tetezina ilay string hypothese mba ijerena tsirairay ny proposition ao anaty hypothese
            for(int i = 0; i < hypothese.Length; i++)
            {
                string prop = "";
                //raha ohatra '/' ilay hypothese[i] dia atsofoka ao anaty liste de proposition ohatra /p1
                if (hypothese[i] == '/')
                {
                    prop += hypothese[i];
                    prop += hypothese[i + 1];
                    prop += hypothese[i + 2];
                    i += 2;
                }
                //rahah ohatra 'p' ilay hypothese[i] dia atsofoka ao anaty liste de proposition ohatra p1
                else if (hypothese[i] == 'p')
                {
                    prop += hypothese[i];
                    prop += hypothese[i + 1];
                    i ++;
                }
                //rehefa ohatra ka tsy vide ilay proposition sy mbola tsy misy an'ilay proposition ao 
                //anatin'ilay listeProposition dia atsofoka ao ny prop
                if(prop != "" && !listeProposition.Contains(prop))
                    listeProposition.Add(prop);
            }
        }

        //InitialisationPropositionGauche rehefa tsy atao par Constructeur
        public void InitialisationPropositionGauche(Proposition p)
        {
            this.propositionGauche = p;
        }

        //InitialisationPropositionDroite rehefa tsy atao par Constructeur
        public void InitialisationPropositionDroite(Proposition p)
        {
            this.propositionDroite = p;
        }

        //InitialisationLogique rehefa tsy atao par Constructeur
        public void InitialisationLogique(ILogique l)
        {
            this.logique = l;
        }

        //Fonction heriter avy @ classe Proposition ahafahana mapiseho ny hypothese sous forme string
        public override string Parcourir()
        {
            return "(" + propositionGauche.Parcourir() + logique.nom + propositionDroite.Parcourir() + ")";
        }

        //affichena ny listeProposition
        public void AffichageListeProposition()
        {
            InitialiserLaListeDesPropositions();
            foreach (var element in listeProposition)
            {
                //Console.WriteLine("data");
                Console.Write(element + " ");
            }
        }

        //Affichage ny hypotheses @ console directement
        public override void Affichage()
        {
            Console.Write("(");
            this.propositionGauche.Affichage();
            this.logique.Affichage();
            this.propositionDroite.Affichage();
            Console.Write(")");
        }

        //Affichena PropositionGauche
        public void AffichagePropositionGauche()
        {
            this.propositionGauche.Affichage();
        }

        //Affichena PropositionDroite
        public void AffichagePropositionDroite()
        {
            this.propositionDroite.Affichage();
        }

        //Affichena Logique
        public void AffichageLogique()
        {
            this.logique.Affichage();
        }

        //Fonction heritena avy @ classe Proposition ahafahana miverina any @ branche mere
        public override Proposition RetourVersLaRacine()
        {
            if (precedentProposition != null)
                return precedentProposition.precedentProposition;
            else
                return this;
        }

        ////////////////////////////////LES LOIS///////////////////////////////////////////

        //Regle de Contrapositivite
        public void Contrapositivite()
        {
            Proposition droite;
            //raha ohatra ka de Type Proposition le propositionGauche dia
            if (propositionGauche.GetType() != this.GetType())
            {
                droite = new Proposition(this.propositionGauche.nom, this.propositionGauche.isTrue);
                //jerena aloha hoe inona ny type an le proposition droite
                //raha ohatra ka Proposition dia
                if (propositionDroite.GetType() != this.GetType())
                {
                    //afamadihana fotsiny ny proposition droite sy gauche
                    //Ex: p1-p2 => p2-p1
                    Proposition gauche;
                    gauche = new Proposition(this.propositionDroite.nom, this.propositionDroite.isTrue);
                    propositionDroite = new Proposition(droite.nom, droite.isTrue);
                    propositionGauche = new Proposition(gauche.nom, gauche.isTrue);
                }
                //raha ohatra ka GrandProposition dia
                else
                {
                    //mamorona grandproposition gauche aloha dia omena azy ny valeur propositionDroite
                    //aveo afamadika @ zay propositionGauche sy droite @ alalany grandproposition gauche
                    //Ex: p1-(p2|p3) => (p2|p3)-p1
                    GrandProposition gauche;
                    gauche = new GrandProposition(
                        ((GrandProposition)propositionDroite).nom,
                        ((GrandProposition)propositionDroite).isTrue,
                        ((GrandProposition)propositionDroite).propositionGauche,
                        ((GrandProposition)propositionDroite).logique,
                        ((GrandProposition)propositionDroite).propositionDroite);
                    Console.WriteLine("tada");
                    propositionDroite = new Proposition(droite.nom, droite.isTrue);
                    propositionGauche = new GrandProposition(gauche.nom, gauche.isTrue,gauche.propositionGauche,
                        gauche.logique,gauche.propositionDroite);
                }

                //Negation propositionGauche sy Droite 
                //Ex: p1-p2 => /p1-/p2; p1-(p2|p3) => /p1-(/p2&/p3)
                propositionGauche.Negation();
                propositionDroite.Negation();
            }
            //Raha ohatra ka type Grandproposition le propositionDroite
            else
            {
                //mbola instanciena en tant que GrandProposition aloha droite
                droite = new GrandProposition(
                    ((GrandProposition)propositionGauche).nom,
                    ((GrandProposition)propositionGauche).isTrue,
                    ((GrandProposition)propositionGauche).propositionGauche,
                    ((GrandProposition)propositionGauche).logique,
                    ((GrandProposition)propositionGauche).propositionDroite
                    );
                //rehefa type Proposition le propositionGauche
                if (propositionDroite.GetType() != this.GetType())
                {
                    //Ampifamadihana le gauche sy droite
                    propositionGauche = new Proposition(this.propositionDroite.nom,
                        this.propositionDroite.isTrue);
                    propositionDroite = new GrandProposition(droite.nom, droite.isTrue,
                        ((GrandProposition)droite).propositionGauche,
                        ((GrandProposition)droite).logique,
                        ((GrandProposition)droite).propositionDroite
                        );
                }
                else
                {
                    GrandProposition gauche;
                    gauche = new GrandProposition(
                        ((GrandProposition)propositionDroite).nom,
                        ((GrandProposition)propositionDroite).isTrue,
                        ((GrandProposition)propositionDroite).propositionGauche,
                        ((GrandProposition)propositionDroite).logique,
                        ((GrandProposition)propositionDroite).propositionDroite);
                    propositionGauche = new GrandProposition(gauche.nom, gauche.isTrue, gauche.propositionGauche,
                        gauche.logique, gauche.propositionDroite);
                    propositionDroite = new GrandProposition(droite.nom, droite.isTrue,
                        ((GrandProposition)droite).propositionGauche,
                        ((GrandProposition)droite).logique,
                        ((GrandProposition)droite).propositionDroite
                        );
                }
                propositionGauche.Negation();
                propositionDroite.Negation();
            }
        }

        public override void Negation()
        {
            if (logique.nom == "&" || logique.nom == "|")
            {
                this.propositionGauche.Negation();
                if (logique.nom == "|")
                {
                    logique = new Et();
                }
                else if (logique.nom == "&")
                {
                    logique = new Ou();
                }
                this.propositionDroite.Negation();
            }
            else if (logique.nom == "-")
            {
                this.Contrapositivite();
            }
        }

        public void EquivalenceImplication()
        {
            propositionGauche.Negation();
            logique = new Ou();
        }

        public Proposition ModusPonens(Proposition p)
        {
            //if(p.GetType() == propositionGauche.GetType() && logique.nom == "-")
            //{
                //Console.WriteLine("Ok");
                if (propositionGauche.Parcourir() == p.Parcourir())
                {
                    if (propositionDroite.GetType() == this.GetType())
                    {
                        propositionGauche = ((GrandProposition)propositionDroite).propositionGauche;
                        logique = ((GrandProposition)propositionDroite).logique;
                        propositionDroite = ((GrandProposition)propositionDroite).propositionDroite;
                        propositionGauche.InitialisationPropositionPrecedente(this);
                        propositionDroite.InitialisationPropositionPrecedente(this);
                    }
                    else
                    {
                        Console.WriteLine("Chemin 2");
                        if(((GrandProposition)precedentProposition).propositionDroite == this)
                        {
                            propositionGauche.InitialisationPropositionPrecedente(precedentProposition);
                            return (Proposition)propositionDroite;
                        }
                    }
                }
            //}
            return (GrandProposition)this;
        }

        public Proposition ModusTollens(Proposition p)
        {
            //if (p.GetType() == propositionDroite.GetType() && logique.nom == "-")
            //{
                //p.Negation();
                //Console.WriteLine(propositionDroite.Parcourir() + "8" + p.Parcourir());
                if (propositionDroite.Parcourir() == p.Parcourir())
                {
                    if (propositionGauche.GetType() == this.GetType())
                    {
                        //((GrandProposition)propositionGauche).Negation();
                        logique = ((GrandProposition)propositionGauche).logique;
                        propositionDroite = ((GrandProposition)propositionGauche).propositionDroite;
                        propositionGauche = ((GrandProposition)propositionGauche).propositionGauche;
                        propositionGauche.InitialisationPropositionPrecedente(precedentProposition);
                        propositionDroite.InitialisationPropositionPrecedente(precedentProposition);
                    }
                    else
                    {
                        //propositionGauche.Negation();
                        propositionGauche.InitialisationPropositionPrecedente(precedentProposition);
                        return (Proposition)propositionGauche;
                    }
                }
            //}
            return (GrandProposition)this;
        }

        public GrandProposition Syllogisme(GrandProposition gp)
        {
            if (propositionGauche.Parcourir() == gp.propositionDroite.Parcourir() && this.logique.nom == "-"
                && gp.logique.nom == "-")
            {
                GrandProposition grandProposition = new GrandProposition(this.nom,  
                    this.isTrue, gp.propositionGauche,this.logique, this.propositionDroite);
                return grandProposition;
            }
            return this;
        }

        /*Ity tsy mande fa apetrako eto ihany sao dia ilaina any aorina any
        public override Proposition ParcoursPrincipeDeRobinson(string proposition)
        {
            if (propositionGauche.ParcoursPrincipeDeRobinson(proposition) != null)
            {
                return propositionGauche.ParcoursPrincipeDeRobinson(proposition);
            }
            else if (propositionDroite.ParcoursPrincipeDeRobinson(proposition) != null)
            {
                return propositionDroite.ParcoursPrincipeDeRobinson(proposition);
            }
            return null;
        }*/
    }
}
