using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDL
{
    public class RegisterClientMDL
    {
        public string clientName { get; set; }
        public int clientId { get; set; }
        public string dashboardUrl { get; set; }
        public string ackUrl { get; set; }
        public string alertUrl { get; set; }
        public string broadcastCommandUrl { get; set; }
        public bool ackSend { get; set; }
        public bool alertSend { get; set; }
        public bool dashboardSend { get; set; }
        public bool broadcastCommandSend { get; set; }
        public string operation { get; set; }
        public string obddashboardUrl { get; set; }
        public bool obdSend { get; set; }

        public bool locationBuildAllow { get; set; }
        public string etmsTripAlertURL { get; set; }
        public bool etmsTripAlertSend { get; set; }
        public string inboundTripAlertURL { get; set; }
        public bool inboundTripAlertSend { get; set; }

    }
}
