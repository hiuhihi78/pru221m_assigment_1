using System;
using UnityEditor;
using UnityEngine;

namespace Entity
{


    public class Tank
    {
        public Direction Direction { get; set; }
        public string Name { get; set; }
        public int Point { get; set; }
        public int Hp { get; set; }
<<<<<<< HEAD
        //public GUID Guid { get; set; }
=======

        /// <summary>
        //public GUID Guid { get; set; }
        /// </summary>
>>>>>>> cc62fd24227ff854afd09781c84dc497d3516a6d
        public Vector3 Position { get; set; }

        public void Move(float x, float y)
        {
            this.Position = new Vector3(x, y, 0);
        }
    }
}