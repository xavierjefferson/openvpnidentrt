using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using log4net;

namespace openvpnidentrt
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        private static void Main(string[] args)
        {
            IPAddress pAddress = null;
            IPAddress pAddress1;
            int num;
            log.InfoFormat("{1} called with arguments {0}", string.Join(" ", args), MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetName());
            if ((args.Length < 6 || args[5] != "init" && args[5] != "restart" ||
                 !IPAddress.TryParse(args[4], out pAddress) || !IPAddress.TryParse(args[3], out pAddress1) ||
                 !int.TryParse(args[2], out num) || !int.TryParse(args[1], out num)))
            {
                log.InfoFormat("Invalid args {0}", string.Join(" ", args));
            }
            else
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo =
                        {
                            FileName = "route.exe",
                            Arguments = string.Format("ADD 0.0.0.0 MASK 0.0.0.0 {0} metric 50", pAddress)
                        }
                    };
                    log.DebugFormat("Running route.exe with {0}", process.StartInfo.Arguments);
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception exception)
                {
                    log.Error("Can't execute route.exe", exception);
                }
            }
        }
    }
}