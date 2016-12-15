using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Input;
using OpenTkExample.SquareType;
using System.IO;

namespace OpenTkExample
{
    class ShaderGame : GameWindow
    {
        int _vbo_position, _vbo_color, _vbo_point_position, _vbo_normal;

        float deltaTime = 0.0f;
        float time = 0.0f , etime = 0.0f;
        float xdist = 0.0f, ydist = 0.0f, zdist = 2.0f;
        float xldist = 0.0f, yldist = 0.0f, zldist = 2.0f;
        private static float xangle, yangle, xlangle, ylangle;

        bool autoRotate = false;
        bool right = false, left = false, up = false, down = false;
        bool lright = false, lleft = false, lup = false, ldown = false;


        OpenTK.Input.KeyboardState lastKeystate;
        Vector3[] vertdata, coldata, ncoldata, normaldata , point_vert;
        private static System.Diagnostics.Stopwatch watch;

        Triangle triangle;

        Triangle point;



        int programId, light_program_id;
        Matrix4 projectionMatrix;

        public ShaderGame() :
            base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
        {
            Mouse.Move += Mouse_Move;
            Mouse.WheelChanged += Mouse_WheelChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            init();
            initLight();

            vertdata = new Vector3[] {
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)};

            point_vert = new Vector3[]
            {
                new Vector3(-0.8f, -0.8f, 0f),
                new Vector3( 0.8f, -0.8f, 0f),
                new Vector3( 0f,  0.8f, 0f)
            };

            Vector3[] point_normals = CalculateNormals(point_vert);



            coldata = new Vector3[] {
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 0f, 1f),
                new Vector3( 0f, 0f, 1f)
            };

            triangle = new Triangle(vertdata, coldata);
            triangle.Position = new Vector3(0f, 0f, -2.0f);
            triangle.Rotation = new Vector3(0f, 0f, 0f);
            triangle.Scale = new Vector3(3f, 3f, 3f);
            triangle.CalculateModelMatrix();

            point = new Triangle(point_vert, coldata);
            point.Position = new Vector3(0f, 0f, -1.0f);
            point.Rotation = new Vector3(0f, 0f, 0f);
            point.Scale = new Vector3(.1f, .1f, .1f);

            point.SetNormals(point_normals);
            point.CalculateModelMatrix();



            GL.GenBuffers(1, out _vbo_position);
            GL.GenBuffers(1, out _vbo_color);
            GL.GenBuffers(1, out _vbo_point_position);
            GL.GenBuffers(1, out _vbo_normal);

            GL.ClearColor(Color.Transparent);
            watch = System.Diagnostics.Stopwatch.StartNew();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            watch.Stop();
            deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
            watch.Restart();

            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.UseProgram(programId);

            int _position = GL.GetAttribLocation(programId, "vPosition");
            int _color = GL.GetAttribLocation(programId, "vColor");
            int _normal = GL.GetAttribLocation(programId, "inNormal");

            ///////

            GL.EnableVertexAttribArray(_position);
            GL.EnableVertexAttribArray(_color);
            GL.EnableVertexAttribArray(_normal);
            int _uniform = GL.GetUniformLocation(programId, "modelview");
            if (_uniform != -1)
            {
                GL.UniformMatrix4(_uniform, false, ref triangle.ModelViewProjectionMatrix);
            }

            int view_matrix_uniform = GL.GetUniformLocation(programId, "view_matrix");
            GL.UniformMatrix4(view_matrix_uniform, false, ref triangle.ViewMatrix);



