using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace Game
{
    public class Sphere
    {
        public Vector3 position;
        public bool dead = false;
        public float radius;
        public Sphere(Random gen)
        {
            radius = gen.Next(3,6);
            position = new Vector3(gen.Next((int)radius, 100 - (int)radius),50,gen.Next((int)radius, 100 - (int)radius));
        }
        public void UpdateValues(Vector2 player)
        {
            if (dead)
            {
                radius -= 0.1f;
            }
            else if (position.Y != radius)
            {
                position.Y -= 0.5f;
                return;
            }
            else
            {
                Vector2 movement = player - new Vector2(position.X, position.Z);
                movement = movement / movement.Length;
                movement *= 0.5f;
                Vector3 newPos = new Vector3(position.X, position.Y, position.Z);
                newPos += new Vector3(movement.X, 0, movement.Y);
                if ((newPos.X > 0 || newPos.X < 100) && (newPos.Z > 0 || newPos.Z < 100))
                {
                    position = newPos;
                }
            }
        }
    }
}
