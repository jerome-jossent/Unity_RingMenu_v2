using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Anneau
{
    public int index;
    public Dictionary<int, RingButton_EditorMode> butons_on_ring;
    public float r_int;
    public float r_ext;
    public float marge;
    public float icon_factor;

    public Anneau(int index, float R_interne, float R_externe, float marge, float icon_factor)
    {
        this.index = index;
        r_int = R_interne;
        r_ext = R_externe;
        this.marge = marge;
        this.icon_factor = icon_factor;
        butons_on_ring = new Dictionary<int, RingButton_EditorMode>();
    }
}