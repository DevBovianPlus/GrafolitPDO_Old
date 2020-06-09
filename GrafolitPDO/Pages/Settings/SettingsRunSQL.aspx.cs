using DatabaseWebService.ModelsPDO.Inquiry;
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
    public partial class SettingsRunSQL : ServerMasterPage
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

                model = CheckModelValidation(GetDatabaseConnectionInstance().GetSettings());

                if (model != null)
                {
                  
                }
            }
        }

        private void Initialize()
        {

        }

              

        protected void btnRunSQL_Click(object sender, EventArgs e)
        {
            bool b = CheckModelValidation(GetDatabaseConnectionInstance().RunSQLString(memSQL.Text));
        }
    }
}