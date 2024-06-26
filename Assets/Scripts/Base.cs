using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;
using TMPro;
using UnityEngine.UI;
using System.IO;


enum FigureType
{
    pawn,
    elephant,
    horse,
    Officer,
    queen,
    king
}

enum color 
{
    white,
    black
}
class ChessVector
{
    public float X;
    public float Y;
    public int x
    {
        get { return Convert.ToInt32(Math.Round(X)); }
        set { X = value; }
    }
    public int y
    {
        get { return Convert.ToInt32(Math.Round(Y)); }
        set { Y = value; }
    }
    public ChessVector(float[] xy)
    {
        this.X = xy[0];
        this.Y = xy[1];
    }
    public ChessVector(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }
    public bool isValid()
    {
        return this.x >= 1 && this.x <= 8 &&
                this.y >= 1 && this.y <= 8;
    }
    public float[] Normalize()
    {
        float biggest = Math.Abs(this.X) > Math.Abs(this.Y) ? Math.Abs(this.X) : Math.Abs(this.Y);
        float x = this.X / biggest;
        float y = this.Y / biggest;
        return new float[] { x, y };
    }
    public bool isBetween(ChessVector v1, ChessVector v2)
    {
        int[] biggest = new int[2] { v1.x > v2.x ? v1.x : v2.x, v1.y > v2.y ? v1.y : v2.y },
              smallest = new int[2] { v1.x < v2.x ? v1.x : v2.x, v1.y < v2.y ? v1.y : v2.y };
        return ((this.x > smallest[0] && this.x < biggest[0]) || v1.x == v2.x) &&
               ((this.y > smallest[1] && this.y <= biggest[1]) || v1.y == v2.y);
    }
    public static int counter = 0;
    public ChessVector NegateTo(ChessVector vector)
    {
        ChessVector newVector = vector + vector - this;
        Debug.Log($"{counter}x:{vector.x}y:{vector.y}");
        Debug.Log($"{counter}x:{this.x}y:{this.y}");
        Debug.Log($"{counter}x:{newVector.x}y:{newVector.y}");
        counter++;
        return newVector;
    }
    public ChessVector Negate()
    {
        ChessVector vector = new ChessVector(-x, -y);
        return vector;
    }
    public static ChessVector operator +(ChessVector v1, float[] v2)
    {
        return new ChessVector(v1.X + v2[0], v1.Y + v2[1]);
    }
    public static ChessVector operator +(ChessVector v1, ChessVector v2)
    {
        return new ChessVector(v1.x + v2.x, v1.y + v2.y);
    }
    public static ChessVector operator -(ChessVector v1, ChessVector v2)
    {
        return new ChessVector(v1.x - v2.x, v1.y - v2.x);
    }
    public static bool operator ==(ChessVector v1, ChessVector v2)
    {
        object notNull1 = null ?? v1,
               notNull2 = null ?? v2;
        if(notNull1 == null || notNull2 == null)
            return false;
        return v1.x == v2.x && v1.y == v2.y;
    }
    public static bool operator !=(ChessVector v1, ChessVector v2)
    {
        return !(v1 == v2);
    }
    public static Vector3 GetVectorFromCoordinates(ChessVector vector)
    {
        return new Vector3(vector.x - 4.5f, Config.size / 2, vector.y - 4.5f);
    }
    public static ChessVector GetCoordinatesFromVector(Vector3 vector)
    {
        return new ChessVector(System.Convert.ToInt32(vector.x + 4.5f), System.Convert.ToInt32(vector.z + 4.5f));
    }

    public override bool Equals(object obj)
    {
        return obj is ChessVector vector &&
               x == vector.x &&
               y == vector.y;
    }
    public override string ToString()
    {
        return $"{X},{Y}";
    }
}

