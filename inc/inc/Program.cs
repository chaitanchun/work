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
        String uri = "http://tfs.autodesk.com:8080/tfs/AcadCollection";
        String idMaxKeywork = "ACADV_BLDMAJOR";
        String idMinKeywork = "ACADV_BLDMINOR";
        String verMaxKeywork = "HEIDI_BLDMAJOR_VERSION";
        String verMinKeywork = "HEIDI_BLDMINOR_VERSION";
        
        //The following setting need to based on your current environment!
        String idfile = @"D:\Maestro\U\components\global\src\objectdbx\inc\_idver.h";  /*Local file path*/
        String verfile = @"D:\Maestro\U\components\global\src\heidi\source\heidi\_version.h";  /*Local file path*/
        String userName = @"chait";          /* Current user name*/
        String workspaceName = @"SIN2XMGF22_M_U";            /* Current workspace */
        
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
                Console.WriteLine("Connect to TFS is fail!");
                return;
            }

            if(!ChangeBuildID(idfile, idMaxKeywork, idMinKeywork) || !ChangeBuildID(verfile, verMaxKeywork, verMinKeywork))
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
            WorkspaceInfo wsInfo = Workstation.Current.GetLocalWorkspaceInfo(vcServer, workspaceName, userName);
            if(wsInfo == null)
                return false;
            ws = vcServer.GetWorkspace(wsInfo);
            return true;
        }

        public bool ChangeBuildID(String file, String maxKeyword, String minKeyword)
        {
            //checkout fils
            try
            {
                if (ws.PendEdit(file) != 1)
                    return false;

                String[] readText = File.ReadAllLines(file);
                bool isMaxChanged = false;
                for (int i = 0; i < readText.Length; i++ )
                {
                    if (readText[i].Contains(maxKeyword))
                    {
                        //change id
                        readText[i] = "#define " + maxKeyword + "  " + maxNum;
                        isMaxChanged = true;
                        continue;
                    }

                    if (readText[i].Contains(minKeyword))
                    {
                        readText[i] = "#define " + minKeyword + "  " + minNum;
                        if (isMaxChanged)
                            break;
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
            id += maxNum + "." + minNum;
            
            // Get the pending change, and check in the new revision.
            String[] items = new String[2];
            items[0] = idfile;
            items[1] = verfile;
            var pendingChanges = ws.GetPendingChanges(items);
            int changesetForChange = ws.CheckIn(pendingChanges, "Increment Build ID to " + id);
            if (changesetForChange > 0)
            {
                Console.WriteLine("Checked in changeset {0}", changesetForChange);
            }
            else
            {
                Console.WriteLine("Submit changelist is fail!");
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
            if(maxVer <= 0 || maxVer > 100 || minVer < 0 || minVer > 300)
            {
                Console.WriteLine("maxVer <= 0 || maxVer > 100 || minVer < 0 || minVer > 300");
                return;
            }

            Program program = new Program(args[0], args[1]);
            program.Run();

            return;
        }
    }
}
