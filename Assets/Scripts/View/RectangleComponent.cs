using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RectangleComponent {
    private GameObject myGameObject;
    private Transform myTransform;
    private BoxCollider2D myCollider;
    private SpriteRenderer myRenderer;

    private MouseListener myListener;

    private string myName;

    private float myX;
    private float myY;
    private float myW;
    private float myH;

    private int myXGrid;
    private int myYGrid;

    public RectangleComponent(string theName, Sprite theSprite, float theX, float theY, float theW, float theH) {
        myName = theName;
        
        myGameObject = new GameObject(theName, typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(MouseListener));
        myTransform = myGameObject.GetComponent<Transform>();
        myCollider = myGameObject.GetComponent<BoxCollider2D>();
        myRenderer = myGameObject.GetComponent<SpriteRenderer>();
        myListener = myGameObject.GetComponent<MouseListener>();

        myListener.SetParent(this);

        myRenderer.sprite = theSprite;

        SetPosition(theX, theY);
        SetScale(theW, theH);

        myCollider.size = new Vector2(1, 1);
    }

    public void SetPosition(float theX, float theY) {
        myX = theX;
        myY = theY;
        myTransform.localPosition = new Vector3(theX, theY, 0);
    }

    public void SetScale(float theW, float theH) {
        myW = theW;
        myH = theH;
        myTransform.localScale = new Vector3(theW, theH, 1);
    }

    public void SetColor(Color theColor) {
        myRenderer.color = theColor;
    }

    public void SetSprite(Sprite theSprite) {
        myRenderer.sprite = theSprite;
    }

    public void RegisterModel(MinesweeperModel theModel) {
        myListener.GiveModel(theModel);
    }

    public void RegisterGridCoordinates(int theX, int theY) {
        myListener.GiveGridCoordinates(theX, theY);
    }

    private class MouseListener : MonoBehaviour {
        private RectangleComponent myParent;

        MinesweeperModel myModel;
        private int myXGrid;
        private int myYGrid;

        private bool myLeftHeldFlag = false;
        private bool myRightHeldFlag = false;

        public void SetParent(RectangleComponent theParent) {
            myParent = theParent;
        }

        public void GiveModel(MinesweeperModel theModel) {
            myModel = theModel;
        }

        public void GiveGridCoordinates(int theX, int theY) {
            myXGrid = theX;
            myYGrid = theY;
        }

        public void OnMouseOver() {
            //If neither mouse button is held down, do the regular mouse over action
            if (!myLeftHeldFlag && !myRightHeldFlag) {
                myParent.SetColor(Color.green);
            }
            
            if (Input.GetMouseButton(0)) {
                if (!myLeftHeldFlag) {
                    //Left Mouse Pressed Action
                    myModel.ColapseCell(myXGrid, myYGrid);

                    myLeftHeldFlag = true;
                    myParent.SetColor(Color.blue);
                }
            } else {
                if (myLeftHeldFlag) {
                    //Left Mouse Released Action
                    myLeftHeldFlag = false;
                    myParent.SetColor(Color.green);
                }
            }

            if (Input.GetMouseButton(1)) {
                if (!myRightHeldFlag) {
                    //Right Mouse Pressed Action
                    myModel.FlagCell(myXGrid, myYGrid);

                    myRightHeldFlag = true;
                    myParent.SetColor(Color.red);
                }
            } else {
                if (myRightHeldFlag) {
                    //Right Mouse Released Action
                    myRightHeldFlag = false;
                    myParent.SetColor(Color.green);
                }
            }
        }

        public void OnMouseExit() {
            myParent.SetColor(Color.white);
        }
    }
}