abstract class Figure
{
    //public static float size = 0.7f;
    public static Material[] materials = { Resources.Load<Material>("Materials/White chess"), Resources.Load<Material>("Materials/Black chess"),
                                           Resources.Load<Material>("Materials/MovePad"), Resources.Load<Material>("Materials/Invisible") };
    public static GameObject[] models = new GameObject[6];
    public static GameObject text = Resources.Load<GameObject>("Models/Text on figure");
    /*public static ChessVector[] castRay(ChessVector start, int[] direction)
    {
        List<ChessVector> ray = new List<ChessVector>();
        for (; (start + direction).isValid();)
        {
            start += direction;
            ray.Add(start);
            if (GetFigure((start)) != null)
                break;
        }
        return ray.ToArray();
    }
    public static ChessVector castRaySingle(ChessVector start, int[] direction)
    {
        start += direction;
        for (; GetFigure(start) == null && start.isValid(); start += direction);
        return start;
    }
    public static ChessVector castRaySingleThroughKing(ChessVector start, int[] direction, color color)
    {
        ChessVector retme = castRaySingle(start, direction);
        if(GetFigure(retme) is King && GetFigure(retme).color == color)
            retme = castRaySingle(retme, direction);
        return retme;
    }
    public static ChessVector castRaySingleThroughFigure(ChessVector start, int[] direction)
    {
        ChessVector retme = castRaySingle(start, direction);
        if(GetFigure(retme) != null)
            retme = castRaySingle(retme, direction);
        return retme;
    }
    public static bool isOnRay(ChessVector start, int[] direction, ChessVector end)
    {
        for (; start != end && start.isValid() && GetFigure(start) != null; start += direction) ;
        return start == end;
    }
   /* public static Figure operator =(Figure figure1, Figure figure2)
    {
        
    }*/
    public Figure(ChessVector vector, color color, GameObject model)
    {
        this.coords = vector;
        if (!vector.isValid())
        {
            return;
        }
        Console.WriteLine("beiubviuerbvhj");
        model.SetActive(true);
        if (model.transform.childCount == 0)
            this.figure = model;
        else
        {
            this.figure = model.transform.GetChild(0).gameObject;
        }
        if (figure.GetComponent<Collider>() == null)
            figure.AddComponent<MeshCollider>();
        this.color = color;
        figure.transform.position = ChessVector.GetVectorFromCoordinates(coords);
        figure.GetComponent<Renderer>().material = materials[(int)color];
        figure.AddComponent<ForFigures>();
        figure.AddComponent<OnChessUI>();
        figure.GetComponent<OnChessUI>().mouseEnter = (e, s) =>
        {
            if(ActiveUI.builder && Input.GetMouseButton(0) && !FigureMaker.isDragging)
            {
                Logger.ui.log($"OnChessUI.OnMouseEnter(o = {figure.name})");
                Debug.Log(figure.name);
                SetFigure(FigureMaker.selected, ChessVector.GetCoordinatesFromVector(figure.transform.position), (color)FigureMaker.intColor);
            }
        };
        figure.GetComponent<OnChessUI>().mouseDown = (e, s) =>
        {
            Logger.ui.log($"OnChessUI.OnMouseDown(o = {figure.name})");
            if (ActiveUI.builder)
            {
                SetFigure(FigureMaker.selected, ChessVector.GetCoordinatesFromVector(figure.transform.position), (color)FigureMaker.intColor);
            }
            else
            {
                ChessVector vector = ChessVector.GetCoordinatesFromVector(figure.transform.position);
                select(vector);
            }
        };
        mover = new Mover("", coords);
        figure.name = $"{color}{this.GetType()}{coords.x}{coords.y}";
        GameObject text = GameObject.Instantiate(Figure.text);
        text.transform.SetParent(figure.transform);
        text.transform.localPosition = new Vector3(0, 0.5f, 0);
        text.transform.localScale = Vector3.one;
        text.GetComponent<TextMeshPro>().text = $"{this.GetType()}";
        int brightness = (int)color * 255;
        text.GetComponent<TextMeshPro>().color = new Color(brightness, brightness, brightness);
        figure.transform.Rotate(0, -180 * (int)color, 0);
    }
    public Figure(Figure figure)
    {
        this.figure = figure.figure;
        this.color = figure.color;
        this.coords = figure.coords;
    }
    public static void Destroy(ChessVector vector)
    {
        /*Logger.debug.startFunc("Destroy", $"v = {vector}")*/;
        Figure figure = GetFigure(vector);
        if (figure == null)
        {
            /*Logger.debug.endFunc("Destroy")*/;
            return;
        }
        DelFigure(vector);
        GameObject.Destroy(figure.figure);
        if(vector == selected)
            OnSelect = (sender, e) => { };
        /*Logger.debug.endFunc("Destroy")*/;
    }
    public virtual ChessVector[] canBeat()
    {
        /*Logger.debug.startFunc("Figure.canBeat", $"f = {coords}")*/;
        return /*Logger.debug.endFunc("Figure.canBeat",*/ mover.canMove()/*)*/;
    }
    public virtual bool canBeat(ChessVector vector, bool isForDefended = true)
    {
        /*Logger.debug.startFunc("Figure.canBeat", $"v = {vector}, iD = {isForDefended}")*/;
        return /*Logger.debug.endFunc("Figure.canBeat",*/ mover.canBeat(vector, isForDefended)/*)*/;
    }
    /*public static ChessVector[] kingDefenders(color color)
    {
        List<ChessVector> kingDefenders = new List<ChessVector>(),
                          kingDefendedAtVectors = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    j++;
                kingDefenders.Add(castRaySingle(kingCoords[(int)color], new int[] { i, j }));
            }
        clone.AddRange(kingDefenders);
        foreach (ChessVector i in clone)
            if (GetFigure(i) != null || GetFigure(i).color != color)
                kingDefenders.Remove(i);

        kingDefenders.ForEach(i => kingDefendedAtVectors.Add(new ChessVector((i - kingCoords[(int)color]).Normalize())));

        clone.Clear();
        clone.AddRange(kingDefendedAtVectors);
        foreach (ChessVector i in clone)
            if (GetFigure(castRaySingleThroughFigure(kingCoords[(int)color], new int[] {i.x, i.y })).canBeat(kingDefenders[kingDefendedAtVectors.IndexOf(i)]))
                kingDefenders.RemoveAt(kingDefendedAtVectors.IndexOf(i));

        return kingDefenders.ToArray();
    }*/
    public static void setKingDef(ChessVector vector, Moves saveKingMoves, bool isDef = true)
    {
        /*Logger.debug.startFunc("setKingDef", $"v = {vector}, m = {saveKingMoves}, iD = {isDef}")*/;
        chessBoard[vector.x - 1, vector.y - 1].mover.isKingDefender = isDef;
        if(isDef)
            saveKingMoves.direction = saveKingMoves.direction.Negate();
        chessBoard[vector.x - 1, vector.y - 1].mover.saveKingMoves = saveKingMoves;
        Debug.Log($"{GetFigure(vector).figure.name} {GetFigure(vector).mover.isKingDefender} {isDef}");
        /*Logger.debug.endFunc("setKingDef")*/;
    }



