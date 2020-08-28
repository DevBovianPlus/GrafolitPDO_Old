using DatabaseWebService.DomainPDO;
using DatabaseWebService.ModelsPDO.Inquiry;
using DevExpress.Web;
using GrafolitPDO.Common;
using GrafolitPDO.Helpers;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrafolitPDO.Pages.Inquiry
{
    public partial class InquiryForm : ServerMasterPage
    {

        InquiryFullModel model = null;
        int inquiryID = -1;
        int action = -1;
        int selectedInquryID = 0;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                inquiryID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            this.Master.DisableNavBar = true;

            ASPxGridViewInquiryPosition.Settings.GridLines = GridLines.Both;
            GridLookupBuyer.GridView.Settings.GridLines = GridLines.Both;
            //odstranimo sejo tako da se bo na client-u prikazalo samo enkrat (takrat ko se bo stisnil gumb oddaj povpraševanje)
            RemoveSession(Enums.InquirySession.SuppliersValidOnSubmittingInquiry);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    if (inquiryID > 0)
                    {
                        if (GetInquiryDataProvider().GetInquiryModel() != null)
                            model = GetInquiryDataProvider().GetInquiryModel();
                        else
                        {
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryByID(inquiryID, false, 0));
                        }

                        if (model != null)
                        {
                            GetInquiryDataProvider().SetInquiryModel(model);
                            FillForm();
                        }
                    }
                }
                else if (action == (int)Enums.UserAction.Add)
                {
                    SetFormDefaultValues();
                }
                UserActionConfirmBtnUpdate(btnSaveChanges, action);
                btnSaveChanges.Text = (action == (int)Enums.UserAction.Delete) ? "Izbriši povpraševanje" : btnSaveChanges.Text;
            }
            else
            {
                if (model == null && SessionHasValue(Enums.InquirySession.InquiryModel))
                    model = GetInquiryDataProvider().GetInquiryModel();
            }
        }

        private void Initialize()
        {
            GridLookupBuyer.DataBind();
            GetInquiryDataProvider().SetInquiryStatuses(CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryStatuses()));
            GridLookupStatus.DataBind();



            if (SessionHasValue(Enums.CommonSession.StayOnFormAndOpenPopup))
            {
                bool isValid = SetSessionsAndOpenPopUp("1", Enums.InquirySession.InquiryPositionID, null);
                PopupControlInquiryPos.ShowOnPageLoad = isValid;

                RemoveSession(Enums.CommonSession.StayOnFormAndOpenPopup);
            }
        }

        private void FillForm()
        {
            txtInquiryNum.Text = model.PovprasevanjeStevilka;
            //txtStatus.Text = model.StatusPovprasevanja.Naziv;
            GridLookupStatus.Value = model.StatusID;
            GridLookupBuyer.Value = GetInquiryDataProvider().GetBuyerListModel().Where(b => b.NazivPrvi.Trim() == model.KupecNaziv_P.Trim()).FirstOrDefault().TempID;
            txtInquiryName.Text = model.Naziv;

            DateEditSupplyDate.Date = model.DatumPredvideneDobave;

            ASPxGridViewInquiryPosition.DataBind();

            //če je status povpraševanje oddano (ODDANO), potem uporabniku ne dovolimo več dodajanja novih pozicij ali ponovnega pošiljanja ali shranjevanja povpraševanja
            if (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString() || (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.NAROCENO.ToString()) || (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO.ToString()) || (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.USTVARJENO_NAROCILO.ToString()) || (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.PRIPRAVLJENO.ToString()))
                EnableUserControl(false);

            if (PrincipalHelper.IsUserSuperAdmin())
            {
                btnSaveChanges.ClientEnabled = true;
                GridLookupStatus.ClientEnabled = true;
            }
        }

        private void SetFormDefaultValues()
        {
            if (GetInquiryDataProvider().GetInquiryStatuses() != null)
            {
                string status = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString();
                string naziv = GetInquiryDataProvider().GetInquiryStatuses().Where(r => r.Koda == status)
                    .FirstOrDefault().Naziv;
                //txtStatus.Text = naziv;
                GridLookupStatus.Text = naziv;
            }
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = GetInquiryDataProvider().GetInquiryModel() != null ? GetInquiryDataProvider().GetInquiryModel() : new InquiryFullModel();

                model.PovprasevanjeID = 0;
                model.ts = DateTime.Now;
                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            }
            else if (model == null && !add)
            {
                model = GetInquiryDataProvider().GetInquiryModel();
            }

            model.tsUpdateUserID = PrincipalHelper.GetUserPrincipal().ID;

            string sKoda = GetInquiryDataProvider().GetInquiryStatus().ToString();
            model.StatusID = GetInquiryDataProvider().GetInquiryStatuses() != null ? GetInquiryDataProvider().GetInquiryStatuses().Where(ps => ps.Koda == sKoda).FirstOrDefault().StatusPovprasevanjaID : 0;

            if (sKoda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString() && model.EmployeeSubmitInquiry)
            {
                model.PovprasevanjeOddalID = PrincipalHelper.GetUserPrincipal().ID;
                model.DatumOddajePovprasevanja = DateTime.Now;
            }

            if (sKoda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.PRIPRAVLJENO.ToString())
            {
                model.PovprasevanjeOddalID = PrincipalHelper.GetUserPrincipal().ID;
                model.DatumOddajePovprasevanja = DateTime.Now;
            }

            //model.KupecID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupBuyer));
            model.KupecNaziv_P = GridLookupBuyer.Text;

            model.Naziv = txtInquiryName.Text;
            model.DatumPredvideneDobave = DateEditSupplyDate.Date;

            InquiryFullModel returnModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveInquiry(model));

            RemoveSession(Enums.InquirySession.InquiryStatus);

            if (returnModel != null)
            {
                //this we need if we want to add new client and then go and add new Plan with no redirection to Clients page
                model = returnModel;//if we need updated model in the same request;
                GetInquiryDataProvider().SetInquiryModel(model);
                inquiryID = model.PovprasevanjeID;

                return true;
            }
            else
                return false;
        }

        protected void GridLookupBuyer_DataBinding(object sender, EventArgs e)
        {
            var list = CheckModelValidation(GetDatabaseConnectionInstance().GetBuyerList());
            GetInquiryDataProvider().SetBuyerListModel(list);

            (sender as ASPxGridLookup).DataSource = list;

            GridLookupBuyer.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void GridLookupStatus_DataBinding(object sender, EventArgs e)
        {
            var list = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryStatuses());
            GetInquiryDataProvider().SetInquiryStatuses(list);

            (sender as ASPxGridLookup).DataSource = list;

            GridLookupStatus.GridView.Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewInquiryPosition_DataBinding(object sender, EventArgs e)
        {
            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null)
                (sender as ASPxGridView).DataSource = model.PovprasevanjePozicija;
        }

        protected void ASPxGridViewSelectedArtikel_DataBinding(object sender, EventArgs e)
        {
            //model = GetInquiryDataProvider().GetInquiryPositionModel();
            //if (model != null)
            //    (sender as ASPxGridView).DataSource = model.PovprasevanjePozicijaArtikel;

            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null && model.PovprasevanjePozicija.Count > 0)
            {
                List<InquiryPositionArtikelModel> lsPosArtikel = model.PovprasevanjePozicija.Where(t => t.PovprasevanjePozicijaID == selectedInquryID).FirstOrDefault().PovprasevanjePozicijaArtikel;
                (sender as ASPxGridView).DataSource = lsPosArtikel;
                (sender as ASPxGridView).Settings.GridLines = GridLines.Both;
            }
        }

        protected void ASPxGridViewSelectedArtikel_BeforePerformDataSelect(object sender, EventArgs e)
        {
            selectedInquryID = CommonMethods.ParseInt((sender as ASPxGridView).GetMasterRowKeyValue());

        }


        protected void ASPxGridViewInquiryPosition_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

        }

        #region Helper methods

        private bool DeleteObject()
        {
            return CheckModelValidation(GetDatabaseConnectionInstance().DeleteInquiry(inquiryID));
        }

        private void ProcessUserAction(bool stayOnForm = false)
        {
            bool isValid = false;
            bool isDeleteing = false;

            switch (action)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    isDeleteing = true;
                    break;
            }

            if (isValid)
            {
                ClearSessionsAndRedirect(isDeleteing, stayOnForm);
            }
        }
        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool stayOnForm = false, bool submitInquiry = false, bool bNotSentPDFAndEmailToSupplier = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = inquiryID.ToString() }
            };

            /*else*/
            if (isIDDeleted)
                redirectString = "InquiryTable.aspx";
            else
                redirectString = GenerateURI("InquiryTable.aspx", queryStrings);

            if (model != null)
            {
                if (bNotSentPDFAndEmailToSupplier)
                {
                    queryStrings.Add(new QueryStrings { Attribute = "NotSendPDFAndEmailsToSupplier", Value = "1" });
                    redirectString = GenerateURI("InquiryTable.aspx", queryStrings);
                }
            }

            if (stayOnForm)
            {
                queryStrings.Add(new QueryStrings { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                redirectString = GenerateURI("InquiryForm.aspx", queryStrings);
            }

            if (submitInquiry)
            {//prikažemo pojavno okno o uspešnem oddanem/poslanem povpraševanju
                queryStrings.Add(new QueryStrings { Attribute = Enums.QueryStringName.submitInquiry.ToString(), Value = "1" });
                redirectString = GenerateURI("InquiryTable.aspx", queryStrings);
            }

            model = GetInquiryDataProvider().GetInquiryModel();



            //RemoveSession(Enums.RecallSession.);
            RemoveSession(Enums.CommonSession.UserActionPopUp);

            List<Enums.InquirySession> list = Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList();
            ClearAllSessions(list, redirectString, IsCallback);
        }

        private bool HasSessionModelStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry status)
        {
            if (model == null) return false;

            if (GetInquiryDataProvider().GetInquiryStatuses() != null)
            {
                string statusOdp = status.ToString();
                int statusID = GetInquiryDataProvider().GetInquiryStatuses().Where(os => os.Koda == statusOdp).FirstOrDefault().StatusPovprasevanjaID;

                return (model.StatusID == statusID);
            }

            return false;
        }
        #endregion

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            model.StatusID = CommonMethods.ParseInt(GetGridLookupValue(GridLookupStatus));

            if (action == (int)Enums.UserAction.Add)
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA);
            else
            { //ko shranjujemo odpoklic ko ni več v delovni verziji
                string code = GetInquiryDataProvider().GetInquiryStatuses().Where(rs => rs.StatusPovprasevanjaID == model.StatusID).FirstOrDefault().Koda;
                DatabaseWebService.Common.Enums.Enums.StatusOfInquiry enumValue = DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.NEZNAN;
                Enum.TryParse(code, out enumValue);
                GetInquiryDataProvider().SetInquiryStatus(enumValue);
            }
            ProcessUserAction();
        }

        protected void InquiryCallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            object valueID = null;

            if (action == (int)Enums.UserAction.Add)
            {
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA);
                ProcessUserAction(true);
                //v sejo si zapolnimo vrednost, ki jo ob ponovnem nalaganju strani preverimo in po potrebi odpremo popup
                AddValueToSession(Enums.CommonSession.StayOnFormAndOpenPopup, true);
            }
            else if (e.Parameter == "InquiryPosReturn")
            {
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryByID(inquiryID, false, 0));
                GetInquiryDataProvider().SetInquiryModel(model);
                if (model != null)
                {
                    ASPxGridViewInquiryPosition.DataBind();
                }
            }
            else
            {
                //če je status povpraševanje oddano (ODDANO), potem uporabniku ne dovolimo več dodajanja novih pozicij 
                if ((model != null && model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString()) || (model != null && model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.NAROCENO.ToString()))
                    return;

                if (CommonMethods.ParseInt(e.Parameter) != (int)Enums.UserAction.Add)
                    valueID = ASPxGridViewInquiryPosition.GetRowValues(ASPxGridViewInquiryPosition.FocusedRowIndex, "PovprasevanjePozicijaID");

                bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.InquirySession.InquiryPositionID, valueID);

                PopupControlInquiryPos.ShowOnPageLoad = isValid;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearSessionsAndRedirect();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            var list = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryPositionsGroupedBySupplier(inquiryID));

            if (IsSuppliersEmailValid(list))
            {
                //Generiraj pdf reporte in pošlji potrebne email-e!
                //list = ReportHelper.CreateSubmitInquiryReport(list);
                UpdateSessionValue(list);
                model.EmployeeSubmitInquiry = true;
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO);
                AddOrEditEntityObject();

                ClearSessionsAndRedirect(false, false, true);
            }
        }

        protected void btnFinishInquiry_Click(object sender, EventArgs e)
        {
            ////Generiraj pdf reporte in pošlji potrebne email-e!
            //var list = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryPositionsGroupedBySupplier(inquiryID));

            //if (IsSuppliersEmailValid(list))
            //{
            //    list = ReportHelper.CreateSubmitInquiryReport(list);
            //    UpdateSessionValue(list);
            //    model.EmployeeSubmitInquiry = true;

            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null)
            {
                model.NotSendPDFAndEmailsToSupplier = true;
            }

            GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.PRIPRAVLJENO);
            AddOrEditEntityObject();
            ClearSessionsAndRedirect(false, false, true, true);

        }

        protected void PopupControlInquiryPos_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.InquirySession.InquiryPositionID);
            RemoveSession(Enums.InquirySession.InquiryPositionModel);
            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);
            RemoveSession(Enums.InquirySession.SupplierListModel);
            RemoveSession(Enums.InquirySession.ReturnSupplierVal);
        }

        private void EnableUserControl(bool enable)
        {
            btnSaveChanges.ClientEnabled = enable;
            btnSubmit.ClientEnabled = enable;
            btnAddPos.ClientEnabled = enable;
            btnEditPos.ClientEnabled = enable;
            btnDeletePos.ClientEnabled = enable;
            btnCopyPos.ClientEnabled = enable;
            btnFinish.ClientEnabled = enable;
        }

        private bool IsSuppliersEmailValid(List<GroupedInquiryPositionsBySupplier> list)
        {
            RemoveSession(Enums.InquirySession.SuppliersValidOnSubmittingInquiry);
            string message = "";
            if (list == null)
            {
                message = message.Insert(0, "Ni bilo pridobljenih email naslovov :  <br /><ul>");
                return false;
            }

            foreach (var item in list)
            {

                if (item.SelectedContactPersonsEmails.Length == 0)
                    message += "<li>" + item.Supplier.NazivPrvi + "</li>";

            }

            if (String.IsNullOrEmpty(message))
                return true;
            else
            {
                message = message.Insert(0, "Naslednjim dobaviteljem manjka(-jo) elektronski naslov(-i) :  <br /><ul>");
                message += "</ul>";
                AddValueToSession(Enums.InquirySession.SuppliersValidOnSubmittingInquiry, message);
                return false;
            }
        }

        private void UpdateSessionValue(List<GroupedInquiryPositionsBySupplier> list)
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();
            if (model != null)
            {
                foreach (var item in model.PovprasevanjePozicija)
                {

                    var filePath = (from l in list
                                    where l.Supplier.idStranka == item.DobaviteljID
                                    select l.ReportFilePath).FirstOrDefault();
                    var EmailBody = (from l in list
                                     where l.Supplier.idStranka == item.DobaviteljID
                                     select l.EmailBody).FirstOrDefault();

                    item.PotDokumenta = filePath;
                    item.EmailBody = EmailBody;

                }
            }
        }

        protected void btnCopyPos_Click(object sender, EventArgs e)
        {
            InquiryPositionModel modelPos = null;

            object valueID = ASPxGridViewInquiryPosition.GetRowValues(ASPxGridViewInquiryPosition.FocusedRowIndex, "PovprasevanjePozicijaID");
            if (valueID != null)
            {
                int SelectedID = CommonMethods.ParseInt(valueID);

                modelPos = CheckModelValidation(GetDatabaseConnectionInstance().CopyInquiryPositionByID(SelectedID));

                if (modelPos != null)
                {
                    ClearSessionsAndRedirect(false, true, false);
                }
            }
        }

        protected void ASPxGridViewInquiryPosition_DataBound(object sender, EventArgs e)
        {
            ((ASPxGridView)sender).DetailRows.ExpandAllRows();
            ((ASPxGridView)sender).SettingsDetail.ShowDetailButtons = true;
        }
    }
}