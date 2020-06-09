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

namespace GrafolitPDO.Pages.Inquiry
{
    public partial class InquiryTable : ServerMasterPage
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
                    ASPxGridViewInquiry.FocusedRowIndex = ASPxGridViewInquiry.FindVisibleIndexByKeyValue(inquiryIDFocusedRowIndex);
                    ASPxGridViewInquiry.ScrollToVisibleIndexOnClient = ASPxGridViewInquiry.FindVisibleIndexByKeyValue(inquiryIDFocusedRowIndex);
                }

                ASPxGridViewInquiry.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void ASPxGridViewInquiry_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] split = e.Parameters.Split(';');
            if (split[0] == "DblClick" && !String.IsNullOrEmpty(split[1]))
            {
                ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());
                ASPxWebControl.RedirectOnCallback(GenerateURI("InquiryForm.aspx", (int)Enums.UserAction.Edit, split[1]));
            }
        }

        protected void ASPxGridViewInquiry_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fffac9");//#
            else if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString())
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffebc9");//#fcf8e3

            //if (CommonMethods.ParseBool(e.GetValue("Zakleni")))
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2dede");

            if (CommonMethods.ParseInt(e.GetValue("NarociloID")) > 0)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#9bd19d");
        }

        protected void ASPxGridViewInquiry_DataBinding(object sender, EventArgs e)
        {
            List<InquiryModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllInquiries());

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
            object valueID = ASPxGridViewInquiry.GetRowValues(ASPxGridViewInquiry.FocusedRowIndex, "PovprasevanjeID");
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
            object valueID = ASPxGridViewInquiry.GetRowValues(ASPxGridViewInquiry.FocusedRowIndex, "PovprasevanjeID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());

            RedirectWithCustomURI("InquiryForm.aspx", (int)Enums.UserAction.Edit, valueID);
        }

        private void InitializeEditDeleteButtons()
        {
            if (ASPxGridViewInquiry.VisibleRowCount <= 0)
            {
                btnEdit.ClientVisible = false;
                btnDelete.ClientVisible = false;
                btnCopyPos.ClientVisible = false;
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            List<int> valueIDs = ASPxGridViewInquiry.GetSelectedFieldValues("PovprasevanjeID").OfType<int>().ToList();
            ClearAllSessions(Enum.GetValues(typeof(Enums.OrderSession)).Cast<Enums.OrderSession>().ToList());

            //zaklenemo povpraševanje da lahko samo en uporabnik naenkrat dela naročilo iz povpraševanja
            bool isLocked = CheckModelValidation(GetDatabaseConnectionInstance().LockInquiry(valueIDs[0], PrincipalHelper.GetUserPrincipal().ID));
            AddValueToSession("PageName", "Inquiry/InquiryTable.aspx");
            //PrincipalHelper.GetUserPrincipal().LockedInquiryByUser = valueIDs[0];

            if (isLocked)
                RedirectWithCustomURI("../Order/OrderForm.aspx", (int)Enums.UserAction.Add, valueIDs[0]);
        }

        protected void ASPxGridViewInquiry_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            object locked = ASPxGridViewInquiry.GetRowValues(e.VisibleIndex, "Zakleni");
            object status = ASPxGridViewInquiry.GetRowValues(e.VisibleIndex, "StatusPovprasevanja.Koda");
            object order = ASPxGridViewInquiry.GetRowValues(e.VisibleIndex, "NarociloID");

            //bool isLocked = locked != null ? (CommonMethods.ParseBool(locked) ? true : false) : false;
            bool isinquiryNotSubmited = status != null ? (status.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString() ? true : false) : false;
            bool isinquiryFinished = status != null ? (status.ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.PRIPRAVLJENO.ToString() ? true : false) : false;
            bool orderExist = order != null ? (CommonMethods.ParseInt(order) > 0 ? true : false) : false;

            if (isinquiryNotSubmited || isinquiryFinished)//če je povpraševanje zaklenjeno potem uporabniku onemogočimo da bi izbral to povpraševanje in naredil naročilo
                e.Visible = true;
            else
                e.Visible = false;
        }

        protected void btnCopyInqury_Click(object sender, EventArgs e)
        {
            object valueID = ASPxGridViewInquiry.GetRowValues(ASPxGridViewInquiry.FocusedRowIndex, "PovprasevanjeID");
            ClearAllSessions(Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList());

            CheckModelValidation(GetDatabaseConnectionInstance().CopyInquiryByID(Convert.ToInt32(valueID)));
            ASPxGridViewInquiry.DataBind();
        }
    }
}