    public bool canWalk()
    {
        /*Logger.debug.startFunc("Figure.canBeBeaten", $"v = {coords}")*/;
        return /*Logger.debug.endFunc("Figure.canBeBeaten",*/ canBeat().Length > 0/*)*/;
    }
    public static bool canBeBeaten(ChessVector vector, color color)
    {
        /*Logger.debug.startFunc("Figure.canBeBeaten", $"v = {vector}, col = {color}")*/;
        if (vector == null)
            return /*Logger.debug.endFunc("Figure.canBeBeaten",*/ true/*)*/;
        if (!vector.isValid())
            return /*Logger.debug.endFunc("Figure.canBeBeaten",*/ true/*)*/;
        /*
                      
        List<Figure> beaters = new List<Figure>();

        for(int i = -1; i <= 1; i += 2)
            for(int j = -1; j <= 1; j += 2)
            {
                beaters.Add(GetFigure(castRaySingleThroughKing(vector, new int[] { i, j }, color)));
            }
        foreach (Figure i in beaters)
            if (i != null)
                if ((i is Officer || i is Queen) && isThatEnemy(i.coords, color))
                    return true;
        beaters.Clear();


        beaters.Add(GetFigure(castRaySingleThroughKing(vector, new int[] { 1, 0 }, color)));
        beaters.Add(GetFigure(castRaySingleThroughKing(vector, new int[] { -1, 0 }, color)));
        beaters.Add(GetFigure(castRaySingleThroughKing(vector, new int[] { 0, 1 }, color)));
        beaters.Add(GetFigure(castRaySingleThroughKing(vector, new int[] { 0, -1 }, color)));
        foreach (Figure i in beaters)
            if (i != null)
                if ((i is Elephant || i is Queen) && isThatEnemy(i.coords, color))
                    return true;
        beaters.Clear();

        beaters.Add(GetFigure(vector + new int[] { 2, 1 }));
        beaters.Add(GetFigure(vector + new int[] { 2, -1 }));
        beaters.Add(GetFigure(vector + new int[] { 1, 2 }));
        beaters.Add(GetFigure(vector + new int[] { -1, 2 }));
        beaters.Add(GetFigure(vector + new int[] { 1, -2 }));
        beaters.Add(GetFigure(vector + new int[] { -1, -2 }));
        beaters.Add(GetFigure(vector + new int[] { -2, -1 }));
        beaters.Add(GetFigure(vector + new int[] { -2, 1 }));
        foreach (Figure i in beaters)
            if (i != null)
                if ((i is Horse) && isThatEnemy(i.coords, color))
                    return true;
        beaters.Clear();

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    j++;
                beaters.Add(GetFigure(vector + new int[] { i, j }));
            }
        foreach (Figure i in beaters)
            if (i != null)
                if ((i is King) && isThatEnemy(i.coords, color))
                    return true;
        beaters.Clear();

        for (int i = -1; i <= 1; i += 2)
            for (int j = -1; j <= 1; j += 2)
            {
                beaters.Add(GetFigure(vector + new int[] { i, j }));
            }
        foreach (Figure i in beaters)
            if (i != null && i is Pawn)
                if(i.canBeat(vector) && isThatEnemy(i.coords, color))
                    return true;

        return false;*/
        return /*Logger.debug.endFunc("Figure.canBeBeaten",*/ Mover.canBeBeaten(vector, color)/*)*/;
    }

