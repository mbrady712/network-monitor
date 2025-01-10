namespace network_monitor.Models
{
    public class SubnetCalcModel
    {
        public string? SubnetMask { get; set; }
        public string? IPAddress { get; set; }
        public string? NetworkAddress { get; set; }
        public string? FirstUsableAddress { get; set; }

        public string? LastUsableAddress { get; set; }

        public string? BroadcastAddress { get; set; }

        public string? TotalNumHosts { get; set; }

        public string? NumUsableHosts { get; set; }

        public string? BinarySubnetMask { get; set; }

        public string? CIDR { get; set; }

        public Dictionary<string, string> propertyLabels = new Dictionary<string, string>
            {
                { nameof(SubnetMask), "Subnet Mask" },
                { nameof(IPAddress), "IP Address" },
                { nameof(NetworkAddress), "Network Address" },
                { nameof(FirstUsableAddress), "First Usable Address" },
                { nameof(LastUsableAddress), "Last Usable Address" },
                { nameof(BroadcastAddress), "Broadcast Address" },
                { nameof(TotalNumHosts), "Total Number of Hosts" },
                { nameof(NumUsableHosts), "Number of Usable Hosts" },
                { nameof(BinarySubnetMask), "Binary Subnet Mask" },
                { nameof(CIDR), "CIDR Notation" },
            };
        }

    }

