using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
namespace Game
{
    class Camera
    {
        const float rotateSpeed = 0.0002f;
        const float moveValue = 0.007f;
        Vector3 position, direction, right, up;
        float horizontalAngle, verticalAngle;
        float deltaTime;
        Stopwatch sw;
        public Camera(Vector3 pos)
        {
            sw = new Stopwatch();
            sw.Start();
            verticalAngle = 0;
            horizontalAngle = (float)Math.PI;
            position = pos;
            MoveMouse(0, 0);
        }
        public Matrix4 GetMatrix()
        {
            return Matrix4.LookAt(position, position + direction, up);
        }
        public void MoveMouse(float xDiff, float yDiff)
        {
            horizontalAngle += deltaTime * rotateSpeed * xDiff;
            verticalAngle += deltaTime * rotateSpeed * yDiff;
            if (verticalAngle > Math.PI/2.0f)
            {
                verticalAngle = (float)Math.PI/2.0f;
            }
            else if (verticalAngle < -Math.PI / 2.0f)
            {
                verticalAngle = -(float)Math.PI / 2.0f;
            }
            direction = new Vector3(cos(verticalAngle) * sin(horizontalAngle), sin(verticalAngle), cos(verticalAngle) * cos(horizontalAngle));
            right = new Vector3(sin(horizontalAngle - (float)Math.PI / 2.0f), 0, cos(horizontalAngle - (float)Math.PI / 2.0f));
            up = Vector3.Cross(right, direction);
        }
        public void MoveForward(bool forward)
        {
            Vector3 modifiedDirection = new Vector3(direction);
            modifiedDirection.Y = 0;
            modifiedDirection /= modifiedDirection.Length;
            if (forward)
            {
                modifiedDirection = position + modifiedDirection * deltaTime * moveValue;
            }
            else
            {
                modifiedDirection = position - modifiedDirection * deltaTime * moveValue;
            }
            if (CollisionCheck(modifiedDirection))
            {
                position = modifiedDirection;
                return;
            }
        }
        public void MoveRight(bool right1)
        {
            Vector3 modifiedRight = new Vector3(right);
            modifiedRight.Y = 0;
            modifiedRight /= modifiedRight.Length;
            if (right1)
            {
                modifiedRight = position + modifiedRight * deltaTime * moveValue;
            }
            else
            {
                modifiedRight = position - modifiedRight * deltaTime * moveValue;
            }
            if (CollisionCheck(modifiedRight))
            {
                position = modifiedRight;
                return;
            }
        }
        private float cos(float input)
        {
            return (float)(Math.Cos((double)input));
        }
        private float sin(float input)
        {
            return (float)(Math.Sin((double)input));
        }
        public void UpdateWatch()
        {
            deltaTime = sw.ElapsedMilliseconds;
            sw.Reset();
            sw.Start();
        }
        public Vector3 GetPosition
        {
            get
            {
                return position;
            }
        }
        public Vector3 GetDirection
        {
            get
            {
                return direction;
            }
        }
        private bool CollisionCheck(Vector3 position)
        {
            if (position.X > 0 && position.X < 100 && position.Z > 0 && position.Z < 100)
            {
                return true;
            }
            return false;
        }
    }
}
