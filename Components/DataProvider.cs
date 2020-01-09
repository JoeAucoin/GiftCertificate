using System;
using System.Data;
using DotNetNuke;
using DotNetNuke.Framework;

namespace GIBS.Modules.GiftCertificate.Components
{
    public abstract class DataProvider
    {

        #region common methods

        /// <summary>
        /// var that is returned in the this singleton
        /// pattern
        /// </summary>
        private static DataProvider instance = null;

        /// <summary>
        /// private static cstor that is used to init an
        /// instance of this class as a singleton
        /// </summary>
        static DataProvider()
        {
            instance = (DataProvider)Reflection.CreateObject("data", "GIBS.Modules.GiftCertificate.Components", "");
        }

        /// <summary>
        /// Exposes the singleton object used to access the database with
        /// the conrete dataprovider
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return instance;
        }

        #endregion


        #region Abstract methods

        /* implement the methods that the dataprovider should */

        public abstract int GiftCertAddGiftCert(int moduleId, double certAmount, string mailTo, string toName, string toAddress, string toAddress1, string toCity, string toState, string toZip, string fromName, int fromUserID, string fromPhone, string fromEmail, string notes, int userId);
        public abstract int GiftCertUpdateWithPayPalReponse(int itemId, string paypalpaymentstate, string mailTo, string toAddress, string toAddress1, string toCity, string toState, string toZip, string notes, string pP_PaymentId, string pP_Response);

        public abstract IDataReader GetGiftCerts(int moduleId, DateTime startDate, DateTime endDate);
        public abstract IDataReader GetGiftCert(int itemId);
        public abstract void DeleteGiftCert(int moduleId, int itemId);

        public abstract void GiftCertUpdateGiftCert(int moduleId, int itemId, double certAmount, string mailTo, string toName, string toAddress, string toAddress1, string toCity, string toState, string toZip, string fromName, string fromPhone, string fromEmail, string notes, int updatedByUserID, bool isProcessed);


        public abstract int GiftCertUpdateShipAddress(int moduleId, int itemId, string mailTo, string toAddress, string toAddress1, string toCity, string toState, string toZip);


        //public abstract IDataReader GetGiftCertificates(int moduleId);
        //public abstract IDataReader GetGiftCertificate(int moduleId, int itemId);
        //public abstract void AddGiftCertificate(int moduleId, string content, int userId);
        //public abstract void UpdateGiftCertificate(int moduleId, int itemId, string content, int userId);
        //public abstract void DeleteGiftCertificate(int moduleId, int itemId);

        #endregion

    }



}
