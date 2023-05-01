using System;

public class MinesweeperModel {
    private static readonly int DEFAULT_GAME_SIZE = 10;
    private static readonly float DEFAULT_BOMB_DENSITY = 0.25f;

    private Cell[, ] myCells;

    private GameState myState = GameState.InSession;

    private bool myInitialColapseFlag;
    private int mySize;
    private int myNumBombs;

    private int myNumFlags;
    private int myFlagedBombs;

    public MinesweeperModel() : this(DEFAULT_GAME_SIZE, DEFAULT_BOMB_DENSITY) {}

    public MinesweeperModel(int theSize, float theBombDensity) {
        myCells = new Cell[theSize, theSize];
        myInitialColapseFlag = true;
        mySize = theSize;
        myNumFlags = 0;
        myFlagedBombs = 0;
        myNumBombs = (int)Math.Floor(theBombDensity * theSize * theSize);


        //Initialize the cells
        for (int y = 0; y < theSize; y++) {
            for (int x = 0; x < theSize; x++) {
                myCells[y, x] = new Cell();
            }
        }
    }

    public int GetSize() {
        return mySize;
    }

    public CellState GetCellState(int theX, int theY) {
        if (myCells[theY, theX].GetFlag()) {
            return CellState.Flaged;
        }

        else if (!myCells[theY, theX].GetColapseFlag()) {
            return CellState.Closed;
        }

        else if (myCells[theY, theX].GetBombFlag()) {
            return CellState.Bomb;
        }

        return (CellState)myCells[theY, theX].GetHint();
    }

    public GameState GetGameState() {        
        if (myState == GameState.Won || myState == GameState.Lost) {
            GameState temp = myState;
            myState = GameState.Results;
            return temp;
        }
        
        return myState;
    }

    public void ColapseCell(int theX, int theY) {
        if (myState == GameState.Results || theX < 0 || theX >= mySize || theY < 0 || theY >= mySize || myCells[theY, theX].GetColapseFlag() || myCells[theY, theX].GetFlag()) {
            return;
        }
        
        //Do initial setup if this is the first colapse of the game
        if (myInitialColapseFlag) {
            myInitialColapseFlag = false;

            //Place Bombs
            Random r = new Random();
            for(int n = 0; n < myNumBombs;) {
                int x = r.Next(mySize), y = r.Next(mySize);
                if (!myCells[y, x].GetBombFlag() && !(Math.Abs(x - theX) <= 1 && Math.Abs(y - theY) <= 1)) {
                    myCells[y, x].SetBombFlag(true);

                    //Increemnt Adjacent Hints
                    incrementHint(x - 1, y - 1);
                    incrementHint(x - 1, y);
                    incrementHint(x - 1, y + 1);
                    incrementHint(x, y - 1);
                    incrementHint(x, y + 1);
                    incrementHint(x + 1, y - 1);
                    incrementHint(x + 1, y);
                    incrementHint(x + 1, y + 1);

                    n++;
                }
            }
        }

        myCells[theY, theX].SetColapseFlag(true);

        if (myCells[theY, theX].GetBombFlag()) {
            myState = GameState.Lost;
            UncoverBombs();
            return;
        }

        //Colapse adjacent cells if hint is 0
        if (myCells[theY, theX].GetHint() == 0) {
            ColapseCell(theX - 1, theY - 1);
            ColapseCell(theX - 1, theY);
            ColapseCell(theX - 1, theY + 1);
            ColapseCell(theX, theY - 1);
            ColapseCell(theX, theY + 1);
            ColapseCell(theX + 1, theY - 1);
            ColapseCell(theX + 1, theY);
            ColapseCell(theX + 1, theY + 1);
        }
    }

    private void UncoverBombs() {
        for (int y = 0; y < mySize; y++) {
            for (int x = 0; x < mySize; x++) {
                if (myCells[y, x].GetBombFlag()) {
                    myCells[y, x].SetColapseFlag(true);
                }
            }
        }
    }

    private void incrementHint(int theX, int theY) {
        if (theX < 0 || theX >= mySize || theY < 0 || theY >= mySize) {
            return;
        }

        myCells[theY, theX].IncrementHint();
    }

    public void FlagCell(int theX, int theY) {
        if (myState == GameState.Results || theX < 0 || theX >= mySize || theY < 0 || theY >= mySize || myCells[theY, theX].GetColapseFlag()) {
            return;
        }

        myCells[theY, theX].ToggleFlag();

        //Update flag counts based on the state of the cell.
        if (myCells[theY, theX].GetFlag()) {
            myNumFlags++;
            if (myCells[theY, theX].GetBombFlag()) {
                myFlagedBombs++;
            }

        } else {
            myNumFlags--;
            if (myCells[theY, theX].GetBombFlag()) {
                myFlagedBombs--;
            }
        }

        if (myFlagedBombs == myNumFlags && myFlagedBombs == myNumBombs) {
            myState = GameState.Won;
        }
    }

    private class Cell {
        private bool myColapseFlag;
        private bool myFlag;
        private bool myBombFlag;
        private int myHint;

        public Cell() {
            myColapseFlag = false;
            myFlag = false;
            myBombFlag = false;
            myHint = 0;
        }

        public void SetColapseFlag(bool theColapseFlag) {
            myColapseFlag = theColapseFlag;
        }

        public bool GetColapseFlag() {
            return myColapseFlag;
        }

        public void ToggleFlag() {
            myFlag = !myFlag;
        }

        public bool GetFlag() {
            return myFlag;
        }

        public void SetBombFlag(bool theBombFlag) {
            myBombFlag = theBombFlag;
        }

        public bool GetBombFlag() {
            return myBombFlag;
        }

        public void IncrementHint() {
            myHint += 1;
        }

        public int GetHint() {
            return myHint;
        }
    }
}




