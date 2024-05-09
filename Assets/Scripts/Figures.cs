using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class Pawn : Figure
{
    public Pawn(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[0]))
    {
        int colorMult = color == color.white ? 1 : -1;
        mover = new Mover($"c0,{colorMult}m;c1,{colorMult}b;c-1,{colorMult}b;c0,{colorMult * 2}ms;", coords);
    }
    /*
    override public ChessVector[] canBeat()
    {
        /*List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();
        if (color == color.white)
        {
            if (coords.y == 2 && GetFigure(coords + new int[] { 0, 2 }) == null
                && GetFigure(coords + new int[] { 0, 1 }) == null) canBeat.Add(coords + new int[] { 0, 2 });
            if (GetFigure(coords + new int[] { 0, 1 }) == null) canBeat.Add(coords + new int[] { 0, 1 });
            if (isThatEnemy(coords + new int[] { 1, 1 }, color)
                         && GetFigure(coords + new int[] { 1, 1 }) != null) canBeat.Add(coords + new int[] { 1, 1 });
            if (isThatEnemy(coords + new int[] { -1, 1 }, color)
                         && GetFigure(coords + new int[] { -1, 1 }) != null) canBeat.Add(coords + new int[] { -1, 1 });
        } else
        {
            if (coords.y == 7 && GetFigure(coords + new int[] { 0, -2 }) == null
                && GetFigure(coords + new int[] { 0, -1 }) == null) canBeat.Add(coords + new int[] { 0, -2 });
            if (GetFigure(coords + new int[] { 0, -1 }) == null) canBeat.Add(coords + new int[] { 0, -1 });
            if (isThatEnemy(coords + new int[] { -1, -1 }, color)
                         && GetFigure(coords + new int[] { -1, -1 }) != null) canBeat.Add(coords + new int[] { -1, -1 });
            if (isThatEnemy(coords + new int[] { 1, -1 }, color)
                         && GetFigure(coords + new int[] { 1, -1 }) != null) canBeat.Add(coords + new int[] { 1, -1 });
        }

        if (isShah && color == nowMove)
        {
            clone.AddRange(canBeat);
            List<ChessVector> permittedMoves = new List<ChessVector>();
            permittedMoves.AddRange(saveKingCoords(color));
            foreach (ChessVector i in clone)
                if (!permittedMoves.Contains(i))
                    canBeat.Remove(i);
        }

        return canBeat.ToArray();*//*
        return mover.canMove();
    }*/
    /*public override bool canBeat(ChessVector vector)
    {
        bool canBeat = false;
        if (color == color.white)
        {
            if (coords + new int[] { -1, 1 } == vector) canBeat = true;
            if (coords + new int[] { 1, 1 } == vector) canBeat = true;
        }
        else
        {
            if (coords + new int[] { -1, -1 } == vector) canBeat = true;
            if (coords + new int[] { 1, -1 } == vector) canBeat = true;
        }
        return canBeat;
    }*/

    /*public override bool canBeatKingThrough(ChessVector vector)
    {
        return false;
    }*/
}
class Elephant : Figure
{
    public Elephant(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[1]))
    {
        figure.transform.GetChild(0).localPosition = new Vector3(0, 1f, 0);
        mover = new Mover($"r1,0bm;r0,1bm;r-1,0bm;r0,-1bm;", coords);
    }
    /*
    override public ChessVector[] canBeat()
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.AddRange(castRay(coords, new int[] { 0, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { 1, 0 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, 0 }));
        canBeat.AddRange(castRay(coords, new int[] { 0, -1 }));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        if (isShah && color == nowMove)
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> permittedMoves = new List<ChessVector>();
            permittedMoves.AddRange(saveKingCoords(color));
            foreach (ChessVector i in clone)
                if (!permittedMoves.Contains(i))
                    canBeat.Remove(i);
        }

        return canBeat.ToArray();
    }
    public override bool canBeat(ChessVector vector)
    {
        bool can = false;
        int[,] vectors = new int[4, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        for (int i = 0; i < 4; i++)
        {
            if (isOnRay(coords, new int[] { vectors[i, 0], vectors[i, 1] }, vector))
            {
                can = true;
                break;
            }
        }
        return can;
    }*/
    /*public override bool canBeatKingThrough(ChessVector vector)
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 0, 1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, 0 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, 0 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 0, -1 }, vector));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        return canBeat.Contains(kingCoords[((int)color + 1) % 2]);
    }*/
}
class Horse : Figure
{
    public Horse(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[2]))
    {
        //figure.transform.GetChild(0).Rotate(0, 0, 45);
        mover = new Mover($"c1,2bmt;c2,1bmt;c-1,2bmt;c-2,1bmt;c1,-2bmt;c2,-1bmt;c-1,-2bmt;c-2,-1bmt;", coords);
    }/*
    override public ChessVector[] canBeat()
    {
        List<ChessVector> canBeat = new List<ChessVector>();

        canBeat.Add(coords + new int[] { 2, 1 });
        canBeat.Add(coords + new int[] { 2, -1 });
        canBeat.Add(coords + new int[] { 1, 2 });
        canBeat.Add(coords + new int[] { -1, 2 });
        canBeat.Add(coords + new int[] { 1, -2 });
        canBeat.Add(coords + new int[] { -1, -2 });
        canBeat.Add(coords + new int[] { -2, -1 });
        canBeat.Add(coords + new int[] { -2, 1 });

        List<ChessVector> clone = new List<ChessVector>();
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!i.isValid() || (GetFigure(i) != null && !isThatEnemy(i, color)))
                canBeat.Remove(i);

        if (isShah && color == nowMove)
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> permittedMoves = new List<ChessVector>();
            permittedMoves.AddRange(saveKingCoords(color));
            foreach (ChessVector i in clone)
                if (!permittedMoves.Contains(i))
                    canBeat.Remove(i);
        }

        return canBeat.ToArray();
    }*/
    /*public override bool canBeatKingThrough(ChessVector vector)
    {
        return false;
    }*/
}
class Officer : Figure
{
    public Officer(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[3]))
    {
        figure.transform.GetChild(0).Rotate(0, 0, 0);
        figure.transform.GetChild(0).localPosition = new Vector3(0, 0, -0.5f);
        figure.transform.GetChild(0).localScale = new Vector3(1, 2, 1);
        figure.transform.Rotate(-180 * (int)color, 0, 0);
        mover = new Mover($"r1,1bm;r1,-1bm;r-1,1bm;r-1,-1bm;", coords);
    }
    /*
    override public ChessVector[] canBeat()
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.AddRange(castRay(coords, new int[] { 1, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { 1, -1 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, -1 }));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        if (isShah && color == nowMove)
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> permittedMoves = new List<ChessVector>();
            permittedMoves.AddRange(saveKingCoords(color));
            foreach (ChessVector i in clone)
                if (!permittedMoves.Contains(i))
                    canBeat.Remove(i);
        }

        return canBeat.ToArray();
    }
    public override bool canBeat(ChessVector vector)
    {
        bool can = false;
        int[,] vectors = new int[4, 2] { { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };
        for (int i = 0; i < 4; i++)
        {
            if (isOnRay(coords, new int[] { vectors[i, 0], vectors[i, 1] }, vector))
            {
                can = true;
                break;
            }
        }
        return can;
    }*/
    /*public override bool canBeatKingThrough(ChessVector vector)
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, 1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, -1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, 1 }, vector));   
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, -1 }, vector));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        return canBeat.Contains(kingCoords[((int)color + 1) % 2]);
    }*/
}
class Queen : Figure
{
    public Queen(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[4]))
    {
        mover = new Mover($"r1,1bm;r1,-1bm;r-1,1bm;r-1,-1bm;r1,0bm;r0,1bm;r-1,0bm;r0,-1bm;", coords);
    }
    /*
    override public ChessVector[] canBeat()
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.AddRange(castRay(coords, new int[] { 1, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { 1, -1 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, -1 }));
        canBeat.AddRange(castRay(coords, new int[] { 0, 1 }));
        canBeat.AddRange(castRay(coords, new int[] { 1, 0 }));
        canBeat.AddRange(castRay(coords, new int[] { -1, 0 }));
        canBeat.AddRange(castRay(coords, new int[] { 0, -1 }));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        if (isShah && color == nowMove)
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> permittedMoves = new List<ChessVector>();
            permittedMoves.AddRange(saveKingCoords(color));
            foreach (ChessVector i in clone)
                if (!permittedMoves.Contains(i))
                    canBeat.Remove(i);
        }

        return canBeat.ToArray();
    }
    public override bool canBeat(ChessVector vector)
    {
        bool can = false;
        int[,] vectors = new int[8, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };
        for (int i = 0; i < 8; i++)
        {
            if (isOnRay(coords, new int[] { vectors[i, 0], vectors[i, 1] }, vector))
            {
                can = true;
                break;
            }
        }
        return can;
    }*/
