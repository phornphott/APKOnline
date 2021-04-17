using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace APKOnline
{
    public class NotiHub : Hub
    {
        public void GetStaffNoti(int index, string message)
        {
            Clients.All.NotiData(index, message);
        }
    }
}