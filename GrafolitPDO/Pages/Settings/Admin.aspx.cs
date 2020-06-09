using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Settings;
using DevExpress.Web;
using GrafolitPDO.Common;
using GrafolitPDO.Helpers;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO.Compression;

namespace GrafolitPDO.Pages.Settings
{
    public partial class Admin : ServerMasterPage
    {

        SettingsModel model = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!PrincipalHelper.IsUserAdmin() && !PrincipalHelper.IsUserSuperAdmin()) RedirectHome();

            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }

        private void Initialize()
        {

        }

        private void EnableUserControl(bool enable)
        {

        }

        protected void SettingsCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "")
            {

            }
        }

        protected void btnIzdelajNarocilnicoInPoslji_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().CreatePDFAndSendPDOOrdersMultiple());
        }

        protected void btnRunPantheon_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().RunPantheon(txtFile.Text, txtArgs.Text));
        }

        protected void btnChangeConfig_Click(object sender, EventArgs e)
        {
            CheckModelValidation(GetDatabaseConnectionInstance().ChangeConfigValue(txtConfigName.Text, txtConfigValue.Text));
        }

        protected void btnGetConfigVal_Click(object sender, EventArgs e)
        {
           lblRezultat.Text = CheckModelValidation(GetDatabaseConnectionInstance().GetConfigValue(txtConfigNameRet.Text));
        }

        protected void btnGetLogs_Click(object sender, EventArgs e)
        {
            byte[] bytes = CheckModelValidation(GetDatabaseConnectionInstance().GetWebServiceLogFile());
            byte[] UtilityServbytes = CheckModelValidation(GetDatabaseConnectionInstance().GetUtilityServiceLogFile());

            string applicationLogFile = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            byte[] applicationBytes = System.IO.File.ReadAllBytes(applicationLogFile);

            List<FileToDownload> list = new List<FileToDownload> { new FileToDownload { Name = "WebServiceLog.txt", Content = bytes, Extension=".txt" },
                new FileToDownload { Name = "ApplicationLog", Content = applicationBytes, Extension=".txt" }, new FileToDownload { Name = "UtilityServiceLog.txt", Content = UtilityServbytes, Extension=".txt" } };

            byte[] zip = CommonMethods.GetZipMemmoryStream(list);

            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment;filename=Logs.zip");
            Response.Buffer = true;
            Response.BinaryWrite(zip);
            
            Response.Flush();
            Response.End();
        }
    }
}
