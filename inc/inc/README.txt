
1.	Open inc.csproj from VS2013. 
2.	Change the following setting to match with your environment.  
    String idfile = @"F:\Maestro\U\components\global\src\objectdbx\inc\_idver.h";  /*Local file path*/
    String verfile = @"F:\Maestro\U\components\global\src\heidi\source\heidi\_version.h";  /*Local file path*/
    String userName = @"chait";          /* Current user name*/
    String workspaceName = @"SINSGH128SDGL_M_U";            /* Current workspace */
3.	Build project. 
4.	Run program by “./inc.exe 4 2” .  It takes 2 parameters. The first is Major build id. The second is Minor buld id. 
5.	If it is success. You will get change list id. If you get any other message, please check you environment or contact tristan.chai@autodesk.com.
