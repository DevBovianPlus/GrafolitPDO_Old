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

namespace GrafolitPDO.Pages.Order
{
    public partial class PurchaseTable : ServerMasterPage
    {
        int inquiryIDFocusedRowIndex = 0;
        int filterType = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                inquiryIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString[Enums.QueryStringName.filter.ToString()] != null)
                filterType = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.filter.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (inquiryIDFocusedRowIndex > 0)
                {
                    ASPxGridViewIPurchase.FocusedRowIndex = ASPxGridViewIPurchase.FindVisibleIndexByKeyValue(inquiryIDFocusedRowIndex);
                    ASPxGridViewIPurchase.ScrollToVisibleIndexOnClient = ASPxGridViewIPurchase.FindVisibleIndexByKeyValue(inquiryIDFocusedRowIndex);
                }

                ASPxGridViewIPurchase.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void ASPxGridViewIPurchase_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
                AddValueToSession("PageName", "Order/PurchaseTable.aspx");
                ASPxWebControl.RedirectOnCallback(GenerateURI("OrderForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewIPurchase_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
 
        }

        protected void ASPxGridViewIPurchase_DataBinding(object sender, EventArgs e)
        {
            List<InquiryModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllPurchases());

            if (filterType > 0)
            {
                string statusKoda = "";
                switch (filterType)
                {
                    case 1:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString();
                        list = list.Where(l => l.StatusPovprasevanja.Koda == statusKoda).ToList();
                        break;
                    case 2:
                        statusKoda = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString();
                        list = list.Where(l => l.StatusPovprasevanja.Koda == statusKoda).ToList();
                        break;
                }
            }


            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewIPurchase.GetRowValues(ASPxGridViewIPurchase.FocusedRowIndex, "PovprasevanjeID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());

            RedirectWithCustomURI("InquiryForm.aspx", (int)Enums.UserAction.Delete, valueID);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
            RedirectWithCustomURI("InquiryForm.aspx", (int)Enums.UserAction.Add, 0);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewIPurchase.GetRowValues(ASPxGridViewIPurchase.FocusedRowIndex, "PovprasevanjeID");

            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
            AddValueToSession("PageName", "Order/PurchaseTable.aspx");
            Response.Redirect(GenerateURI("OrderForm.aspx", (int)Enums.UserAction.Edit, valueID));
        }

        private void InitializeEditDeleteButtons()
        {
            if (ASPxGridViewIPurchase.VisibleRowCount <= 0)
            {
                btnEdit.ClientVisible = false;
                btnDelete.ClientVisible = false;
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            List<int> valueIDs = ASPxGridViewIPurchase.GetSelectedFieldValues("PovprasevanjeID").OfType<int>().ToList();
            ClearAllSessions(Enum.GetValues(typeof(Enums.OrderSession)).Cast<Enums.OrderSession>().ToList());

            //zaklenemo povpraševanje da lahko samo en uporabnik naenkrat dela naročilo iz povpraševanja
            bool isLocked = CheckModelValidation(GetDatabaseConnectionInstance().LockInquiry(valueIDs[0], PrincipalHelper.GetUserPrincipal().ID));

            //PrincipalHelper.GetUserPrincipal().LockedInquiryByUser = valueIDs[0];

            if (isLocked)
                RedirectWithCustomURI("../Order/OrderForm.aspx", (int)Enums.UserAction.Add, valueIDs[0]);
        }

        protected void ASPxGridViewIPurchase_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            object locked = ASPxGridViewIPurchase.GetRowValues(e.VisibleIndex, "Zakleni");
            object status = ASPxGridViewIPurchase.GetRowValues(e.VisibleIndex, "StatusPovprasevanja.Koda");
            object order = ASPxGridViewIPurchase.GetRowValues(e.VisibleIndex, "NarociloID");

            bool isLocked = locked != null ? (CommonMethods.ParseBool(locked) ? true : false) : false;
            bool isinquiryNotSubmited = status != null ? (status.ToString() != DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString() ? true : false) : false;
            bool orderExist = order != null ? (CommonMethods.ParseInt(order) > 0 ? true : false) : false;

            if (isLocked || isinquiryNotSubmited || orderExist)//če je povpraševanje zaklenjeno potem uporabniku onemogočimo da bi izbral to povpraševanje in naredil naročilo
                e.Visible = false;
            else
                e.Visible = true;
        }
    }
}