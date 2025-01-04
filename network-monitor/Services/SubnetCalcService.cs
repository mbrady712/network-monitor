using System.Reflection.Metadata.Ecma335;
using network_monitor.Models;

namespace network_monitor.Services
{
    public class SubnetCalcService
    {
    public SubnetCalcModel Calculate(SubnetCalcModel model) {

        // Get the IP address and subnet mask from the model
        string ipAddress = model.IPAddress;
        string subnetMask = model.SubnetMask;

        // Split the IP address and subnet mask into octets
        string[] ipOctets = ipAddress.Split('.');
        string[] subnetOctets = subnetMask.Split('.');

        // Convert the octets to integers
        int[] ipInts = new int[4];
        int[] subnetInts = new int[4];
        for (int i = 0; i < 4; i++)
        {
            ipInts[i] = int.Parse(ipOctets[i]);
            subnetInts[i] = int.Parse(subnetOctets[i]);
        }

        // Calculate the network address
        int[] networkInts = new int[4];
        for (int i = 0; i < 4; i++)
        {
            networkInts[i] = ipInts[i] & subnetInts[i];
        }
        model.NetworkAddress = string.Join('.', networkInts);

        // Calculate the first usable address
        int[] firstUsableInts = networkInts;
        firstUsableInts[3] = firstUsableInts[3] + 1;
        model.FirstUsableAddress = string.Join('.', firstUsableInts);

        // Calculate binary subnet mask and CIDR notation
        string[] binarySubnetMask = new string[4];
        binarySubnetMask = decimalToBinary(subnetInts);
        int CIDR = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 8; k++)
            {
                if (binarySubnetMask[i][k] == '1')
                {
                    CIDR++;
                }
            }
        }
        model.BinarySubnetMask = string.Join(".", binarySubnetMask);
        model.CIDR = "/" + CIDR.ToString();

        // Calculate the total number of hosts
        int totalHosts = 2 ^ (32 - CIDR);
        model.NumUsableHosts = (totalHosts - 2).ToString();
        model.TotalNumHosts = totalHosts.ToString();

        // Calculate the broadcast address
        //Convert network address to binary
        string[] binaryNetworkAddress = new string[4];
        binaryNetworkAddress = decimalToBinary(networkInts);
        //Set host bits to 1
        int hostBits = 32 - CIDR;
        int hostBitsRemaining = hostBits;
        int j;
        for (int i = 3; i >= 0; i--)
        {
            j = 7;
            while (hostBitsRemaining > 0 && j >= 0)
            {
                if (binaryNetworkAddress[i][j] == '0')
                {
                    binaryNetworkAddress[i] = binaryNetworkAddress[i].Remove(j, 1).Insert(j, "1");
                }
                j--;
                hostBitsRemaining--;
            }
        }
        string[] binaryBroadcastAddress = binaryNetworkAddress;
        //Convert binary broadcast address to decimal
        int[] broadcastInts = binaryToDecimal(binaryBroadcastAddress);
        model.BroadcastAddress = string.Join('.', broadcastInts);

        // Calculate the last usable address
        int[] lastUsableInts = broadcastInts;
        lastUsableInts[3] = lastUsableInts[3] - 1;
        model.LastUsableAddress = string.Join('.', lastUsableInts);

        static string[] decimalToBinary(int[] decimalInts)
            {
                string[] binary = ["", "", "", ""];
                int dividend, remainder, quotient;
                string stringRemainder;
                for (int i = 0; i < 4; i++)
                {
                    dividend = decimalInts[i];
                    for (int j = 0; j < 8; j++)
                    {
                        remainder = dividend % 2;
                        binary[i] = binary[i].Insert(0, remainder.ToString());
                        quotient = dividend / 2;
                        dividend = quotient;
                    }
                }
                return binary;
            }

        static int[] binaryToDecimal(string[] binary)
            {
                int[] decimals = [0,0,0,0];
                int octet;
                for (int i = 0; i < 4; i++)
                {
                    octet = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if (binary[i][j] == '1')
                        {
                            octet += (Convert.ToInt32(Math.Pow(2, Math.Abs(j - 7))));
                        }
                    }
                    decimals[i] = octet;
                }
                return decimals;
            }

            return model;
    
        }
    }
}
