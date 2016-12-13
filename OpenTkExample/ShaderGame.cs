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

        int programId;

        public ShaderGame() :
            base(512, 512, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 4))
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            init();


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

            int pos_loc = GL.GetAttribLocation(programId, "vPosition");
            int col_loc = GL.GetAttribLocation(programId, "vColor");
            int uni_loc = GL.GetUniformLocation(programId, "modelview");


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

    }
}
