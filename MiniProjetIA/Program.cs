using System;
using System.Collections.Generic;

namespace MiniProjetIA
{
    class Compilateur
    {
        public int nbrhypothese { get; set; }
        public string[] tabhypothese { get; set; }
        public string conclusion { get; set; }
        public List<Proposition> hypotheses { get; set; }
        public Proposition grandPropositionConclusion { get; set; }

        public Compilateur(int nbrhypothese)
        {
            this.nbrhypothese = nbrhypothese;
            tabhypothese = new string[nbrhypothese];
            hypotheses = new List<Proposition>();
            for(int i = 0; i < nbrhypothese; i++)
            {
                Console.WriteLine("Saisir hypothese numero " + (i+1));
                this.tabhypothese[i] = Console.ReadLine();
                Proposition hypothese = new GrandProposition("R",true);
                traitementHypothese(this.tabhypothese[i], 0,hypothese);
                if (((GrandProposition)hypothese).propositionDroite == null)
                    hypothese = new Proposition(((GrandProposition)hypothese).propositionGauche.nom,
                        ((GrandProposition)hypothese).isTrue);
                this.hypotheses.Add(hypothese);
            }
            Console.WriteLine("Saisir la conclusion : ");
            this.conclusion = Console.ReadLine();
            grandPropositionConclusion = new GrandProposition("R", true);
            traitementHypothese(this.conclusion, 0,grandPropositionConclusion);
            if (((GrandProposition)grandPropositionConclusion).propositionDroite == null)
                grandPropositionConclusion = new Proposition(((GrandProposition)grandPropositionConclusion).propositionGauche.nom,
                        ((GrandProposition)grandPropositionConclusion).isTrue);
        }

