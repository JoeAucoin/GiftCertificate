using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke;
using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace GIBS.Modules.GiftCertificate
{
    public partial class ViewGiftCertificate : PortalModuleBase, IActionable
    {

        public string FirstName;
        public string LastName;
        int itemId = Null.NullInteger;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "test", "<script language='javascript' type='text/javascript'>EnableValidator();</script>", false);
                    Page.Validate();


                    GetDropDownLists();
                    LoadSettings();


                    if (Request.QueryString["ItemId"] != null)
                    {
                        itemId = Int32.Parse(Request.QueryString["ItemId"]);
                        GetGiftCertRecord(itemId);

                    }
                    
                }
                else
                {
                    if (Request.QueryString["ItemId"] != null)
                    {
                        itemId = Int32.Parse(Request.QueryString["ItemId"]);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
         //   JavaScript.RequestRegistration(CommonJs.DnnPlugins);
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
                    GiftCertificateInfo item = controller.GetGiftCert(itemId);
                    if (item != null)
                    {
                        
                        txtCertAmount.Text = String.Format("{0:f2}", item.CertAmount);
                        
                        txtToName.Text = item.ToName;
                        txtFromName.Text = item.FromName;   
                        txtFromEmail.Text = item.FromEmail;
                        txtFromPhone.Text = item.FromPhone;
                        txtMailToName.Text = item.MailTo;
                        txtToAddress.Text = item.MailToAddress;
                        txtToAddress1.Text = item.MailToAddress1;
                        txtToCity.Text = item.MailToCity;
                        ddlStatesRecipient.SelectedValue = item.MailToState;
                        txtToZip.Text = item.MailToZip;
                        txtNotes.Text = item.Notes;

                        Page.Validate();

                        if (item.PaypalPaymentState.ToString().Length > 0)
                        {
                            Response.Redirect(Globals.NavigateURL("Access Denied"), true);
                        }
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



        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                     true, false);

                return actions;
            }
        }

        #endregion

        public void LoadSettings()

        {

            GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

            //if (settingsData.FromInstructions != null)
            //{
            //    lblFromInstructions.Text = settingsData.FromInstructions;
            //}
            //if (settingsData.SpecialInstructions != null)
            //{
            //    lblSpecialInstructions.Text = settingsData.SpecialInstructions;
            //}

            if (settingsData.ModuleInstructions != null)
            {
                lblModuleInstructions.Text = settingsData.ModuleInstructions;
            }

            //if (settingsData.RecipientInstructions != null )
            //{
            //    lblRecipientInstructions.Text = settingsData.RecipientInstructions;
            //}

            //if (settingsData.CreditCardInstructions != null)
            //{
            //    lblCreditCardInstructions.Text = settingsData.CreditCardInstructions;
            //}

            if (settingsData.ManageUserRole != null)
            {
                if (this.UserInfo.IsInRole(settingsData.ManageUserRole.ToString()))
                {
                    HyperLinkManageCertificates.Visible = true;
                    HyperLinkManageCertificates.NavigateUrl = EditUrl("List");
                }
                
            }

            if (this.UserInfo.IsInRole("Administrators"))
            {

                HyperLinkManageCertificates.Visible = true;
                HyperLinkManageCertificates.NavigateUrl = EditUrl("List");

            }
        
        }


        public void GetDropDownLists()
        {

            try
            {
                // MailTo State
                var regions = new ListController().GetListEntryInfoItems("Region", "Country.US", this.PortalId);
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

        //protected void btnSave_Click(object sender, EventArgs e)
        //{

        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                int MyNewID = Null.NullInteger;

                MyNewID = AddNewGiftCert();


                Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "PayPal", "mid=" + ModuleId.ToString() + "&ItemId=" + MyNewID.ToString()));
          

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public int AddNewGiftCert()
        {

            try
            {
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = new GiftCertificateInfo();

                

                item.CertAmount = Convert.ToDouble(txtCertAmount.Text.ToString().Replace("$", "").ToString());
                item.ToName = txtToName.Text.ToString();
                // item.MailToAddress
                item.MailToAddress = txtToAddress.Text.ToString();
                item.MailToAddress1 = txtToAddress1.Text.ToString();
                item.MailToCity = txtToCity.Text.ToString();
                item.MailToState = ddlStatesRecipient.SelectedValue.ToString();
                item.MailToZip = txtToZip.Text.ToString();

                item.MailTo = txtMailToName.Text.ToString();

                item.FromUserID = this.UserId;
                item.FromName = txtFromName.Text.ToString();

                item.FromPhone = txtFromPhone.Text.ToString();
                item.FromEmail = txtFromEmail.Text.ToString();

                item.Notes = txtNotes.Text.ToString();

                item.ModuleId = this.ModuleId;
                item.CreatedByUserID = this.UserId;
                item.UpdatedByUserID = this.UserId;

                // ADD THE RECORD
                int MyNewID = Null.NullInteger;

//                if (!Null.IsNull(itemId))
                if (itemId > 0)
                {
                    MyNewID = itemId;
                    item.ItemId = itemId;
                    item.IsProcessed = false;
                //    item.UpdatedByUserID = this.UserId;

                    controller.GiftCertUpdateGiftCert(item);
                }

                else
                {
                    MyNewID = controller.GiftCertAddGiftCert(item);
                }


               

            //    string myNewGiftCertID = Convert.ToString(MyNewID);

                return MyNewID;



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return 0;
            }

        }


        public void SendGiftCertEmail(string _purchaserEmailAddress)
        {

            try
            {
                GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

                string RedirectURL = "http://" + Request.Url.Host + "/Success/TabID/" + settingsData.RedirectPage + "/Default.aspx";

                // BUILD E-MAIL BODY

                string EmailContent = "";

                //EmailContent += "<p>" + Localization.GetString("lblCertAmount.Text", LocalResourceFile) + ": " + txtCertAmount.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblMailTo.Text", LocalResourceFile) + " " + item.MailTo + "</p>";


                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblCertAmount.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + String.Format("{0:c}", Convert.ToDouble(txtCertAmount.Text.ToString().Replace("$", "").ToString())) + "</dd>";
                //EmailContent += "</dl>";

                //EmailContent += "<br clear=all>";

                //EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblMailTo.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + _purchaserEmailAddress.ToString() + "</dd>";
                EmailContent += "</dl>";

                EmailContent += "<br clear=all>";

                //EmailContent += "<p>" + Localization.GetString("lblFromName.Text", LocalResourceFile) + ": " + txtFromName.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblFromAddress.Text", LocalResourceFile) + ": " + txtFromAddress.Text.ToString() + "  -  " + txtFromAddress1.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblFromCityStateZip.Text", LocalResourceFile) + ": " + txtFromCity.Text.ToString() + ", " + ddlStatesFrom.SelectedValue.ToString() + " " + txtFromZip.Text.ToString() + "</p>";


                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblFromName.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtFromName.Text.ToString();
                //EmailContent += "<br>" + txtFromAddress.Text.ToString();
                //if (txtFromAddress1.Text.ToString().Length > 1)
                //{
                //    EmailContent += "<br>" + txtFromAddress1.Text.ToString();
                //}
                //EmailContent += "<br>" + txtFromCity.Text.ToString() + ", " + ddlStatesFrom.SelectedValue.ToString() + " " + txtFromZip.Text.ToString();
                EmailContent += "</dd>";
                EmailContent += "</dl>";


                //EmailContent += "<p>" + Localization.GetString("lblFromPhone.Text", LocalResourceFile) + ": " + txtFromPhone.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblFromEmail.Text", LocalResourceFile) + ": " + txtFromEmail.Text.ToString() + "</p>";


                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblFromPhone.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtFromPhone.Text.ToString() + "</dd>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblFromEmail.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtFromEmail.Text.ToString() + "</dd>";
                EmailContent += "</dl>";

                EmailContent += "<br clear=all>";

                //EmailContent += "<p>" + Localization.GetString("lblToName.Text", LocalResourceFile) + ": " + txtToName.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblToAddress.Text", LocalResourceFile) + ": " + txtToAddress.Text.ToString() + "  -  " +  txtToAddress1.Text.ToString() + "<br>";
                //EmailContent += Localization.GetString("lblToCityStateZip.Text", LocalResourceFile) + ": " + txtToCity.Text.ToString() + ", " +  ddlStatesRecipient.SelectedValue.ToString() + " " +  txtToZip.Text.ToString() + "</p>";


                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblToName.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtToName.Text.ToString();
                if (rdoRecipient.Checked == true)
                {
                    EmailContent += "<br>" + txtToAddress.Text.ToString();
                    if (txtToAddress1.Text.ToString().Length > 1)
                    {
                        EmailContent += "<br>" + txtToAddress1.Text.ToString();
                    }
                    EmailContent += "<br>" + txtToCity.Text.ToString() + ", " + ddlStatesRecipient.SelectedValue.ToString() + " " + txtToZip.Text.ToString();
                }
                EmailContent += "</dd>";
                EmailContent += "</dl>";

                EmailContent += "<br clear=all>";

                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("emailNotes.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtNotes.Text.ToString() + "</dd>";
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
                //  DotNetNuke.Services.Mail.Mail.SendEmail(EmailFrom.ToString(), txtFromEmail.Text.ToString(), settingsData.EmailSubject, EmailContent.ToString());
                DotNetNuke.Services.Mail.Mail.SendMail(EmailFrom.ToString(), txtFromEmail.Text.ToString(), "", settingsData.EmailSubject, EmailContent.ToString(), "", "HTML", "", "", "", "");


                // EMAIL THE settingsData.EmailNotify
                // ADD NOTE for ADMINS . . . . 
                EmailContent += "<p><b>Administrators must log in to the site to view payment information.</b></p>";

                if (settingsData.EmailNotify.Length > 1)
                {
                    string FromPurchaserEmail = txtFromEmail.Text.ToString();
                    string emailAddress = settingsData.EmailNotify.Replace(" ", "");
                    string[] valuePair = emailAddress.Split(new char[] { ';' });

                    for (int i = 0; i <= valuePair.Length - 1; i++)
                    {
                        // DotNetNuke.Services.Mail.Mail.SendEmail(FromPurchaserEmail, valuePair[i].ToString().Trim(), settingsData.EmailSubject, EmailContent.ToString());
                        DotNetNuke.Services.Mail.Mail.SendMail(FromPurchaserEmail, valuePair[i].ToString().Trim(), "", settingsData.EmailSubject, EmailContent.ToString(), "", "HTML", "", "", "", "");
                    }

                }

                //if (settingsData.EmailNotify.Length > 0)
                //{
                //    DotNetNuke.Services.Mail.Mail.SendEmail(PortalSettings.Email, settingsData.EmailNotify, settingsData.EmailSubject,EmailContent.ToString());
                //}




                string TempAddUserRole = "";
                if (settingsData.AddUserRole != null)
                {
                    TempAddUserRole = settingsData.AddUserRole;
                }

                //CREATE USER ACCOUNT
                if (settingsData.AddUserRole.Length > 0)
                {
                    CreateNewUser(txtFromName.Text, txtFromEmail.Text, TempAddUserRole);
                }





                if (settingsData.RedirectPage.Length > 0)
                {
                    Response.Redirect(RedirectURL.ToString(), true);
                    Response.Redirect(settingsData.RedirectPage, true);
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


        public void CreateNewUser(string FullName, string Email, string AddUserRole)
        {

            try
            {

                NameSplit(FullName);

                UserInfo oUser = new UserInfo();

                oUser.PortalID = this.PortalId;
                oUser.IsSuperUser = false;
                oUser.FirstName = FirstName;
                oUser.LastName = LastName;
                oUser.Email = Email;
                oUser.Username = Email;
                oUser.DisplayName = FullName;

                //Fill MINIMUM Profile Items (KEY PIECE)
                oUser.Profile.PreferredLocale = PortalSettings.DefaultLanguage;
                oUser.Profile.PreferredTimeZone = PortalSettings.TimeZone;
                oUser.Profile.FirstName = oUser.FirstName;
                oUser.Profile.LastName = oUser.LastName;


                //Set Membership
                UserMembership oNewMembership = new UserMembership(oUser);
                oNewMembership.Approved = true;
                oNewMembership.CreatedDate = System.DateTime.Now;

            //    oNewMembership.Email = oUser.Email;
                oNewMembership.IsOnLine = false;
            //    oNewMembership.Username = oUser.Username;
                oNewMembership.Password = GenerateRandomString(7);

                //Bind membership to user
                oUser.Membership = oNewMembership;

                //Add the user, ensure it was successful 
                if (DotNetNuke.Security.Membership.UserCreateStatus.Success == UserController.CreateUser(ref oUser))
                {
                    //Add Role if passed something from module settings

                    if (AddUserRole.Length > 0)
                    {
                        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
                        //retrieve role
                        string groupName = AddUserRole;
                        DotNetNuke.Security.Roles.RoleInfo ri = rc.GetRoleByName(PortalId, groupName);
                        rc.AddUserRole(this.PortalId, oUser.UserID, ri.RoleID, Null.NullDate);
                    }
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public static string GenerateRandomString(int length)
        {
            //Removed O, o, 0, l, 1 
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[length];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
        }

        public void NameSplit(string name)
        {
            if (name.Length > 0)
            {
                // Check for a comma
                if (name.IndexOf(",") > 0)
                {
                    LastName = name.Substring(0, name.IndexOf(",")).Trim();
                    FirstName = name.Substring(name.IndexOf(",") + 1).Trim();
                }
                else if (name.IndexOf(" ") > 0)
                {
                    FirstName = name.Substring(0, name.IndexOf(" ")).Trim();
                    LastName = name.Substring(name.IndexOf(" ") + 1).Trim();
                }
                else
                {
                    FirstName = "-";
                    LastName = name;
                }
            }
        }
        

    }
}