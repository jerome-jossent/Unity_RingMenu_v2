using RingMenuJJ;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RingButton_EditorMode
{
    public bool visible = true;
    public string name;
    [HideInInspector] public int button_index_on_ring_int;

    public Color button_color = Color.white;
    public bool icon_show;
    public Texture2D icon;
    public Label label = new Label();
    public Events events = new Events();

    internal RingButton_Manager ringButtonManager;

    public RingButton_EditorMode() { visible = true; }
    public RingButton_EditorMode(Texture2D icon) : this() { this.icon = icon; }
    public RingButton_EditorMode(Texture2D icon, Color button_color) : this(icon) { this.button_color = button_color; }
    public RingButton_EditorMode(Label label) : this() { this.label = label; }
    public RingButton_EditorMode(Label label, Color button_color) : this(label) { this.button_color = button_color; }
}

[Serializable]
public class Label
{
    public string label;
    public bool label_show = true;

    public Font label_font;
    public FontStyle label_fontStyle;
    public bool label_resizeTextForBestFit = true;
    public int label_fontSize;
    public Color label_color;

    public Label() { }

    public Label(string label, Color label_color)
    {
        this.label = label;
        this.label_color = label_color;
    }
}

[Serializable]
public class Events
{
    public UnityEngine.UI.Button.ButtonClickedEvent _OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
    public UnityEngine.UI.Button.ButtonClickedEvent _OnEnter = new UnityEngine.UI.Button.ButtonClickedEvent();
    public UnityEngine.UI.Button.ButtonClickedEvent _OnExit = new UnityEngine.UI.Button.ButtonClickedEvent();
}