    public static bool isStalemate(color color)
    {
        /*Logger.debug.startFunc("isStalemate", $"col = {color}")*/;
        List<ChessVector> figures = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        figures.AddRange((color == color.white) ? white : black);
        clone.AddRange(figures);

        foreach (ChessVector i in clone)
            if (!GetFigure(i).canWalk())
                figures.Remove(i);

        if (figures.Count == 0)
            return /*Logger.debug.endFunc("isStalemate",*/ true/*)*/;
        else 
            if(white.Count == 1 && black.Count == 1)
                return /*Logger.debug.endFunc("isStalemate",*/ true/*)*/;
            else 
                return /*Logger.debug.endFunc("isStalemate",*/ false/*)*/;
    }
    /*public static Figure[] potentialBeaters(ChessVector vector, color color)
    {
        List<Figure> beaters = new List<Figure>();
        List<Figure> clone = new List<Figure>();

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    j++;
                beaters.Add(GetFigure(castRaySingle(vector, new int[] { i, j })));
            }

        beaters.Add(GetFigure(vector + new int[] { 2, 1 }));
        beaters.Add(GetFigure(vector + new int[] { 2, -1 }));
        beaters.Add(GetFigure(vector + new int[] { 1, 2 }));
        beaters.Add(GetFigure(vector + new int[] { -1, 2 }));
        beaters.Add(GetFigure(vector + new int[] { 1, -2 }));
        beaters.Add(GetFigure(vector + new int[] { -1, -2 }));
        beaters.Add(GetFigure(vector + new int[] { -2, -1 }));
        beaters.Add(GetFigure(vector + new int[] { -2, 1 }));

        clone.AddRange(beaters);

        foreach (Figure i in clone)
            if (i != null)
                if (!(i.canBeat(vector) && isThatEnemy(i.coords, color)))
                    beaters.Remove(i);
            else
                beaters.Remove(i);
        return beaters.ToArray();
    }
    public static ChessVector[] saveKingCoords(color color)
    {
        List<ChessVector> coords = new List<ChessVector>();
        List<Figure> beaters = new List<Figure>();
        beaters.AddRange(potentialBeaters(kingCoords[(int)color], color));
        if (beaters.Count > 1)
            return coords.ToArray();

        coords.AddRange(beaters[0].canBeat());
        
        List<ChessVector> clone = new List<ChessVector>();
        clone.AddRange(coords);
        foreach (ChessVector i in clone)
        {
            if(!i.isBetween(kingCoords[(int)color], i))
            {
                coords.Remove(i);
            }
        }
        return coords.ToArray();

    }*/
    public static bool isThatEnemy(ChessVector vector, color color)
    {
        /*Logger.debug.startFunc("isThatEnemy", $"v = {vector}, col = {color}")*/;
        Figure figure = GetFigure(vector);
        if (figure == null) return /*Logger.debug.endFunc("isThatEnemy",*/ true/*)*/;
        if (figure.color != color) return /*Logger.debug.endFunc("isThatEnemy",*/ true/*)*/;
        else return /*Logger.debug.endFunc("isThatEnemy",*/ false/*)*/;
    }
    virtual public void move(ChessVector vector)
    {
        OnSelect(GetFigure(vector), EventArgs.Empty);
        OnSelect = (sender, e) => { };
        /*Logger.debug.startFunc("move", $"v = {vector}")*/;

        Destroy(vector);
        ChessVector temp = coords;
        coords = vector;
        figure.transform.position = ChessVector.GetVectorFromCoordinates(vector);
        SetFigure(this);
        DelFigure(temp);
        this.mover.moveTo(vector);
        Mover.SetKingDefs(nowMove);
        nowMove = (color)(((int)nowMove + 1) % 2);
        isShah = canBeBeaten(kingCoords[(int)nowMove], nowMove);
        Mover.isShah();
        if(isShah && kingCoords[(int)nowMove] != new ChessVector(0,0))
        {
            /*ChessVector[] coords = saveKingCoords(nowMove);
            List<ChessVector> kingMoves = new List<ChessVector>();
            kingMoves.AddRange(GetFigure(kingCoords[(int)nowMove]).canBeat());*/
            Debug.Log("Shah");

           /* if (coords.Length <= 1 && kingMoves.Count == 0)
            {
                Debug.Log("Checkmate");
                finishGame("��, �����, ������� ����");
            }*/
        } else if (isStalemate(nowMove))
        {
            //stalemale
        }
        camera.transform.Rotate(new Vector3(0, 0, 180));
        /*Logger.debug.endFunc("move")*/;
    }
    public static void select(ChessVector vector)
    {
        /*Logger.debug.startFunc("select", $"v = {vector}")*/;
        OnSelect(GetFigure(vector), EventArgs.Empty);
        OnSelect = (sender, e) => { };

        selected = vector;
        List<ChessVector> selectedCanBeat = new List<ChessVector>();
        selectedCanBeat.AddRange(GetFigure(vector).canBeat());
        GameObject parent = GetFigure(vector).figure;
        bool isItsMove = nowMove == GetFigure(vector).color;
        GameObject[] moveSpheres = new GameObject[selectedCanBeat.Count];
        int counter = 0;

        foreach(ChessVector i in selectedCanBeat)
        {
            Vector3 coords = ChessVector.GetVectorFromCoordinates(i);
            bool isFigure = GetFigure(i) != null;
            moveSpheres[counter] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            moveSpheres[counter].transform.position = new Vector3(coords.x, 0.025f, coords.z);
            moveSpheres[counter].transform.localScale = new Vector3(1, 0.05f, 1);
            moveSpheres[counter].AddComponent<ForMovePads>();
            moveSpheres[counter].GetComponent<ForMovePads>().isMove = isItsMove;
            if (isFigure)
            {
                moveSpheres[counter].GetComponent<Renderer>().material = materials[((int)(GetFigure(i).color) + 1) % 2];
                GameObject killButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                killButton.transform.position = new Vector3(coords.x, Config.size / 2, coords.z);
                killButton.transform.localScale = new Vector3(1, Config.size + 0.1f, 1);
                killButton.GetComponent<Renderer>().material = materials[3];
                killButton.transform.parent = parent.transform;
                killButton.AddComponent<ForMovePads>();
                killButton.GetComponent<ForMovePads>().isMove = isItsMove;
                OnSelect += killButton.GetComponent<ForMovePads>().delete;
            }
            else
                moveSpheres[counter].GetComponent<Renderer>().material = materials[2];
            OnSelect += moveSpheres[counter].GetComponent<ForMovePads>().delete;
            moveSpheres[counter].transform.parent = parent.transform;
        }
        /*Logger.debug.endFunc("select")*/;
    }
    public static EventHandler OnSelect = (sender, e) => { };
    public GameObject figure;
    public Mover mover;
    public static ChessVector[] kingCoords = new ChessVector[2] {new ChessVector(0,0), new ChessVector(0,0)};
    public ChessVector coords;
    public color color;

