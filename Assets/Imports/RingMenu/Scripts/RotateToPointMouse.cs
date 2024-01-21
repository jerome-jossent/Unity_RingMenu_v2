using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPointMouse : MonoBehaviour
{
    public Vector3 c;
    public Vector3 s;
    public Vector3 ss;
    public float a;

    public Vector2 mousePos_xy;
    public Vector2 center_xy;
    public Vector2 vector1;
    public Vector2 vector2;

    void Start()
    {
        c = transform.position;
    }

    void Update()
    {
        s = Input.mousePosition;
        s.z = Camera.main.nearClipPlane;
        ss = Camera.main.ScreenToWorldPoint(s);

        mousePos_xy = new Vector2(ss.x, ss.y);
        center_xy = new Vector2(c.x, c.y);

        vector1 = center_xy - mousePos_xy; // VectorToMoveTo
        vector2 = center_xy - new Vector2(transform.position.x, transform.position.y); // Vector at the center line of the bottle.
        vector2 = new Vector2(0, 1); // Vector at the center line of the bottle.

        a = Vector2.Angle(vector1.normalized, vector2.normalized);

        if (mousePos_xy.x < center_xy.x)
            a = -a;

        transform.rotation = Quaternion.Euler(0, 0, a + 180);
    }

    //void Update()
    //{
    //    //Vector3 direction = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
    //    //transform.LookAt(new Vector3(0, direction.x, direction.y));


    //    Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(c);
    //    transform.LookAt(transform.position + new Vector3(0, direction.x, direction.y));


    //    //s = Input.mousePosition;
    //    //s.z = Camera.main.nearClipPlane;

    //    //ss = Camera.main.ScreenToWorldPoint(s);


    //    //a= Mathf.Atan2(ss.y, ss.x) * Mathf.Rad2Deg;


    //    ////a = Vector2.Angle(c, ss) * Mathf.Rad2Deg;
    //    ////if (a < 0) a += 360;

    //    //transform.rotation = Quaternion.Euler(0, 0, a);
    //}
}
