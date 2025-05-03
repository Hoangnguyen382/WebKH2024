using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebKH2024.Models.Momo
{
    public class MoMoPaymentConfig
    {
        public string PartnerCode { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string EndPoint { get; set; }
    }
}