/*    public override bool canBeatKingThrough(ChessVector vector)
    {
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();

        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, 1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, -1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, 1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, -1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 0, 1 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 1, 0 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { -1, 0 }, vector));
        canBeat.Add(castRaySingleThroughFigure(coords, new int[] { 0, -1 }, vector));
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (!isThatEnemy(i, color) && GetFigure(i) != null)
                canBeat.Remove(i);

        return canBeat.Contains(kingCoords[((int)color + 1) % 2]);
    }*/
}
class King : Figure
{
    public King(ChessVector vector, color color) : base(vector, color, GameObject.Instantiate(models[5]))
    {
        figure.transform.parent.name = figure.name;
//        Debug.Log($"Created {figure.name}");
        kingCoords[(int)color] = vector; 
        mover = new Mover($"k;d;c1,1bm;c1,-1bm;c-1,1bm;c-1,-1bm;c1,0bm;c0,1bm;c-1,0bm;c0,-1bm;", coords);
    }
    public override void move(ChessVector vector)
    {
        base.move(vector);
        kingCoords[(int)color] = vector;
    }

    /*override public ChessVector[] canBeat()
    {
        List<ChessVector> canBeat = new List<ChessVector>();

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    j++;
                canBeat.Add(coords + new int[] { i, j });
            }

        List<ChessVector> clone = new List<ChessVector>();
        canBeat.ForEach(i => clone.Add(i));

        foreach (ChessVector i in clone)
            if (canBeBeaten(i, color) || !isThatEnemy(i, color))
                canBeat.Remove(i);

        return canBeat.ToArray();
    }*/

    /*override public bool canBeat(ChessVector vector)
    {
        List<ChessVector> canBeat = new List<ChessVector>();

        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (j == 0 && i == 0)
                    j++;
                canBeat.Add(coords + new int[] { i, j });
            }

        return canBeat.Contains(vector);
    }*/
    /*public override bool canBeatKingThrough(ChessVector vector)
    {
        return false;
    }*/
}