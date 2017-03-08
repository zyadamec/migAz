using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace MigAz.Core
{
    public class VersionCheck
    {
        public async Task NewVersionAvailable()
        {
            // AWS
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.migaz.tools/v1/version/AWStoARM");
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/version");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                string version = "\"" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\"";
                string availableversion = result.ToString();

                if (version != availableversion)
                {
                    //MessageBox.Show("New version " + availableversion + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
