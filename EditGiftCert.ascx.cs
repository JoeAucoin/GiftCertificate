using System;
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Common.Lists;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
//using PayPal;



namespace GIBS.Modules.GiftCertificate
{
    public partial class EditGiftCert : PortalModuleBase, IActionable
    {

        int itemId = Null.NullInteger;
        public bool _isAuthorized = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                LoadSettings();
                if (_isAuthorized)
                {

                }
                else
                {
                    Response.Redirect(Globals.NavigateURL("Access Denied"), true);
                }

                if (Request.QueryString["ItemId"] != null)
                {
                    itemId = Int32.Parse(Request.QueryString["ItemId"]);
                }

                if (!IsPostBack)
                {
                    //load the data into the control the first time
                    //we hit this page
                    GetDropDownLists();

                    LoadModuleTitle();




                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                    //check we have an item to lookup
                    if (!Null.IsNull(itemId))
                    {
                        //load the item
                        GiftCertificateController controller = new GiftCertificateController();
                        GiftCertificateInfo item = controller.GetGiftCert(itemId);

                        if (item != null)
                        {
                            //  Response.Write(item);

                            //lblMailToRec.Text = item.MailTo;
                            txtCertAmount.Text = String.Format("{0:f2}", item.CertAmount);


                            isProcessed.SelectedIndex = Convert.ToInt32(item.IsProcessed);


                            txtFromEmail.Text = item.FromEmail;
                            txtFromName.Text = item.FromName;
                            txtFromPhone.Text = item.FromPhone;
                            ddlStatesRecipient.SelectedValue = item.MailToState;
                            txtMailToName.Text = item.MailTo;
                            txtToAddress.Text = item.MailToAddress;
                            txtToAddress1.Text = item.MailToAddress1;
                            txtToCity.Text = item.MailToCity;
                            txtToZip.Text = item.MailToZip;
                            txtRecipientName.Text = item.ToName;

                            txtSpecialInstructions.Text = item.Notes;

                            txtPaypalPaymentStatee.Text = item.PaypalPaymentState.ToUpper();
                            if (txtPaypalPaymentStatee.Text.ToString() == "APPROVED")
                            {
                                lblOrderStatus.Text = "PAYMENT " + txtPaypalPaymentStatee.Text.ToString();
                            }

                            else
                            {
                                lblOrderStatus.Text = "<span style='color:Red;'>NO RECORD OF PAYMENT</span>";
                            }


                            txtPP_PaymentId.Text = item.PP_PaymentId;
                            txtPP_Response.Text = item.PP_Response;

                            string MailAddress = "";
                            string line2Address = "";
                            if (item.MailToAddress1.ToString().Length > 1)
                            {
                                line2Address = Environment.NewLine + item.MailToAddress1.ToString();
                            }
                            MailAddress = item.MailTo + Environment.NewLine + item.MailToAddress  
                                + line2Address.ToString() 
                                + Environment.NewLine + item.MailToCity + ", " + item.MailToState + " " + item.MailToZip;

                            txtMailingAddresses.Text = MailAddress.ToString();

                        }
                        else
                            Response.Redirect(Globals.NavigateURL(), true);
                    }
                    else
                    {
                        cmdDelete.Visible = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void LoadSettings()
        {

            GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);



            if (settingsData.ManageUserRole != null)
            {
                if (this.UserInfo.IsInRole(settingsData.ManageUserRole.ToString()))
                {
                    _isAuthorized = true;

                }

            }

            if (this.UserInfo.IsInRole("Administrators"))
            {

                _isAuthorized = true;

            }

        }


        public void LoadModuleTitle()
        {

            //// LoadModuleTitle()
            // Control ctl = Globals.FindControlRecursiveDown(this.ContainerControl, "lblTitle");
            // if ((ctl != null))
            // {

            //     lblModuleTitle.Text = ((Label)ctl).Text;
            // }


        }

        public void EmailPurchaser()
        {

            try
            {
                GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);
                // BUILD E-MAIL BODY

                // BUILD E-MAIL BODY

                string EmailContent = "";

                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblCertAmount.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + String.Format("{0:c}", txtCertAmount.Text.ToString()) + "<br />&nbsp;</dd>";

                //       EmailContent += "<br clear=all>";

                EmailContent += "<dt><strong>" + Localization.GetString("lblToName.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtRecipientName.Text.ToString() + "<br />&nbsp;</dd>";
                EmailContent += "</dl>";

                //     EmailContent += "<br clear=all>";

                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("lblFromName.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtFromName.Text.ToString();
                EmailContent += "<br>" + txtFromPhone.Text.ToString();
                EmailContent += "<br>" + txtFromEmail.Text.ToString();
                EmailContent += "<br />&nbsp;</dd>";
                EmailContent += "</dl>";



                //      EmailContent += "<br clear=all>";


                EmailContent += "<dl>";

                EmailContent += "<dt><strong>" + Localization.GetString("lblMailTo.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtMailToName.Text.ToString();

                EmailContent += "<br>" + txtToAddress.Text.ToString();
                if (txtToAddress1.Text.ToString().Length > 1)
                {
                    EmailContent += "<br>" + txtToAddress1.Text.ToString();
                }
                EmailContent += "<br>" + txtToCity.Text.ToString() + ", " + ddlStatesRecipient.SelectedValue.ToString() + " " + txtToZip.Text.ToString();

                EmailContent += "<br />&nbsp;</dd>";
                EmailContent += "</dl>";

                //       EmailContent += "<br clear=all>";

                EmailContent += "<dl>";
                EmailContent += "<dt><strong>" + Localization.GetString("emailNotes.Text", LocalResourceFile) + "</strong></dt>";
                EmailContent += "<dd>" + txtSpecialInstructions.Text.ToString() + "<br />&nbsp;</dd>";
                EmailContent += "</dl>";


                EmailContent += "<p>" + PortalSettings.PortalName.ToString() + "</p>";

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
                
                DotNetNuke.Services.Mail.Mail.SendMail(EmailFrom.ToString(), txtFromEmail.Text.ToString(), "", "Your Gift Certificate Has Been Processed", EmailContent.ToString(), "", "HTML", "", "", "", "");

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }

        public void GetDropDownLists()
        {

            try
            {
                var regions = new ListController().GetListEntryInfoItems("Region", "Country.US", this.PortalId);

                //// Recipient State
                ddlStatesRecipient.DataTextField = "Value";
                ddlStatesRecipient.DataValueField = "Value";
                ddlStatesRecipient.DataSource = regions;
                ddlStatesRecipient.DataBind();
                ddlStatesRecipient.Items.Insert(0, new ListItem("-Select-", "-1"));
                ddlStatesRecipient.SelectedValue = "MA";

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                GiftCertificateController controller = new GiftCertificateController();
                GiftCertificateInfo item = new GiftCertificateInfo();

                item.IsProcessed = Convert.ToBoolean(isProcessed.SelectedValue.ToString());
                item.ItemId = itemId;
                item.ModuleId = this.ModuleId;
                item.UpdatedByUserID = this.UserId;
                item.ToName = txtRecipientName.Text.ToString();
                item.CertAmount = Convert.ToDouble(txtCertAmount.Text.ToString());
                item.MailTo = txtMailToName.Text.ToString();
                item.MailToAddress = txtToAddress.Text.ToString();
                item.MailToAddress1 = txtToAddress1.Text.ToString();
                item.MailToCity = txtToCity.Text.ToString();
                item.MailToState = ddlStatesRecipient.SelectedValue.ToString();
                item.MailToZip = txtToZip.Text.ToString();

                item.MailTo = txtMailToName.Text.ToString();
                item.FromName = txtFromName.Text.ToString();

                item.FromPhone = txtFromPhone.Text.ToString();
                item.FromEmail = txtFromEmail.Text.ToString();


                item.Notes = txtSpecialInstructions.Text.ToString();

                controller.GiftCertUpdateGiftCert(item);
                //.UpdateGiftCert(item);
                // Response.Redirect("MakePayment.aspx", true);

                if (cbxEmailPurchaser.Checked)
                {
                    EmailPurchaser();
                }
                //Response.Redirect(Globals.NavigateURL(), true);
                Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "List", "mid=" + ModuleId.ToString()), true);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "List", "mid=" + ModuleId.ToString()), true);
                //               Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Null.IsNull(itemId))
                {
                    GiftCertificateController controller = new GiftCertificateController();
                    controller.DeleteGiftCert(this.ModuleId, itemId);
                    Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "List", "mid=" + ModuleId.ToString()), true);
                    // Response.Redirect(Globals.NavigateURL(), true);
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


                actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.EditContent, this.LocalResourceFile),
 ModuleActionType.EditContent, "", "", EditUrl("List"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
 true, false);

                actions.Add(GetNextActionID(), DotNetNuke.Services.Localization.Localization.GetString("ReturnText", this.LocalResourceFile),
 ModuleActionType.AddContent, "", "", Globals.NavigateURL(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
 true, false);

                return actions;
            }
        }

        #endregion


    }
}