using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsPDO;
using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Order;
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
    public partial class SystemEmailBody_popup : ServerMasterPage
    {
        int systemEmailMessageID = -1;
        string emailBody = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            systemEmailMessageID = CommonMethods.ParseInt(GetStringValueFromSession("SystemEmailMessageID"));
            emailBody = GetStringValueFromSession("SystemEmailMessageBody");            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxHtmlEditorEmailBody.Html = emailBody;
        }

        protected void btnConfirmPopUp_Click(object sender, EventArgs e)
        {
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



            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "EmailList"), true);

        }
    }
}