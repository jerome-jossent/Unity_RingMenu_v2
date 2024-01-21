using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class RingMenu_Creator_v2 : MonoBehaviour
{
    public LoadDB db;
    public RingMenu_Interactions rmI;
    public RingMenu_Manager rmM;
    public RingMenu_EditorMode rmEM;

    public string resourcesPathOrigin = "Emoticons";
    public string resourcesPath = "Interactions/Manager";
    public float icon_factor = 2;

    [Range(0, 1)] public float marge = 0.05f;
    [Range(0, 1)] public float r_interne = 0.50f;
    [Range(0, 1)] public float r_externe = 0.100f;
    public Color colorButton = new Color(0.7f, 0.7f, 0.7f, 0.8f);
    public Color colorChild = new Color(0.7f, 0.7f, 0.7f, 0.8f);
    public Color colorLastChild = new Color(0.7f, 0.7f, 0.7f, 0.8f);

    public float t0;

    public void Start()
    {
        t0 = Time.time;
        db._db_isRead.AddListener(DB_ok);
        db._Read_DB("BaseDefs13508_V2.fr");
    }

    private void DB_ok()
    {
        if (db._premiersCode == null) return;

        List<Label> labels = new List<Label>();
        foreach (string premiercode in db._premiersCode)
            labels.Add(new Label(label: premiercode, label_color: Color.black));

        rmM = rmI.ringMenu_Manager;
        rmEM.Clear();
        rmEM.icon_factor = icon_factor;
        rmEM.marge = marge;
        rmEM.r_interne = r_interne;
        rmEM.r_externe = r_externe;

        foreach (Label label in labels)
        {
            RingButton_EditorMode rbEM = new RingButton_EditorMode(label, colorButton);
            rmEM.Add(rbEM);
        }
        rmEM.Draw();// defaultcolor: true);

        for (int i = 0; i < rmEM.Boutons.Length; i++)
        {
            string name = rmEM.Boutons[i].name;
            rmEM.Boutons[i].events._OnClick.AddListener(delegate { EnterCode(name); });
        }

    }

    private void EnterCode(string parentname)
    {
        Code codeparent = db._codes[parentname];
        if (codeparent.children.Count == 0)
        {
            Debug.Log(parentname);
            DB_ok();
            return;
        }
        if (codeparent.children.Count == 1)
        {
            EnterCode(codeparent.children.First().code);
            return;
        }

        rmM = rmI.ringMenu_Manager;
        rmEM.Clear();
        rmEM.icon_factor = icon_factor;
        rmEM.marge = marge;
        rmEM.r_interne = r_interne;
        rmEM.r_externe = r_externe;

        foreach (Code c in codeparent.children)
        {
            Color label_col = (c.children.Count > 0) ? colorChild : colorLastChild;

            Label label = new Label(label: c.code, label_color: label_col);

            RingButton_EditorMode rbEM = new RingButton_EditorMode(label, colorButton);
            rmEM.Add(rbEM);
        }
        rmEM.Draw();// defaultcolor: true);

        for (int i = 0; i < rmEM.Boutons.Length; i++)
        {
            string name = rmEM.Boutons[i].name;
            rmEM.Boutons[i].events._OnClick.AddListener(delegate { EnterCode(name); });
        }
    }

    public void Menu1()
    {
        rmM = rmI.ringMenu_Manager;

        //object[] icons = Resources.LoadAll("Emoticons", typeof(Texture2D));
        //object[] icons = Resources.LoadAll("Interactions/Manager", typeof(Texture2D));
        object[] icons = Resources.LoadAll(resourcesPath, typeof(Texture2D));
        //print(icons.Length);

        rmEM.Clear();
        rmEM.icon_factor = icon_factor;
        rmEM.marge = marge;

        foreach (var obj_icon in icons)
        {
            RingButton_EditorMode rbEM = new RingButton_EditorMode(obj_icon as Texture2D,
                                                                   colorButton);
            rmEM.Add(rbEM);
        }
        rmEM.Draw();// defaultcolor: true);

        rmEM.Boutons[0].events._OnClick.AddListener(Menu0);
    }

    private void Menu0()
    {
        rmM = rmI.ringMenu_Manager;

        object[] icons = Resources.LoadAll(resourcesPathOrigin, typeof(Texture2D));
        //print(icons.Length);

        rmEM.Clear();
        rmEM.icon_factor = 1;
        rmEM.marge = 0.05f;

        foreach (var obj_icon in icons)
        {
            RingButton_EditorMode rbEM = new RingButton_EditorMode(obj_icon as Texture2D,
                                                                   colorButton);
            rmEM.Add(rbEM);
        }
        rmEM.randomColors = true;
        rmEM.setDefaultColors = true;
        rmEM.Draw();// defaultcolor: true);

        rmEM.Boutons[0].events._OnClick.AddListener(Menu1);
    }
}
