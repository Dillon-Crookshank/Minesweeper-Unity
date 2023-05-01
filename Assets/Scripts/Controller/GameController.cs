using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {
    [SerializeField] private Sprite[] mySprites;
    [SerializeField] private Vector2 Origin;
    private RectangleComponent[,] myGrid;

    private MinesweeperModel model;

    public void Start() {
        model = new MinesweeperModel();
        myGrid = new RectangleComponent[model.GetSize(), model.GetSize()];
        for (int y = 0; y < model.GetSize(); y++) {
            for (int x = 0; x < model.GetSize(); x++) {
                myGrid[y, x] = new RectangleComponent("Grid(" + x + ", " + y + ")", mySprites[9], Origin.x + 1.0f * x, Origin.y - 1.0f * y, 1.0f, 1.0f);
                myGrid[y, x].RegisterModel(model);
                myGrid[y, x].RegisterGridCoordinates(x, y);
            }
        }
    }



    public void Update() {
        for (int y = 0; y < model.GetSize(); y++) {
            for (int x = 0; x < model.GetSize(); x++) {
                myGrid[y, x].SetSprite(mySprites[(int)model.GetCellState(x, y)]);
            }
        }

        if (model.GetGameState() == GameState.Won) {
            Debug.Log("You Win!");
        }
            
        else if (model.GetGameState() == GameState.Lost) {
            Debug.Log("You Lost!");
        }

        if (Input.GetKeyDown(KeyCode.End)) {
            model = new MinesweeperModel();

            //Give the new model instance to the view
            for (int y = 0; y < model.GetSize(); y++) {
                for (int x = 0; x < model.GetSize(); x++) {
                    myGrid[y, x].RegisterModel(model);
                }
            }
        }
    }
}

public enum CellState {
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Closed,
    Flaged,
    Bomb
}

public enum GameState {
    InSession,
    Won,
    Lost, 
    Results
}
