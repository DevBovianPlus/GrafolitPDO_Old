using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsPDO;
using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Order;
using DatabaseWebService.ModelsPDO.Settings;
using DevExpress.Web;
using GrafolitPDO.Common;
using GrafolitPDO.Helpers;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrafolitPDO.Pages.Settings
{
    public partial class SystemEmail_popup : ServerMasterPage
    {
        int action = -1;
        int mailID = -1;
        PDOEmailModel model;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            action = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            mailID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.SystemEmailMessageSession.SystemMessageID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    if (mailID > 0)
                    {
                        if (GetSystemEmailDataProvider().GetSystemEmailMessageModel() != null)
                            model = GetSystemEmailDataProvider().GetSystemEmailMessageModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetMailByID(mailID));
                        }

                        if (model != null)
                        {
                            GetSystemEmailDataProvider().SetSystemEmailMessageModel(model);
                            FillForm();
                        }
                    }
                }
            }
            else
            {
                if (GetSystemEmailDataProvider().GetSystemEmailMessageModel() != null)
                    model = GetSystemEmailDataProvider().GetSystemEmailMessageModel();
            }
        }

        private void FillForm()
        {
            txtEmailFrom.Text = model.EmailFrom;
            txtEmailTo.Text = model.EmailTo;
            txtSubject.Text = model.EmailSubject;
            ASPxHtmlEditorEmailBody.Html = model.EmailBody;
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new PDOEmailModel();

                model.SystemEmailMessageID = 0;

                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                model = GetSystemEmailDataProvider().GetSystemEmailMessageModel();
            }

            model.EmailFrom = txtEmailFrom.Text;
            model.EmailBody = ASPxHtmlEditorEmailBody.Html;
            model.EmailSubject = txtSubject.Text;
            model.EmailTo = txtEmailTo.Text;

            PDOEmailModel newModel = CheckModelValidation(GetDatabaseConnectionInstance().CreateMailCopy(model));

            if (newModel != null)//If new record is added we need to refresh aspxgridview. We add new record to session model.
            {
                model = newModel;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
            AddOrEditEntityObject();
            RemoveSessionsAndClosePopUP(true);
        }

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {            
            RemoveSessionsAndClosePopUP(false);
        }


        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageModel);
            RemoveSession(Enums.SystemEmailMessageSession.SystemMessageID);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "EditEmailAndSend"), true);

        }
    }
}