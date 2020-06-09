using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Order;
using GrafolitPDO.Common;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafolitPDO.Helpers.DataProviders
{
    public class OrderDataProvider : ServerMasterPage
    {
        public void SetOrderModel(OrderPDOFullModel model)
        {
            AddValueToSession(Enums.OrderSession.OrderModel, model);
        }

        public OrderPDOFullModel GetOrderModel()
        {
            if (SessionHasValue(Enums.OrderSession.OrderModel))
                return (OrderPDOFullModel)GetValueFromSession(Enums.OrderSession.OrderModel);

            return null;
        }

        //Order position
        public void SetOrderPositionModel(OrderPDOPositionModel model)
        {
            AddValueToSession(Enums.OrderSession.OrderPositionModel, model);
        }

        public OrderPDOPositionModel GetOrderPositionModel()
        {
            if (SessionHasValue(Enums.OrderSession.OrderPositionModel))
                return (OrderPDOPositionModel)GetValueFromSession(Enums.OrderSession.OrderPositionModel);

            return null;
        }

        //searched product list
        public void SetSearchedProductListModel(List<ProductModel> model)
        {
            AddValueToSession(Enums.OrderSession.ProductListModel, model);
        }

        public List<ProductModel> GetSearchedProductListModel()
        {
            if (SessionHasValue(Enums.OrderSession.ProductListModel))
                return (List<ProductModel>)GetValueFromSession(Enums.OrderSession.ProductListModel);

            return null;
        }

        //Selected searched product
        public void SetSelectedSearchedProduct(ProductModel model)
        {
            AddValueToSession(Enums.OrderSession.SelectedSearchedProduct, model);
        }

        public ProductModel GetSelectedSearchedProduct()
        {
            if (SessionHasValue(Enums.OrderSession.SelectedSearchedProduct))
                return (ProductModel)GetValueFromSession(Enums.OrderSession.SelectedSearchedProduct);

            return null;
        }

        //Order Status
        public void SetOrderStatuses(List<InquiryStatusModel> list)
         {
             AddValueToSession(Enums.OrderSession.OrderStatuses, list);
         }

         public List<InquiryStatusModel> GetOrderStatuses()
         {
             if (SessionHasValue(Enums.OrderSession.OrderStatuses))
                 return (List<InquiryStatusModel>)GetValueFromSession(Enums.OrderSession.OrderStatuses);
             return null;
         }

         public void SetOrderStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry status)
         {
             AddValueToSession(Enums.OrderSession.OrderStatus, status);
         }

         public DatabaseWebService.Common.Enums.Enums.StatusOfInquiry GetOrderStatus()
         {
             if (SessionHasValue(Enums.OrderSession.OrderStatus))
                 return (DatabaseWebService.Common.Enums.Enums.StatusOfInquiry)GetValueFromSession(Enums.OrderSession.OrderStatus);

             return DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.NEZNAN;
         }
    }
}