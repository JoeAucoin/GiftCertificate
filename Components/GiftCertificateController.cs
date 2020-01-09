using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace GIBS.Modules.GiftCertificate.Components
{
    public class GiftCertificateController : ISearchable, IPortable
    {
        #region public method
        public int GiftCertAddGiftCert(GiftCertificateInfo info)
        {
            //check we have some content to store
            if (info.ToName != string.Empty)
            {
                return Convert.ToInt32(DataProvider.Instance().GiftCertAddGiftCert(info.ModuleId, info.CertAmount, info.MailTo, info.ToName, info.MailToAddress,
                info.MailToAddress1,
                info.MailToCity,
                info.MailToState,
                info.MailToZip,
                info.FromName,
                info.FromUserID,
                info.FromPhone,
                info.FromEmail,
                info.Notes, info.CreatedByUserID));
            }
            else
            {
                return 0;
            }
        }
        public int GiftCertUpdateWithPayPalReponse(GiftCertificateInfo info)
        {
            //check we have some content to store
            if (info.ItemId >0)
            {
                return Convert.ToInt32(DataProvider.Instance().GiftCertUpdateWithPayPalReponse(
                    info.ItemId,
                    info.PaypalPaymentState,
                    info.MailTo,
                    info.MailToAddress,
                    info.MailToAddress1,
                    info.MailToCity,
                    info.MailToState,
                    info.MailToZip,
                    info.Notes,
                    info.PP_PaymentId,
                    info.PP_Response));
            }
            else
            {
                return 0;
            }
        }


        public int GiftCertUpdateShipAddress(GiftCertificateInfo info)
        {
            //check we have some content to store
            if (info.ItemId > 0)
            {
                return Convert.ToInt32(DataProvider.Instance().GiftCertUpdateShipAddress(
                    info.ModuleId,
                    info.ItemId,

                    info.MailTo,
                    info.MailToAddress,
                    info.MailToAddress1,
                    info.MailToCity,
                    info.MailToState,
                    info.MailToZip
                    ));
            }
            else
            {
                return 0;
            }
        }


        public void GiftCertUpdateGiftCert(GiftCertificateInfo info)
        {
            //check we have some content to update
            if (info.ItemId > 0)
            {
                DataProvider.Instance().GiftCertUpdateGiftCert(info.ModuleId,
                    info.ItemId,
                    info.CertAmount,
                    info.MailTo,
                    info.ToName,
                    info.MailToAddress,
                    info.MailToAddress1,
                    info.MailToCity,
                    info.MailToState,
                    info.MailToZip,
                    info.FromName,
                    info.FromPhone,
                    info.FromEmail,
                    info.Notes,
                    info.UpdatedByUserID,
                    info.IsProcessed);
            }
        }


        public List<GiftCertificateInfo> GetGiftCerts(int moduleId, DateTime startDate, DateTime endDate)
        {
            return CBO.FillCollection<GiftCertificateInfo>(DataProvider.Instance().GetGiftCerts(moduleId, startDate, endDate));
        }

        /// <summary>
        /// Get an info object from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public GiftCertificateInfo GetGiftCert(int itemId)
        {
            return (GiftCertificateInfo)CBO.FillObject(DataProvider.Instance().GetGiftCert(itemId), typeof(GiftCertificateInfo));
        }


        public void DeleteGiftCert(int moduleId, int itemId)
        {
            DataProvider.Instance().DeleteGiftCert(moduleId, itemId);
        }

        /// <summary>
        /// Gets all the GiftCertificateInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        //public List<GiftCertificateInfo> GetGiftCertificates(int moduleId)
        //{
        //    return CBO.FillCollection<GiftCertificateInfo>(DataProvider.Instance().GetGiftCertificates(moduleId));
        //}

        ///// <summary>
        ///// Get an info object from the database
        ///// </summary>
        ///// <param name="moduleId"></param>
        ///// <param name="itemId"></param>
        ///// <returns></returns>
        //public GiftCertificateInfo GetGiftCertificate(int moduleId, int itemId)
        //{
        //    return (GiftCertificateInfo)CBO.FillObject(DataProvider.Instance().GetGiftCertificate(moduleId, itemId), typeof(GiftCertificateInfo));
        //}


        ///// <summary>
        ///// Adds a new GiftCertificateInfo object into the database
        ///// </summary>
        ///// <param name="info"></param>
        //public void AddGiftCertificate(GiftCertificateInfo info)
        //{
        //    //check we have some content to store
        //    if (info.Content != string.Empty)
        //    {
        //        DataProvider.Instance().AddGiftCertificate(info.ModuleId, info.Content, info.CreatedByUserID);
        //    }
        //}

        ///// <summary>
        ///// update a info object already stored in the database
        ///// </summary>
        ///// <param name="info"></param>
        //public void UpdateGiftCertificate(GiftCertificateInfo info)
        //{
        //    //check we have some content to update
        //    if (info.Content != string.Empty)
        //    {
        //        DataProvider.Instance().UpdateGiftCertificate(info.ModuleId, info.ItemId, info.Content, info.CreatedByUserID);
        //    }
        //}


        ///// <summary>
        ///// Delete a given item from the database
        ///// </summary>
        ///// <param name="moduleId"></param>
        ///// <param name="itemId"></param>
        //public void DeleteGiftCertificate(int moduleId, int itemId)
        //{
        //    DataProvider.Instance().DeleteGiftCertificate(moduleId, itemId);
        //}


        #endregion

        #region ISearchable Members

        /// <summary>
        /// Implements the search interface required to allow DNN to index/search the content of your
        /// module
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

            //List<GiftCertificateInfo> infos = GetGiftCertificates(modInfo.ModuleID);

            //foreach (GiftCertificateInfo info in infos)
            //{
            //    SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Notes, info.CreatedByUserID, info.CreatedDate,
            //                                        modInfo.ModuleID, info.ItemId.ToString(), info.Notes, "Item=" + info.ItemId.ToString());
            //    searchItems.Add(searchInfo);
            //}

            return searchItems;
        }

        #endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            //List<GiftCertificateInfo> infos = GetGiftCertificates(moduleID);

            //if (infos.Count > 0)
            //{
            //    sb.Append("<GiftCertificates>");
            //    foreach (GiftCertificateInfo info in infos)
            //    {
            //        sb.Append("<GiftCertificate>");
            //        sb.Append("<content>");
            //        sb.Append(XmlUtils.XMLEncode(info.Notes));
            //        sb.Append("</content>");
            //        sb.Append("</GiftCertificate>");
            //    }
            //    sb.Append("</GiftCertificates>");
            //}

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "GiftCertificates");

            foreach (XmlNode info in infos.SelectNodes("GiftCertificate"))
            {
                //GiftCertificateInfo GiftCertificateInfo = new GiftCertificateInfo();
                //GiftCertificateInfo.ModuleId = ModuleID;
                //GiftCertificateInfo.Content = info.SelectSingleNode("content").InnerText;
                //GiftCertificateInfo.CreatedByUserID = UserID;

                //AddGiftCertificate(GiftCertificateInfo);
            }
        }

        #endregion
    }
}
