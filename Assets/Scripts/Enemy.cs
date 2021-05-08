using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class of cell
/// </summary>
public class Cell : System.IEquatable<Cell>, System.IComparable<Cell>
{
    #region Fields
    // X-axis value
    private int x;
    
    // Y-axis value
    private int y;
    
    // Cost of the cell
    private int cellCost;

    // Reference to the previous cell
    private Cell cellParent;
    #endregion

    #region Properties
    /// <summary>
    /// Get X-axis value. 
    /// </summary>
    /// <value>The value type is an integer</value>
    public int XCell
    {
        get
        {
            return x; 
        }
    }

    /// <summary>
    /// Get Y-axis value. 
    /// </summary>
    /// <value>The value type is an integer</value>
    public int YCell
    {
        get
        {
            return y;
        }
    }

    /// <summary>
    /// Get coast of the cell.
    /// Set coast of the cell.
    /// </summary>
    /// <value>The value type is an integer</value>
    public int CellCost
    {
        get
        {
            return cellCost;
        }
        set 
        {
            cellCost = value;
        }
    }

    /// <summary>
    /// Get the cell type. 
    /// Returns false if no previous cell is specified, 
    /// otherwise returns true.
    /// </summary>
    /// <value>The value type is a boolean</value>
    public bool IsBasic
    {
        get
        {
            return cellParent == null;
        }
    }

    /// <summary>
    /// Get the reference to the previous cell.
    /// </summary>
    /// <value>The value type is a Cell</value>
    public Cell CellParent
    {
        get
        {
            return cellParent;
        }
    }
    #endregion

    #region Constructors
    /// <summary>
    /// The cell constructor.
    /// </summary>
    /// <param name="xCell">X-axis value.</param>
    /// <param name="yCell">Y-axis value.</param>
    /// <param name="parent">Reference to the previous cell.</param>
    public Cell(int xCell, int yCell, Cell parent)
    {
        Player player = GameObject.FindObjectOfType<Player>();
        x = xCell;
        y = yCell;
        cellParent = parent;

        if (player != null)
        {
            int xPlayer = player.XPozition;
            int yPlayer = player.YPozition;

            cellCost = (System.Math.Max(x, xPlayer) - System.Math.Min(x, xPlayer)) + (System.Math.Max(y, yPlayer) - System.Math.Min(y, yPlayer));
        }
        else
        {
            cellCost = 0;
        }
    }
    #endregion

    #region Public methods
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Cell objAsPart = obj as Cell;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public int CompareTo(Cell compareCell)
    {
        if (compareCell == null)
            return 1;

        else
            return this.cellCost.CompareTo(compareCell.cellCost);
    }
    public override int GetHashCode()
    {
        return cellCost;
    }
    public bool Equals(Cell other)
    {
        if (other == null) return false;
        return (this.x.Equals(other.x) && this.y.Equals(other.y));
    }
    #endregion
}


public class Enemy : MonoBehaviour
{
    #region Fields
    [SerializeField] private float speed;
    
    private LevelGenerator levelGenirator;
    private System.Random random;
    private Timer speedTimer;
    #endregion

    #region Properties
    public int XPozition
    {
        get
        {
            return (int)(transform.position.x - 0.5f);
        }
    }

    public int YPozition
    {
        get
        {
            return (int)(transform.position.y - 0.5f);
        }
    }
    #endregion

    #region Private methods
    // Start is called before the first frame update
    private void Start()
    {
        levelGenirator = GameObject.FindObjectOfType<LevelGenerator>();
        random = levelGenirator.SysRandom;

        int side = random.Next(1, 5);
        switch (side)
        {
            case 1:
                transform.position = new Vector3(random.Next(-5, 5) + 0.5f, random.Next(3, 5) + 0.5f, 0);
                break;
            case 2:
                transform.position = new Vector3(random.Next(3, 5) + 0.5f, random.Next(-5, 5) + 0.5f, 0);
                break;
            case 3:
                transform.position = new Vector3(random.Next(-5, 5) + 0.5f, random.Next(-5, -3) + 0.5f, 0);
                break;
            case 4:
                transform.position = new Vector3(random.Next(-5, -3) + 0.5f, random.Next(-5, 5) + 0.5f, 0);
                break;
        }

        speedTimer = gameObject.AddComponent<Timer>();
        speedTimer.Duration = Random.Range(0.0f, 1.0f); 
        speedTimer.Run();
    }

    // Update is called once per frame
    private void Update()
    {
        if (speedTimer.Finished && GameObject.FindObjectOfType<Player>() != null)
        {
            List<Cell> listCells = new List<Cell>();
            
            for (int counter = 0; counter < 4; counter++)
            {
                int xNext = XPozition;
                int yNext = YPozition;
                switch ((Side)counter)
                {
                    case Side.Up:
                        yNext++;
                        break;
                    case Side.Right:
                        xNext++;
                        break;

                    case Side.Down:
                        yNext--;
                        break;

                    case Side.Left:
                        xNext--;
                        break;
                }

                if (!levelGenirator.CheckTheWall(XPozition, YPozition, (Side)counter))
                {
                    listCells.Add(CrateCellWithCorrectCost(xNext, yNext));
                }
            }
            listCells.Sort();

            bool busyCell = false;
            foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
            {
                busyCell = listCells[0].XCell == enemy.XPozition && listCells[0].YCell == enemy.YPozition && enemy.gameObject != gameObject;
            }
            if (!busyCell)
            {
                transform.position = new Vector3(listCells[0].XCell + 0.5f, listCells[0].YCell + 0.5f, 0);
                speedTimer.Duration = 1f / speed;
                speedTimer.Run();
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// This method estimates the number of steps from each cell to the player.
    /// </summary>
    private Cell CrateCellWithCorrectCost(int xCell, int yCell)
    {

        List<Cell> listCells = new List<Cell>();
        foreach (Enemy enemy in GameObject.FindObjectsOfType<Enemy>())
        {
            listCells.Insert(0 , new Cell(enemy.XPozition, enemy.YPozition, null));
            listCells[0].CellCost = 1001;
        }

        listCells.Insert(0, new Cell(xCell, yCell, null));

        int countWhile = 0;

        while (listCells[0].CellCost != 0 && countWhile <= 1000)
        {
            for (int counter = 0; counter < 4; counter++)
            {
                int xNext = listCells[0].XCell;
                int yNext = listCells[0].YCell;
                switch ((Side)counter)
                {
                    case Side.Up:
                        yNext++; 
                        break;
                    case Side.Right:
                        xNext++;
                        break;

                    case Side.Down:
                        yNext--;
                        break;
                        
                    case Side.Left:
                        xNext--;
                        break;
                }

                if (!listCells.Contains(new Cell(xNext, yNext, null)) && !levelGenirator.CheckTheWall(listCells[0].XCell, listCells[0].YCell, (Side)counter))
                {
                    listCells.Add(new Cell(xNext, yNext, listCells[0]));
                }
                listCells[0].CellCost = 1000;
            }
            listCells.Sort();
            countWhile++;
        }

        if (listCells[0].CellCost != 0)
        {
            listCells[0].CellCost = 100;
            return listCells[0];
        }

        Cell cellTarget = listCells[0];
        int counterCost = 0;
        while (!cellTarget.IsBasic)
        {
            cellTarget = cellTarget.CellParent;
            counterCost++;
        }

        cellTarget.CellCost = counterCost;

        return cellTarget;
    }
    #endregion
}
