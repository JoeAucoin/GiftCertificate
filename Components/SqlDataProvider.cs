using System;
using System.Data;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace GIBS.Modules.GiftCertificate.Components
{
    public class SqlDataProvider : DataProvider
    {


        #region vars

        private const string providerType = "data";
        private const string moduleQualifier = "GIBS_";

        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private string connectionString;
        private string providerPath;
        private string objectQualifier;
        private string databaseOwner;

        #endregion

        #region cstor

        /// <summary>
        /// cstor used to create the sqlProvider with required parameters from the configuration
        /// section of web.config file
        /// </summary>
        public SqlDataProvider()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (connectionString == string.Empty)
                connectionString = provider.Attributes["connectionString"];

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (objectQualifier != string.Empty && !objectQualifier.EndsWith("_"))
                objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if (databaseOwner != string.Empty && !databaseOwner.EndsWith("."))
                databaseOwner += ".";
        }

        #endregion

        #region properties

        public string ConnectionString
        {
            get { return connectionString; }
        }


        public string ProviderPath
        {
            get { return providerPath; }
        }

        public string ObjectQualifier
        {
            get { return objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        #endregion

        #region private methods

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + moduleQualifier + name;
        }

        private object GetNull(object field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region override methods
        public override int GiftCertAddGiftCert(int moduleId, double CertAmount, string MailTo, string ToName, string mailToAddress, string mailToAddress1, string mailToCity, string mailToState, string mailToZip, string fromName, int fromUserID, string fromPhone, string fromEmail, string notes, int userId)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, GetFullyQualifiedName("GiftCertAddGiftCert"), moduleId, CertAmount, MailTo, ToName,
            mailToAddress,
            mailToAddress1,
            mailToCity,
            mailToState,
            mailToZip,
            fromName,
            fromUserID,            
            fromPhone,
            fromEmail,
            notes, userId));
        }
        public override int GiftCertUpdateWithPayPalReponse(int itemID, string paypalpaymentstate, string mailTo, string mailToAddress, string mailToAddress1, string mailToCity, string mailToState, string mailToZip, string notes, string pP_PaymentId, string pP_Response)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, GetFullyQualifiedName("GiftCertUpdateWithPayPalReponse"), 
                itemID, 
                paypalpaymentstate, 
                mailTo,
                mailToAddress,
                mailToAddress1,
                mailToCity,
                mailToState,
                mailToZip,
                notes, pP_PaymentId, pP_Response));
        }

        public override int GiftCertUpdateShipAddress(int moduleId, int itemId, string mailTo, string toAddress, string toAddress1, string toCity, string toState, string toZip)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, GetFullyQualifiedName("GiftCertUpdateShipAddress"),
                moduleId,
                itemId,

                mailTo,
                toAddress,
                toAddress1,
                toCity,
                toState,
                toZip));
        }

        public override IDataReader GetGiftCerts(int moduleId, DateTime startDate, DateTime endDate)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GiftCertGetGiftCerts"), moduleId, startDate, endDate);
        }

        public override IDataReader GetGiftCert(int itemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GiftCertGetGiftCert"), itemId);
        }

        public override void DeleteGiftCert(int moduleId, int itemId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("GiftCertDeleteGiftCert"), moduleId, itemId);
        }

        public override void GiftCertUpdateGiftCert(int moduleId, int itemId, double certAmount, string mailTo, string toName, string toAddress, string toAddress1, 
            string toCity, string toState, string toZip, string fromName, string fromPhone, string fromEmail, string notes, int updatedByUserID, bool isProcessed)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("GiftCertUpdateGiftCert"),
                moduleId, itemId, certAmount, mailTo, toName,
                toAddress,
                toAddress1,
                toCity,
                toState,
                toZip,
                fromName,
                fromPhone,
                fromEmail,
                notes,
                updatedByUserID,
                isProcessed);
        }


        //public override IDataReader GetGiftCertificates(int moduleId)
        //{
        //    return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetGiftCertificates"), moduleId);
        //}

        //public override IDataReader GetGiftCertificate(int moduleId, int itemId)
        //{
        //    return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetGiftCertificate"), moduleId, itemId);
        //}

        //public override void AddGiftCertificate(int moduleId, string content, int userId)
        //{
        //    SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("AddGiftCertificate"), moduleId, content, userId);
        //}

        //public override void UpdateGiftCertificate(int moduleId, int itemId, string content, int userId)
        //{
        //    SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("UpdateGiftCertificate"), moduleId, itemId, content, userId);
        //}

        //public override void DeleteGiftCertificate(int moduleId, int itemId)
        //{
        //    SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DeleteGiftCertificate"), moduleId, itemId);
        //}

        #endregion
    }
}
