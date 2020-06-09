using Newtonsoft.Json;
using GrafolitPDO.Common;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrafolitPDO.Helpers;
using DatabaseWebService.ModelsPDO;

namespace OptimizacijaTransprotov
{
    public partial class Home : ServerMasterPage
    {
        private DatabaseConnection dbConnection;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Request.IsAuthenticated)
                MasterPageFile = "~/Main.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                ASPxFormLayoutLogin.Visible = false;
                FormLayoutWrap.Style.Add("display", "none");
                MainDashboard.Style.Add("display", "block");
                this.Master.PageHeadlineTitle = "PDO - Povpraševanje dobaviteljem";

                DashboardPDOModel data = CheckModelValidation(GetDatabaseConnectionInstance().GetDashboardPDOData());
                if (data != null)
                {
                    lblAllInquiries.Text = data.InquiryCount.ToString();
                    lblConfirmedInquiries.Text = data.ConfirmedInquiries.ToString();
                    lblInPurchase.Text = data.InquiriesInPurchase.ToString();
                    lblInquiriesInProgress.Text = data.InquiriesInProgress.ToString();
                    lblSubmitedOrder.Text = data.SubmitedOrders.ToString();
                }
            }
        }

        protected void LoginCallback_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            Authentication auth = new Authentication();
            bool signInSuccess = false;
            string message = "";
            string username = CommonMethods.Trim(txtUsername.Text);
            string password = CommonMethods.Trim(txtPassword.Text);

            try
            {
                if (username != "" && password != "")
                {
                    CommonMethods.LogThis("Before Authenticate");
                    signInSuccess = auth.Authenticate(username, password);
                    CommonMethods.LogThis("After Authenticate : " + signInSuccess);
                }
            }
            catch (Exception ex)
            {
                CommonMethods.LogThis(ex.Message);
                message = ex.Message;
            }

            if (signInSuccess)
            {
                Session.Remove("PreviousPage");
            }
            else
            {
                LoginCallback.JSProperties["cpResult"] = message;
            }
        }

        protected void ChartsCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (PrincipalHelper.GetUserPrincipal() != null && PrincipalHelper.GetUserPrincipal().ID > 0)
            {
                dbConnection = new DatabaseConnection();
                dbConnection.UnLockInquiriesByUserID(PrincipalHelper.GetUserPrincipal().ID);
            }

            if (e.Parameter == "RefreshCharts" && Request.IsAuthenticated)
            {
                DashboardPDOModel data = CheckModelValidation(GetDatabaseConnectionInstance().GetDashboardPDOData());

                ChartsCallbackPanel.JSProperties["cpChartData"] = JsonConvert.SerializeObject(data.CurrentYearInquiry);
                ChartsCallbackPanel.JSProperties["cpChartDataEmployees"] = JsonConvert.SerializeObject(data.EmployeesInquiryCount);
                /*ChartsCallbackPanel.JSProperties["cpChartDataTransporters"] = JsonConvert.SerializeObject(data.TransporterRecallCount);
                ChartsCallbackPanel.JSProperties["cpChartDataRoutes"] = JsonConvert.SerializeObject(data.RouteRecallCount);
                ChartsCallbackPanel.JSProperties["cpChartDataSupplier"] = JsonConvert.SerializeObject(data.SupplierRecallCount);*/
            }
        }
    }
}