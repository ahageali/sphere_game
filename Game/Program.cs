// Released to the public domain. Use, modify and relicense at will.

using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace Game
{
    class Game : GameWindow
    {
        int listGrid, listSphere;
        const float SPHERE_RADIUS = 0.5f;
        Camera camera;
        List<Sphere> listEnemies = new List<Sphere>();
        List<Attack> listAttack = new List<Attack>();
        Random gen;

        /// <summary>Creates a screen size window with the specified title.</summary>
        public Game()
            : base(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, new GraphicsMode(32, 0, 0, 4), "Attack of the Spheres")
        {
            Location = new System.Drawing.Point(0, 0);
            VSync = VSyncMode.On;
            gen = new Random();
        }
        int InitializeGrid(int list)
        {
            int listGrid = list;
            GL.NewList(listGrid, ListMode.Compile);
            GL.Begin(BeginMode.Lines);
            for (int x = 0; x <= 100; x++)
            {
                //X of side wall
                GL.Vertex3(new Vector3(x, 0, 0));
                GL.Vertex3(new Vector3(x, 100, 0));
                //Y of side wall
                GL.Vertex3(new Vector3(0, x, 0));
                GL.Vertex3(new Vector3(100, x, 0));
                //reverse side wall x
                GL.Vertex3(new Vector3(x, 0, 100));
                GL.Vertex3(new Vector3(x, 100, 100));
                //reverse side wall y
                GL.Vertex3(new Vector3(0, x, 100));
                GL.Vertex3(new Vector3(100, x, 100));
                //side x
                GL.Vertex3(new Vector3(0, 0, x));
                GL.Vertex3(new Vector3(0, 100, x));
                //side y
                GL.Vertex3(new Vector3(0, x, 0));
                GL.Vertex3(new Vector3(0, x, 100));
                //reverse side X
                GL.Vertex3(new Vector3(100, 0, x));
                GL.Vertex3(new Vector3(100, 100, x));
                //reverse side Y
                GL.Vertex3(new Vector3(100, x, 0));
                GL.Vertex3(new Vector3(100, x, 100));
                //X floor
                GL.Vertex3(new Vector3(x, 0, 0));
                GL.Vertex3(new Vector3(x, 0, 100));
                //Y floor
                GL.Vertex3(new Vector3(0, 0, x));
                GL.Vertex3(new Vector3(100, 0, x));
                //roof x
                GL.Vertex3(new Vector3(x, 100, 0));
                GL.Vertex3(new Vector3(x, 100, 100));
                //Y roof
                GL.Vertex3(new Vector3(0, 100, x));
                GL.Vertex3(new Vector3(100, 100, x));
            }
            GL.End();
            GL.EndList();
            return listGrid;
        }

        int InitializeSphere(int list)
        {
            int listSphere = list + 1;
            GL.NewList(listSphere, ListMode.Compile);
            for (int rotation = 0; rotation < 18; rotation++)
            {
                GL.Rotate(10 * rotation, Vector3.UnitY);
                GL.Begin(BeginMode.Lines);
                for (float i = 0; i < (float)Math.PI * 2; i += (float)Math.PI / 180)
                {
                    float x = (SPHERE_RADIUS * (float)Math.Cos(i));
                    float y = (SPHERE_RADIUS * (float)Math.Sin(i));
                    GL.Vertex3(x, y, 0);
                    x = (SPHERE_RADIUS * (float)Math.Cos(i + 0.1f));
                    y = (SPHERE_RADIUS * (float)Math.Sin(i + 0.1f));
                    GL.Vertex3(x, y, 0);
                }
                GL.End();
            }
            GL.Rotate(90, Vector3.UnitX);
            GL.Begin(BeginMode.Lines);
            for (float i = 0; i < (float)Math.PI * 2; i += (float)Math.PI / 180)
            {
                float x = (SPHERE_RADIUS * (float)Math.Cos(i));
                float y = (SPHERE_RADIUS * (float)Math.Sin(i));
                GL.Vertex3(x, y, 0);
                x = (SPHERE_RADIUS * (float)Math.Cos(i + 0.1f));
                y = (SPHERE_RADIUS * (float)Math.Sin(i + 0.1f));
                GL.Vertex3(x, y, 0);
            }
            GL.End();
            GL.EndList();
            return listSphere;
        }
        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.CreateProgram();
            int listBase = GL.GenLists(2);
            GL.ListBase(listBase);
            listGrid = InitializeGrid(listBase);
            listSphere = InitializeSphere(listBase);
            GL.ClearColor(Color4.Black);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            System.Windows.Forms.Cursor.Hide();
            camera = new Camera(new Vector3(50, 2, 50));
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI/2, Width / (float)Height, 0.2f, 200.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            camera.UpdateWatch();
            camera.MoveMouse((float)(Width / 2) - Mouse.X, (float)(Height / 2) - Mouse.Y);
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Location.X + (Width / 2)+ 8, Location.Y + (Height/2) + 30);
            if (Keyboard[Key.Escape])
                Exit();
            if (Keyboard[Key.Left])
            {
                camera.MoveRight(false);
            }
            if (Keyboard[Key.Right])
            {
                camera.MoveRight(true);
            }
            if (Keyboard[Key.Up])
            {
                camera.MoveForward(true);
            }
            if (Keyboard[Key.Down])
            {
                camera.MoveForward(false);
            }
            if (Mouse[MouseButton.Left])
            {
                listAttack.Add(new Attack(camera.GetPosition, camera.GetDirection, 0.1f));
            }
            if (listAttack.Count != 0)
            {
                for (int i = 0; i < listAttack.Count && i >= 0; i++ )
                {
                    listAttack[i].currentPos += listAttack[i].direction;
                    Vector3 currentPos = listAttack[i].currentPos + (listAttack[i].direction * listAttack[i].length);
                    if (currentPos.X > 100 || currentPos.X < 0 || currentPos.Y > 100 || currentPos.Y < 0 || currentPos.Z > 100 || currentPos.Z < 0)
                    {
                        listAttack.Remove(listAttack[i]);
                        i--;
                    }
                    else if (listEnemies.Count != 0)
                    {
                        for (int j = 0; j < listEnemies.Count && j >= 0; j++ )
                        {
                            Sphere sphere = listEnemies[j];
                            if (((currentPos.X - sphere.position.X) * (currentPos.X - sphere.position.X) + (currentPos.Y - sphere.position.Y) * (currentPos.Y - sphere.position.Y) + (currentPos.Z - sphere.position.Z) * (currentPos.Z - sphere.position.Z)) <= (sphere.radius * sphere.radius))
                            {
                                sphere.dead = true;
                                j--;
                                listAttack.Remove(listAttack[i]);
                                i--;
                                break;
                            }
                        }
                    }
                }
            }
            if (listEnemies.Count != 0)
            {
                Vector2 playerPos = new Vector2(camera.GetPosition.X,camera.GetPosition.Z);
                for (int i = 0; i < listEnemies.Count && i >= 0; i++)
                {
                    listEnemies[i].UpdateValues(playerPos);
                    if (listEnemies[i].dead && listEnemies[i].radius < 0)
                    {
                        listEnemies.Remove(listEnemies[i]);
                        i--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    listEnemies.Add(new Sphere(gen));
                }
            }
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 currentView = camera.GetMatrix();
            GL.LoadMatrix(ref currentView);
            GL.Color3(0, 50.0f / 250.0f, 175.0f / 250.0f);
            GL.PushMatrix();
            GL.CallList(listGrid);
            GL.PopMatrix();
            GL.Color4(Color4.Yellow);
            foreach (Sphere currentEnemy in listEnemies)
            {
                GL.PushMatrix();
                GL.Translate(currentEnemy.position);
                GL.Scale(currentEnemy.radius + currentEnemy.radius, currentEnemy.radius + currentEnemy.radius, currentEnemy.radius + currentEnemy.radius);
                GL.CallList(listSphere);
                GL.Flush();
                GL.PopMatrix();
               
            }
            GL.Color3(140.0f / 250.0f, 0, 0);
            foreach (Attack currentBullet in listAttack)
            {
                GL.PushMatrix();
                GL.Translate(currentBullet.currentPos);
                GL.Scale(currentBullet.length, currentBullet.length, currentBullet.length);
                GL.CallList(listSphere);
                GL.Flush();
                GL.PopMatrix();
            }
            SwapBuffers();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}