    public static Figure GetFigure(ChessVector vector)
    {
        /*Logger.debug.startFunc("GetFigure", $"v = {vector}")*/;
        if (vector.isValid() && chessBoard[vector.x - 1, vector.y - 1] != null)
            return /*Logger.debug.endFunc("GetFigure",*/ chessBoard[vector.x - 1, vector.y - 1]/*)*/;
        else
        {
            /*Logger.debug.endFunc("GetFigure", "null" )*/;
            return null;
        }
    }
    public static void SetFigure(Figure figure)
    {
        /*Logger.debug.startFunc("SetFigure", $"f = {figure}")*/;
        if (!figure.coords.isValid())
            return;
        Destroy(figure.coords);
        chessBoard[figure.coords.x - 1, figure.coords.y - 1] = figure;
        if (figure.color == color.white)
            white.Add(figure.coords);
        else
            black.Add(figure.coords);
        figures.Add(figure.coords);
        Mover.SetColor(figure.coords, figure.color);
        /*Logger.debug.endFunc("SetFigure")*/;
    }
    public static void SetFigure(int configIndex, ChessVector vector, color color)
    {
        /*Logger.debug.startFunc("SetFigure", $"c = {configIndex}, v = {vector}, col = {color}")*/;
        switch (configIndex)
        {
            case 0:
                Figure.SetFigure(new Pawn(vector, color));
                break;
            case 1:
                Figure.SetFigure(new Elephant(vector, color));
                break;
            case 2:
                Figure.SetFigure(new Horse(vector, color));
                break;
            case 3:
                Figure.SetFigure(new Officer(vector, color));
                break;
            case 4:
                Figure.SetFigure(new Queen(vector, color));
                break;
            case 5:
                Figure.SetFigure(new King(vector, color));
                break;
            case 6:
                Figure.Destroy(vector);
                break;
        }
        /*Logger.debug.endFunc("SetFigure")*/;
    }
    public static void DelFigure(ChessVector coords)
    {
        /*Logger.debug.startFunc("DelFigure", $"c = {coords}")*/;
        if (GetFigure(coords).color == color.white)
            white.Remove(coords);
        else
            black.Remove(coords);
        figures.Remove(coords);
        chessBoard[coords.x - 1, coords.y - 1] = null;
        Mover.SetColor(coords, null);
        /*Logger.debug.endFunc("DelFigure")*/;
    }
    public static void startGame()
    {
        /*Logger.debug.startFunc("startGame")*/;
        makeChessboard();
        clearChessboard();
        
        SetFigure(new Elephant(new ChessVector(1, 1), color.white));
        SetFigure(new Elephant(new ChessVector(8, 1), color.white));
        SetFigure(new Horse(new ChessVector(2, 1), color.white));
        SetFigure(new Horse(new ChessVector(7, 1), color.white));
        SetFigure(new Officer(new ChessVector(3, 1), color.white));
        SetFigure(new Officer(new ChessVector(6, 1), color.white));
        SetFigure(new Queen(new ChessVector(4, 1), color.white));
        SetFigure(new King(new ChessVector(5, 1), color.white));
        for (int i = 1; i <= 8; i++)
            SetFigure(new Pawn(new ChessVector(i, 2), color.white));

        SetFigure(new Elephant(new ChessVector(1, 8), color.black));
        SetFigure(new Elephant(new ChessVector(8, 8), color.black));
        SetFigure(new Horse(new ChessVector(2, 8), color.black));
        SetFigure(new Horse(new ChessVector(7, 8), color.black));
        SetFigure(new Officer(new ChessVector(3, 8), color.black));
        SetFigure(new Officer(new ChessVector(6, 8), color.black));
        SetFigure(new Queen(new ChessVector(4, 8), color.black));
        SetFigure(new King(new ChessVector(5, 8), color.black));
        for (int i = 1; i <= 8; i++)
            SetFigure(new Pawn(new ChessVector(i, 7), color.black));
        /*Logger.debug.endFunc("startGame")*/;
    }
    public static void finishGame(string text)
    {
        /*Logger.debug.startFunc("finishGame")*/;
        GameObject textMesh = GameObject.Instantiate(Resources.Load<GameObject>("Models/New game")),
                   button = GameObject.Instantiate(Resources.Load<GameObject>("Models/Button")),
                   canvas = GameObject.Find("Canvas");
        button.transform.SetParent(canvas.transform, false);
        textMesh.transform.SetParent(canvas.transform, false);
        TextMeshProUGUI finishText = textMesh.GetComponent<TextMeshProUGUI>();
        finishText.text = text;
        button.GetComponent<Button>().onClick.AddListener(canvas.GetComponent<Butto>().DeleteChild);
        /*Logger.debug.endFunc("finishGame")*/;
    }
    private static void clearChessboard()
    {
        /*Logger.debug.startFunc("clearChessboard")*/;
        Mover.ClearDesk();
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Destroy(new ChessVector(i, j));
            }
        }
        /*Logger.debug.endFunc("clearChessboard")*/;
    }
    private static void makeChessboard()
    {
        /*Logger.debug.startFunc("makeChessboard")*/;
        GameObject parent = GameObject.Find("Chessboard");
        generateOnChessUI();
        Material[] materials = { Resources.Load<Material>("Materials/MeshMaterial1"),
                                 Resources.Load<Material>("Materials/MeshMaterial2")};
        for (int i = 1; i <= 8; i++)
        {
            GameObject row = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 position = ChessVector.GetVectorFromCoordinates(new ChessVector(i, 1));
            position.y = -0.5f;
            row.transform.position = position;
            row.transform.localScale = Vector3.one;
            row.GetComponent<Renderer>().material = Figure.materials[3];
            row.name = $"{i}row";
            row.transform.parent = parent.transform;
            for (int j = 1; j <= 8; j++)
            {
                position = ChessVector.GetVectorFromCoordinates(new ChessVector(i, j));
                position.y = -0.025f;
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.parent = row.transform;
                cell.transform.position = position;
                cell.transform.localScale = new Vector3(1, 0.05f, 1);
                cell.GetComponent<Renderer>().material = materials[(i % 2 + j % 2) % 2];
                cell.name = $"{j}cell";
                cell.AddComponent<OnChessUI>();
                cell.GetComponent<OnChessUI>().mouseDown = (s, e) =>
                {
                    Logger.ui.log($"OnChessUI.OnMouseDown(o = {cell.name})");
                    if (ActiveUI.builder)
                    {
                        SetFigure(FigureMaker.selected, ChessVector.GetCoordinatesFromVector(cell.transform.position), (color)FigureMaker.intColor);
                    }
                    OnSelect(0, EventArgs.Empty);
                    OnSelect = (s, e) => { };
                };
                cell.GetComponent<OnChessUI>().mouseEnter = (e, s) =>
                {
                    if (ActiveUI.builder && Input.GetMouseButton(0) && !FigureMaker.isDragging)
                    {
                        Logger.ui.log($"OnChessUI.OnMouseEnter(o = {cell.name})");
                        Debug.Log(cell.transform.position.ToString());
                        SetFigure(FigureMaker.selected, ChessVector.GetCoordinatesFromVector(cell.transform.position), (color)FigureMaker.intColor);
                    }
                };
            }
        }
        /*Logger.debug.endFunc("makeChessboard")*/;
        /*
        for (int i = 8; i >= 1; i--)
        {
            GameObject row = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 position = ChessVector.GetVectorFromCoordinates(new ChessVector(i, 1));
            position.y = 0;
            row.transform.position = position;
            row.transform.localScale = Vector3.one;
            row.GetComponent<Renderer>().material = Figure.materials[3];
            row.name = $"{i}row";
            row.transform.parent = chessButtons.transform;
            for (int j = 8; j >= 1; j--)
            {
                position = ChessVector.GetVectorFromCoordinates(new ChessVector(i, j));
                position.y = size + 0.1f;
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cell.transform.parent = row.transform;
                cell.transform.position = position;
                cell.transform.localScale = new Vector3(1, 0.1f, 1);
                cell.GetComponent<Renderer>().material = Figure.materials[3];
                cell.AddComponent<ForButtons>();
                cell.name = $"{j}cell";
            }
        }
        */
    }
    public static void generateOnChessUI()
    {
        /*Logger.debug.startFunc("generateOnChessUI")*/;
        generateBuilder();
        /*Logger.debug.endFunc("generateOnChessUI")*/;
    }
    public static void generateBuilder()
    {
        /*Logger.debug.startFunc("generateBuilder")*/;
        GameObject parent = GameObject.CreatePrimitive(PrimitiveType.Cube);
        parent.GetComponent<Renderer>().material = materials[3];
        parent.transform.SetParent(camera.transform.GetChild(0));
        parent.name = "Builder";


        GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 position = ChessVector.GetVectorFromCoordinates(new ChessVector(9, 4));
        position.y = -0.025f;
        button.transform.position = position;
        button.transform.localScale = new Vector3(1, 0.05f, 1);
        button.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MeshMaterial2");
        button.AddComponent<OnChessUI>();
        button.transform.SetParent(camera.transform.GetChild(0).GetChild(0));
        button.name = "Builder";

        GameObject changeColor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 changePosition = ChessVector.GetVectorFromCoordinates(new ChessVector(0, 1));
        changePosition.y = -0.025f;
        changeColor.transform.position = changePosition;
        changeColor.transform.localScale = new Vector3(1, 0.05f, 1);
        changeColor.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MeshMaterial2");
        changeColor.AddComponent<OnChessUI>();
        changeColor.GetComponent<OnChessUI>().mouseDown = (sender, e) => { Logger.ui.log($"OnChessUI.OnMouseDown(o = {changeColor.name})"); FigureMaker.ChangeColor(); };
        changeColor.transform.SetParent(parent.transform);

        GameObject del = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 delPosition = ChessVector.GetVectorFromCoordinates(new ChessVector(0, 8));
        delPosition.y = -0.025f;
        del.transform.position = delPosition;
        del.transform.localScale = new Vector3(1, 0.05f, 1);
        del.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/MeshMaterial2");
        del.AddComponent<FigureMaker>();
        del.GetComponent<FigureMaker>().TYPE = 6;
        del.transform.SetParent(parent.transform);

        button.GetComponent<OnChessUI>().mouseDown = (sender, e) =>
        {
            Logger.ui.log($"OnChessUI.OnMouseDown(o = {button.name})");
            parent.SetActive(!parent.activeSelf);
            ActiveUI.builder = parent.activeSelf;
        };

        for (int i = 0; i < 6; i++)
        {
            GameObject figureMaker = GameObject.Instantiate(models[i]);
            figureMaker.AddComponent<FigureMaker>();
            figureMaker.AddComponent<ForFigures>();
            figureMaker.transform.position = ChessVector.GetVectorFromCoordinates(new ChessVector(0, i + 2));
            figureMaker.SetActive(true);
            figureMaker.GetComponent<FigureMaker>().TYPE = i;
            figureMaker.transform.SetParent(parent.transform);
            if(i == 5)
                figureMaker.AddComponent<MeshCollider>();
        }
        /*GameObject king = GameObject.Instantiate(models[5]);
        king.SetActive(true);
        //king = king.transform.GetChild(0).gameObject;
        king.transform.position = ChessVector.GetVectorFromCoordinates(new ChessVector(0, 5 + 2));
        king.AddComponent<FigureMaker>();
        king.GetComponent<FigureMaker>().TYPE = 5;
        king.AddComponent<MeshCollider>();
        king.transform.SetParent(parent.transform);*/

        button.GetComponent<OnChessUI>().OnMouseDown();
        FigureMaker.SetColor("white");
        /*Logger.debug.endFunc("generateBuilder")*/;
    }
    public override string ToString()
    {
        return figure.name;
    }

    public static ChessVector selected;
    public static color nowMove = color.white;
    public static bool isShah = false;
    public static List<ChessVector> white = new List<ChessVector>();
    public static List<ChessVector> black = new List<ChessVector>();
    public static List<ChessVector> figures = new List<ChessVector>();
    public static GameObject chessButtons = GameObject.Find("ChessButtons");
    public static GameObject camera = GameObject.Find("Main Camera");
    public static Figure[,] chessBoard = new Figure[8, 8];
}

