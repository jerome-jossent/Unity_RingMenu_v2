using System.Collections.Generic;

public class Code
{
    string line;
    int index;

    public bool iscode = true;
    public string code;
    public string commentaire;
    public string titre;
    public string Q1;
    public string N;
    public string E;
    public string AD;
    public string RESTE;

    char[] sep = new char[1] { ';' };
    string sepT = ";T[";
    string sepQ1 = ";Q1[";
    string sepN = ";N[";
    string sepE = ";E[";
    string sepAD = ";AD[";
    string sep_fin = "]";

    string wild = "@";

    public Code parent;
    public List<Code> children = new List<Code>();

    public Code(string line, int index)
    {
        this.line = line;
        this.index = index;

        string[] elements = line.Split(sep, System.StringSplitOptions.RemoveEmptyEntries);
        if (elements.Length < 4)
        {
            iscode = false;
            return;
        }

        //code
        for (int i = 0; i < 5; i++)
            code += elements[i];
        code = code.Replace(wild, "");

        //commentaire
        if (elements.Length > 5)
            commentaire = elements[5];

        Set_Titre();
        Set_Q();
        Set_N();
        Set_E();
        Set_AD();
    }

    public override string ToString()
    {
        return $"line {index + 1}{code}:{titre}";
    }

    void Set_Titre() { titre = Cut(sepT); }
    void Set_Q() { Q1 = Cut(sepQ1); }
    void Set_N() { N = Cut(sepN); }
    void Set_E() { E = Cut(sepE); }
    void Set_AD() { AD = Cut(sepAD); }

    string Cut(string sep)
    {
        int carA, carZ;
        string txt;
        carA = line.IndexOf(sep);

        if (carA == -1)
            return null;

        txt = line.Substring(carA);
        carZ = txt.IndexOf(sep_fin);
        return txt.Substring(0, carZ - 1);
    }
}
