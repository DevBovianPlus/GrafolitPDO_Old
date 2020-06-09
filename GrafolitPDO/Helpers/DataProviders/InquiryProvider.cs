using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsPDO.Inquiry;
using GrafolitPDO.Common;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OptimizacijaTransprotov.Helpers.DataProviders
{
    public class InquiryProvider : ServerMasterPage
    {
        public void SetInquiryModel(InquiryFullModel model)
        {
            AddValueToSession(Enums.InquirySession.InquiryModel, model);
        }

        public InquiryFullModel GetInquiryModel()
        {
            if (SessionHasValue(Enums.InquirySession.InquiryModel))
                return (InquiryFullModel)GetValueFromSession(Enums.InquirySession.InquiryModel);

            return null;
        }

        //Inquiry Status
        public void SetInquiryStatuses(List<InquiryStatusModel> list)
        {
            AddValueToSession(Enums.InquirySession.InquiryStatuses, list);
        }

        public List<InquiryStatusModel> GetInquiryStatuses()
        {
            if (SessionHasValue(Enums.InquirySession.InquiryStatuses))
                return (List<InquiryStatusModel>)GetValueFromSession(Enums.InquirySession.InquiryStatuses);
            return null;
        }

        public void SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry status)
        {
            AddValueToSession(Enums.InquirySession.InquiryStatus, status);
        }

        public DatabaseWebService.Common.Enums.Enums.StatusOfInquiry GetInquiryStatus()
        {
            if (SessionHasValue(Enums.InquirySession.InquiryStatus))
                return (DatabaseWebService.Common.Enums.Enums.StatusOfInquiry)GetValueFromSession(Enums.InquirySession.InquiryStatus);

            return DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.NEZNAN;
        }

        //Inquiry Position
        public void SetInquiryPositionModel(InquiryPositionModel model)
        {
            AddValueToSession(Enums.InquirySession.InquiryPositionModel, model);
        }

        public InquiryPositionModel GetInquiryPositionModel()
        {
            if (SessionHasValue(Enums.InquirySession.InquiryPositionModel))
                return (InquiryPositionModel)GetValueFromSession(Enums.InquirySession.InquiryPositionModel);

            return null;
        }

        //Inquiry Position
        public void SetInquiryPositionArtikelModel(InquiryPositionArtikelModel model)
        {
            AddValueToSession(Enums.InquirySession.InquiryPositionArtikelModel, model);
        }

        public InquiryPositionArtikelModel GetInquiryPositionArtikelModel()
        {
            if (SessionHasValue(Enums.InquirySession.InquiryPositionModel))
                return (InquiryPositionArtikelModel)GetValueFromSession(Enums.InquirySession.InquiryPositionArtikelModel);

            return null;
        }

        //searched suppliers list
        public void SetSearchedSupplierListModel(List<ClientSimpleModel> model)
        {
            AddValueToSession(Enums.InquirySession.SupplierListModel, model);
        }

        public List<ClientSimpleModel> GetSearchedSupplierListModel()
        {
            if (SessionHasValue(Enums.InquirySession.SupplierListModel))
                return (List<ClientSimpleModel>)GetValueFromSession(Enums.InquirySession.SupplierListModel);

            return null;
        }

        //BuyerList
        public void SetBuyerListModel(List<ClientSimpleModel> model)
        {
            AddValueToSession(Enums.InquirySession.BuyerList, model);
        }

        public List<ClientSimpleModel> GetBuyerListModel()
        {
            if (SessionHasValue(Enums.InquirySession.BuyerList))
                return (List<ClientSimpleModel>)GetValueFromSession(Enums.InquirySession.BuyerList);

            return null;
        }

       
    }
}