        public static int traitementHypothese(string hypothese,int i, Proposition grandProposition)
        {
            if (i < hypothese.Length && hypothese[i] != ')')
            {
                if (hypothese[i] == 'p')
                {
                    string prop = "";
                    prop += hypothese[i];
                    prop += hypothese[i + 1];
                    //Console.WriteLine(prop);
                    Proposition p = new Proposition(prop, true, ref grandProposition);
                    if (i > 0 && (hypothese[i - 1] == '&' || hypothese[i - 1] == '|' || hypothese[i - 1] == '-'))
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionDroite(p);
                        //Console.WriteLine(i + " Droite");
                    }
                    else
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionGauche(p);
                        //Console.WriteLine(i + " Gauche");
                    }
                    i++;
                }
                else if (hypothese[i] == '/')
                {
                    string prop = "";
                    prop += hypothese[i + 1];
                    prop += hypothese[i + 2];
                    //Console.WriteLine(prop);
                    Proposition p = new Proposition(prop, false, ref grandProposition);
                    if (i > 0 && (hypothese[i - 1] == '&' || hypothese[i - 1] == '|' || hypothese[i - 1] == '-'))
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionDroite(p);
                        //Console.WriteLine(i+" Droite");
                    }
                    else
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionGauche(p);
                        //Console.WriteLine(i + " Gauche");
                    }
                    i += 2;
                }
                else if (hypothese[i] == '(')
                {
                    //Console.WriteLine("Sous proposition");
                    string nom = "R";
                    nom += i;
                    GrandProposition grandPropositionSuivant = new GrandProposition(nom, true, ref grandProposition);
                    int iter = i;
                    i++;
                    i = traitementHypothese(hypothese, i, grandPropositionSuivant);
                    if (iter>0 && (hypothese[iter - 1] == '|' || hypothese[iter - 1] == '&' || hypothese[iter - 1] == '-'))
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionDroite(grandPropositionSuivant);
                        //Console.WriteLine(i + " Droite");
                    }
                    else
                    {
                        ((GrandProposition)grandProposition).InitialisationPropositionGauche(grandPropositionSuivant);
                        //Console.WriteLine(i + " Gauche");
                    }
                }
                else if (hypothese[i] == '|')
                {
                    Ou logique = new Ou();
                    ((GrandProposition)grandProposition).InitialisationLogique(logique);
                }
                else if (hypothese[i] == '&')
                {
                    Et logique = new Et();
                    ((GrandProposition)grandProposition).InitialisationLogique(logique);
                }
                else if (hypothese[i] == '-')
                {
                    Implication logique = new Implication();
                    ((GrandProposition)grandProposition).InitialisationLogique(logique);
                }
                return traitementHypothese(hypothese, i + 1, grandProposition);
            }
            else
            {
                return i;
            }
        }

        public bool TesteResolutionDeRobinson()
        {
            bool test = true;
            foreach(var element in hypotheses)
            {
                string h = element.Parcourir();
                if (h.Contains("-") || h.Contains("&"))
                {
                    test = false;
                }
            }
            return test;
        }

        public List<Proposition> Simplification(Proposition proposition)
        {
            List<Proposition> liste = new List<Proposition>();
            if (proposition.Parcourir().Length>3 && ((GrandProposition)proposition).logique.nom == "&"){
                Console.WriteLine("teste");
                (((GrandProposition)proposition).propositionGauche).InitialisationPropositionPrecedente(null);
                liste.Add(((GrandProposition)proposition).propositionGauche);
                (((GrandProposition)proposition).propositionDroite).InitialisationPropositionPrecedente(null);
                liste.Add(((GrandProposition)proposition).propositionDroite);
            }
            return liste;
        }

        public bool TesteSiFaux(List<Proposition> liste)
        {
            List<Proposition> listeLiteral = new List<Proposition>();
            foreach (var element in liste)
            {
                if (element.Parcourir().Length <= 3)
                    listeLiteral.Add(element);
                else
                    ((GrandProposition)element).InitialiserLaListeDesPropositions();
            }
            if (listeLiteral.Capacity > 0)
            {
                for(int i = 0; i < listeLiteral.Capacity-1; i++)
                {
                    listeLiteral[i].Negation();
                    for(int j = i; j < listeLiteral.Capacity; j++)
                    {
                        if (listeLiteral[i].Parcourir() == listeLiteral[j].Parcourir())
                            return false;
                    }
                }
            }
            return true;
        }

        public bool ModificationPrincipeDeRobinson()
        {
            while (!TesteResolutionDeRobinson())
            {
                List<Proposition> liste = new List<Proposition>();
                liste.AddRange(hypotheses);
                Console.WriteLine(liste.Capacity);
                foreach (var element in liste)
                {
                    if (element.Parcourir().Length > 3 && (element.Parcourir().Contains("-") || element.Parcourir().Contains("&")) && ((GrandProposition)element).logique.nom == "|")
                        element.Negation();
                    else if (element.Parcourir().Length > 3 && ((GrandProposition)element).logique.nom == "-")
                        ((GrandProposition)element).EquivalenceImplication();
                    if (Simplification(element).Capacity != 0)
                        hypotheses.Remove(element);
                    hypotheses.AddRange(Simplification(element));
                }
                AffichageDeTousLesHypotheses();
            }
            return TesteSiFaux(hypotheses);
        }

        public void PrincipeDeRobinson()
        {
            bool test = ModificationPrincipeDeRobinson();
            if (test)
            {
                Transformation(hypotheses, 0, 0);
            }
        }

        public void Transformation(List<Proposition> l, int iteration, int iteration2)
        {
            if (TesteSiFaux(l) && iteration < l.Capacity - 1)
            {
                List<Proposition> liste = new List<Proposition>();
                liste.AddRange(l);
                Proposition p;
                if (l[iteration2].Parcourir().Length > 3)
                {
                    p = new Proposition((((GrandProposition)l[iteration2]).listeProposition[iteration].Contains("/")) ?
                        ((GrandProposition)l[iteration2]).listeProposition[iteration].Replace("/", "") :
                        ((GrandProposition)l[iteration2]).listeProposition[iteration],
                        (((GrandProposition)l[iteration2]).listeProposition[iteration].Contains("/")) ? false : true);

                }
                else
                {
                    p = new Proposition(l[iteration2].nom, l[iteration2].isTrue);
                }
                p.Negation();
                foreach (var element in liste)
                {
                    if (l[iteration2] != element)
                    {
                        int i = l.IndexOf(element);
                        bool teste = false;
                        Proposition prop = Compilateur.Parcourir(null, l[i], p, ref teste);
                        if (prop != null)
                        {
                            Console.WriteLine("teste");
                            l[i] = prop;
                            p.Negation();
                            prop = Compilateur.Parcourir(null, l[iteration], p, ref teste);
                            if (prop != null)
                                l[iteration] = prop;
                            if (l[iteration].Parcourir().Length > 3)
                                ((GrandProposition)l[iteration]).InitialiserLaListeDesPropositions();
                        }
                        else
                        {
                            if (teste)
                            {
                                p.Negation();
                                prop = Compilateur.Parcourir(null, l[iteration], p, ref teste);
                                if (prop != null)
                                    l[iteration] = prop;
                                if (l[iteration].Parcourir().Length > 3)
                                    ((GrandProposition)l[iteration]).InitialiserLaListeDesPropositions();
                            }
                            Console.WriteLine("vide");
                        }
                        if (l[i].Parcourir().Length > 3)
                            ((GrandProposition)l[i]).InitialiserLaListeDesPropositions();
                    }
                }
                if (liste == l)
                {
                    iteration++;
                    if (l[iteration2].Parcourir().Length > 3)
                    {
                        if (iteration > ((GrandProposition)l[iteration2]).listeProposition.Capacity)
                        {
                            iteration2++;
                        }
                    }
                    else
                    {
                        iteration2++;
                    }
                }
                else
                {
                    iteration = 0;
                }
                //Transformation(l, iteration, iteration2);
            }
        }

        public static Proposition Parcourir(Proposition gp,Proposition p, Proposition literal,ref bool teste)
        {
            if(p.Parcourir() == literal.Parcourir())
            {
                if(p == ((GrandProposition)gp).propositionGauche)
                {
                    gp = ((GrandProposition)gp).ModusPonens(literal);
                    //Console.WriteLine(gp.Parcourir());
                }
                else
                {
                    gp = ((GrandProposition)gp).ModusTollens(literal);
                    //Console.WriteLine(gp.Parcourir());
                }
                teste = true;
                if(gp != null)
                    return gp.RetourVersLaRacine();
                else
                    return null;
            }
            else
            {
                if(p.Parcourir().Length > 3)
                {
                    Proposition prop = Parcourir(p, ((GrandProposition)p).propositionGauche, literal, ref teste);
                    if (prop == null)
                    {
                        if (p.Parcourir().Length > 3)
                            return Parcourir(p, ((GrandProposition)p).propositionDroite, literal, ref teste);
                    }
                    else
                    {
                        return prop;
                    }
                }
                return null;
            }
        }

        public void AffichageDeTousLesHypotheses()
        {
            int i = 1;
            foreach (var element in hypotheses)
            {
                if(element != null)
                {
                    Console.Write("(H" + i + "):");
                    Console.WriteLine(element.Parcourir());
                    i++;
                }
            }
            Console.WriteLine("--------------------------");
            Console.Write("(C):");
            Console.WriteLine(grandPropositionConclusion.Parcourir());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int nbrhypothese = 0;
            Console.WriteLine("Saisir le nombre d'hypothese : ");
            int.TryParse(Console.ReadLine(), out nbrhypothese);
            Compilateur compilateur = new Compilateur(nbrhypothese);
            compilateur.AffichageDeTousLesHypotheses();
            //Console.WriteLine(compilateur.TesteResolutionDeRobinson());
            compilateur.PrincipeDeRobinson();
            compilateur.AffichageDeTousLesHypotheses();
            //SimpleEssaiDeLoiEtDeValeurDeProposition();
        }

        static void SimpleEssaiDeLoiEtDeValeurDeProposition()
        {
            string h = "(p2|p3)|/p1";
            Proposition hypothese = new GrandProposition("R0", true);
            int iteration = 0;
            Compilateur.traitementHypothese(h, iteration,(GrandProposition)hypothese);
            //string conclusion = "(p1&p3)-p2";
            //GrandProposition hypotheseConclusion = new GrandProposition("R0", "X", true);
            Proposition p = new Proposition("p1", false);
            bool teste = false;
            hypothese = Compilateur.Parcourir(null, hypothese, p, ref teste);
            //Compilateur.traitementHypothese(conclusion, 0, hypotheseConclusion);
            //hypothese = ((GrandProposition)hypothese).Syllogisme(hypotheseConclusion);
            hypothese.Affichage();
            //((GrandProposition)hypothese).AffichageListeProposition();
            //hypothese.EquivalenceImplication();
            //hypothese.Affichage();
        }
    }
}
