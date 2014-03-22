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
        String idfile = @"";
        String verfile = @"";
        String uri = "";
        VersionControlServer vcServer = null;
        Workspace ws = null;
        String maxNum;
        String minNum;

        public Program(String max, String min)
        {
            maxNum = max;
            minNum = min;
        }

        public void Run()
        {
            if(!ConnectTFS())
            {
                Console.WriteLine("Connect TFS is fail!");
                return;
            }

            if(!ChangeBuildID(idfile) || !ChangeBuildID(verfile))
            {
                Console.WriteLine("Change files is fail!");
                return;
            }

            CheckIn();
        }

        public bool ConnectTFS()
        {
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(uri));
            vcServer = tpc.GetService<VersionControlServer>();
            if (vcServer == null)
                return false;
            WorkspaceInfo wsInfo = Workstation.Current.GetLocalWorkspaceInfo(vcServer, @"workspacename", @"chait");
            if(wsInfo == null)
                return false;
            ws = vcServer.GetWorkspace(wsInfo);
            return true;
        }

        public bool ChangeBuildID(String file, String keyword)
        {
            //checkout fils
            try
            {
                if (ws.PendEdit(file) != 1)
                    return false;

                String[] readText = File.ReadAllLines(file);
                for (int i = 0; i < readText.Length; i++ )
                {
                    if (readText[i].Contains(keyword))
                    {
                        //change id
                        readText[i] = keyword + "    " + keyword;
                    }

                }

                File.WriteAllLines(file, readText);
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
            {
                Console.WriteLine("Checked in changeset {0}", changesetForChange);
            }
            else
            {
                Console.WriteLine("Submit change is fail!");
                ws.Undo(pendingChanges);
            }
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
            program.Run();

            return;
        }
    }
}
