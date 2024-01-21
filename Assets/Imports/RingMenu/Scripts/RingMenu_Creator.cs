using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu_Creator : MonoBehaviour
{
    public RingMenu_Interactions rmI;
    public RingMenu_Manager rmM;
    public RingMenu_EditorMode rmEM;

    public string resourcesPathOrigin = "Emoticons";
    public string resourcesPath = "Interactions/Manager";
    public float icon_factor = 2;
    public float marge = 0.05f;
    public Color color = new Color(0.7f, 0.7f, 0.7f, 0.8f);

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
                                                                   color);
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
                                                                   color);
            rmEM.Add(rbEM);
        }
        rmEM.randomColors = true;
        rmEM.setDefaultColors = true;
        rmEM.Draw();// defaultcolor: true);

        rmEM.Boutons[0].events._OnClick.AddListener(Menu1);
    }
}
