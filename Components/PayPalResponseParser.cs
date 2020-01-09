using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using GIBS.Modules.GiftCertificate;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common;
using PayPal.Api;
using PayPal;
using System.Net;
using System.Text;
using System.IO;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace GIBS.Modules.GiftCertificate.Components
{
    public class PayPalResponseParser
    {

        public string rawJsonResponse { get; set; }
        public lRootObject jsonObj { get; set; }
        public void ExecutePayment(string PaymentID,string PayerID)
        {
            var apiContext = GIBS.Modules.GiftCertificate.Components.Configuration.GetAPIContext();
            var paymentId = PaymentID.ToString();   // Request.QueryString["paymentId"]; ;
            var payerId = PayerID.ToString();   // Request.QueryString["PayerID"].ToString();
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            var executedPayment = payment.Execute(apiContext, paymentExecution);
            rawJsonResponse = Common.FormatJsonString(executedPayment.ConvertToJson());
            jsonObj = new JavaScriptSerializer().Deserialize<lRootObject>(executedPayment.ConvertToJson());
            
           //payment.

        }
   
    }


    public class lShippingAddress
    {
        public string recipient_name { get; set; }
        public string line1 { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
        public string postal_code { get; set; }
        public string state { get; set; }
    }

    public class lPayerInfo
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string payer_id { get; set; }
        public ShippingAddress shipping_address { get; set; }
    }

    public class lPayer
    {
        public string payment_method { get; set; }
        public string status { get; set; }
        public PayerInfo payer_info { get; set; }
    }

    public class lDetails
    {
        public string subtotal { get; set; }
    }

    public class lAmount
    {
        public string currency { get; set; }
        public string total { get; set; }
        public Details details { get; set; }
    }

    public class lItem
    {
        public string quantity { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string sku { get; set; }
    }

    public class lItemList
    {
        public List<Item> items { get; set; }
    }

    public class lTransaction
    {
        public List<object> related_resources { get; set; }
        public Amount amount { get; set; }
        public string description { get; set; }
        public string invoice_number { get; set; }
        public ItemList item_list { get; set; }
    }

    public class lLink
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class lRootObject
    {
        public string id { get; set; }
        public string intent { get; set; }
        public Payer payer { get; set; }
        public List<Transaction> transactions { get; set; }
        public string state { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public List<lLink> links { get; set; }
    }
}