public class Base : MonoBehaviour
{
    void Start()
    {
        /*Logger.debug.startFunc("Base.Start")*/;
        Figure.models[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Figure.models[0].transform.localScale = new Vector3(Config.size, Config.size, Config.size);
        Figure.models[1] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Figure.models[1].transform.localScale = new Vector3(Config.size, Config.size / 2, Config.size);
        Figure.models[2] = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Figure.models[2].transform.localScale = new Vector3(Config.size, Config.size, Config.size);
        Figure.models[2].transform.Rotate(0, 45, 0);
        Figure.models[3] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Figure.models[3].transform.Rotate(90, 0, 0);
        Figure.models[3].transform.localScale = new Vector3(Config.size, Config.size / 2, Config.size);
        Figure.models[4] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Figure.models[4].transform.localScale = new Vector3(Config.size, Config.size, Config.size);
        Figure.models[5] = Resources.Load<GameObject>("Models/King").transform.GetChild(0).gameObject;
        Figure.models[5].transform.localScale = new Vector3(Config.size, Config.size, Config.size);
        Figure.models[5].transform.gameObject.AddComponent<MeshCollider>();
        foreach (GameObject model in Figure.models)
            model.SetActive(false);
        Console.WriteLine("lorem");
        Figure.startGame();
        /*Logger.debug.endFunc("Base.Start finish")*/;
        /*
        if (i)
            Debug.Log(i);
        else
            Debug.Log(i);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
