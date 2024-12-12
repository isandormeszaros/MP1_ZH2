using System.Collections.Generic;

namespace XJ3UMD;

class Program
{
    //2. feladat
    static int F2_Telitettseg(Vizsga vizsga)
    {
        return (int)((vizsga.jelentkezettekSzama / (double)vizsga.maxLetszam) * 100);
    }

    //5. feladat
    static bool F5_Tagozat(Vizsga vizsga, string tantargyNeve, bool vizsgaTipusa, string tagozat)
    {

        string vizsgaTagozat = vizsga.kod[0] switch
        {
            'N' => "nappali",
            'L' => "levelező",
            'T' => "távoktatás"
        };

        return vizsga.nev == tantargyNeve && vizsga.szobeli == vizsgaTipusa && vizsgaTagozat == tagozat.ToLower();
    }

    //7. feladat
    static List<string> F7_EgyediLista(List<Vizsga> vizsga)
    {
        List<string> EgyediLista = new List<string>();

        EgyediLista.AddRange(vizsga.Select(l => l.nev).Distinct());
        EgyediLista.Sort();
        EgyediLista.Reverse();

        return EgyediLista;
    }

    static void Main(string[] args)
    {
        //1. feladat
        List<Vizsga> vizsgak = new List<Vizsga>();
        using (StreamReader sr = new StreamReader("MP1_2024_25_ZH2_input.csv"))
        {
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(";");
                Vizsga ujVizsga = new Vizsga();
                ujVizsga.kod = sor[0];
                ujVizsga.nev = sor[1];
                ujVizsga.szobeli = sor[2] == "SZ";
                ujVizsga.idopont = DateTime.Parse(sor[3]);
                ujVizsga.maxLetszam = int.Parse(sor[4]);
                ujVizsga.jelentkezettekSzama = int.Parse(sor[5]);
                ujVizsga.terem = sor[6];
                vizsgak.Add(ujVizsga);
            }
            sr.Close();
        }

        //3. feldat
        Console.WriteLine("3. feladat");
        Console.WriteLine($"December elejei 70% fölötti vizsgák:");
        int osszTeli = 0;
        int osszMax = 0;

        foreach (Vizsga vizsga in vizsgak)
        {
            if (DateTime.Parse("2024.12.12") > vizsga.idopont && F2_Telitettseg(vizsga) > 70)
            {
                Console.WriteLine($"{vizsga.idopont:MMMM dd HH:MM} - {vizsga.kod} - {vizsga.nev} ({(vizsga.szobeli ? "Szóbeli" : "Írásbeli")}) - {vizsga.jelentkezettekSzama}/{vizsga.maxLetszam} ({F2_Telitettseg(vizsga)}%)");
                osszTeli += vizsga.jelentkezettekSzama;
                osszMax += vizsga.maxLetszam;
                //Console.WriteLine($"{vizsga.idopont:MMMM dd HH:MM} - {vizsga.kod} - {vizsga.nev} ({vizsga.szobeli}) - {vizsga.jelentkezettekSzama}/{vizsga.maxLetszam} ({F2_Telitettseg(vizsga)}%)");
            }
        }

        double atlagosTelitettseg = ((int)osszTeli / (double)osszMax) * 100;
        Console.WriteLine($"Ezen vizsgák átlagos telítettsége {atlagosTelitettseg:F2}%.");

        //4. feladat
        Console.WriteLine("\n4.feladat");
        int legnagyobbMagprog = -1;
        int legnagyobbMagprogIndex = 0;
        int legnagyobbBevGraf = -1;
        int legnagyobbBevGrafIndex = 0;
        vizsgak.Reverse();
        for (int i = 0; i < vizsgak.Count; i++)
        {
            if (vizsgak[i].nev == "Bevezetés a számítógépi grafikába" && vizsgak[i].maxLetszam == vizsgak[i].jelentkezettekSzama)
            {
                legnagyobbBevGraf = vizsgak[i].jelentkezettekSzama;
                legnagyobbBevGrafIndex = i;
            }

            //vizsgak.Where(v => v.nev == "Magasszintű programozási nyelvek 1" && v.maxLetszam == v.jelentkezettekSzama)
            if (vizsgak[i].nev == "Magasszintű programozási nyelvek I" && vizsgak[i].maxLetszam == vizsgak[i].jelentkezettekSzama)
            {
                legnagyobbMagprog = vizsgak[i].jelentkezettekSzama;
                legnagyobbMagprogIndex = i;
            }
        }

        if (legnagyobbBevGraf == legnagyobbMagprog)
            Console.WriteLine("Egyenlően jelentkeztek");

        else if (legnagyobbMagprog > legnagyobbBevGraf)
            Console.WriteLine($"{vizsgak[legnagyobbMagprogIndex].idopont:MMMM dd HH:MM} - Magasszintű programozási nyelvek 1 - {legnagyobbMagprog} fő");

        else
            Console.WriteLine($"{vizsgak[legnagyobbBevGrafIndex].idopont:MMMM dd HH:MM} - Bevezetés a számítógépi grafikába - {legnagyobbBevGraf} fő");

        Console.WriteLine(legnagyobbBevGraf);
        Console.WriteLine(legnagyobbMagprog);
        vizsgak.Reverse();


        //6. feladat
        Console.WriteLine("\n6.feladat");
        Console.Write("Tantárgy: ");
        string tantargyNeve = Console.ReadLine();
        Console.Write("Vizsga típusa: ");
        string vizsgaTipusa = Console.ReadLine().ToLower();
        bool randomValtozo = vizsgaTipusa == "szóbeli";
        Console.Write("Tagozat: ");
        string tagozatBeker = Console.ReadLine();

        Console.WriteLine("Az Ön számára javasolt vizsgák:");
        bool vanMegjelenithetoVizsga = false;
        foreach (Vizsga vizsga in vizsgak)
        {
            bool talalt = F5_Tagozat(vizsga, tantargyNeve, randomValtozo, tagozatBeker);
            if (talalt && vizsga.jelentkezettekSzama < vizsga.maxLetszam && DateTime.Parse("2024. 12. 12") < vizsga.idopont)
            {
                Console.WriteLine($"{vizsga.idopont:yyyy MM dd h:mm} - {vizsga.jelentkezettekSzama}/{vizsga.maxLetszam} ({vizsga.terem})");
                vanMegjelenithetoVizsga = true;
            }
        }

        if (!vanMegjelenithetoVizsga)
        {
            Console.WriteLine("Nincs megjeleníthető vizsga.");
        }

        //8. feladat
        Console.WriteLine("\n8.feladat");
        List<string> tantargyak = F7_EgyediLista(vizsgak);

        foreach (string tantargy in tantargyak)
        {
            var vizsgakTantargyhoz = vizsgak
                .Where(v => v.nev == tantargy && v.maxLetszam >= 10)
                .ToList();

            Vizsga legmagasabbLetszamuVizsga = vizsgakTantargyhoz
                .OrderByDescending(v => v.maxLetszam)
                .First();

            Console.WriteLine($"{legmagasabbLetszamuVizsga.nev}: {legmagasabbLetszamuVizsga.idopont:yyyy. MM. dd h:mm}, {legmagasabbLetszamuVizsga.maxLetszam}, {legmagasabbLetszamuVizsga.terem}");
        }
    }
}

