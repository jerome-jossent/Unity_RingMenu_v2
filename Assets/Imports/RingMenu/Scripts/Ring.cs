using UnityEngine;

namespace RingMenuJJ
{
    public class Ring
    {
        public static GameObject DrawRing(Anneau anneau,
                                          float angle_initial,
                                          bool sens_horaire)
        {
            float r_int = anneau.r_int;
            float r_ext = anneau.r_ext;
            float marge = anneau.marge * (r_ext - r_int);

            GameObject go = new GameObject();
            go.name = "ring_" + anneau.index;

            int nbrboutons = anneau.butons_on_ring.Count;

            float angle_ouverture_deg = (float)360 / nbrboutons;

            if (sens_horaire)
                angle_initial = -angle_initial;

            float angle_position_deg_init = angle_initial + angle_ouverture_deg / 2;

            foreach (RingButton_EditorMode bouton in anneau.butons_on_ring.Values)
            {
                float angle_position_deg;
                if (sens_horaire)
                    angle_position_deg = angle_position_deg_init - (bouton.button_index_on_ring_int + 1) * angle_ouverture_deg;
                else
                    angle_position_deg = angle_position_deg_init + (bouton.button_index_on_ring_int - 1) * angle_ouverture_deg;

                //Debug.Log("Bouton \"" + bouton.name + "\" angle " + (int)angle_position_deg + " sur " + (int)angle_ouverture_deg + "°");

                GameObject btn;
                try
                {
                    btn = RingButton.DrawButton(r_int,
                                                r_ext,
                                                angle_position_deg,
                                                angle_position_deg + angle_ouverture_deg,
                                                marge);

                    if (btn == null) continue;

                    btn.name = go.name + "_btn:" + bouton.name;
                    btn.transform.parent = go.transform;

                    RingButton_Manager rb = btn.AddComponent<RingButton_Manager>();
                    bouton.ringButtonManager = rb;
                    rb._ring_index = anneau.index;
                    rb._index = bouton.button_index_on_ring_int;
                    rb._SetColors(bouton.button_color);

                    //icône
                    if (bouton.icon_show && bouton.icon != null)
                    {
                        try
                        {
                            Texture texture = bouton.icon;
                            GameObject icn = RingButton.DrawIcon(texture, 
                                                                anneau.icon_factor,
                                                                r_ext, 
                                                                r_int, 
                                                                marge,
                                                                angle_position_deg + angle_ouverture_deg / 2,
                                                                angle_ouverture_deg);
                            icn.transform.parent = rb.gameObject.transform;
                            rb._icone = icn;
                        }
                        catch (System.Exception ex)
                        {
                            Debug.Log(ex.Message + "\n" + ex.StackTrace);
                        }
                    }

                    //texte
                    if (bouton.label.label_show && bouton.label.label != "")
                    {
                        bouton.label.label_color.a = 1f;

                        GameObject txt = RingButton.DrawText(
                            rb,
                            bouton,
                            anneau.icon_factor,
                            r_ext, 
                            r_int,
                            marge,
                            angle_position_deg + angle_ouverture_deg / 2,
                            angle_position_deg,
                            angle_ouverture_deg);
                    }

                    //events
                    rb._OnClick = bouton.events._OnClick;
                    rb._OnEnter = bouton.events._OnEnter;
                    rb._OnExit = bouton.events._OnExit;
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message + "\n" + ex.StackTrace);
                }
            }
            return go;
        }
    }
}