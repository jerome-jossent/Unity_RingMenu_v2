using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LoadDB : MonoBehaviour
{
    public UnityEvent _db_isRead;
    public string[] _premiersCode;
    public Dictionary<string, Code> _codes = new Dictionary<string, Code>();

    void Start()
    {
        //Load_DB();"BaseDefs13508_V2.fr"
    }

    /// <summary>
    /// use "_db_isRead" event to trig db exploitation
    /// </summary>
    public void _Read_DB(string filename)
    {
        StartCoroutine(Load_DB(filename));
    }

    IEnumerator Load_DB(string filename)
    {
        _codes = Load_DB(filename, out _premiersCode);
        _db_isRead?.Invoke();
        yield return null;
    }

    /// <summary>
    ///Dictionary<string, Code> codes = LoadDB.Load_DB("BaseDefs13508_V2.fr", out string[] premiersCode);
    /// </summary>
    public static Dictionary<string, Code> Load_DB(string filename, out string[] premiersCode)
    {
        TextAsset mytxtData = (TextAsset)Resources.Load(filename);
        if (mytxtData == null)
        {
            print($"{filename} not found");
            premiersCode = null;
            return null;
        }

        Dictionary<string, Code> codes = new Dictionary<string, Code>();

        //ligne par ligne
        string[] lines = mytxtData.text.Split(new char[] { '\n' });
        for (int i = 0; i < lines.Length; i++)
        {
            Code code = new Code(lines[i], i);
            if (!code.iscode)
                continue;
            codes.Add(code.code, code);
        }

        premiersCode = MakeLinksBetweenCodes(codes);
        return codes;
    }

    static string[] MakeLinksBetweenCodes(Dictionary<string, Code> codes)
    {
        List<string> premiersCode_list = new List<string>();

        foreach (var current_code in codes)
            foreach (var code in codes)
                if (IsChildren(current_code.Key, code.Key))
                {
                    current_code.Value.parent = code.Value;
                    code.Value.children.Add(current_code.Value);
                }

        //recherche les "sans parents" => "premiers individus"
        foreach (var code in codes)
            if (code.Value.parent == null)
                premiersCode_list.Add(code.Key);

        return premiersCode_list.ToArray();
    }

    static bool IsChildren(string enfant_potentiel, string parent_potentiel)
    {
        //parent (pas grand-parent)
        if (parent_potentiel.Length != enfant_potentiel.Length - 1)
            return false;

        for (int i = 0; i < parent_potentiel.Length; i++)
            if (parent_potentiel[i] != enfant_potentiel[i])
                return false;

        return true;
    }
}
