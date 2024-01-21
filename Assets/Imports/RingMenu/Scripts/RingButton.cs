using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RingMenuJJ
{
    public class RingButton
    {
        public static GameObject DrawButton(float rayon_int,
                                                float rayon_ext,
                                                float angle_deg_debut,
                                                float angle_deg_fin,
                                                float marge,
                                                int? nbrsegments = null,
                                                string name = "Sector3D")
        {
            //j'ai estimé qu'une "courbure" ne se voyait plus en dessous de 5°
            if (nbrsegments == null)
                nbrsegments = Mathf.CeilToInt((angle_deg_fin - angle_deg_debut) / 5);

            if (rayon_ext > rayon_int)
            {
                Mesh mesh;
                //cas où il n'y a qu'1 => pas de marge
                if (angle_deg_debut == angle_deg_fin - 360)
                    mesh = CreateMesh(rayon_int, rayon_ext, angle_deg_debut, angle_deg_fin, 0, (int)nbrsegments);
                else
                    mesh = CreateMesh(rayon_int, rayon_ext, angle_deg_debut, angle_deg_fin, marge, (int)nbrsegments);
                GameObject obj = new GameObject("Sector3D");
                try
                {
                    obj.AddComponent<MeshCollider>().sharedMesh = mesh;
                    obj.AddComponent<MeshFilter>().sharedMesh = mesh;
                    Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
                    obj.AddComponent<MeshRenderer>().sharedMaterial = mat;
                }
                catch (System.Exception)
                {
                    return null;
                }

                obj.name = name;
                return obj;
            }
            else
                return null;
        }

        static Mesh CreateMesh(float rayon_int,
                               float rayon_ext,
                               float angle_debut_deg,
                               float angle_fin_deg,
                               float marge,
                               int nbrsegments)
        {
            #region infos
            //c'est la gestion de la marge qui rend les choses complexes
            //l'idée : le secteur (angle béta) a créer est centré sur l'axe Y
            //il sera pivoter plus tard
            //on créé les points du centre vers le sens horaire jusqu'à béta/2 = alpha
            //puis du centre vers le sens anti horaire idem jusqu'à - alpha
            #endregion

            #region variables
            nbrsegments /= 2;
            if (nbrsegments < 1) nbrsegments = 1;
            float M = marge;
            float Ri = rayon_int + M;
            float Re = rayon_ext;
            float a_0 = angle_debut_deg;
            float a_1 = angle_fin_deg;

            float beta = a_1 - a_0;
            float alpha = beta / 2;
            float rotation = alpha + angle_debut_deg;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();

            Vector2 O = new Vector2(0, 0);
            Math_JJ.Cercle ce = new Math_JJ.Cercle() { O = O, r = Re };
            Math_JJ.Cercle ci = new Math_JJ.Cercle() { O = O, r = Ri };

            // origine prime (à cause de la marge) sur l'axe Y (donc O'x=0)
            float angle_Opy = 90 - alpha;
            float Opy = (angle_Opy == 0) ? 0 : M / Mathf.Sin(angle_Opy / 180 * Mathf.PI);
            #endregion

            #region premiers points en partant du milieu (sur l'axe Y)
            Vector2 A = new Vector2(0, Ri);
            Vector2 B = new Vector2(0, Re);

            vertices.Add(A);
            vertices.Add(B);
            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);
            #endregion

            #region points => horaire
            int it = 0; //indextriangles
            for (int i = 1; i < nbrsegments + 1; i++)
            {
                float a_01 = alpha * i / nbrsegments;
                float a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
                Math_JJ.Droite d = new Math_JJ.Droite() { a = a1, b = Opy };
                A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
                B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);

                if (float.IsNaN(A.x) || float.IsNaN(A.y) || float.IsNaN(B.x) || float.IsNaN(B.y))
                    return null;

                vertices.Add(A);
                vertices.Add(B);
                uv.Add(Vector3.forward);
                uv.Add(Vector3.forward);
                triangles.AddRange(new int[] { it    , it + 1, it + 2, // a, b, c
                                           it + 1, it + 3, it + 2  // b, d, c
                                         });
                it += 2;
            }
            #endregion

            #region (encore) premiers points en partant du milieu (sur l'axe Y)
            A = new Vector2(0, Ri);
            B = new Vector2(0, Re);
            vertices.Add(A);
            vertices.Add(B);
            uv.Add(Vector3.forward);
            uv.Add(Vector3.forward);
            #endregion

            #region points => anti-horaire
            it += 2;
            for (int i = 1; i < nbrsegments + 1; i++)
            {
                float a_01 = alpha * i / nbrsegments;
                float a1 = Mathf.Tan((90 - a_01) / 180 * Mathf.PI);
                Math_JJ.Droite d = new Math_JJ.Droite() { a = a1, b = Opy };
                A = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ci, d, a_01);
                B = Math_JJ._Intersection2D_DroiteCoupantUnCercle(ce, d, a_01);

                if (float.IsNaN(A.x) || float.IsNaN(A.y) || float.IsNaN(B.x) || float.IsNaN(B.y))
                    return null;

                vertices.Add(new Vector3(-A.x, A.y));
                vertices.Add(new Vector3(-B.x, B.y));
                uv.Add(Vector3.forward);
                uv.Add(Vector3.forward);
                triangles.AddRange(new int[] { it,   it+2, it+1, //a, c, b
                                           it+1, it+2, it+3  //b, c, d
                                         });
                it += 2;
            }
            #endregion

            #region rotation de tous les points (= du secteur) à la position demandée
            for (int i = 0; i < vertices.Count; i++)
                vertices[i] = Quaternion.Euler(0, 0, rotation) * vertices[i];
            #endregion

            #region création du MESH
            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
            #endregion
        }

        public static GameObject DrawIcon(
            Texture icone, 
            float icon_factor,
            float r_ext, 
            float r_int, 
            float marge, 
            float angle, 
            float angleouverture)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            GameObject.DestroyImmediate(quad.GetComponent<Collider>());
            //rendu
            Material mat = new Material(Shader.Find("Unlit/TransparentColored"));
            mat.renderQueue = 3500;
            mat.mainTexture = icone;
            mat.SetColor("_Color", Color.white);
            quad.GetComponent<Renderer>().material = mat;

            //taille
            float hauteurmax = (r_ext - r_int - marge) / Mathf.Pow(2, 0.5f);
            float largeurmax = 2 * r_int * Mathf.Tan(angleouverture / 2 * Mathf.PI / 180) - marge / 2;
            //Debug.Log((int)hauteurmax + " / " + (int)largeurmax);
            float radius = Mathf.Min(hauteurmax, largeurmax) * icon_factor;
            quad.transform.localScale = new Vector3(radius, radius);

            //position
            float distance = (r_ext + r_int + marge) / 2;
            float x = distance * Mathf.Cos((90 + angle) / 180 * Mathf.PI);
            float y = distance * Mathf.Sin((90 + angle) / 180 * Mathf.PI);
            quad.transform.localPosition = new Vector3(x, y, -0.001f);

            return quad;
        }

        internal static GameObject DrawText(
            RingButton_Manager rb,
            RingButton_EditorMode bouton,
            float icon_factor,
            float r_ext,
            float r_int,
            float marge,
            float angle,
            float angle_position_deg,
            float angle_ouverture_deg
            )
        {
            GameObject canvas_go = new GameObject();
            canvas_go.transform.SetParent(rb.gameObject.transform);
            canvas_go.name = "canvas";

            //taille
            float hauteurmax = (r_ext - r_int - marge) / Mathf.Pow(2, 0.5f);
            float largeurmax = 2 * r_int * Mathf.Tan(angle_ouverture_deg / 2 * Mathf.PI / 180) - marge / 2;
            //Debug.Log((int)hauteurmax + " / " + (int)largeurmax);
            float radius = Mathf.Min(hauteurmax, largeurmax) * icon_factor;
            //canvas_go.transform.localScale = new Vector3(radius, radius);

            //position
            float distance = (r_ext + r_int + marge) / 2;
            float x = distance * Mathf.Cos((90 + angle) / 180 * Mathf.PI);
            float y = distance * Mathf.Sin((90 + angle) / 180 * Mathf.PI);
            canvas_go.transform.localPosition = new Vector3(x, y, -0.002f);
            //rendu
            //canvas_go.layer = 5;

            Canvas canvas = canvas_go.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(radius, radius);
            //rendu
            canvas.sortingOrder = 1;

            TMPro.TextMeshProUGUI text = canvas.gameObject.AddComponent<TMPro.TextMeshProUGUI>();
            text.alignment = TMPro.TextAlignmentOptions.Top;
            text.alignment = TMPro.TextAlignmentOptions.Midline;
            text.text = bouton.label.label;
            text.fontSizeMin = 0.01f;
            text.enableAutoSizing = true;
            text.color = bouton.label.label_color;

            return canvas_go;
        }
    }
}