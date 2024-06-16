using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

enum MoveType
{
    b, // beat
    m, // move
    bm // move & beat
}
class Mover
{
    public static Nullable<color>[,] chessBoard = new Nullable<color>[8,8];
    public static List<Moves> kingBeats = new();
    public static List<Mover> kingSavers = new();
    public Moves saveKingMoves;
    public ChessVector beater;
    public static bool shah
    {
        get
        {
            bool retme = false;
            kingBeats.ForEach(i => retme = true);
            return retme;
        }
    }
    public ChessVector coords;
    public bool moved; 
    public bool isDef;
    public bool isKing;
    public bool isKingDefender = false;
    public List<Moves> moves = new List<Moves>(),
                       movesFromStart = new List<Moves>();
    public static bool Contains(ChessVector vector)
    {
        /*Logger.debug.startFunc("Contains", $"v = {vector}")*/;
        try
        {
            return /*Logger.debug.endFunc("Contains",*/ chessBoard[vector.x - 1, vector.y - 1] != null/*)*/;
        }
        catch (Exception)
        {
           // Debug.Log($"{vector.x}, {vector.y}");
            return /*Logger.debug.endFunc("Contains",*/ false/*)*/;
        }
    }
    public Mover(string moves, ChessVector coords)
    {
        isDef = Regex.IsMatch(moves, "d;");
        isKing = Regex.IsMatch(moves, "k;");
        foreach (Match i in Regex.Matches(moves, "(r|c){1}[-0-9]+,{1}[-0-9]+(bm|m|b){1}(t[0-9]*)?;"))
        {
            this.moves.Add(Moves.ConstructMove(i.Value));
        }
        foreach (Match i in Regex.Matches(moves, "(r|c){1}[-0-9]+,{1}[-0-9]+(bm|m|b){1}(t[0-9]*)?s{1};"))
        {
            this.movesFromStart.Add(Moves.ConstructMove(i.Value));
        }
        moved = false;
        this.coords = coords;
    }
    public Mover(Mover mover)
    {
        this.isDef = mover.isDef;
        this.isKing = mover.isKing;
        mover.moves.ForEach(i => this.moves.Add(i.Copy()));
        mover.movesFromStart.ForEach(i => this.movesFromStart.Add(i.Copy()));
        this.moved = mover.moved;
        this.coords = mover.coords;
    }
    public ChessVector[] canMove(bool isForDefended = true)
    {
        /*Logger.debug.startFunc("Contains", $"ifd = {isForDefended}")*/;
        List<ChessVector> canBeat = new List<ChessVector>(),
                          clone = new List<ChessVector>();
        foreach (Moves i in moves)
        {
                canBeat.AddRange(i.Extract(coords));
        }
        if (!moved)
        {
            movesFromStart.ForEach(i => canBeat.AddRange(i.Extract(coords)));
        }
        clone.AddRange(canBeat);
        foreach(ChessVector i in clone)
        { 
            if (!i.isValid() || GetColor(i) == GetColor(coords) || (isDef && isForDefended && canBeBeaten(i, GetColor(coords))))
                canBeat.Remove(i);
        };
        if (isDef && isForDefended && canBeBeaten(coords, GetColor(coords)))
        {
            clone.Clear();
            clone.AddRange(canBeat);

            List<ChessVector> beats = new();
            List<Moves> beaters = new();
            beaters.AddRange(GetBeaters(coords, GetColor(coords)));
            beaters.ForEach(k => beats.AddRange(k.Extract(coords)));
            clone.ForEach(i =>
            {
                if (beats.Contains(i))
                    canBeat.Remove(i);
            });
            /*foreach(ChessVector i in clone)
            {
                ChessVector j = i.NegateTo(coords);
                if(!canBeat.Contains(j))
                {
                    List<ChessVector> beats = new();
                    List<Moves> beaters = new();
                    beaters.AddRange(GetBeaters(j, GetColor(coords)));
                    beaters.ForEach(k => beats.AddRange(k.Extract(coords)));
                    if (beats.Contains(j))
                        canBeat.Remove(i);
                }
            }*/
        }

        if (!isKing && shah)
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> saveKingCoords = new();
            if (kingBeats.Count == 1)
                saveKingCoords.AddRange(kingBeats[0].Extract(Figure.kingCoords[(int)GetColor(coords)]));
            foreach (ChessVector i in clone)
                if (!saveKingCoords.Contains(i))
                    canBeat.Remove(i);
        }
        if (isKingDefender)
        {
            List<ChessVector> saveKingCoords = new();
            saveKingCoords.AddRange(saveKingMoves.Extract(Figure.kingCoords[(int)GetColor(coords)]));
            clone.Clear();
            clone.AddRange(canBeat);
            foreach (ChessVector i in clone)
                if (!saveKingCoords.Contains(i))
                    canBeat.Remove(i);

        }


