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
using DatabaseWebService.Models;
using DatabaseWebService.ModelsPDO.Settings;

namespace GrafolitPDO.Pages.Settings
{
    public partial class SystemEmailTable : ServerMasterPage
    {
        int employeeIDFocusedRowIndex = 0;
        int filterType = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
                employeeIDFocusedRowIndex = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());

            if (Request.QueryString[Enums.QueryStringName.filter.ToString()] != null)
                filterType = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.filter.ToString()].ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (employeeIDFocusedRowIndex > 0)
                {
                    ASPxGridViewEmails.FocusedRowIndex = ASPxGridViewEmails.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                    ASPxGridViewEmails.ScrollToVisibleIndexOnClient = ASPxGridViewEmails.FindVisibleIndexByKeyValue(employeeIDFocusedRowIndex);
                }

                ASPxGridViewEmails.DataBind();
                InitializeEditDeleteButtons();
            }
        }

        protected void ASPxGridViewEmails_DataBinding(object sender, EventArgs e)
        {
            List<PDOEmailModel> list = CheckModelValidation(GetDatabaseConnectionInstance().GetAllEmails());

            (sender as ASPxGridView).DataSource = list;
            (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
        }

        private void InitializeEditDeleteButtons()
        {

        }


        protected void PopupControlEmployee_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.EmployeeSession.EmployeeID);
            RemoveSession(Enums.EmployeeSession.EmployeeModel);
        }

        protected void ASPxPopupControlSystemEmailBody_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession("SystemEmailMessageID");
            RemoveSession("SystemEmailMessageBody");
        }

        protected void SytemEmailMessageCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] split = e.Parameter.Split('|');

            if (split[0] == "DblClickShowEmailBody")
            {
                string emailID = split[1];
                string emailBody = split[2];
                AddValueToSession("SystemEmailMessageID", emailID);
                AddValueToSession("SystemEmailMessageBody", emailBody);

                ASPxPopupControlSystemEmailBody.ShowOnPageLoad = true;
            }
            else if (split[0] == "ShowAndEditEmail")
            {
                var messageID = ASPxGridViewEmails.GetRowValues(ASPxGridViewEmails.FocusedRowIndex, "SystemEmailMessageID");
                bool isValid = SetSessionsAndOpenPopUp("2", Enums.SystemEmailMessageSession.SystemMessageID, messageID);
                ASPxPopupControlSystemEmail.ShowOnPageLoad = isValid;
            }
        }

        protected void btnCopyEmailAndSend_Click(object sender, EventArgs e)
        {
            var mailID = ASPxGridViewEmails.GetRowValues(ASPxGridViewEmails.FocusedRowIndex, "SystemEmailMessageID");
            CheckModelValidation(GetDatabaseConnectionInstance().CreateMailCopy((int)mailID));

            ASPxGridViewEmails.DataBind();
        }

        protected void ASPxPopupControlSystemEmail_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageModel);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageID);
        }
    }
}