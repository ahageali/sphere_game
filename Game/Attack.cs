using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Game
{
    class Attack
    {
        public Vector3 currentPos;
        public Vector3 direction;
        public float length;
        public Attack(Vector3 pos, Vector3 direct, float len)
        {
            currentPos = pos;
            direction = direct;
            length = len;
        }
    }
}