        return /*Logger.debug.endFunc("canMove",*/ canBeat.ToArray()/*)*/;
    }
    public static Moves[] GetBeaters(ChessVector coords, Nullable<color> color)
    {
        /*Logger.debug.startFunc("Moves.GetBeaters", $"v = {coords}, c = {color}")*/;
        if (color == null)
            return new Moves[] {};
        List<Moves> moves = new List<Moves>();
        List<ChessVector> col = new();
        col.AddRange((int)color == 0 ? Figure.black : Figure.white);
        foreach (ChessVector i in col)
            if (Figure.GetFigure(i).canBeat(coords))
                moves.AddRange(Figure.GetFigure(i).mover.GetBeaters(coords));
        return /*Logger.debug.endFunc("Moves.GetBeaters",*/ moves.ToArray()/*)*/;
    }
    public Moves[] GetBeaters(ChessVector vector)
    {
        /*Logger.debug.startFunc("GetBeaters", $"v = {vector}")*/;
        List<Moves> beaters = new();
        List<ChessVector> beats = new List<ChessVector>();
        foreach (Moves i in moves)
        {
            beats.AddRange(i.Extract(coords));
            if(beats.Contains(vector))
                beaters.Add(i);
            beats.Clear();
        }
        return /*Logger.debug.endFunc("GetBeaters",*/ beaters.ToArray()/*)*/;
    }
    public ChessVector[] GetBeaters()
    {
        /*Logger.debug.startFunc("GetBeaters")*/;
        List<ChessVector> beaters = new(), clone = new();
        beaters.AddRange(canMove());
        clone.AddRange(beaters);
        foreach (ChessVector i in clone)
            if (GetColor(i) == null || GetColor(i) == GetColor(coords))
                beaters.Remove(i);
        clone.Clear();
        clone.AddRange(beaters);
        foreach (ChessVector i in clone)
            if (!Figure.GetFigure(i).canBeat(coords))
                beaters.Remove(i);
        return /*Logger.debug.endFunc("GetBeaters",*/ beaters.ToArray()/*)*/;
    }
    public ChessVector[] canBeat(bool isForDefended = true)
    {
        /*Logger.debug.startFunc("canBeat", $"ifd = {isForDefended}")*/;
        List<Moves> bm = new List<Moves>();
        List<ChessVector> canBeat = new(), clone = new();
        moves.ForEach(i =>
        {
            if(i.type == MoveType.b || i.type == MoveType.bm)
                bm.Add(i.CopyAsBM());
        });
        bm.ForEach(i => canBeat.AddRange(i.Extract(coords)));

        clone.AddRange(canBeat);
        clone.ForEach(i =>
        {
            if (!i.isValid() || (isDef && isForDefended && canBeBeaten(i, GetColor(coords))))
                canBeat.Remove(i);
        });

        if (!isKing && shah && Figure.nowMove == GetColor(coords))
        {
            clone.Clear();
            clone.AddRange(canBeat);
            List<ChessVector> saveKingCoords = new();
            if (kingBeats.Count == 1)
                saveKingCoords.AddRange(kingBeats[0].Extract(Figure.kingCoords[(int)GetColor(coords)]));
            foreach (ChessVector i in clone)
                if (!saveKingCoords.Contains(i))
                    canBeat.Remove(i);
           // saveKingCoords.Clear();
        }
        if(isKingDefender)
        {
            List<ChessVector> saveKingCoords = new();
            saveKingCoords.AddRange(saveKingMoves.Extract(Figure.kingCoords[(int)GetColor(coords)]));
            clone.Clear();
            clone.AddRange(canBeat);
            foreach (ChessVector i in clone)
                if (!saveKingCoords.Contains(i))
                    canBeat.Remove(i);
            
        }
        else
        {
            //Debug.Log($"{Figure.GetFigure(coords).figure.name} is not a king defender");
        }
        return /*Logger.debug.endFunc("canBeat",*/ canBeat.ToArray()/*)*/;
    }
    public bool canBeat(ChessVector vector, bool isForDefended = true)
    {
        /*Logger.debug.startFunc("canBeat", $"v = {vector}, ifd = {isForDefended}")*/;
        List<ChessVector> canBeat = new();
        canBeat.AddRange(this.canBeat(isForDefended));
        return /*Logger.debug.endFunc("canBeat",*/ canBeat.Contains(vector)/*)*/;
    }
    public static bool canBeBeaten(ChessVector vector, Nullable<color> color)
    {
        /*Logger.debug.startFunc("Mover.canBeBeaten", $"v = {vector}, c = {color}")*/;
        if (color == null)
            return /*Logger.debug.endFunc("Mover.canBeBeaten",*/ true/*)*/;
        List<ChessVector> col = new();
        col.AddRange((int)color == 0? Figure.black : Figure.white);
        foreach (ChessVector i in col)
            if (Figure.GetFigure(i).canBeat(vector, false)) 
                return /*Logger.debug.endFunc("Mover.canBeBeaten",*/ true/*)*/;
        return /*Logger.debug.endFunc("Mover.canBeBeaten",*/ false/*)*/;
    }
    public void moveTo(ChessVector vector)
    {
        /*chessBoard[vector.x - 1, vector.y - 1] = chessBoard[coords.x - 1, coords.y - 1];
        chessBoard[coords.x - 1, coords.y - 1] = null;*/
        /*Logger.debug.startFunc("moveTo", $"v = {vector}")*/;
        if (!moved)
        {
            moved = true;
        }
        coords = vector;
        /*Logger.debug.endFunc("moveTo")*/;
    }
    public static void ClearDesk()
    {
        /*Logger.debug.startFunc("Mover.ClearDesk")*/;
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                chessBoard[i, j] = null;
        /*Logger.debug.endFunc("Mover.ClearDesk")*/;
    }
    public static void SetColor(ChessVector vector, Nullable<color> color)
    {
        /*Logger.debug.startFunc("Mover.SetColor", $"v = {vector}, c = {color}")*/;
        chessBoard[vector.x - 1, vector.y - 1] = color;
        /*Logger.debug.endFunc("Mover.SetColor")*/;
    }
    public static Nullable<color> GetColor(ChessVector vector)
    {
        /*Logger.debug.startFunc("Mover.GetColor", $"v = {vector}")*/;
        try
        {
            return /*Logger.debug.endFunc("Mover.GetColor",*/ chessBoard[vector.x - 1, vector.y - 1]/*)*/;
        }
        catch (IndexOutOfRangeException)
        {
            /*Logger.debug.endFunc("Mover.GetColor",*/ "null"/*)*/;
            return null;
        }
    }
    public static bool isShah()
    {
        /*Logger.debug.startFunc("Mover.isShah")*/;
        List<Moves> temp = new();
        kingBeats.Clear();
        SetKingDefs(Figure.nowMove);
        if (canBeBeaten(Figure.kingCoords[(int)Figure.nowMove], Figure.nowMove))
        {
            temp.AddRange(GetBeaters(Figure.kingCoords[(int)Figure.nowMove], Figure.nowMove));
            temp.ForEach(i => kingBeats.Add(i.Copy()));
            kingBeats.ForEach(i => i.direction = i.direction.Negate());
            kingBeats.ForEach(i => i.throught = i.throught + ((i.throught == -1) ? 0 : 1));
            temp.Clear();
            return /*Logger.debug.endFunc("Mover.isShah",*/ true/*)*/;
        }
        return /*Logger.debug.endFunc("Mover.isShah",*/ false/*)*/;
    }
    public static void SetKingDefs(Nullable<color> color)
    {
        /*Logger.debug.startFunc("Mover.SetKingDefs", $"c = {color}")*/;
        if (color == null)
        {
            /*Logger.debug.endFunc("Mover.SetKingDefs")*/;
            return;
        }
        List<ChessVector> col = new();
        Mover move;
        List<ChessVector> figures = new();
        col.AddRange((int)color == 0 ? Figure.black : Figure.white);
        ((int)color == 0 ? Figure.white : Figure.black).ForEach(i => Figure.setKingDef(i, null, false));
        foreach (ChessVector i in col)
        {
            move = Figure.GetFigure(i).mover.Copy();
            move.moves.ForEach(i => i.throught = i.throught + ((i.throught == -1) ? 0 : 1));
            if (move.canBeat(Figure.kingCoords[(int)color]))
            {
                foreach (Moves j in move.moves)
                {
                    List<ChessVector> defs = new();
                    defs.AddRange(j.ExtractBeats(i));
                    if (defs.Contains(Figure.kingCoords[(int)color]))
                    {
                        defs.Remove(Figure.kingCoords[(int)color]);
                        foreach (ChessVector def in defs)
                        {
                            if(GetColor(def) == color)
                                Figure.setKingDef(def, j);
                        }
                    }
                    defs.Clear();
                }
            }
        }
        /*Logger.debug.endFunc("Mover.SetKingDefs")*/;
    }

    public override bool Equals(object obj)
    {
        return obj is Mover mover &&
               EqualityComparer<ChessVector>.Default.Equals(coords, mover.coords);
    }
    public Mover Copy()
    {
        /*Logger.debug.startFunc("Copy")*/;
        return /*Logger.debug.endFunc("Copy",*/ new Mover(this)/*)*/;
    }
    public override string ToString()
    {
        string retme = "";
        if (isDef)
            retme += "d;";
        if (isKing)
            retme += "k;";
        foreach(Moves i in moves)
            retme += i.ToString();
        if(!moved)
            foreach(Moves i in movesFromStart)
            {
                retme += i.ToString().Replace(";", "s;");
            }
        retme += coords.ToString() + ';';
        return retme;
    }
}
abstract class Moves
{
    public ChessVector direction;
    public float x
    {
        get { return direction.X; }
        set { direction.X = value; }
    }
    public float y
    {
        get { return direction.Y; }
        set { direction.Y = value; }
    }
    public MoveType type;
    public int throught;
    public Moves(float x, float y, MoveType type, int throught)
    {
        direction = new ChessVector(x, y);
        this.type = type;
        this.throught = throught;
    }
    public static Moves ConstructMove(string move)
    {
        /*Logger.debug.startFunc("ConstructMove", $"m = {move}")*/;
        int[] xy = new int[2];
        int k = 0;
        string type = Regex.Match(move, "(bm|m|b){1}").Value;
        int throught = 0;
        MoveType moveType;
        foreach (Match j in Regex.Matches(move, @"[-0-9]+"))
        {
            // Debug.Log(j.Value);
            xy[k++] = int.Parse(j.Value);
        }
        if (Regex.IsMatch(move, "t{1}[0-9]*"))
        {
            if (Regex.IsMatch(move, "t{1}[0-9]+"))
            {
                string value = Regex.Match(move, "t{1}[0-9]+").Value;
                throught = int.Parse(Regex.Match(value, "[0-9]+").Value);
            }
            else
                throught = -1;
        }
        if (type == "bm")
            moveType = MoveType.bm;
        else if (type == "m")
            moveType = MoveType.m;
        else
            moveType = MoveType.b;
        if (move[0] == 'c')
            return /*Logger.debug.endFunc("ConstructMove",*/ new ToCell(xy[0], xy[1], moveType, throught)/*)*/;
        if (move[0] == 'r')
            return /*Logger.debug.endFunc("ConstructMove",*/ new ToRay(xy[0], xy[1], moveType, throught)/*)*/;
        else
            throw new Exception("Incorrect move string");
    }
    public Moves Copy()
    {
        if (this is ToCell)
            return new ToCell(x, y, type, throught);
        else if (this is ToRay)
            return new ToRay(x, y, type, throught);
        else throw new Exception("Unknown move!");
    }
    public Moves CopyAsBM()
    {
        if (this is ToCell)
            return new ToCell(x, y, MoveType.bm, throught);
        else if (this is ToRay)
            return new ToRay(x, y, MoveType.bm, throught);
        else throw new Exception("Unknown move!");
    }
    public Moves CopyAsThrough(int through)
    {
        if (this is ToCell)
            return new ToCell(x, y, type, through);
        else if (this is ToRay)
            return new ToRay(x, y, type, through);
        else throw new Exception("Unknown move!");
    }
    public abstract ChessVector[] Extract(ChessVector vector);
    public ChessVector[] ExtractBeats(ChessVector vector)
    {
        List<ChessVector> beats = new(), clone = new();
        beats.AddRange(Extract(vector));
        clone.AddRange(beats);
        foreach(ChessVector beat in clone)
            if(!Mover.Contains(beat))
                beats.Remove(beat);
        return beats.ToArray();
    }

}
class ToCell : Moves
{
    public ToCell(float x, float y, MoveType type, int Through) : base(x, y, type, Through) {}
    public override ChessVector[] Extract(ChessVector vector)
    {
        /*Logger.debug.startFunc("Extract", $"v = {vector}")*/;
        ChessVector cell = vector + new float[] { x, y };
        if (type == MoveType.bm || 
            (type == MoveType.m && !Mover.Contains(cell)) || 
            (type == MoveType.b && Mover.Contains(cell)))
            return /*Logger.debug.endFunc("Extract",*/ rayTo(vector + direction.Normalize(), cell, throught)/*)*/;
        return /*Logger.debug.endFunc("Extract",*/ new ChessVector[] { }/*)*/;
    }
    public ChessVector[] rayTo(ChessVector from, ChessVector to, int throught)
    {
        /*Logger.debug.startFunc("rayTo", $"f = {from}, t = {to}, th = {throught}")*/;
        for (; from != to && from.isValid() && (!Mover.Contains(from) || throught != 0); from += direction.Normalize())
            if(Mover.Contains(from))
                throught--;
        if (from == to)
            return /*Logger.debug.endFunc("rayTo",*/ new ChessVector[] { from }/*)*/;
        else
            return /*Logger.debug.endFunc("rayTo",*/ new ChessVector[] { }/*)*/;
    }
    public override string ToString()
    {
        return $"c{x},{y}{type}{(throught != 0? 't' + (throught != -1? throught.ToString() : "") : "")}";
    }
}
class ToRay : Moves
{
    public ToRay(float x, float y, MoveType type, int Through) : base(x, y, type, Through) { }
    public override ChessVector[] Extract(ChessVector vector)
    {
        /*Logger.debug.startFunc("Extract", $"v = {vector}")*/;
        int throught = this.throught;
        List<ChessVector> vectors = new List<ChessVector>();
        ChessVector start = vector + new float[] {x, y};

        if (type == MoveType.b)
        {
            for (; start.isValid() && !Mover.Contains(start) ; start += new float[] { this.x, this.y }) ;
            if(start.isValid())
                vectors.Add(start);
            return vectors.ToArray();
        }
        for (; start.isValid() && (!Mover.Contains(start) || throught != 0); start += new float[] { x, y })
        {
            if(Mover.Contains(start))
                throught--;
            if (start.isValid())
                vectors.Add(start);
            else
                break;
        }
        if (type == MoveType.m)
        {
            List<ChessVector> clone = new();
            clone.AddRange(vectors);
            foreach (ChessVector i in clone)
                if (Mover.Contains(i))
                    clone.Remove(i);
        }
        if (start.isValid() && type == MoveType.bm)
            vectors.Add(start);
        return /*Logger.debug.endFunc("Extract",*/ vectors.ToArray()/*)*/;
    }
    public override string ToString()
    {
        return $"r{x},{y}{type}{(throught != 0 ? 't' + (throught != -1 ? throught.ToString() : "") : "")}";
    }
}