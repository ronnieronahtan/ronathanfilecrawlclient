using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using ronathanFileCrawler;
//using ServiceModel;
//using System.Manage
namespace ConsoleApp1
{
    class ronnieboyAccessToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int not_before_policy { get; set; }
        public string session_state { get; set; }
        public string scope { get; set; }
    }

    //subscriptoin class
    public class Subscription
    {
        public string User { get; set; }
        public string Topic { get; set; }
        public string Callback { get; set; }
    }

    class Program
    {

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Ronathan File System Crawler!");
            
            Console.WriteLine("1) To upload a single document for some webhook");
            //Don't use these options option
            Console.WriteLine("2) To Crawl local Filesystem");
            Console.WriteLine("3) To Crawl Organization (network connected systems)");
            Console.WriteLine("4) To Crawl System");
            Console.WriteLine("6) print file as 64");
           
            Console.WriteLine("Enter Choice: ");
            var choice = Console.ReadLine();
            
            
            if(choice == "1")
            {
                
                Console.WriteLine("Enter the file path: ");
                var filePath = Console.ReadLine();
                //check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist");
                    return;
                }
                var AuthenticationType = "JWT";
                var client = new HttpClient();
                var authHeader = "Basic "+createBasicAuthHeader("CHANGEME", "CHANGEME");
                var accessToken2 = "";
                if (AuthenticationType == "keycloak")
                {
                    //var myToken = await ServiceModel.signalRHelper.Signalrclient.getAccessToken();
                    var myToken = await ronathanFileCrawler.Helper.getAccessToken();
                    var  accessToken = JsonSerializer.Deserialize<ronnieboyAccessToken>(myToken);
                    //post the file to the server at the url of https://ronathanbeta.esr-inc.com:6001/api/Document/validateControlImplementation 

                    //check if the access token is null
                    if (accessToken == null)
                    {
                        Console.WriteLine("Access Token is null");
                        return;
                    }
                    authHeader = "Bearer " + accessToken.access_token;
                    accessToken2 = accessToken.access_token;
                } else if (AuthenticationType == "JWT")
                {
                    var myToken = await ronathanFileCrawler.Helper.getAccessToken();
                    //var  accessToken = JsonSerializer.Deserialize<ronnieboyAccessToken>(myToken);

                    //post the file to the server at the url of https://ronathanbeta.esr-inc.com:6001/api/Document/validateControlImplementation
                    if (myToken == null)
                    {
                        Console.WriteLine("Access Token is null");
                        return;
                    }
                    authHeader = "Bearer " + myToken;
                    accessToken2 = myToken;
                }
                var miguid = Guid.NewGuid().ToString();


                subscribeToWebhookAsync(miguid, accessToken2);


                //add the access token to the header of the request
                client.DefaultRequestHeaders.Add("Authorization", authHeader);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://ronathanbeta.esr-inc.com:6001/api/Document/validateControlImplementation")
                  
                  
                };
                //read the file and convert it to a byte array
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                //convert the byte array to a base64 string
                string base64String = Convert.ToBase64String(fileBytes);
                
                //create a ImplemenationValidationRequest object 
               
               ImplemenationValidationRequest requestObj = new ImplemenationValidationRequest();
               // requestObj.answer = "yes, we maintain an audit policy!";
               // requestObj.question = "Do you have a policy and procedure or standard for how systems should be logged or audited?";
                //populate a new fileForValidation object with the file name and the base64 string
                fileForValidation fileForValidation = new fileForValidation();
                //get the file name from the file path
                var fileName = System.IO.Path.GetFileName(filePath);
                fileForValidation.fileName = fileName;
                fileForValidation.file = base64String;
                requestObj.evidence = new fileForValidation[1];
                requestObj.evidence[0] = fileForValidation;
                requestObj.controls = new string[5];
                requestObj.controls[0] = "AU-8";
                requestObj.controls[1] = "AU-9";
                requestObj.controls[2] = "AU-12";
                requestObj.controls[3] = "3.3.4";
                requestObj.controls[4] = "3.3.1";

                requestObj.valiationId = miguid;
                Console.WriteLine("Validation ID: " + requestObj.valiationId);
                //convert the object to a json string
                string json = JsonSerializer.Serialize(requestObj);

                //set the content of the request to the json string
                request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                //send the request

                var response = await client.SendAsync(request);
                Console.WriteLine("Uploaded Document: "+response);
            }
            else if(choice == "2")
            {

                //removed for simplicity extration
              


            }
            else if(choice == "3")
            {
                //removed for extration simplicity
               // crawlOrganization();
            }
            else if(choice == "4")
            {
                //removed for extration simplicity
                // crawlSystem();
            }
            else if(choice == "6")
            {
                Console.WriteLine("Enter the file path: ");
                var filePath = Console.ReadLine();
                //check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist");
                    return;
                }
                PrintFileAsBase64(filePath);
            }
            else
            {
                Console.WriteLine("Invalid Choice");
            }
           
        }


        //creates a basic authentication header
        static string createBasicAuthHeader(string username, string password)
        {
            //create a string with the username and password
            string usernamePassword = username + ":" + password;
            //convert the string to a byte array
            byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(usernamePassword);
            //convert the byte array to a base64 string
            string base64String = Convert.ToBase64String(byteArray);
            //return the base64 string
            return base64String;
        }

        // public record Subscription(string User, string Topic, string Callback);
        //subsribes to the webhook for the document upload https://ronathanbeta.esr-inc.com:6001/subscribe
        static async Task subscribeToWebhookAsync(string myvalId, string token)
        {
            //create a new http client
            var client = new HttpClient();
            //create a new http request message
            var request = new HttpRequestMessage(HttpMethod.Post, "https://ronathanbeta.esr-inc.com:6001/api/document/subscribe");
            //set the content type to application/json

            //set the body of the request to the Subscription record
            var subscription = new Subscription
            {
                User = "beta@ronathan.ai",
                Topic = myvalId,
                Callback = "http://10.0.0.205:6002/api/cc/ronathanagentfilecrawler"
            };


            var authHeader = "Bearer " + token;

            request.Content = new StringContent(JsonSerializer.Serialize(subscription));
            client.DefaultRequestHeaders.Add("Authorization", authHeader);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            //send the request
            var response = await client.SendAsync(request);
            Console.WriteLine("Response: WTF");
            //check if the response was successful
            if (response.IsSuccessStatusCode)
            {

                //write the response to the console
                Console.WriteLine("Secuess: " + response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Console.WriteLine(response.StatusCode.ToString());

                //write the response to the console
                Console.WriteLine("Fail: " + response.Content.ReadAsStringAsync().Result);
                //printout the response status code
                Console.WriteLine("Status Code: " + response.StatusCode);
                //printout the response reason phrase
                Console.WriteLine("Reason Phrase: " + response.ReasonPhrase);


            }
        }


        public static void PrintFileAsBase64(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(fileBytes);
            Console.WriteLine(base64String);
            using (StreamWriter writer = new StreamWriter("d:\\ronathan\\base64.txt"))
            {
                writer.WriteLine(base64String);
            }
        }

        [DllImport("iphlpapi.dll", EntryPoint = "GetIpNetTable")]
        public static extern int GetIpNetTable(IntPtr pIpNetTable, ref int pdwSize, bool bOrder);

        //fucntion to get the ip address of the machine
        public static IPAddress GetIPAddress()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addr;
                }
            }
            return null;
        }
        //function to get the mac address of the machine
        public static PhysicalAddress GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress();
        }
        private static Dictionary<IPAddress, PhysicalAddress> GetAllDevicesOnLAN()
        {
            Dictionary<IPAddress, PhysicalAddress> all = new Dictionary<IPAddress, PhysicalAddress>();

            all.Add(GetIPAddress(), GetMacAddress());
            int spaceForNetTable = 0;

            GetIpNetTable(IntPtr.Zero, ref spaceForNetTable, false);
            IntPtr rawTable = IntPtr.Zero;

            try
            {
                rawTable = Marshal.AllocCoTaskMem(spaceForNetTable);
                int errorCode = GetIpNetTable(rawTable, ref spaceForNetTable, false);
                if (errorCode != 0)
                {
                    throw new Exception(string.Format(
                                            "Unable to retrieve network table. Error code {0}", errorCode));
                }
                int rowsCount = Marshal.ReadInt32(rawTable);
                IntPtr currentBuffer = new IntPtr(rawTable.ToInt64() + Marshal.SizeOf(typeof(Int32)));
                MIB_IPNETROW[] rows = new MIB_IPNETROW[rowsCount];
                for (int index = 0; index < rowsCount; index++)
                {
                    rows[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new IntPtr(currentBuffer.ToInt64() +
                                                                                  (index * Marshal.SizeOf(typeof(MIB_IPNETROW)))
                                                                                  ),
                                                                       typeof(MIB_IPNETROW));
                }
                PhysicalAddress virtualMAC = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
                PhysicalAddress broadcastMAC = new PhysicalAddress(new byte[] { 255, 255, 255, 255, 255, 255 });
                foreach (MIB_IPNETROW row in rows)
                {
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    byte[] rawMAC = new PhysicalAddress(BitConverter.GetBytes(row.dwPhysAddrLen)).GetAddressBytes();
                    PhysicalAddress pa = new PhysicalAddress(rawMAC);
                    if (!pa.Equals(virtualMAC) && !pa.Equals(broadcastMAC) )
                    {
                        //Console.WriteLine("IP: {0}\t\tMAC: {1}", ip.ToString(), pa.ToString());
                        if (!all.ContainsKey(ip))
                        {
                            all.Add(ip, pa);
                        }
                    }
                }
            }
            finally
            {
                // Release the memory.
                Marshal.FreeCoTaskMem(rawTable);
            }
            return (all);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_IPNETTABLE
    {
        public int dwNumEntries;
        public MIB_IPNETROW table;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_IPNETROW
    {
        public int dwIndex;
        public int dwPhysAddrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bPhysAddr;
        public int dwAddr;
        public int dwType;
    }
    //ronathan FileSystem Crawler
    public class Crawler
    {
        public static void crawlOrganization()
        {
            //get all the drives on the system
            string[] drives = System.Environment.GetLogicalDrives();
            //iterate through the drives looking for pdf, text, word  , and image files
            var fileTypes = new string[] { ".pdf", ".txt", ".doc", ".docx", ".jpg", ".png", ".gif",".jpeg" };
            var listOfFiles = new List<string>();
            foreach (var drive in drives)
            {
                string[] files = Directory.GetFiles(drive+ @":\", "*", SearchOption.AllDirectories);
                //if the file is a pdf, text, word  , or image file add it to the list
                foreach (var file in files)
                {
                    if (fileTypes.Contains(Path.GetExtension(file)))
                    {
                        listOfFiles.Add(file);
                    }
                }
            }
            

        }
        public static void runCrawlerOnDiffrentSystem(string ipAddress)
        {
            //ManagementScope scope = new ManagementScope("\\\\"+ipAddress+"\\root\\cimv2");
            //scope.Connect();

            //ManagementPath path = new ManagementPath("Win32_Process");
            //ManagementClass processClass = new ManagementClass(scope, path, null);

            //object[] methodArgs = { "path_to_exe_file" };
            //processClass.InvokeMethod("Create", methodArgs);
        }

    }
}
