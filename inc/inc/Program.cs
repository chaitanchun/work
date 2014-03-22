using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.VersionControl;
using Microsoft.TeamFoundation.Client;

namespace inc
{
    class Program
    {
        String user = "";
        String workspace = "";
        String password = "";
        String idfile = "";
        String verfile = "";
        String maxKeyword = "";

        String minKeywork = "";

        public bool ChangeBuildID(String file, String max, String min)
        {
            return true;
        }

        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("Please give the correct number of parameter. Maxver Minver is 2 parameter!");
                return;
            }
            int maxVer = 0;
            int minVer = -1;
            try
            {
                maxVer = Convert.ToInt32(args[0]);
                minVer = Convert.ToInt32(args[1]);
            }
            catch (FormatException )
            {
                Console.WriteLine("Input string is not a sequence of digits.");
                return;
            }
            catch (OverflowException)
            {
                Console.WriteLine("The parameter is overflow.");
                return;
            }
            if(maxVer <= 0 || maxVer > 100 || minVer < 0 || minVer > 20)
            {
                Console.WriteLine("maxVer <= 0 || maxVer > 100 || minVer < 0 || minVer > 20");
                return;
            }

            Program program = new Program();

        }
    }
}
