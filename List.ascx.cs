using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using GIBS.Modules.GiftCertificate.Components;
using System.Text;

namespace GIBS.Modules.GiftCertificate
{
    public partial class List : PortalModuleBase, IActionable
    {

        public bool _isAuthorized = false;
        int _CurrentPage = 1;
        int PageSize = 10;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            if (_isAuthorized)
            {
                this.ModuleConfiguration.ModuleTitle = "Manage Gift Certificates";
            }
            else
            {
                Response.Redirect(Globals.NavigateURL("Access Denied"), true);
            }

            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Today.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Today.AddDays(1).ToShortDateString();
                
                FillGrid();
            }
        //    FillGrid();
        }


        protected void btnUpdateReport_Click(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }



        public void FillGrid()
        {

            try
            {

                if (Request.QueryString["currentpage"] != null)
                {
                    _CurrentPage = Convert.ToInt32(Request.QueryString["currentpage"].ToString());
                }
                else
                {
                    _CurrentPage = 1;
                }
                
                List<GiftCertificateInfo> items;
                GiftCertificateController controller = new GiftCertificateController();

                items = controller.GetGiftCerts(this.ModuleId, DateTime.Parse(txtStartDate.Text.ToString()), DateTime.Parse(txtEndDate.Text.ToString()));


                PagedDataSource objPagedDataSource = new PagedDataSource();
                objPagedDataSource.DataSource = items;

                if (objPagedDataSource.PageCount > 0)
                {
                    objPagedDataSource.PageSize = PageSize;
                    objPagedDataSource.CurrentPageIndex = _CurrentPage - 1;
                    objPagedDataSource.AllowPaging = true;
                }


                //bind the data
                GridView1.DataSource = objPagedDataSource;
                GridView1.DataBind();

  
                if (PageSize == 0 || items.Count <= PageSize)
                {
                    PagingControl1.Visible = false;
                }
                else
                {
                    PagingControl1.Visible = true;
                    PagingControl1.TotalRecords = items.Count;
                    PagingControl1.PageSize = PageSize;
                    PagingControl1.CurrentPage = _CurrentPage;
                    PagingControl1.TabID = TabId;
                    PagingControl1.QuerystringParams = "ctl=List&mid=" + this.ModuleId; 

                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected static string GenerateQueryStringParameters(HttpRequest request, params string[] queryStringKeys)
        {
            StringBuilder queryString = new StringBuilder(64);
            foreach (string key in queryStringKeys)
            {
                if (request.QueryString[key] != null)
                {
                    if (queryString.Length > 0)
                    {
                        queryString.Append("&");
                    }

                    queryString.Append(key).Append("=").Append(request.QueryString[key]);
                }
            }

            return queryString.ToString();
        }

        public void LoadSettings()
        {

            GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

            if (settingsData.NumPerPage != null)
            {
                PageSize = Int32.Parse(settingsData.NumPerPage.ToString());
            }

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

       // GridView1_RowEditing
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "mid=" + ModuleId.ToString() + "&ItemId=" + itemID));

        }
        


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;

            GiftCertificateController controller = new GiftCertificateController();

            controller.DeleteGiftCert(this.ModuleId, itemID);
            FillGrid();

        }
         
        
        // PRINT VIEW   
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;
            Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "MakeCertificate", "mid=" + ModuleId.ToString() + "&ItemId=" + itemID));


            //int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;
            //Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "SkinSrc=[G]" + Globals.QueryStringEncode(SkinController.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "popUpSkin") + "&ContainerSrc=" + Globals.QueryStringEncode("/portals/_default/containers/_default/no%20container") + "&mid=" + ModuleId.ToString() + "&ItemId=" + itemID + "&dnnprintmode=true"), true);

            // MakeCertificate
           // string MyURL =  Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "SkinSrc=[G]" + Globals.QueryStringEncode(SkinController.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin") + "&mid=" + ModuleId.ToString() + "&ItemId=" + itemID);

        }


        public static void OpenPopUp(System.Web.UI.WebControls.WebControl opener, string PagePath)
        {
            string clientScript = null;

            //Building the client script- window.open
            clientScript = "window.open('" + PagePath + "')";
            //register the script to the clientside click event of the opener control
            opener.Attributes.Add("onClick", clientScript);
        }




        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();

                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                //    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                //     true, false);
                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.EditContent, this.LocalResourceFile),
                // ModuleActionType.EditContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                // true, false);

                actions.Add(GetNextActionID(), DotNetNuke.Services.Localization.Localization.GetString("ReturnText", this.LocalResourceFile),
                 ModuleActionType.AddContent, "", "", Globals.NavigateURL(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                 true, false);

                return actions;
            }
        }

        #endregion

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
               


            
            
            if(RadioButtonList1.SelectedValue == "All" )
            {
           
            }

            else
            {
            
            }


        }


        ////Calculate Sum and display in Footer Row
        ///
        decimal sumFooterValue = 0;

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string CertAmount = ((Label)e.Row.FindControl("Label7")).Text;
                
                decimal totalvalue =  Convert.ToDecimal(CertAmount.Replace("$",""));
               // e.Row.Cells[6].Text = totalvalue.ToString();
                sumFooterValue += totalvalue;
        }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblTotal");
                lbl.Text = String.Format("{0:C}", sumFooterValue);
            }


        }
    }
}