using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.SimpleClasses
{
    public struct Game
    {
        public string name;
        public int n;
        public int mines;
        public GameMode mode;
        public bool multiplayer;
        //konstruktor
        public Game(string name, int n, int mines, GameMode mode, bool multiplayer)
        {
            this.name = name;
            this.n = n;
            this.mines = mines;
            this.mode = mode;
            this.multiplayer = multiplayer;


        }
        public override string ToString()
        {
            return mode.ToString();
        }
    }

    public struct FieldStruct
    {
        public int value;
        public Field fieldClass;
        public bool flooded; //volt-e már rajta a Flood? (endgame)
    }

    public static class MessageTypes
    {
        public static short INTEGER_MESSAGE = 100;
        public static short MINES_MESSAGE = 101;
    }
    public static class IntegerMessages
    {
        public const int CLIENT_READY = 0;
    }


    class MinesMessage : MessageBase
    {
        public Vector2[] mines;

    }
}
