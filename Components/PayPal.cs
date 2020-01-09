using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Collections.Specialized;

using PayPal;
using PayPal.Exception;
using PayPal.Util;
using PayPal.AdaptivePayments;
using PayPal.AdaptivePayments.Model;

using System.Configuration;

namespace GIBS.Modules.GiftCertificate
{
    public class GIBSPayPal
    {
        public string PayPalRedirectUrl { get; set; }
        public string PayPalPaykey { get; set; }
        public string PayPalPaymentExecStatus { get; set; }
        public bool PayPalError { get; set; }
        public string PayPalErrorMessage { get; set; }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void Pay()
        {
            Random r = new Random();
            int PayPalPurchaseTransactionId = r.Next();// -10;
            string PayPalRedirectUrl = "www.cnn.com";// ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"];
            // Instantiate PayPalResponse class
            GIBSPayPal objPayPalResponse = new GIBSPayPal();
            objPayPalResponse.PayPalPaykey = " ";
            objPayPalResponse.PayPalPaymentExecStatus = " ";
            objPayPalResponse.PayPalRedirectUrl = PayPalRedirectUrl;
            objPayPalResponse.PayPalErrorMessage = " ";
            objPayPalResponse.PayPalError = false;
            string currentPath =
                    System.Web.HttpContext.Current.Request.Url.OriginalString
                    .Replace(@"/PayPal_Call_back.ashx", "");
            
            string strActionType = "PAY";
            string currencyCode = "USD";
            string cancelUrl = string.Format(@"{0}&mode=cancel", currentPath);
            string returnUrl = String.Format(@"{0}&payment=complete", currentPath);
            string IpnURL = String.Format(@"{0}/PayPal/IPNListener.aspx", currentPath);
            
            ReceiverList receiverList = new ReceiverList();
            receiverList.receiver = new List<Receiver>();
            
            Receiver Receiver1 = new Receiver(Decimal.Parse("5.59"));
            Receiver1.email = "joe-facilitator@gibs.com";
            Receiver1.primary = false;
            Receiver1.invoiceId = "";
            
            Receiver1.paymentType = "SERVICE";
            
            receiverList.receiver.Add(Receiver1);
           
            PayRequest req = new PayRequest(new RequestEnvelope("en_US"),
            strActionType,
            cancelUrl,
            currencyCode,
            receiverList,
            returnUrl);
            
            //// IPN Url (only enable with a published internet accessable application)
            //req.ipnNotificationUrl = IpnURL;
            //req.reverseAllParallelPaymentsOnError = true;
            //req.trackingId = PayPalPurchaseTransactionId.ToString();


            // Call PayPal to get PayKey  
            Dictionary<string, string> configMap = new Dictionary<string, string>();
            //configMap = GetConfig();
            configMap.Add("mode", "sandbox");
            // Signature Credential
            configMap.Add("account1.apiUsername", "joe-facilitator_api1.gibs.com");
            configMap.Add("account1.apiPassword", "5UX8EJBTREJQ33MH");
            configMap.Add("account1.apiSignature", "An5ns1Kso7MWUdW4ErQKJJJ4qi4-AM3p4jdO1vcSS2ClRObAqbmoxWeN");
            configMap.Add("account1.applicationId", "APP-80W284485P519543T");

            //configMap.Add("account1.apiUsername", "joe_api1.gibs.com");
            //configMap.Add("account1.apiPassword", "Y5M699WXLTEF976T");
            //configMap.Add("account1.apiSignature", "AUHOQLvCB6h8yLQ2ZC0YcUb3EuZjAY-OG4J5Hkaj35SjMEkMxhoMslvt");
            //configMap.Add("account1.applicationId", "APP-80W284485P519543T");
            // Sandbox Email Address
            //configMap.Add("sandboxEmailAddress", "ray-facilitator@cape.com");

            AdaptivePaymentsService service = new AdaptivePaymentsService(configMap);
            PayResponse resp = null;
            try
            {
                resp = service.Pay(req);
            }

            catch (System.Exception e)
            {
                objPayPalResponse.PayPalError = true;
                objPayPalResponse.PayPalErrorMessage = e.Message;
                PayPalPaykey = " Catch " + e.Message ;// resp.payKey;
            }
            // Check for errors
            if ((resp.responseEnvelope.ack == AckCode.FAILURE) ||
                (resp.responseEnvelope.ack == AckCode.FAILUREWITHWARNING))
            {
                string strError = "";
                objPayPalResponse.PayPalError = true;
                foreach (var error in resp.error)
                {
                    strError = strError + " " + error.message;
                }
                objPayPalResponse.PayPalErrorMessage = strError;
                PayPalPaykey = " " + strError;// resp.payKey;
            }
            else
            {
                objPayPalResponse.PayPalPaykey = resp.payKey;
                PayPalPaykey =resp.payKey;
                objPayPalResponse.PayPalPaymentExecStatus = resp.paymentExecStatus;
            }
        }
    }
}

