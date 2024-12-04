namespace network_monitor.Helpers
{
    using System.Diagnostics;

    public static class NetworkUtils
    {
        public static string ExecuteArpCommand()
        {
            try
            {
                // Initialize the process start info
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c arp -a",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Start the process
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();

                    // Read the standard output and wait for process to exit
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return output;
                }
            }
            catch (Exception ex)
            {
                return $"Error executing command: {ex.Message}";
            }
        }
    }

}