            GL.DrawArrays(BeginMode.Triangles, 0, 3);
            GL.DisableVertexAttribArray(_position);
            GL.DisableVertexAttribArray(_color);
            GL.DisableVertexAttribArray(_normal);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(triangle.GetVertex().Length * Vector3.SizeInBytes),
                triangle.GetVertex(),
                BufferUsageHint.StaticDraw
                );
            GL.VertexAttribPointer(_position, 3, VertexAttribPointerType.Float, false, 0, 0);


            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo_color);
            GL.BufferData<Vector3>(
                BufferTarget.ArrayBuffer,
                (IntPtr)(triangle.GetColors().Length * Vector3.SizeInBytes),
                triangle.GetColors(),
                BufferUsageHint.StaticDraw
                );
            GL.VertexAttribPointer(_color, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo_normal);
            GL.BufferData<Vector3>(
                BufferTarget.ArrayBuffer,
                (IntPtr)(point.GetNormals().Length*Vector3.SizeInBytes),
                point.GetNormals(),
                BufferUsageHint.StaticDraw
                );
            GL.VertexAttribPointer(_normal, 3, VertexAttribPointerType.Float, false, 0, 0);


       /////


            GL.UseProgram(light_program_id);

            int light_position_location = GL.GetAttribLocation(light_program_id, "lPosition");
            int light_modelview_uniform = GL.GetUniformLocation(light_program_id, "lmodelview");

            if (light_position_location != -1 && light_modelview_uniform != -1)
            {

                GL.EnableVertexAttribArray(light_position_location);

                GL.UniformMatrix4(light_modelview_uniform, false, ref point.ModelViewProjectionMatrix);

                GL.DrawArrays(BeginMode.Triangles, 0, 3);

                GL.DisableVertexAttribArray(light_position_location);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo_point_position);
                GL.BufferData<Vector3>(
                    BufferTarget.ArrayBuffer,
                    (IntPtr)(point.GetVertex().Length * Vector3.SizeInBytes),
                    point.GetVertex(),
                    BufferUsageHint.StaticDraw
                    );
                GL.VertexAttribPointer(light_position_location, 3, VertexAttribPointerType.Float, false, 0, 0);


            }



            time += (float)e.Time;
            etime = (float)e.Time;


            Inputs();
            Sides();


            triangle.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
            triangle.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zdist), Vector3.Zero, Vector3.UnitY);
            triangle.ModelViewProjectionMatrix = triangle.ModelMatrix * triangle.ViewMatrix * triangle.ViewProjectionMatrix;


            point.ViewProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 1.0f, 100.0f);
            point.ViewMatrix = Matrix4.LookAt(new Vector3(0, 0, zldist), Vector3.Zero, Vector3.UnitY);
            point.ModelViewProjectionMatrix = point.ModelMatrix * point.ViewMatrix * point.ViewProjectionMatrix;


            GL.Flush();
            SwapBuffers();
        }



        private void init()
        {
            programId = GL.CreateProgram();
            if(programId == 0)
            {
                Console.WriteLine("Error Creating ProgramId " + programId);
                return;
            }
            int vertexShaderId = LoadShader(programId, "C:\\CrossPlatform\\OpenTkExample\\OpenTkExample\\vs.c", ShaderType.VertexShader);
            int fragmentShaderId = LoadShader(programId, "C:\\CrossPlatform\\OpenTkExample\\OpenTkExample\\fs.c", ShaderType.FragmentShader);
            // Link Program
            GL.LinkProgram(programId);

            string s = GL.GetShaderInfoLog(fragmentShaderId);
            string s2 = GL.GetShaderInfoLog(vertexShaderId);
            if(s.Length > 0)
            {
                Console.WriteLine("Error " + s);
            }

            if (s2.Length > 0)
            {
                Console.WriteLine("Error " + s2);
            }

            int pos_loc = GL.GetAttribLocation(programId, "vPosition");
            int col_loc = GL.GetAttribLocation(programId, "vColor");
            int uni_loc = GL.GetUniformLocation(programId, "modelview");
            int nomal_loc = GL.GetUniformLocation(programId, "normalMatrix");
            int inNormal = GL.GetAttribLocation(programId, "inNormal");
        }
        
        private void initLight()
        {
            light_program_id = GL.CreateProgram();
            if(light_program_id == 0)
            {
                Console.WriteLine("Error Creating ProgramId " + programId);
                return;
            }

            int light_vertex_shader_id = LoadShader(light_program_id, "C:\\CrossPlatform\\OpenTkExample\\OpenTkExample\\vs_light.c", ShaderType.VertexShader);
            int light_fragment_shader_id = LoadShader(light_program_id, "C:\\CrossPlatform\\OpenTkExample\\OpenTkExample\\fs_light.c", ShaderType.FragmentShader);


            
            GL.LinkProgram(light_program_id);

            string s = GL.GetShaderInfoLog(light_vertex_shader_id);
            string s2 = GL.GetShaderInfoLog(light_fragment_shader_id);

            int _light_pos = GL.GetAttribLocation(light_program_id, "lPosition");
            int _light_modelview = GL.GetUniformLocation(light_program_id, "lmodelview");

        }

        public int LoadShader(int programId, string file, ShaderType type)
        {
            int address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(file))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(programId, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
            return address;
        }


        private void Sides()
        {
            // perform rotation of the cube depending on the keyboard state
            if (autoRotate)
            {
                xangle = deltaTime / 2;
                yangle = deltaTime;
                triangle.SetValueX(Matrix4.CreateRotationX(xangle));
                triangle.SetValueY(Matrix4.CreateRotationY(yangle));
            }
            if (right)
            {
                yangle = deltaTime;

                triangle.SetValueY(Matrix4.CreateRotationY(yangle));
            }
            if (left)
            {
                yangle = -deltaTime;
                triangle.SetValueY(Matrix4.CreateRotationY(yangle));
                Console.WriteLine("Left");
            }
            if (up)
            {
                xangle = deltaTime;
                triangle.SetValueX(Matrix4.CreateRotationX(xangle));
            }
            if (down)
            {
                xangle = -deltaTime;
                triangle.SetValueX(Matrix4.CreateRotationX(xangle));
            }


            // Light 
            if (lright)
            {
                ylangle = deltaTime;

                point.SetValueY(Matrix4.CreateRotationY(ylangle));
            }
            if (lleft)
            {
                ylangle = -deltaTime;
                point.SetValueY(Matrix4.CreateRotationY(ylangle));
                Console.WriteLine("Left");
            }
            if (lup)
            {
                xlangle = deltaTime;
                point.SetValueX(Matrix4.CreateRotationX(xlangle));
            }
            if (ldown)
            {
                xlangle = -deltaTime;
                point.SetValueX(Matrix4.CreateRotationX(xlangle));
            }

        }

        private void Inputs()
        {

            OpenTK.Input.KeyboardState state = OpenTK.Input.Keyboard.GetState();

            if (state.IsKeyDown(Key.Space) && lastKeystate.IsKeyUp(Key.Space))
            {
                autoRotate = !autoRotate;
            }
            this.lastKeystate = state;
            if (state.IsKeyDown(Key.A))
            {
                left = true;
            }

            if (state.IsKeyDown(Key.S))
            {
                down = true;
            }
            if (state.IsKeyDown(Key.D))
            {
                right = true;
            }
            if (state.IsKeyDown(Key.W))
            {
                up = true;
            }

            if (state.IsKeyUp(Key.A))
            {
                left = false;
            }

            if (state.IsKeyUp(Key.S))
            {
                down = false;
            }
            if (state.IsKeyUp(Key.D))
            {
                right = false;
            }
            if (state.IsKeyUp(Key.W))
            {
                up = false;
            }
            if (state.IsKeyDown(Key.Right))
            {
                xdist = 0.05f;
                triangle.SetTranslation(xdist, 'X');
            }
            if (state.IsKeyDown(Key.Left))
            {
                xdist = -0.05f;
                triangle.SetTranslation(xdist, 'X');
            }
            if (state.IsKeyDown(Key.Up))
            {
                ydist = 0.05f;
                triangle.SetTranslation(ydist, 'Y');
            }
            if (state.IsKeyDown(Key.Down))
            {
                ydist = -0.05f;
                triangle.SetTranslation(ydist, 'Y');
            }

            if (state.IsKeyDown(Key.F1))
            {
                triangle.SetColors(ncoldata);
            }
            if (state.IsKeyDown(Key.F2))
            {
                triangle.SetColors(coldata);
            }



            // Light Move
            if (state.IsKeyDown(Key.Keypad4)) // Left
            {
                xldist = -0.05f;
                point.SetTranslation(xldist, 'X');
            }
            if (state.IsKeyDown(Key.Keypad6)) // Right
            {
                xldist = 0.05f;
                point.SetTranslation(xldist, 'X');
            }
            if (state.IsKeyDown(Key.Keypad8)) // Up
            {
                yldist = 0.05f;
                point.SetTranslation(yldist, 'Y');
            }
            if (state.IsKeyDown(Key.Keypad2)) // Down
            {
                yldist = -0.05f;
                point.SetTranslation(yldist, 'Y');
            }

            // w
            if (state.IsKeyUp(Key.U))
            {
                lup = true;
            }
            if (state.IsKeyDown(Key.U))
            {
                lup = false;
            }
            // s
            if (state.IsKeyUp(Key.J))
            {
                ldown = true;
            }
            if (state.IsKeyDown(Key.J))
            {
                ldown = false;
            }
            // a
            if (state.IsKeyUp(Key.H))
            {
                lleft = true;
            }
            if (state.IsKeyDown(Key.H))
            {
                lleft = false;
            }
            // d
            if (state.IsKeyUp(Key.K))
            {
                lright = true;
            }

            if (state.IsKeyDown(Key.K))
            {
                lright = false;
            }
            if (state.IsKeyDown(Key.Keypad7))
            {
                zldist += 0.05f;
            }
            if (state.IsKeyDown(Key.Keypad1))
            {
                zldist -= 0.05f;
            }


            if (state.IsKeyDown(Key.Escape))
            {
                this.Exit();
            }

            



        }

        public Vector3[] CalculateNormals(Vector3[] verts)
        {
            Vector3[] normals = new Vector3[verts.Length];
            int[] inds = new int[] { 0, 1, 2 };
            for (int i = 0; i < inds.Length; i += 3)
            {
                Vector3 v1 = verts[inds[i]];
                Vector3 v2 = verts[inds[i + 1]];
                Vector3 v3 = verts[inds[i + 2]];

                normals[inds[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[inds[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }
            for (int j = 0; j < normals.Length; j++)
            {
                Console.WriteLine("X : " + normals[j].X + " Y : " + normals[j].Y + " Z : " + normals[j].Z);
            }
            return normals;
        }

        private void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
        {
            // zoom in
            if (e.DeltaPrecise > 0)
            {
                zdist += 0.05f;
            }
            // zoom out
            if (e.DeltaPrecise < 0)
            {
                zdist -= 0.05f;
            }
        }

        private void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (OpenTK.Input.Mouse.GetState()[OpenTK.Input.MouseButton.Left])
            {

                down = up = left = right = false;

                // Move Up
                if (e.YDelta < 0)
                {
                    xangle = deltaTime * 2;
                    triangle.SetValueX(Matrix4.CreateRotationX(xangle));
                }
                // Move Down
                if (e.YDelta > 0)
                {
                    xangle = -deltaTime * 2;
                    triangle.SetValueX(Matrix4.CreateRotationX(xangle));
                }
                // Move Left
                if (e.XDelta < 0)
                {
                    yangle = -deltaTime * 2;
                    triangle.SetValueY(Matrix4.CreateRotationY(yangle));
                }

                // Move Right
                if (e.XDelta > 0)
                {
                    yangle = deltaTime * 2;
                    triangle.SetValueY(Matrix4.CreateRotationY(yangle));
                }
            }
        }

    }
}
