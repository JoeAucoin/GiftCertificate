using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;

namespace GIBS.Modules.GiftCertificate.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class GiftCertificateSettings
    {
        ModuleController controller;
        int tabModuleId;

        public GiftCertificateSettings(int tabModuleId)
        {
            controller = new ModuleController();
            this.tabModuleId = tabModuleId;
        }

        protected T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = controller.GetTabModuleSettings(this.tabModuleId);

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        protected void WriteSetting(string settingName, string value)
        {
            controller.UpdateTabModuleSetting(this.tabModuleId, settingName, value);
        }

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        /// 

        public string PdfFilesFolder
        {
            get { return ReadSetting<string>("pdfFilesFolder", null); }
            set { WriteSetting("pdfFilesFolder", value); }
        }

        //txtPayPalPayee
        public string PayPalPayee
        {
            get { return ReadSetting<string>("payPalPayee", null); }
            set { WriteSetting("payPalPayee", value); }
        }

        //PayPalMode
        public string PayPalSandboxMode
        {
            get { return ReadSetting<string>("payPalSandboxMode", null); }
            set { WriteSetting("payPalSandboxMode", value); }
        }

        public string NumPerPage
        {
            get { return ReadSetting<string>("numPerPage", null); }
            set { WriteSetting("numPerPage", value); }
        }


        public string RedirectPage
        {
            get { return ReadSetting<string>("redirectPage", null); }
            set { WriteSetting("redirectPage", value); }
        }
        public string EmailFrom
        {
            get { return ReadSetting<string>("emailFrom", null); }
            set { WriteSetting("emailFrom", value); }
        }
        public string EmailNotify
        {
            get { return ReadSetting<string>("emailNotify", null); }
            set { WriteSetting("emailNotify", value); }
        }
        public string EmailSubject
        {
            get { return ReadSetting<string>("emailSubject", null); }
            set { WriteSetting("emailSubject", value); }
        }

        public string ModuleInstructions
        {
            get { return ReadSetting<string>("moduleInstructions", null); }
            set { WriteSetting("moduleInstructions", value); }
        }


        public string CertBannerText
        {
            get { return ReadSetting<string>("certBannerText", null); }
            set { WriteSetting("certBannerText", value); }
        }


        public string CertFooterText
        {
            get { return ReadSetting<string>("certFooterText", null); }
            set { WriteSetting("certFooterText", value); }
        }

        public string CertWatermark
        {
            get { return ReadSetting<string>("certWatermark", null); }
            set { WriteSetting("certWatermark", value); }
        }

        public string CertLogo
        {
            get { return ReadSetting<string>("certLogo", null); }
            set { WriteSetting("certLogo", value); }
        }

        public string SpecialInstructions
        {
            get { return ReadSetting<string>("specialInstructions", null); }
            set { WriteSetting("specialInstructions", value); }
        }

        public string AddUserRole
        {
            get { return ReadSetting<string>("addUserRole", null); }
            set { WriteSetting("addUserRole", value); }
        }


        public string CertReturnAddress
        {
            get { return ReadSetting<string>("certReturnAddress", null); }
            set { WriteSetting("certReturnAddress", value); }
        }

        public string ManageUserRole
        {
            get { return ReadSetting<string>("manageUserRole", null); }
            set { WriteSetting("manageUserRole", value); }
        }


        #endregion
    }
}
