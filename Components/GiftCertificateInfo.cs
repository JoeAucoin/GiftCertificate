using System;
//using DotNetNuke.Entities.Portals;
//using DotNetNuke.Entities.Users;

namespace GIBS.Modules.GiftCertificate.Components
{
    public class GiftCertificateInfo
    {
        //private vars exposed thro the
        //properties

        private int moduleId;
        private int itemId;
        private double certAmount;
        private string mailTo;
        private string toName;
        private string mailToAddress;
        private string mailToAddress1;
        private string mailToCity;
        private string mailToState;
        private string mailToZip;
        private int fromUserID;
        private string fromName;
        private string fromPhone;
        private string fromEmail;
        private string notes;
        private int createdByUserID;
        private int updatedByUserID;
        private DateTime createdDate;
        private bool isProcessed;
        private string createdByUserName;
        private string updatedByUserName;
        private string pP_PaymentId;
        private string pP_Response;
        private DateTime updateDate;
        private string paypalPaymentState;

       


        /// <summary>
        /// empty cstor
        /// </summary>
        public GiftCertificateInfo()
        {
        }


        #region properties
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public string PaypalPaymentState
        {
            get { return paypalPaymentState; }
            set { paypalPaymentState = value; }
        }
        
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public double CertAmount
        {
            get { return certAmount; }
            set { certAmount = value; }
        }
        public string MailTo
        {
            get { return mailTo; }
            set { mailTo = value; }
        }
        public string ToName
        {
            get { return toName; }
            set { toName = value; }
        }
        public string MailToAddress
        {
            get { return mailToAddress; }
            set { mailToAddress = value; }
        }
        public string MailToAddress1
        {
            get { return mailToAddress1; }
            set { mailToAddress1 = value; }
        }
        public string MailToCity
        {
            get { return mailToCity; }
            set { mailToCity = value; }
        }
        public string MailToState
        {
            get { return mailToState; }
            set { mailToState = value; }
        }
        public string MailToZip
        {
            get { return mailToZip; }
            set { mailToZip = value; }
        }
        public string FromName
        {
            get { return fromName; }
            set { fromName = value; }
        }

        public int FromUserID
        {
            get { return fromUserID; }
            set { fromUserID = value; }
        }

        public string FromPhone
        {
            get { return fromPhone; }
            set { fromPhone = value; }
        }

        public string FromEmail
        {
            get { return fromEmail; }
            set { fromEmail = value; }
        }


        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public int CreatedByUserID
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public int UpdatedByUserID
        {
            get { return updatedByUserID; }
            set { updatedByUserID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public bool IsProcessed
        {
            get { return isProcessed; }
            set { isProcessed = value; }

        }

        public string CreatedByUserName
        {
            get { return createdByUserName; }
            set { createdByUserName = value; }      
        }

        public string UpdatedByUserName
        {
            get { return updatedByUserName; }
            set { updatedByUserName = value; }
        }

        //updateDate
        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        public string PP_PaymentId
        {
            get { return pP_PaymentId; }
            set { pP_PaymentId = value; }
        }

        public string PP_Response
        {
            get { return pP_Response; }
            set { pP_Response = value; }
        }


        //public string CreatedByUserName
        //{
        //    get
        //    {
        //        if (createdByUserName == null)
        //        {
        //            int portalId = PortalController.GetCurrentPortalSettings().PortalId;
        //            UserInfo user = UserController.GetUser(portalId, createdByUser, false);
        //            createdByUserName = user.DisplayName;
        //        }

        //        return createdByUserName;
        //    }
        //}

        #endregion
    }
}
