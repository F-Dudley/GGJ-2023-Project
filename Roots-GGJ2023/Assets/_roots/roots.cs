using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public static class ROOTS
{
    public const string R = "R";
    public const string O = "O";
    public const string T = "T";
    public const string S = "S";

    public const int r = 1;
    public const int o = 2;
    public const int oo = 3;
    public const int t = 4;
    public const int s = 5;
}

namespace RoOtS
{
    public class roots : MonoBehaviour
    {
        [Header(ROOTS.R + ROOTS.O + ROOTS.O + ROOTS.T + ROOTS.S)]
        [SerializeField] private int RooTS = 1;

        [Header(ROOTS.R)]
        [SerializeField] private Sprite r;
        [SerializeField] private AudioClip R;

        [Header(ROOTS.O)]
        [SerializeField] private Sprite o;
        [SerializeField] private AudioClip O;

        [Header(ROOTS.O)]
        [SerializeField] private Sprite oo;
        [SerializeField] private AudioClip OO;

        [Header(ROOTS.T)]
        [SerializeField] private Sprite t;
        [SerializeField] private AudioClip T;

        [Header(ROOTS.S)]
        [SerializeField] private Sprite s;
        [SerializeField] private AudioClip S;

        [Header("root")]
        [SerializeField] private GameObject root;
        [SerializeField] private AudioSource rOOts;

        private void Start()
        {
            Debug.Log(ROOTS.R);
            Debug.Log(ROOTS.O);
            Debug.Log(ROOTS.O);
            Debug.Log(ROOTS.T);
            Debug.Log(ROOTS.S);
        }

        private void Update()
        {
            const int roots = (ROOTS.r + ROOTS.o + ROOTS.o + ROOTS.t + ROOTS.s)-(ROOTS.r + ROOTS.o + ROOTS.o + ROOTS.t + ROOTS.s);

            if (Input.GetMouseButtonDown(roots))
            {

                Vector3 rOOt = Input.mousePosition;
                Vector3 Root = Camera.main.ScreenToWorldPoint(rOOt);
                Vector2 rooT = new Vector2(Root.x, Root.y);

                RaycastHit2D rooTs = Physics2D.Raycast(rooT, Vector2.zero);
                if (rooTs.collider == null) return;

                if (rooTs.collider.gameObject.tag.Equals("root"))
                {
                    Debug.Log(ROOTS.R + ROOTS.O + ROOTS.O + ROOTS.T);

                    RooTS = RooTS + ROOTS.r;

                    if (RooTS > string.Concat(ROOTS.R, ROOTS.O, ROOTS.O, ROOTS.T, ROOTS.S).Length)
                    {
                        RooTS = ROOTS.r;
                    }

                    switch (RooTS)
                    {
                        case ROOTS.r:
                            R___();
                            break;
                        case ROOTS.o:
                            _O___();
                            break;
                        case ROOTS.oo:
                            __O__();
                            break;
                        case ROOTS.t:
                            ___T_();
                            break;
                        case ROOTS.s:
                            ____S();
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        #region roots
        #region R
        private void R___()
        {
            root.GetComponent<SpriteRenderer>().sprite = r;
            rOOts.PlayOneShot(R);
        }
        #endregion

        #region O
        private void _O___()
        {
            root.GetComponent<SpriteRenderer>().sprite = o;
            rOOts.PlayOneShot(O);
        }
        #endregion

        #region OO
        private void __O__()
        {
            root.GetComponent<SpriteRenderer>().sprite = oo;
            rOOts.PlayOneShot(OO);
        }
        #endregion

        #region T
        private void ___T_()
        {
            root.GetComponent<SpriteRenderer>().sprite = t;
            rOOts.PlayOneShot(T);
        }
        #endregion

        #region S
        private void ____S()
        {
            root.GetComponent<SpriteRenderer>().sprite = s;
            rOOts.PlayOneShot(S);
        }
        #endregion
        #endregion
    }
}
