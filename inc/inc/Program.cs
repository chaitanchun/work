using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.VersionControl;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;

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
        String uri = "";
        String minKeywork = "";
        VersionControlServer vcServer = null;
        Workspace ws = null;
        String maxNum;
        String minNum;

        public Program(String max, String min)
        {
            maxNum = max;
            minNum = min;
        }

        public bool ConnectTFS()
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(uri));
            vcServer = tpc.GetService<VersionControlServer>();
            if (vcServer == null)
                return false;
            WorkspaceInfo wsInfo = Workstation.Current.GetLocalWorkspaceInfo(vcServer, @"workspacename", @"UserName");
            if(wsInfo == null)
                return false;
            ws = vcServer.GetWorkspace(wsInfo);
            return true;
        }

        public bool ChangeBuildID(String file)
        {
            //checkout fils
            try
            {
                if (ws.PendEdit(file) != 1)
                    return false;

                //Edit file based on current id

            }
            catch (IOException e)
            {
                Console.WriteLine("Error to edit file {1} :{0}", e.Message, file);
                return false;
            }
            return true;
        }

        public void CheckIn()
        {
            String id = "M0";
            if (maxNum.Length == 1)
                id += "0";
            id += maxNum + "U0";
            if (minNum.Length == 1)
                id += "0";
            id += minNum;
            
            // Get the pending change, and check in the new revision.
            var pendingChanges = ws.GetPendingChanges();
            int changesetForChange = ws.CheckIn(pendingChanges, "Change Build ID to " + id);
            if (changesetForChange > 0)
                Console.WriteLine("Checked in changeset {0}", changesetForChange);
            else
                Console.WriteLine("Submit change is fail!");
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

            Program program = new Program(args[0], args[1]);
            if( !program.ConnectTFS() )
            {
                Console.WriteLine("Connect TFS is fail!");
                return;
            }


            return;
        }
    }
}
