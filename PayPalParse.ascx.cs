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
using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common;
using PayPal.Api;
using PayPal;
using System.Net;
using System.Text;
using System.IO;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Entities.Controllers;

namespace GIBS.Modules.GiftCertificate
{
    public partial class PayPalParse : PortalModuleBase
    {

       // string[] arrNoAttachements = new string[] { };
      //  string[] emailAttachemnts1 = new string[0];

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
          //  JavaScript.RequestRegistration(CommonJs.DnnPlugins);
        }

        
        
        protected void Page_Load(object sender, EventArgs e)
        {

            hlBuyAnother.NavigateUrl = Globals.NavigateURL();

            if (!IsPostBack)
            {
                GetDropDownListStates();

                if (Request.QueryString["paymentId"] != null)
                {
                    ExecutePayement();
                }

                //if (Request.QueryString["cancel"] != null && Request.QueryString["itemId"] != null)
                if (Request.QueryString["cancel"] != null)
                {
                    if (Request.QueryString["cancel"].ToString() == "true")
                    {
                  //     Response.Redirect(Globals.NavigateURL());
                        Response.Redirect(Globals.NavigateURL(this.TabId, "PayPal", "mid=" + this.ModuleId.ToString() + "&ItemId=" + Request.QueryString["itemId"].ToString() + "&PaymentCancelled=true"));
                        
                    }
                }

            }
        }


