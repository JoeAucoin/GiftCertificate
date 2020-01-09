using System;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Framework.JavaScriptLibraries;
using System.Globalization;
using DotNetNuke.Common;
using PayPal.Api;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Common.Lists;
using System.Net;
using System.Text;
using System.IO;


namespace GIBS.Modules.GiftCertificate
{
    public partial class MakePayment : PortalModuleBase 
    {
     //   public bool _PayPalSandboxMode;
        public string _PayPalPayee;
        public string _PWD;
        public string _SIGNATURE;
        int itemId = Null.NullInteger;
        GiftCertificateInfo item;
        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);

            LoadSettings();

            if (!IsPostBack)
            {
                try
                {

                    if (Request.QueryString["PaymentCancelled"] != null)
                    {
                        lblDebug.Text = "Payment Incomplete . . . Try Again?";
                    }

                    if (Request.QueryString["itemId"] != null)
                    {
                        itemId = Int32.Parse(Request.QueryString["itemId"]);
                        GetGiftCertRecord(itemId);
                        this.CreateLink(item,true);
                    }




                }
                catch (Exception ex)
                {
                    lblDebug.Text += "Error : " + ex.Message;
                }
            }
        }


        public void LoadSettings()
        {

            GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);


            //if (settingsData.PayPalSandboxMode != null)
            //{
            //    _PayPalSandboxMode = bool.Parse(settingsData.PayPalSandboxMode);
            //}
            //else
            //{
            //    _PayPalSandboxMode = true;
            //}

            if (settingsData.PayPalPayee != null)
            {
                _PayPalPayee = settingsData.PayPalPayee;
            }
            else
            {
                _PayPalPayee = "paypal@gibs.com";
            }



        }



        public void GetGiftCertRecord(int itemID)
        {
            try
            {
                //check we have an item to lookup
                if (!Null.IsNull(itemId))
                {
                    //load the item
                    GiftCertificateController controller = new GiftCertificateController();
                    item = controller.GetGiftCert(itemId);
                    if (item != null)
                    {

                        if (item.PaypalPaymentState.ToString().Length > 0)
                        {
                            Response.Redirect(Globals.NavigateURL("Access Denied"), true);
                        }

                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                        //NumberToWords(Int32.Parse(item.CertAmount.ToString())).ToString()

                        lblCertAmount.Text = String.Format("{0:f2}", item.CertAmount) + " - " + textInfo.ToTitleCase(NumberToWords(Int32.Parse(item.CertAmount.ToString())).ToString()).ToString() + " Dollars";
                        lblRecipient.Text = item.ToName;
                        lblPurchaser.Text = item.FromName + "<br />" + item.FromEmail + "<br />" + item.FromPhone;
                        lblMailingTo.Text = item.MailTo + "<br />" + item.MailToAddress;
                        if (item.MailToAddress1.ToString().Length > 0)
                        {
                            lblMailingTo.Text += "<br />" + item.MailToAddress1;
                        }
                        
                        lblMailingTo.Text += "<br />" + item.MailToCity + ", " + item.MailToState + " " + item.MailToZip;
                        string MailAddress = "";
                        MailAddress = item.ToName + Environment.NewLine + item.MailToAddress + " " + item.MailToAddress1 + Environment.NewLine + item.MailToCity + ", " + item.MailToState + " " + item.MailToZip;
                        lblNotes.Text = item.Notes;
                    }
                    else
                    {
                        Response.Redirect(Globals.NavigateURL(), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        protected void CreateLink(GiftCertificateInfo item ,bool sendShippingAddress )
        {


            //Components.Configuration mySettings = new Components.Configuration();
            //mySettings.SandboxMode = _PayPalSandboxMode;
            
            var apiContext = GIBS.Modules.GiftCertificate.Components.Configuration.GetAPIContext();
            string payerId = Request.Params["PayerID"];
            
            if (string.IsNullOrEmpty(payerId))
            {
                var itemList = new ItemList();
                if (sendShippingAddress == true)
                {
                     itemList = new ItemList()
                    {
                        items = new List<Item>() 
                    {
                        new Item()
                        {
                            name = this.PortalSettings.PortalName.ToString() + " Gift Certificate",
                            currency = "USD",
                            price = item.CertAmount.ToString(),
                            quantity = "1",
                            sku = "GiftCertificate"
                         }
                    }
                        //,
                        //shipping_address = new ShippingAddress()
                        //{
                        //    city = item.MailToCity,
                        //    country_code = "US",
                        //    line1 = item.MailToAddress,
                        //    line2 = item.MailToAddress1,
                        //    postal_code = item.MailToZip,
                        //    state = item.MailToState,
                        //    recipient_name = item.ToName
                        //}

                    };
                }
                else
                {
                     itemList = new ItemList()
                    {
                        items = new List<Item>() 
                    {
                        new Item()
                        {
                            name = "Gift Certificate",
                            currency = "USD",
                            price = item.CertAmount.ToString(),
                            quantity = "1",
                            sku = "GiftCertificate"
                         }
                    }
                      
                    };


                }
                
                
                
                var payer = new Payer() { payment_method = "paypal" };
        //        lblCertAmount.Text = String.Format("{0:f2}", item.CertAmount);
                // ###Redirect URLS
                var baseURI = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "PayPalParse", "mid=" + ModuleId.ToString()) + "?userId=" + UserId.ToString();
                var guid = Convert.ToString((new Random()).Next(100000));
                var redirectUrl = baseURI + "&guid=" + guid + "&itemId=" + Convert.ToInt32(itemId);
                var redirUrls = new RedirectUrls()
                {
                    cancel_url = redirectUrl + "&cancel=true",
                    return_url = redirectUrl
                };
                // ###Details
                // Let's you specify details of a payment amount.
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = item.CertAmount.ToString()
                };
                // ###Amount
                // Let's you specify a payment amount.
                var amount = new Amount()
                {
                    currency = "USD",
                    total = item.CertAmount.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                    details = details
                };

                var _payee = new Payee();
                _payee.email = _PayPalPayee.ToString();     // "paypal@chapinsfishandchips.com";
                
                // ###Transaction
                // A transaction defines the contract of a
                // payment - what is the payment for and who
                // is fulfilling it. 
                var transactionList = new List<Transaction>();
                // The Payment creation API requires a list of
                // Transaction; add the created `Transaction`
                // to a List
                transactionList.Add(new Transaction()
                {
                    payee = _payee,
                    note_to_payee = lblNotes.Text.ToString(),
                    description = "Transaction description.",
                    invoice_number = Common.GetRandomInvoiceNumber(),
                    amount = amount,
                    item_list = itemList
                });
                // ###Payment
                var payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
                //Create a payment using a valid APIContext
                try
                {
                    var createdPayment = payment.Create(apiContext);
             //       lblDebug.Text += " Response from Create: " + createdPayment.ConvertToJson();
                    // Using the `links` provided by the `createdPayment` object, we can give the user the option to redirect to PayPal to approve the payment.
                    var links = createdPayment.links.GetEnumerator();
                    while (links.MoveNext())
                    {
                        var link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                         //   flowGo.Text = "Complete Payment";
                            flowGo.NavigateUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                }
                catch (PayPal.HttpException pp)
                {
                    CreateLink(item, false);
                    //lblDebug.Text += "pp message : " + pp.Message.ToString();
                    //lblDebug.Text += "pp message : " + pp.InnerException.ToString();
                    //lblDebug.Text += "pp message : " + pp.Response.ToString();
                }
            }
        }

        protected void LinkButtonMakeCorrections_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "", "mid=" + ModuleId.ToString() + "&ItemId=" + Request.QueryString["ItemId"]));
          
        }
    }
}