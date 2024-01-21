using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RingMenuJJ
{
    public class RingMenu
    {
        public static GameObject _DrawRingMenu(Dictionary<int, Anneau> anneaux,
                                               float angle_initial,
                                               bool sens_horaire)
        {
            GameObject rm = new GameObject();

            foreach (Anneau anneau in anneaux.Values)
            {
                GameObject ring = null;
                try
                {
                    if (anneau.r_ext == anneau.r_int)
                        continue;
                    ring = Ring.DrawRing(anneau, angle_initial, sens_horaire);
                    ring.transform.parent = rm.transform;
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message + "\n" + ex.StackTrace);
                }
            }

            RingMenu_Manager rmm = rm.AddComponent<RingMenu_Manager>();
            //rmm._ListAllButtons();

            return rm;
        }
    }
}