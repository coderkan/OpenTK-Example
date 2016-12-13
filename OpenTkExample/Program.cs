using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTkExample
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            using (ShaderGame game = new ShaderGame())
            {
                game.Run(30, 30);
            }

            //using (GameSquare game = new GameSquare())
            //{
            //	game.Run(30, 30);
            //}
            //using (Game game = new Game())
            //{
            //	game.Run(30, 30);
            //}
        }
	}
}
