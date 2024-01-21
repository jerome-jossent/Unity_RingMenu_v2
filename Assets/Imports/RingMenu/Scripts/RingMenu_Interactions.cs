using RingMenuJJ;
using UnityEngine;

public class RingMenu_Interactions : MonoBehaviour
{
    public RingMenu_Manager ringMenu_Manager;
    public bool debug;
    public string hitname;

    bool btnclick;
    bool btnunclick;

    void Update()
    {
        if (ringMenu_Manager == null)
        {
            ringMenu_Manager = GetComponentInChildren<RingMenu_Manager>();
            GameObject menu = ringMenu_Manager.gameObject;
            ringMenu_Manager._ListAllButtons();

            RingMenu_Manager[] rs = GetComponentsInChildren<RingMenu_Manager>();
            foreach (RingMenu_Manager r in rs)
            {
                if (r == ringMenu_Manager) continue;

                r._ListAllButtons();
                foreach (var item in r._buttons)
                    ringMenu_Manager._buttons.Add(item.Key, item.Value);
            }
        }

        if (Input.GetMouseButtonDown(0))
            btnclick = true;

        if (Input.GetMouseButtonUp(0))
            btnunclick = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        

        ringMenu_Manager._InteractionManager(ray, btnclick, btnunclick, out hitname, debug);
        btnclick = false;
        btnunclick = false;
    }
}