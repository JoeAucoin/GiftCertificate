using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Entities.Tabs;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Services.FileSystem;

namespace GIBS.Modules.GiftCertificate
{
    public partial class Settings : ModuleSettingsBase
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
               
                
                if (!IsPostBack)
                {

                    GetRoles();


                    this.ddlPageList.DataSource = TabController.GetPortalTabs(this.PortalId, -1, true, "None Specified", true, false, true, true, true);

                    TabController.GetPortalTabs(this.PortalId, 1, false, true);

                    this.ddlPageList.DataBind();



                    GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

                    if (settingsData.PdfFilesFolder != null)
                    {
                        ddlPdfFilesFolder.SelectedValue = settingsData.PdfFilesFolder;
                    }

                    if (settingsData.NumPerPage != null)
                    {
                        ddlNumPerPage.SelectedValue = settingsData.NumPerPage;
                    }

                    if (settingsData.RedirectPage != null)
                    {
                        ddlPageList.SelectedValue = settingsData.RedirectPage;
                    }
                    if (settingsData.EmailFrom != null)
                    {
                        txtEmailFrom.Text = settingsData.EmailFrom;
                    }

                    if (settingsData.EmailNotify != null)
                    {
                        txtEmailNotify.Text = settingsData.EmailNotify;
                    }
                    if (settingsData.EmailSubject != null)
                    {
                        txtEmailSubject.Text = settingsData.EmailSubject;
                    }
                    if (settingsData.ModuleInstructions != null)
                    {
                        txtModuleInstructions.Text = settingsData.ModuleInstructions;
                    }

                    if (settingsData.CertBannerText != null)
                    {
                        txtCertBannerText.Text = settingsData.CertBannerText;
                    }

                    if (settingsData.CertFooterText != null)
                    {
                        txtCertFooterText.Text = settingsData.CertFooterText;
                    }
                    if (settingsData.CertWatermark != null)
                    {
                        txtCertWatermark.Text = settingsData.CertWatermark;   
                    }

                    
                    if (settingsData.CertLogo != null)
                    {
                        txtCertLogo.Text = settingsData.CertLogo;
                    }

                    if (settingsData.SpecialInstructions != null)
                    {
                        txtSpecialInstructions.Text = settingsData.SpecialInstructions;
                    }

                    if (settingsData.CertReturnAddress != null)
                    {
                       txtCertReturnAddress.Text = settingsData.CertReturnAddress;
                    }

                    if (settingsData.AddUserRole != null)
                    {
                        ddlRoles.SelectedValue = settingsData.AddUserRole;
                    }

                    if (settingsData.ManageUserRole != null)
                    {
                        ddlManageUserRole.SelectedValue = settingsData.ManageUserRole;
                    }

                    if (settingsData.PayPalPayee != null)
                    {
                        txtPayPalPayee.Text = settingsData.PayPalPayee;
                    }

                    if (settingsData.PayPalSandboxMode != null)
                    {
                        rblPayPalSandboxMode.SelectedValue = settingsData.PayPalSandboxMode;
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void GetRoles()
        {
            ArrayList myRoles = new ArrayList();

            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();

            myRoles = rc.GetPortalRoles(this.PortalId);
         //   myRoles = rc.GetRoles(this.PortalId,);

            ddlRoles.DataSource = myRoles;
            ddlRoles.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem item = new ListItem();
            item.Text = "-- Select Role to Assign --";
            item.Value = "";
            ddlRoles.Items.Insert(0, item);
            // REMOVE DEFAULT ROLES
            ddlRoles.Items.Remove("Administrators");
            ddlRoles.Items.Remove("Registered Users");
            ddlRoles.Items.Remove("Subscribers");


            ddlManageUserRole.DataSource = myRoles;
            ddlManageUserRole.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem item1 = new ListItem();
            item1.Text = "-- Select Role to Assign --";
            item1.Value = "";
            ddlManageUserRole.Items.Insert(0, item);
            // REMOVE DEFAULT ROLES
            ddlManageUserRole.Items.Remove("Administrators");
            ddlManageUserRole.Items.Remove("Registered Users");
            ddlManageUserRole.Items.Remove("Subscribers");

            // GET FOLDERS
            //var folderInfos = FolderManager.Instance.GetFolders(folder.PortalID).Where(f => f.FolderPath != string.Empty && f.FolderPath.StartsWith(folder.FolderPath)).ToList();
            var folderInfos = FolderManager.Instance.GetFolders(this.PortalId);
            //ddlPdfFilesFolder
            //asp:DropDownList ID="ddlPdfFilesFolder" runat="server" DataTextField="FolderPath" DataValueField="FolderPath" />

            ddlPdfFilesFolder.DataSource = folderInfos;
            ddlPdfFilesFolder.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem itemFolder = new ListItem();
            itemFolder.Text = "-- Select Folder --";
            itemFolder.Value = "";
            ddlPdfFilesFolder.Items.Insert(0, itemFolder);

        }


        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

                settingsData.NumPerPage = ddlNumPerPage.SelectedValue;
                settingsData.RedirectPage = ddlPageList.SelectedValue.ToString();
                settingsData.EmailFrom = txtEmailFrom.Text;
                settingsData.EmailNotify = txtEmailNotify.Text;
                settingsData.EmailSubject = txtEmailSubject.Text;
                settingsData.ModuleInstructions = txtModuleInstructions.Text;
                settingsData.CertBannerText = txtCertBannerText.Text;
                settingsData.AddUserRole = ddlRoles.SelectedValue.ToString();
                settingsData.CertFooterText = txtCertFooterText.Text;
                settingsData.SpecialInstructions = txtSpecialInstructions.Text;
                settingsData.CertReturnAddress = txtCertReturnAddress.Text;
                settingsData.ManageUserRole = ddlManageUserRole.SelectedValue.ToString();
                settingsData.PayPalPayee = txtPayPalPayee.Text;
                settingsData.PayPalSandboxMode = rblPayPalSandboxMode.SelectedValue.ToString();
                settingsData.CertWatermark = txtCertWatermark.Text.ToString();
                settingsData.CertLogo = txtCertLogo.Text.ToString();
                settingsData.PdfFilesFolder = ddlPdfFilesFolder.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}