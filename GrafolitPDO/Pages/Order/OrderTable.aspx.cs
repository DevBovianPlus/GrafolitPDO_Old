using GrafolitPDO.Common;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseWebService.ModelsPDO;
using DevExpress.Web;
using DatabaseWebService.ModelsPDO.Inquiry;
using GrafolitPDO.Helpers;
using DatabaseWebService.ModelsPDO.Order;

namespace GrafolitPDO.Pages.Order
{
    public partial class OrderTable : ServerMasterPage
    {
        int orderIDFocusedRowIndex = 0;
        int filterType = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                orderIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString[Enums.QueryStringName.filter.ToString()] != null)
                filterType = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.filter.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (orderIDFocusedRowIndex > 0)
                {
                    ASPxGridViewOrder.FocusedRowIndex = ASPxGridViewOrder.FindVisibleIndexByKeyValue(orderIDFocusedRowIndex);
                    ASPxGridViewOrder.ScrollToVisibleIndexOnClient = ASPxGridViewOrder.FindVisibleIndexByKeyValue(orderIDFocusedRowIndex);
                }

                ASPxGridViewOrder.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewOrder.GetRowValues(ASPxGridViewOrder.FocusedRowIndex, "NarociloID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());

            RedirectWithCustomURI("OrderForm.aspx", (int)Enums.UserAction.Delete, valueID);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
            RedirectWithCustomURI("OrderForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewOrder.GetRowValues(ASPxGridViewOrder.FocusedRowIndex, "NarociloID");
            AddValueToSession("PageName", "Order/OrderTable.aspx");
            int iOrderID = Convert.ToInt16(valueID);

            OrderPDOFullModel model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderByID(iOrderID));
            if (model != null)
            {
                AddValueToSession(Enums.OrderSession.DobaviteljID, model.StrankaDobaviteljID.ToString());
            }
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());            
            RedirectWithCustomURI("OrderForm.aspx", (int)Enums.UserAction.Edit, model.PovprasevanjeID);
        }

        private void InitializeEditDeleteButtons()
        {
            if (ASPxGridViewOrder.VisibleRowCount <= 0)
            {
                btnEdit.ClientVisible = false;
            }
        }

        protected void ASPxGridViewOrder_DataBinding(object sender, EventArgs e)
        {

            List<OrderPDOFullModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderList());

            /*if (filterType > 0)
            {
                string statusKoda = "";
                switch (filterType)
                {
                    case 1:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POTRJEN.ToString();
                        list = list.Where(l => l.StatusPovprasevanja.Koda == statusKoda).ToList();
                        break;
                    case 2:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ZAVRNJEN.ToString();
                        list = list.Where(l => l.StatusPovprasevanja.Koda == statusKoda).ToList();
                        break;
                    case 3:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString();
                        list = list.Where(l => l.StatusPovprasevanja.Koda == statusKoda).ToList();
                        break;
                }
            }*/


            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewOrder_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                AddValueToSession("PageName", "Order/OrderTable.aspx");
                int iOrderID = Convert.ToInt16(split[1]);

                OrderPDOFullModel model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderByID(iOrderID));
                if (model != null)
                {
                    AddValueToSession(Enums.OrderSession.DobaviteljID, model.StrankaDobaviteljID.ToString());
                }
                ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("OrderForm.aspx", (int)Enums.UserAction.Edit, model.PovprasevanjeID));
            }
        }

        protected void ASPxGridViewOrder_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            //if (e.GetValue("StatusModel.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString())
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA879");//#
            //else if (e.GetValue("StatusModel.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ORDER_NO_SEND.ToString())
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0e462");//#fcf8e3
        }

        protected void ASPxGridViewOrder_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            object status = ASPxGridViewOrder.GetRowValues(e.VisibleIndex, "StatusModel.Koda");
            object order = ASPxGridViewOrder.GetRowValues(e.VisibleIndex, "NarociloID");

            bool isinquiryNotSubmited = status != null ? (status.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ERR_ADMIN_MAIL.ToString() ? true : false) : false;
            bool isOrderCreated = status != null ? (status.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.USTVARJENO_NAROCILO.ToString() ? true : false) : false;
        
            bool orderExist = order != null ? (CommonMethods.ParseInt(order) > 0 ? true : false) : false;

            if (isinquiryNotSubmited || isOrderCreated)//če je povpraševanje zaklenjeno potem uporabniku onemogočimo da bi izbral to povpraševanje in naredil naročilo
                e.Visible = true;
            else
                e.Visible = false;
        }

        protected void btnClearStatus_Click(object sender, EventArgs e)
        {
            List<int> valueIDs = ASPxGridViewOrder.GetSelectedFieldValues("NarociloID").OfType<int>().ToList();

            if (valueIDs.Count == 1)
            {
                CheckModelValidation(GetDatabaseConnectionInstance().ResetOrderStatusByID(valueIDs[0]));
            }

            ASPxGridViewOrder.DataBind();
        }

        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CreatePDFAndSendPDOOrdersMultiple());
        }
    }
}