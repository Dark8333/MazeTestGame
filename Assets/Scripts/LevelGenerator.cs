using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    #region Fields
    [SerializeField] private int amountOfWalls;
    [SerializeField] private int amountOfPeas;
    [SerializeField] private int amountOfEnemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject peas;
    [SerializeField] private GameObject enemy;

    private bool mazeBuilt = false;

    private bool gameStarted = false;

    private int[,,] cellsArray = new int[10, 10, 2];
    
    public System.Random SysRandom = new System.Random();
    #endregion

    #region PrivateMethods
    /// <summary>
    /// This method creates a maze on the display from the matrix of a maze.
    /// </summary>
    private void DisplayÑells()
    {
        var allWalls = GameObject.FindGameObjectsWithTag("Wall");
        for (int counter = allWalls.Length -1; counter >=0; counter--)
        {
            Destroy(allWalls[counter].gameObject);
        }

        GameObject newWall;
        
        for (int y = 0; y <= 9; y++)
        {
            for (int x = 0; x <= 9; x++)
            {
                switch (cellsArray[x, y, 0])
                {
                    case 1:
                        newWall = Instantiate(wall);
                        newWall.transform.position = new Vector3Int(x - 5, y - 5, 0);
                        break;
                    case 2:
                        newWall = Instantiate(wall);
                        newWall.transform.rotation = new Quaternion(0, 0, 90, 90);
                        newWall.transform.position = new Vector3Int(x - 5, y - 5, 0);
                        break;
                    case 3:
                        newWall = Instantiate(wall);
                        newWall.transform.position = new Vector3Int(x - 5, y - 5, 0);
                        newWall = Instantiate(wall);
                        newWall.transform.rotation = new Quaternion(0, 0, 90, 90);
                        newWall.transform.position = new Vector3Int(x - 5, y - 5, 0);
                        break;
                }
            }
        }

        for (int count = -5; count < 5; count++)
        {
            newWall = Instantiate(wall);
            newWall.transform.position = new Vector3Int(count, 5, 0);

            newWall = Instantiate(wall);
            newWall.transform.rotation = new Quaternion(0, 0, 90, 90);
            newWall.transform.position = new Vector3Int(5, count, 0);

            newWall = Instantiate(wall);
            newWall.transform.position = new Vector3Int(count, -5, 0);

            newWall = Instantiate(wall);
            newWall.transform.rotation = new Quaternion(0, 0, 90, 90);
            newWall.transform.position = new Vector3Int(-5, count, 0);
        }
    
    }

    /// <summary>
    /// This method creates a wall in the matrix of a maze
    /// </summary>
    private void CreateWall(int xCell, int yCell, Side side)
    {
        int x = xCell + 5;
        int y = yCell + 5;

        switch (side)
        {
            case Side.Up:
                if (cellsArray[x, y + 1, 0] == 2)
                {
                    cellsArray[x, y + 1, 0] = 3;
                }
                else if (cellsArray[x, y + 1, 0] == 0)
                {
                    cellsArray[x, y + 1, 0] = 1;
                }
                break;
            case Side.Down:
                if (cellsArray[x, y, 0] == 2)
                {
                    cellsArray[x, y, 0] = 3;
                }
                else if (cellsArray[x, y, 0] == 0)
                {
                    cellsArray[x, y, 0] = 1;
                }
                break;
            case Side.Left:
                if (cellsArray[x, y, 0] == 1)
                {
                    cellsArray[x, y, 0] = 3;
                }
                else if (cellsArray[x, y, 0] == 0)
                {
                    cellsArray[x, y, 0] = 2;
                }
                break;
            case Side.Right:
                if (cellsArray[x + 1, y, 0] == 1)
                {
                    cellsArray[x + 1, y, 0] = 3;
                }
                else if (cellsArray[x + 1, y, 0] == 0)
                {
                    cellsArray[x + 1, y, 0] = 2;
                }
                break;
        }
    }

    /// <summary>
    /// This method checks the number of walls in one of the cells in the matrix of a maze.
    /// </summary>
    private int CheckAmountOfWallsInCell(int xCell, int yCell)
    {
        int amountOfWallsInCell = 0;

        for (int counter = 0; counter < 4; counter++)
        {
            if (CheckTheWall(xCell, yCell, (Side)counter))
            {
                amountOfWallsInCell++;
            }
        }

        return amountOfWallsInCell;
    }

    /// <summary>
    /// This method creates the walls in one of the cells the matrix of a maze. 
    /// </summary>
    private void CellProcessing(int xCell, int yCell)
    {
        int variant;
        int amountOfWallsInCell = 4;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        if (CheckTheWall(xCell, yCell, Side.Up) || CheckAmountOfWallsInCell(xCell, yCell + 1) >= 2)
        {
            up = true;
            amountOfWallsInCell--;
        }

        if (CheckTheWall(xCell, yCell, Side.Down) || CheckAmountOfWallsInCell(xCell, yCell - 1) >= 2)
        {
            down = true;
            amountOfWallsInCell--;
        }

        if (CheckTheWall(xCell, yCell, Side.Left) || CheckAmountOfWallsInCell(xCell - 1, yCell) >= 2)
        {
            left = true;
            amountOfWallsInCell--;
        }

        if (CheckTheWall(xCell, yCell, Side.Right) || CheckAmountOfWallsInCell(xCell + 1, yCell) >= 2)
        {
            right = true;
            amountOfWallsInCell--;
        }

        if (amountOfWallsInCell > 2 && amountOfWalls > 0)
        {
            amountOfWalls -= 1;
            variant = SysRandom.Next(1, 5);
            switch (variant)
            {
                case 1:
                    if (up) { goto case 2; }
                    CreateWall(xCell, yCell, Side.Up);
                    break;
                case 2:
                    if (down) { goto case 3; }
                    CreateWall(xCell, yCell, Side.Down);
                    break;
                case 3:
                    if (left) { goto case 4; }
                    CreateWall(xCell, yCell, Side.Left);
                    break;
                case 4:
                    if (right) { goto case 1; }
                    CreateWall(xCell, yCell, Side.Right);
                    break;
            }
        }

    }

    /// <summary>
    /// This method does invisible or shows the elements of the menu. 
    /// </summary>
    private void InterfaceVisibility(bool visibility)
    {
        Button buttonGenerate = GameObject.Find("ButtonGenerate").GetComponent<Button>();
        buttonGenerate.interactable = visibility;
        InputField amountOfWallsInputField = GameObject.Find("AmountOfWallsInputField").GetComponent<InputField>();
        amountOfWallsInputField.interactable = visibility;
        InputField amountOfPeasInputField = GameObject.Find("AmountOfPeasInputField").GetComponent<InputField>();
        amountOfPeasInputField.interactable = visibility;
        InputField amountOfEnemiesInputField = GameObject.Find("AmountOfEnemiesInputField").GetComponent<InputField>();
        amountOfEnemiesInputField.interactable = visibility;

    }
    #endregion

    #region PublicMethods
    /// <summary>
    /// Generator of the maze
    /// </summary>
    public void GenerateMaze()
    {
        InputField amountOfWallsInputField = GameObject.Find("AmountOfWallsInputField").GetComponent<InputField>();
        amountOfWalls = int.Parse(amountOfWallsInputField.text);

        if (mazeBuilt)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    cellsArray[x, y, 0] = 0;
                    cellsArray[x, y, 1] = 0;
                }
            }
        }

        for (int perimeterCounter = 0; perimeterCounter < 5; perimeterCounter++)
        {
            for (int cellÑounter = (perimeterCounter + 1) * -1; cellÑounter < perimeterCounter; cellÑounter++)
            {
                CellProcessing(cellÑounter, perimeterCounter);
                CellProcessing(cellÑounter, (perimeterCounter + 1) * -1);
                CellProcessing(perimeterCounter, cellÑounter);
                CellProcessing((perimeterCounter + 1) * -1, cellÑounter);
            }
        }

        DisplayÑells();
        mazeBuilt = true;

        Button buttonStartGame = GameObject.Find("ButtonStartGame").GetComponent<Button>();
        buttonStartGame.interactable = mazeBuilt;

    }

    /// <summary>
    /// This method checks one of the four sides of the cell for a wall.
    /// </summary>
    public bool CheckTheWall(int xCell, int yCell, Side side)
    {
        bool yesWall = false;
        int x = xCell + 5;
        int y = yCell + 5;

        switch (side)
        {
            case Side.Up:
                if (x <= 9 && x >= 0 && y < 9 && y >= 0)
                {
                    yesWall = (cellsArray[x, y + 1, 0] == 1 || cellsArray[x, y + 1, 0] == 3);
                }
                else
                {
                    yesWall = true;
                }
                break;
            case Side.Down:
                if (x <= 9 && x >= 0 && y <= 9 && y > 0)
                {
                    yesWall = (cellsArray[x, y, 0] == 1 || cellsArray[x, y, 0] == 3);

                }
                else
                {
                    yesWall = true;
                }
                break;
            case Side.Left:
                if (x <= 9 && x > 0 && y <= 9 && y >= 0)
                {
                    yesWall = (cellsArray[x, y, 0] == 2 || cellsArray[x, y, 0] == 3);
                }
                else
                {
                    yesWall = true;
                }
                break;
            case Side.Right:
                if (x < 9 && x >= 0 && y <= 9 && y >= 0)
                {
                    yesWall = (cellsArray[x + 1, y, 0] == 2 || cellsArray[x + 1, y, 0] == 3);
                }
                else
                {
                    yesWall = true;
                }
                break;
        }

        return yesWall;
    }
   
    /// <summary>
    /// This method starts the game.
    /// </summary>
    public void StartGame()
    {
        if (gameStarted)
        {
            var allPlayers = GameObject.FindObjectsOfType<Player>();
            for (int counter = allPlayers.Length - 1; counter >= 0; counter--)
            {
                Destroy(allPlayers[counter].gameObject);
            }

            var allPeas = GameObject.FindObjectsOfType<Peas>();
            for (int counter = allPeas.Length - 1; counter >= 0; counter--)
            {
                Destroy(allPeas[counter].gameObject);
            }

            var allEnemy = GameObject.FindObjectsOfType<Enemy>();
            for (int counter = allEnemy.Length - 1; counter >= 0; counter--)
            {
                Destroy(allEnemy[counter].gameObject);
            }

            InterfaceVisibility(true);

            Text buttonStartGameText = GameObject.Find("ButtonStartGame").GetComponentInChildren<Text>();
            buttonStartGameText.text = "Start game";
            gameStarted = false;
        }
        else
        {
            Instantiate(player).transform.position = new Vector3(0.5f, 0.5f, 0);

            InterfaceVisibility(false);

            InputField amountOfPeasInputField = GameObject.Find("AmountOfPeasInputField").GetComponent<InputField>();
            amountOfPeas = int.Parse(amountOfPeasInputField.text);
            for (int counter = 1; counter <= amountOfPeas; counter++)
            {
                Instantiate(peas);
            }

            InputField amountOfEnemiesInputField = GameObject.Find("AmountOfEnemiesInputField").GetComponent<InputField>();
            amountOfEnemies = int.Parse(amountOfEnemiesInputField.text);
            for (int counter = 1; counter <= amountOfEnemies; counter++)
            {
                Instantiate(enemy);
            }

            GameObject.Find("TextPoints").GetComponent<Text>().text = "Points: 0";

            Text buttonStartGameText = GameObject.Find("ButtonStartGame").GetComponentInChildren<Text>();
            buttonStartGameText.text = "Stop game";
            gameStarted = true;
        }
    }
    #endregion
}