        public void GetDropDownListStates()
        {

            try
            {
                // Get State Dropdown from DNN Lists
                //ListController ctlList = new ListController();
                //ListEntryInfoCollection vStates = ctlList.GetListEntryInfoCollection("Region", "Country.US", this.PortalId);

                var regions = new ListController().GetListEntryInfoItems("Region", "Country.US", this.PortalId);

              // ListController lc = new ListController();

               

                //  State
                ddlStatesRecipient.DataTextField = "Value";
                ddlStatesRecipient.DataValueField = "Value";
                ddlStatesRecipient.DataSource = regions;
                ddlStatesRecipient.DataBind();
                ddlStatesRecipient.Items.Insert(0, new ListItem("--", ""));

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void SendGiftCertEmail(int _itemID)
        {

            try
            {

                //load the item
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = controller.GetGiftCert(_itemID);
                if (item != null)
                {              
                
                    GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

                    string RedirectURL = "http://" + Request.Url.Host + "/Success/TabID/" + settingsData.RedirectPage + "/Default.aspx";

                    // BUILD E-MAIL BODY

                    string EmailContent = "";

                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("lblCertAmount.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + String.Format("{0:c}", item.CertAmount) + "<br />&nbsp;</dd>";

             //       EmailContent += "<br clear=all>";

                    EmailContent += "<dt><strong>" + Localization.GetString("lblToName.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.ToName.ToString() + "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

               //     EmailContent += "<br clear=all>";

                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("lblFromName.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.FromName.ToString();
                    EmailContent += "<br>" + item.FromPhone.ToString();
                    EmailContent += "<br>" + item.FromEmail.ToString();
                    EmailContent += "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";



              //      EmailContent += "<br clear=all>";

    
                    EmailContent += "<dl>";
                    
                    EmailContent += "<dt><strong>" + Localization.GetString("lblMailTo.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.MailTo.ToString();
      
                    EmailContent += "<br>" + item.MailToAddress.ToString();
                    if (item.MailToAddress1.ToString().Length > 1)
                    {
                        EmailContent += "<br>" + item.MailToAddress1.ToString();
                    }
                    EmailContent += "<br>" + item.MailToCity.ToString() + ", " +item.MailToState.ToString() + " " + item.MailToZip.ToString();

                    EmailContent += "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

             //       EmailContent += "<br clear=all>";

                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("emailNotes.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.Notes.ToString() + "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

                    EmailContent += "<p>Page Submitted: " + Globals.NavigateURL() + "</p>";

                    //EMAIL THE PURCHASER
                    string EmailFrom = "";
                    if (settingsData.EmailFrom.Length > 1)
                    {
                        EmailFrom = settingsData.EmailFrom.ToString();
                    }
                    else
                    {
                        EmailFrom = PortalSettings.Email.ToString();
                    }

                  
                    
                    // NEW
                   
                    string SMTPUserName = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPUsername");


                    string[] emptyStringArray = new string[0];

                    DotNetNuke.Services.Mail.Mail.SendMail(SMTPUserName.ToString(), item.FromEmail.Trim().ToString(), "", "", 
                        EmailFrom.ToString(), DotNetNuke.Services.Mail.MailPriority.Normal,
                        settingsData.EmailSubject.ToString(), DotNetNuke.Services.Mail.MailFormat.Html, 
                        System.Text.Encoding.ASCII, EmailContent.ToString(), emptyStringArray, 
                        "", "", "", "", true);


                    // EMAIL THE settingsData.EmailNotify
                    // ADD NOTE for ADMINS . . . . 
                    EmailContent += "<p><b>Administrators must log in to process gift certificate.</b></p>";

                    if (settingsData.EmailNotify.Length > 1)
                    {
                        string FromPurchaserEmail = item.FromEmail.ToString();
                        string emailAddress = settingsData.EmailNotify.Replace(" ", "");
                        string[] valuePair = emailAddress.Split(new char[] { ';' });
                  //      string[] emailAttachemnts = null ;
                        for (int i = 0; i <= valuePair.Length - 1; i++)
                        {

                            DotNetNuke.Services.Mail.Mail.SendMail(SMTPUserName.ToString(), valuePair[i].Trim().ToString(), "", "",
                                FromPurchaserEmail.ToString(), DotNetNuke.Services.Mail.MailPriority.Normal, "New Order - " +
                                settingsData.EmailSubject.ToString(), DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.ASCII, 
                                EmailContent.ToString(), emptyStringArray, "", "", "", "", true);
                                                                                   //MailFrom,             MailTo,                     Cc, Bcc,     ReplyTo,                                        Priority,                          Subject,           BodyFormat, BodyEncoding,           Body,                     Attachments, SMTPServer, SMTPAuthentication, SMTPUsername, SMTPPassword, SMTPEnableSSL
                        }

                    }

 

                    string TempAddUserRole = "";
                    if (settingsData.AddUserRole != null)
                    {
                        TempAddUserRole = settingsData.AddUserRole;
                    }

                    ////CREATE USER ACCOUNT
                    //if (settingsData.AddUserRole.Length > 0)
                    //{
                    //    CreateNewUser(txtFromName.Text, txtFromEmail.Text, TempAddUserRole);
                    //}


                    //if (settingsData.RedirectPage.Length > 0)
                    //{
                    //    Response.Redirect(RedirectURL.ToString(), true);
                    //    Response.Redirect(settingsData.RedirectPage, true);
                    //}
                    //else
                    //{
                    //    Response.Redirect(Globals.NavigateURL(), true);
                    //}


                }
                else
                {
                    Response.Redirect(Globals.NavigateURL(), true);
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }




        protected void ExecutePayement()
        {

            try
            {
                PayPalResponseParser prp = new PayPalResponseParser();
                prp.ExecutePayment(Request.QueryString["paymentId"].ToString(), Request.QueryString["PayerID"].ToString());

                var jsonObj = prp.jsonObj;// new JavaScriptSerializer().Deserialize<lRootObject>(executedPayment.ConvertToJson());
                //Payment state, one of the following: created, approved, failed, canceled, expired, pending, or in_progress. 
            //    lblDebug.Text = jsonObj.ToJson().ToString();
                //   Response.End();

                string _line2 = "";

                if (jsonObj.state.ToLower().ToString() == "approved")
                {
                    lblResults.Text += "<b>Transaction: " + jsonObj.state.ToUpper().ToString();
                    lblResults.Text += "</b><br><br>The gift certificate will be mailed to: <br><br>" + jsonObj.payer.payer_info.shipping_address.recipient_name.ToString();
                    lblResults.Text += "<br>" + jsonObj.payer.payer_info.shipping_address.line1.ToString();

                    //string addLine2 = string.Empty;
                    //addLine2 = jsonObj.payer.payer_info.shipping_address.line2;
                    if (jsonObj.payer.payer_info.shipping_address.line2 != null)
                    {
                        lblResults.Text += "<br>" + jsonObj.payer.payer_info.shipping_address.line2.ToString();
                        _line2 = jsonObj.payer.payer_info.shipping_address.line2.ToString();
                    }
                    lblResults.Text += "<br>" + jsonObj.payer.payer_info.shipping_address.city.ToString();
                    lblResults.Text += ", " + jsonObj.payer.payer_info.shipping_address.state.ToString();
                    lblResults.Text += " " + jsonObj.payer.payer_info.shipping_address.postal_code.ToString();

                    txtMailToName.Text = jsonObj.payer.payer_info.shipping_address.recipient_name.ToString();
                    txtToAddress.Text = jsonObj.payer.payer_info.shipping_address.line1.ToString();
                    if (jsonObj.payer.payer_info.shipping_address.line2 != null)
                    {
                        txtToAddress1.Text = jsonObj.payer.payer_info.shipping_address.line2.ToString();
                    }
                    txtToCity.Text = jsonObj.payer.payer_info.shipping_address.city.ToString();
                    ddlStatesRecipient.SelectedValue = jsonObj.payer.payer_info.shipping_address.state.ToString();
                    txtToZip.Text = jsonObj.payer.payer_info.shipping_address.postal_code.ToString();


                }
                else
                {
                    lblResults.Text = "There was a problem with the processing of the payment.";
                }
                UpdateRecord(
                    jsonObj.state.ToString(),
                    jsonObj.payer.payer_info.shipping_address.recipient_name.ToString(),
                    jsonObj.payer.payer_info.shipping_address.line1.ToString(),
                    _line2.ToString(),
                    jsonObj.payer.payer_info.shipping_address.city.ToString(),
                    jsonObj.payer.payer_info.shipping_address.state.ToString(),
                    jsonObj.payer.payer_info.shipping_address.postal_code.ToString()
                    , "", Request.QueryString["paymentId"], jsonObj.ToJson().ToString());

                SendGiftCertEmail(Int32.Parse(Request.QueryString["itemId"].ToString()));



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


            
        }
        protected void UpdateRecord(string PayPalStatus, string mailToName, string Address,string Address1,string City,string State,string Zip,string Note, string PP_PaymentId, string PP_Response)
        {
            try
            {
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = new GiftCertificateInfo();

                item.ItemId             = Convert.ToInt32(Request.QueryString["itemId"].ToString());// itemId;
                item.MailTo             = mailToName;
                item.MailToAddress      = Address;
                item.MailToAddress1     = Address1;
                item.MailToCity         = City;
                item.MailToState        = State;
                item.MailToZip          = Zip;
                item.Notes              = Note;
                item.PaypalPaymentState = PayPalStatus;
                item.PP_PaymentId = PP_PaymentId;
                item.PP_Response = PP_Response;
                controller.GiftCertUpdateWithPayPalReponse(item);
                //if (cbxEmailPurchaser.Checked)
                //{
                //  EmailPurchaser();
                //}
                //Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "List", "mid=" + ModuleId.ToString()), true);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            try
            {
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = new GiftCertificateInfo();
                
                item.ModuleId = this.ModuleId;
                item.ItemId = Convert.ToInt32(Request.QueryString["itemId"].ToString());// itemId;
                item.MailTo = txtMailToName.Text.ToString();
                item.MailToAddress = txtToAddress.Text.ToString();
                item.MailToAddress1 = txtToAddress1.Text.ToString();
                item.MailToCity = txtToCity.Text.ToString();
                item.MailToState = ddlStatesRecipient.SelectedValue.ToString();
                item.MailToZip = txtToZip.Text.ToString();

                controller.GiftCertUpdateShipAddress(item);


                lblResults.Text = "<b>Shipping Address Updated";
                lblResults.Text += "</b><br><br>The gift certificate will be mailed to: <br><br>" + txtMailToName.Text.ToString();
                lblResults.Text += "<br>" + txtToAddress.Text.ToString();
                if (txtToAddress1.Text.ToString().Length > 1)
                {
                    lblResults.Text += "<br>" + txtToAddress1.Text.ToString();
                }
                lblResults.Text += "<br>" + txtToCity.Text.ToString();
                lblResults.Text += ", " + ddlStatesRecipient.SelectedValue.ToString();
                lblResults.Text += " " + txtToZip.Text.ToString();

                SendGiftCertEmailAddressUpdated(Int32.Parse(Request.QueryString["itemId"].ToString()));

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void SendGiftCertEmailAddressUpdated(int _itemID)
        {

            try
            {

                //load the item
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = controller.GetGiftCert(_itemID);
                if (item != null)
                {

                    GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

                    string RedirectURL = "http://" + Request.Url.Host + "/Success/TabID/" + settingsData.RedirectPage + "/Default.aspx";

                    // BUILD E-MAIL BODY

                    string EmailContent = "";

                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("lblCertAmount.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + String.Format("{0:c}", item.CertAmount) + "<br />&nbsp;</dd>";

    
                    EmailContent += "<dt><strong>" + Localization.GetString("lblToName.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.ToName.ToString() + "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("lblFromName.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.FromName.ToString();
                    EmailContent += "<br>" + item.FromPhone.ToString();
                    EmailContent += "<br>" + item.FromEmail.ToString();
                    EmailContent += "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";


                    EmailContent += "<dl>";

                    EmailContent += "<dt><strong>" + Localization.GetString("lblMailTo.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.MailTo.ToString();

                    EmailContent += "<br>" + item.MailToAddress.ToString();
                    if (item.MailToAddress1.ToString().Length > 1)
                    {
                        EmailContent += "<br>" + item.MailToAddress1.ToString();
                    }
                    EmailContent += "<br>" + item.MailToCity.ToString() + ", " + item.MailToState.ToString() + " " + item.MailToZip.ToString();

                    EmailContent += "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

      
                    EmailContent += "<dl>";
                    EmailContent += "<dt><strong>" + Localization.GetString("emailNotes.Text", LocalResourceFile) + "</strong></dt>";
                    EmailContent += "<dd>" + item.Notes.ToString() + "<br />&nbsp;</dd>";
                    EmailContent += "</dl>";

                    EmailContent += "<p>Page Submitted: " + Globals.NavigateURL() + "</p>";

                    //EMAIL THE PURCHASER
                    string EmailFrom = "";
                    if (settingsData.EmailFrom.Length > 1)
                    {
                        EmailFrom = settingsData.EmailFrom.ToString();
                    }
                    else
                    {
                        EmailFrom = PortalSettings.Email.ToString();
                    }
                    string SMTPUserName = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPUsername");

                   // string[] emailAttachemnts1 = null;
                    string[] emailAttachemnts1 = new string[0];
                    DotNetNuke.Services.Mail.Mail.SendMail(SMTPUserName.ToString(), item.FromEmail.ToString().Trim(), "", "", 
                        EmailFrom.ToString(), DotNetNuke.Services.Mail.MailPriority.Normal, 
                        "Address Correction - " + settingsData.EmailSubject.ToString(), DotNetNuke.Services.Mail.MailFormat.Html, 
                        System.Text.ASCIIEncoding.ASCII, EmailContent.ToString(), emailAttachemnts1, string.Empty, string.Empty, string.Empty, string.Empty, true);


               //     DotNetNuke.Services.Mail.Mail.SendMail(EmailFrom.ToString(), item.FromEmail.ToString(), "", "Address Correction - " + settingsData.EmailSubject, EmailContent.ToString(), "", "HTML", "", "", "", "");


                    // EMAIL THE settingsData.EmailNotify
                    // ADD NOTE for ADMINS . . . . 
                    EmailContent += "<p><b>Administrators must log in to process gift certificate.</b></p>";

                    if (settingsData.EmailNotify.ToString().Length > 1)
                    {
                        string FromPurchaserEmail = item.FromEmail.ToString();
                        string emailAddress = settingsData.EmailNotify.Replace(" ", "");
                        string[] valuePair = emailAddress.Split(new char[] { ';' });

                        //string SMTPUserName = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPUsername");
 

                        for (int i = 0; i <= valuePair.Length - 1; i++)
                        {

                          //  DotNetNuke.Services.Mail.Mail.SendMail(FromPurchaserEmail, valuePair[i].ToString().Trim(), "", "Address Correction - " + settingsData.EmailSubject, EmailContent.ToString(), "", "HTML", "", "", "", "");
                            DotNetNuke.Services.Mail.Mail.SendMail(SMTPUserName.ToString(), 
                                valuePair[i].ToString().Trim(), "", "", 
                                FromPurchaserEmail.ToString(), DotNetNuke.Services.Mail.MailPriority.Normal, 
                                "Address Correction - " + settingsData.EmailSubject, DotNetNuke.Services.Mail.MailFormat.Html, 
                                System.Text.Encoding.Default, EmailContent.ToString(), emailAttachemnts1, "", "", "", "", true);
                        }

                    }




                }
                else
                {
                    Response.Redirect(Globals.NavigateURL(), true);
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


    }
}
