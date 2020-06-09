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
    public partial class SettingsForm : ServerMasterPage
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
                    FillForm();
                }
            }
        }

        private void Initialize()
        {

        }

        private void FillForm()
        {
            txtInquiryNum.Text = model.PovprasevanjeStevilcenjeStev.ToString();
            txtPrefix.Text = model.PovprasevanjeStevilcenjePredpona;

            CheckBoxMailSending.Checked = model.PosiljanjePoste;
            txtSMTPServer.Text = model.EmailStreznik;

            CheckBoxSSLEncrypting.Checked = model.EmailSifriranjeSSL;
            txtPort.Text = model.EmailVrata.ToString();

            MemoNotes.Text = model.Opombe;
        }
        
        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = new SettingsModel();

                model.NastavitveID = 0;
                model.ts = DateTime.Now;
                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            }

            model.tsUpdateUserID = PrincipalHelper.GetUserPrincipal().ID;

            model.PovprasevanjeStevilcenjeStev = CommonMethods.ParseInt(txtInquiryNum.Text);
            model.PovprasevanjeStevilcenjePredpona = txtPrefix.Text;
            model.PosiljanjePoste = CheckBoxMailSending.Checked;
            model.Opombe = MemoNotes.Text;

            model.EmailStreznik = txtSMTPServer.Text;
            model.EmailSifriranjeSSL = CheckBoxSSLEncrypting.Checked;
            model.EmailVrata = CommonMethods.ParseInt(txtPort.Text);

            SettingsModel returnModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveSettings(model));

            RemoveSession(Enums.InquirySession.InquiryStatus);

            if (returnModel != null)
            {
                //this we need if we want to add new client and then go and add new Plan with no redirection to Clients page
                model = returnModel;//if we need updated model in the same request;

                return true;
            }
            else
                return false;
        }

        private void EnableUserControl(bool enable)
        {
            btnSaveChanges.ClientEnabled = enable;
        }

        protected void SettingsCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "SaveSettings")
            {
                AddOrEditEntityObject(true);
            }
        }
    }
}