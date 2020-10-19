using DatabaseWebService.ModelsPDO;
using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Order;
using DevExpress.Web;
using GrafolitPDO.Common;
using GrafolitPDO.Helpers;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrafolitPDO.Pages.Order
{
    public partial class OrderForm : ServerMasterPage
    {
        int action = -1;
        int inquiryID = -1;
        int iSelDobaviteljID = 0;
        OrderPDOFullModel OrderModel;
        InquiryFullModel model = null;
        string sPageName = "";
        bool bIsSelectedArtikel = true;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            this.Master.PageHeadlineTitle = Title;

            if (Request.QueryString[Enums.QueryStringName.recordId.ToString()] != null)
            {
                action = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.action.ToString()].ToString());
                inquiryID = CommonMethods.ParseInt(Request.QueryString[Enums.QueryStringName.recordId.ToString()].ToString());
            }

            if (SessionHasValue("PageName"))
            {
                sPageName = GetValueFromSession("PageName").ToString();
            }

            if (SessionHasValue(Enums.OrderSession.DobaviteljID))
            {
                iSelDobaviteljID = CommonMethods.ParseInt(GetValueFromSession(Enums.OrderSession.DobaviteljID));
            }

            this.Master.DisableNavBar = true;

            ASPxGridViewOrder.Settings.GridLines = GridLines.Both;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
                if (action == (int)Enums.UserAction.Edit || action == (int)Enums.UserAction.Delete)
                {
                    EnableUserControls(false);//onemogočimo spreminjanje podatkov ali ponovno pošiljanje naročila.
                    if (inquiryID > 0)
                    {
                        if (GetOrderDataProvider().GetOrderModel() != null)
                            //model = GetOrderDataProvider().GetOrderModel();
                            model = GetInquiryDataProvider().GetInquiryModel();
                        else
                        {
                            //model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderByID(inquiryIDOrOrderID));
                            //bIsPurchase = (sPageName == "PurchaseTable.aspx") ? true : false;
                            model = CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryByID(inquiryID, bIsSelectedArtikel, iSelDobaviteljID));
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
                    model = CheckModelValidation(GetDatabaseConnectionInstance().GetOrderByInquiryIDForNewOrder(inquiryID));

                    if (model != null)
                    {
                        model.PovprasevanjeID = inquiryID;
                        GetInquiryDataProvider().SetInquiryModel(model);
                        FillForm();
                    }
                }
                UserActionConfirmBtnUpdate(btnSaveChanges, action);

            }
            else
            {
                if (model == null && SessionHasValue(Enums.InquirySession.InquiryModel))
                    model = GetInquiryDataProvider().GetInquiryModel();
            }
        }

        private void Initialize()
        {
            GetOrderDataProvider().SetOrderStatuses(CheckModelValidation(GetDatabaseConnectionInstance().GetInquiryStatuses()));
            ComboBoxOddelek.DataBind();
        }

        private void SetFromDefaultValues()
        {
            ComboBoxOddelek.SelectedIndex = ComboBoxOddelek.Items.IndexOfValue("-1");
        }

        private void FillForm()
        {


            if (model.OpombeNarocilnica == null)
            {
                model.OpombeNarocilnica = "Please send us an order confirmation.";
            }

            if (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO.ToString())
            {
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO);
                ASPxGridViewOrder.Columns[0].Visible = false;
                ASPxGridViewOrder.Columns[1].Visible = false;
                btnCheckArtikels.ClientVisible = false;
                btnEditPos.ClientEnabled = true;
                btnCreateOrder.ClientVisible = true;
                //ASPxGridViewOrder.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow;

                //ASPxGridViewOrder.Columns["KolicinavKG"].Visible = true;

                //ASPxGridViewOrder.Columns["NabCena"].Visible = true;
                //ASPxGridViewOrder.Columns["EnotaM"].Visible = true;                
                //ASPxGridViewOrder.Columns["KolicinavKG"].Visible = false;
                //ASPxGridViewOrder.Columns["EnotaMere"].Visible = false;
                //ASPxGridViewOrder.Columns["KolicinaVPOL"].Visible = false;
                //ASPxGridViewOrder.Columns["KolicinaVPOL"].Visible = false;
                //ASPxGridViewOrder.Columns["Cena"].Visible = false;
            }

            if (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.USTVARJENO_NAROCILO.ToString())
            {
                ASPxGridViewOrder.SettingsEditing.Mode = GridViewEditingMode.Inline;
                ASPxGridViewOrder.Columns[0].Visible = false;
                ASPxGridViewOrder.Columns[1].Visible = false;
                ASPxGridViewOrder.Columns["KolicinavKG"].Visible = true;
                //ASPxGridViewOrder.Columns["NabCena"].Visible = true;
                //ASPxGridViewOrder.Columns["EnotaM"].Visible = true;
                //ASPxGridViewOrder.Columns["Rabat"].Visible = true;
                //ASPxGridViewOrder.Columns["Kol1"].Visible = false;
                //ASPxGridViewOrder.Columns["EM1"].Visible = false;
                //ASPxGridViewOrder.Columns["Kol2"].Visible = false;
                //ASPxGridViewOrder.Columns["EM2"].Visible = false;
                //ASPxGridViewOrder.Columns["Cena"].Visible = false;
            }

            DateEditSupplyDate.Date = model.DatumPredvideneDobave;
            MemoNotes.Text = model.OpombeNarocilnica;

            if (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString())
            {
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO);
            }

            ComboBoxOddelek.SelectedIndex = model.OddelekID > 0 ? ComboBoxOddelek.Items.IndexOfValue(model.OddelekID.ToString()) : 0;

            ASPxGridViewOrder.DataBind();

            if (SessionHasValue("PageName"))
            {
                sPageName = GetValueFromSession("PageName").ToString();
                if (sPageName == "Order/OrderTable.aspx")
                {
                    ASPxGridViewOrder.Selection.SelectAll();
                }
            }
        }

        private bool AddOrEditEntityObject(bool add = false)
        {
            if (add)
            {
                model = GetInquiryDataProvider().GetInquiryModel() != null ? GetInquiryDataProvider().GetInquiryModel() : new InquiryFullModel();

                model.NarociloID = 0;
                model.ts = DateTime.Now;
                model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;

                //if (model.PovprasevanjePozicija != null)//pri dodajanju novega naročila iz povpraševanja imamo v začetku nastavljene začasne id-je. Zato jih je potrebno ob shranjevanju nastavit na 0, da dobijo sql-ove id-je
                //    //model.NarociloPozicija_PDO.ForEach(poz => poz.NarociloPozicijaID = 0);
            }
            else if (model == null && !add)
            {
                model = GetInquiryDataProvider().GetInquiryModel();
            }

            model.tsUpdateUserID = PrincipalHelper.GetUserPrincipal().ID;

            string sKoda = GetInquiryDataProvider().GetInquiryStatus().ToString();
            //model.PovprasevanjeStatusID = GetOrderDataProvider().GetOrderStatuses() != null ? GetOrderDataProvider().GetOrderStatuses().Where(ps => ps.Koda == sKoda).FirstOrDefault().StatusPovprasevanjaID : 0;
            model.StatusID = GetOrderDataProvider().GetOrderStatuses() != null ? GetOrderDataProvider().GetOrderStatuses().Where(ps => ps.Koda == sKoda).FirstOrDefault().StatusPovprasevanjaID : 0;
            model.StatusPovprasevanja = GetOrderDataProvider().GetOrderStatuses().Where(ps => ps.Koda == sKoda).FirstOrDefault();

            model.OpombeNarocilnica = MemoNotes.Text;
            model.DatumPredvideneDobave = DateEditSupplyDate.Date;
            model.OddelekID = CommonMethods.ParseInt(ComboBoxOddelek.Value);

            InquiryFullModel returnModel = null;


            returnModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveInquiryPurchase(model));


            //CheckModelValidation(returnModel);
            //if (returnModel.IsRequestSuccesful) RemoveSessionsAndClosePopUP(true);

            RemoveSession(Enums.OrderSession.OrderStatus);

            if (returnModel != null)
            {
                //this we need if we want to add new client and then go and add new Plan with no redirection to Clients page
                model = returnModel;//if we need updated model in the same request;
                GetInquiryDataProvider().SetInquiryModel(returnModel);

                return true;
            }
            else
                return false;
        }

        protected void ASPxGridViewOrder_DataBinding(object sender, EventArgs e)
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();

            (sender as ASPxGridView).DataSource = model.PovprasevanjePozicijaArtikel;
        }

        protected void ASPxGridViewOrder_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
            if (e.Column.FieldName == "IzbraniArtikelNaziv_P")
            {
                ASPxComboBox cmdPantheonArtikli = e.Editor as ASPxComboBox;
                //cmdPantheonArtikli.Callback += cmdPantheonArtikli_OnCallback;

                int pozicijaArtiklaID = CommonMethods.ParseInt(e.KeyValue);

                if (e.KeyValue != DBNull.Value && e.KeyValue != null)
                {
                    FillPArtikliCombo(cmdPantheonArtikli, pozicijaArtiklaID);
                }
                else
                {
                    cmdPantheonArtikli.DataSourceID = null;
                    cmdPantheonArtikli.Items.Clear();
                }
            }
            else
            {
                e.Editor.Enabled = false;
            }
        }

        /*void cmdPantheonArtikli_OnCallback(object source, CallbackEventArgsBase e)
        {
            FillPArtikliCombo(source as ASPxComboBox, e.Parameter, "");
        }*/

        protected void FillPArtikliCombo(ASPxComboBox cmb, int pozicijaArtikla)
        {
            if (pozicijaArtikla <= 0) return;

            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null && model.PovprasevanjePozicijaArtikel != null)
            {
                var productsPantheon = model.PovprasevanjePozicijaArtikel.Where(poz => poz.PovprasevanjePozicijaArtikelID == pozicijaArtikla).FirstOrDefault();

                if (productsPantheon.EnotaMere1.ToString().ToUpper() == "POL")
                {
                    hlpCalculateWeight hw = CommonMethods.GetCalculateWeight(productsPantheon.IzbraniArtikelNaziv_P);
                    decimal calc = CommonMethods.CalculateSheetInKg(hw, CommonMethods.ParseDecimal(productsPantheon.Kolicina1));
                    if (calc > 0)
                    {
                        productsPantheon.EnotaMere1 = "KG";
                        productsPantheon.Kolicina2 = productsPantheon.Kolicina1;

                        productsPantheon.Kolicina1 = Convert.ToDecimal(calc);
                        productsPantheon.EnotaMere2 = "POL";
                    }
                }
                cmb.DataSourceID = null;
                cmb.DataSource = productsPantheon.ArtikliPantheon;
                cmb.DataBindItems();
            }
        }


        protected void ASPxGridViewOrder_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            //if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.ODDANO.ToString())
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dff0d8");//#
            //else if (e.GetValue("StatusPovprasevanja.Koda").ToString() == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.DELOVNA.ToString())
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFA879");//#fcf8e3

            //if (CommonMethods.ParseBool(e.GetValue("Zakleni")))
            //    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#f2dede");
            if (model.StatusPovprasevanja.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO.ToString())
            {
                string sIzbranArtikel = (e.GetValue("IzbraniArtikelNaziv_P") != null) ? e.GetValue("IzbraniArtikelNaziv_P").ToString() : "";
                decimal dKolicinavKG = (e.GetValue("KolicinavKG") != null) ? CommonMethods.ParseDecimal(e.GetValue("KolicinavKG")) : 0;
                if ((sIzbranArtikel.ToString().Length == 0) || (dKolicinavKG == 0))
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe6e6");
            }
        }

        protected void ComboBoxOddelek_DataBinding(object sender, EventArgs e)
        {
            List<DepartmentModel> types = CheckModelValidation(GetDatabaseConnectionInstance().GetDepartments());
            (sender as ASPxComboBox).DataSource = SerializeToDataTable(types, "OddelekID", "Naziv");
        }

        private void SetKolicineOnSelectedArtikles(List<object> selectedRows)
        {
            foreach (Int32 idS in selectedRows)
            {
                InquiryPositionArtikelModel selArtikel = GetInquiryDataProvider().GetInquiryModel().PovprasevanjePozicijaArtikel.Where(ppa => ppa.PovprasevanjePozicijaArtikelID == idS).FirstOrDefault();

                if (selArtikel != null)
                {
                    //selArtikel.KolicinavKG = selArtikel.Kolicina1;
                    //selArtikel.EnotaMere = selArtikel.EnotaMere1;
                    selArtikel.IzbranArtikel = true;
                }
            }

            GetInquiryDataProvider().SetInquiryModel(model);


        }

        private void SetSupllyDateToArtikles()
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();
            DateTime dtGlobalSupllyDate = Convert.ToDateTime(DateEditSupplyDate.Value);
            foreach (var item in model.PovprasevanjePozicijaArtikel)
            {
                if (item.DatumDobavePos != null && item.DatumDobavePos != dtGlobalSupllyDate)
                    item.DatumDobavePos = dtGlobalSupllyDate;
            }

            GetInquiryDataProvider().SetInquiryModel(model);


        }

        protected void OrderCallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            object valueID = null;
            if (e.Parameter == "SubmitOrderToSupplierDep")
            {


                bool isUnlocked = true;
                //if (action == (int)Enums.UserAction.Add)
                //    isUnlocked = CheckModelValidation(GetDatabaseConnectionInstance().UnLockInquiry(inquiryIDOrOrderID, PrincipalHelper.GetUserPrincipal().ID));

                //PrincipalHelper.GetUserPrincipal().LockedInquiryByUser = 0;
                AddValueToSession("PageName", "Order/PurchaseTable.aspx");
                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.POSLANO_V_NABAVO);

                List<object> selectedRows = ASPxGridViewOrder.GetSelectedFieldValues("PovprasevanjePozicijaArtikelID");
                if (selectedRows.Count == 0)
                {
                    OrderCallbackPanel.JSProperties["cpNoSelectedValidationError"] = true;
                    return;
                }

                // set Količine za izbrane artikle
                SetKolicineOnSelectedArtikles(selectedRows);

                AddOrEditEntityObject(true);

                if (isUnlocked)
                    ClearSessionsAndRedirect(false, false, true);
            }
            else if (e.Parameter == "CreateOrderInPanheon")
            {
                // preveri če so izbrani vsi artikli in so vnesene nabavne cene
                bool b = CheckPantheonArtickelAndPrices();

                if (!b)
                {
                    OrderCallbackPanel.JSProperties["cpOrderPositionValidationError"] = true;
                    return;
                }



                GetInquiryDataProvider().SetInquiryStatus(DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.USTVARJENO_NAROCILO);
                model.StatusID = GetOrderDataProvider().GetOrderStatuses().Where(ps => ps.Koda == DatabaseWebService.Common.Enums.Enums.StatusOfInquiry.USTVARJENO_NAROCILO.ToString()).FirstOrDefault().StatusPovprasevanjaID;
                AddOrEditEntityObject(false);
                GetInquiryDataProvider().SetInquiryModel(model);
                OrderModel = CreateOrder();
                OrderModel = CheckModelValidation(GetDatabaseConnectionInstance().SaveOrder(OrderModel));
                btnSendToOrderDep.ClientVisible = true;

                AddValueToSession("PageName", "Order/OrderTable.aspx");
                ClearSessionsAndRedirect(true, false, true);
            }
            else if (e.Parameter == "CheckArtikels")
            {

                CheckArtikelsWithPantheon();
                model = CheckModelValidation(GetDatabaseConnectionInstance().CheckPantheonArtikles(model));
                GetInquiryDataProvider().SetInquiryModel(model);
                CheckIfPolePozicija();
                ASPxGridViewOrder.DataBind();
                btnSendToOrderDep.ClientVisible = true;

                //Nastavimo prvi zapis na urejanje kateri ima več zadtekov iz pantheona
                int id = GetFirstIDWithMultiplePantheonArtikels();
                if (id > 0)
                    ASPxGridViewOrder.StartEdit(ASPxGridViewOrder.FindVisibleIndexByKeyValue(id));
            }
            else if (e.Parameter == "SupplyDateChanged")
            {

                SetSupllyDateToArtikles();
                ASPxGridViewOrder.DataBind();

            }
            else if (e.Parameter == "RefreshOrderPositions")
            {
                ASPxGridViewOrder.DataBind();
            }
            else
            {
                //if (action == (int)Enums.UserAction.Add)
                //{
                //    AddValueToSession(Enums.OrderSession.EnableEdit, true);
                //}
                //else
                //{
                AddOrEditEntityObject(false);
                //if (CommonMethods.ParseInt(e.Parameter) != (int)Enums.UserAction.Add)
                valueID = ASPxGridViewOrder.GetRowValues(ASPxGridViewOrder.FocusedRowIndex, "PovprasevanjePozicijaArtikelID");

                AddValueToSession(Enums.InquirySession.InquiryPositionArtikelID, valueID);
                //model.OddelekID = ComboBoxOddelek.SelectedItem != null ? CommonMethods.ParseInt(ComboBoxOddelek.SelectedItem.Value) : 0;
                //GetOrderDataProvider().SetOrderModel(model);

                bool isValid = SetSessionsAndOpenPopUp(e.Parameter, Enums.InquirySession.InquiryPositionID, valueID);

                PopupControlOrderPos.ShowOnPageLoad = isValid;
                //}
            }
        }

        private bool CheckPantheonArtickelAndPrices()
        {
            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null)
            {
                int iNiArtikla = model.PovprasevanjePozicijaArtikel.Where(ppa => ppa.IzbraniArtikelIdent_P == null).Count();
                int iNiKolicine = model.PovprasevanjePozicijaArtikel.Where(ppa => ppa.KolicinavKG == 0).Count();

                if (iNiArtikla > 0 || iNiKolicine > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private OrderPDOFullModel CreateOrder()
        {
            model = GetInquiryDataProvider().GetInquiryModel();
            OrderModel = new OrderPDOFullModel();
            if (model != null)
            {
                OrderModel.Oddelek = model.Oddelek;
                OrderModel.PovprasevanjeStatusID = model.StatusID;
                OrderModel.PovprasevanjeID = model.PovprasevanjeID;
                OrderModel.OddelekID = model.OddelekID;
                OrderModel.tsUpdateUserID = model.tsUpdateUserID;
                OrderModel.tsUpdate = model.tsUpdate;
                OrderModel.OddelekNaziv = model.OddelekNaziv;
                OrderModel.Opombe = model.OpombeNarocilnica;
                OrderModel.DatumDobave = model.DatumPredvideneDobave;


                OrderModel.NarociloPozicija_PDO = GetOrderPositions(OrderModel);


            }

            return OrderModel;
        }

        private List<OrderPDOPositionModel> GetOrderPositions(OrderPDOFullModel OrderModel)
        {
            model = GetInquiryDataProvider().GetInquiryModel();
            List<OrderPDOPositionModel> listPosition = new List<OrderPDOPositionModel>();
            if (model != null)
            {
                foreach (var inqPos in model.PovprasevanjePozicijaArtikel)
                {
                    OrderPDOPositionModel pos = new OrderPDOPositionModel();
                    pos.NarociloID = 0;
                    pos.IzbranDobaviteljID = inqPos.IzbranDobaviteljID;
                    pos.IzbraniArtikelNaziv_P = inqPos.IzbraniArtikelNaziv_P;
                    pos.IzbraniArtikelIdent_P = inqPos.IzbraniArtikelIdent_P;
                    pos.ArtikelCena = inqPos.ArtikelCena;
                    pos.DatumDobave = OrderModel.DatumDobave;
                    pos.PovprasevanjePozicijaID = inqPos.PovprasevanjePozicijaID;
                    pos.Opombe = inqPos.Opombe;
                    pos.ts = inqPos.ts;
                    pos.tsIDOsebe = inqPos.tsIDOsebe;
                    pos.tsUpdate = inqPos.tsUpdate;
                    pos.tsUpdateUserID = inqPos.tsUpdateUserID;
                    pos.KolicinavKG = inqPos.KolicinavKG;
                    pos.KolicinaVPOL = inqPos.KolicinaVPOL;
                    pos.NarEnotaMere2 = inqPos.NarEnotaMere2;
                    pos.Rabat = inqPos.Rabat;
                    pos.OddelekID = inqPos.OddelekID;
                    pos.PrikaziKupca = inqPos.PrikaziKupca;
                    pos.Dobavitelj = inqPos.Dobavitelj;
                    pos.OpombaNarocilnica = inqPos.OpombaNarocilnica;
                    pos.DatumDobavePos = inqPos.DatumDobavePos;
                    pos.DatumDobave = inqPos.DatumDobavePos;

                    listPosition.Add(pos);
                }
            }

            return listPosition;
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {

        }

        private void CheckArtikelsWithPantheon()
        {
            List<object> selectedRows = ASPxGridViewOrder.GetSelectedFieldValues("PovprasevanjePozicijaArtikelID");
            ClearSelectedPozicija();

            foreach (var item in selectedRows)
            {
                int id = CommonMethods.ParseInt(item);
                SetSelectedPozicija(id);
            }


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            bool isUnlocked = true;
            if (action == (int)Enums.UserAction.Add)
                isUnlocked = CheckModelValidation(GetDatabaseConnectionInstance().UnLockInquiry(inquiryID, PrincipalHelper.GetUserPrincipal().ID));

            //PrincipalHelper.GetUserPrincipal().LockedInquiryByUser = 0;

            if (isUnlocked)
                ClearSessionsAndRedirect();
        }

        protected void PopupControlOrderPos_WindowCallback(object source, DevExpress.Web.PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.OrderPositionID);
            RemoveSession(Enums.OrderSession.EnableEdit);
            RemoveSession(Enums.OrderSession.OrderPositionModel);
            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);
            RemoveSession(Enums.OrderSession.SelectedSearchedProduct);
        }

        //public bool IsValidOrderPositions()
        //{
        //    if (model == null)
        //        model = GetOrderDataProvider().GetOrderModel();

        //    foreach (var item in model.NarociloPozicija_PDO)
        //    {
        //        if (String.IsNullOrEmpty(item.IzbraniArtikelNaziv_P) || item.IzbranDobaviteljID <= 0 || item.ArtikelCena <= 0)
        //            return false;
        //    }

        //    return true;
        //}

        public void ClearSelectedPozicija()
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();
            if (model.PovprasevanjePozicijaArtikel != null)
            {
                foreach (var item in model.PovprasevanjePozicijaArtikel)
                {
                    item.IzbranArtikel = false;
                }

                GetInquiryDataProvider().SetInquiryModel(model);
            }
        }

        public void CheckIfPolePozicija()
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();

            foreach (var item in model.PovprasevanjePozicijaArtikel)
            {
                if (item.EnotaMere != null)
                {
                    if (item.EnotaMere.ToString().ToUpper() == "POL")
                    {
                        hlpCalculateWeight hw = CommonMethods.GetCalculateWeight(item.IzbraniArtikelNaziv_P);

                        decimal calc = CommonMethods.CalculateSheetInKg(hw, CommonMethods.ParseDecimal(item.KolicinavKG));

                        if (calc == 0) continue;

                        item.EnotaMere = "KG";
                        item.KolicinavKG = Convert.ToDecimal(calc);

                        item.KolicinaVPOL = item.Kolicina1;
                        item.NarEnotaMere2 = "POL";
                    }
                }
            }

            GetInquiryDataProvider().SetInquiryModel(model);
        }




        public void SetSelectedPozicija(int PozicijaID)
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();

            foreach (var item in model.PovprasevanjePozicijaArtikel)
            {
                //if (item.EnotaMere1.ToString().ToUpper() == "POL")
                //{
                //    item.EnotaMere1 = "KG";
                //    item.Kolicina2 = item.Kolicina1;
                //    Decimal v1 = Decimal.Multiply(0.13M, 0.72M);
                //    Decimal v2 = Decimal.Multiply(v1, 1.02M);
                //    item.Kolicina1 = Decimal.Multiply(v2, Convert.ToDecimal(item.Kolicina1));
                //    item.EnotaMere2 = "POL";

                //}

                if (item.PovprasevanjePozicijaArtikelID == PozicijaID)
                {
                    item.IzbranArtikel = true;
                    break;
                }


            }

            GetInquiryDataProvider().SetInquiryModel(model);
        }

        #region Helper Methods

        private void ClearSessionsAndRedirect(bool isIDDeleted = false, bool stayOnForm = false, bool submitOrder = false)
        {
            string redirectString = "";
            List<QueryStrings> queryStrings = new List<QueryStrings> {
                new QueryStrings() { Attribute = Enums.QueryStringName.recordId.ToString(), Value = inquiryID.ToString() }
            };

            sPageName = SessionHasValue("PageName") ? GetValueFromSession("PageName").ToString() : "PurchaseTable.aspx";


            /*else*/
            if (isIDDeleted)
                redirectString = "../" + sPageName;
            else
                redirectString = GenerateURI("../" + sPageName, queryStrings);

            /* if (stayOnForm)
             {
                 queryStrings.Add(new QueryStrings { Attribute = Enums.QueryStringName.action.ToString(), Value = ((int)Enums.UserAction.Edit).ToString() });
                 redirectString = GenerateURI("InquiryForm.aspx", queryStrings);
             }*/

            if (submitOrder)
            {//prikažemo pojavno okno o uspešnem oddanem/poslanem povpraševanju
                queryStrings.Add(new QueryStrings { Attribute = Enums.QueryStringName.submitOrder.ToString(), Value = "1" });
                redirectString = GenerateURI(redirectString, queryStrings);
                //redirectString = GenerateURI("../Order/PurchaseTable.aspx", queryStrings);
            }

            //RemoveSession(Enums.RecallSession.);
            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.DobaviteljID);
            RemoveSession("PageName");

            List<Enums.InquirySession> list = Enum.GetValues(typeof(Enums.InquirySession)).Cast<Enums.InquirySession>().ToList();
            List<Enums.OrderSession> listOrder = Enum.GetValues(typeof(Enums.OrderSession)).Cast<Enums.OrderSession>().ToList();

            ClearAllSessions(listOrder);

            ClearAllSessions(list, redirectString, IsCallback);
        }

        private void EnabledControlsByPageStart()
        {

        }

        private void EnableUserControls(bool enable)
        {
            btnEditPos.ClientEnabled = enable;
            btnSendToOrderDep.ClientEnabled = enable;
            btnCheckArtikels.ClientEnabled = enable;



        }
        #endregion

        protected void ASPxGridViewOrder_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (e.DataColumn.FieldName == "EnotaMere")
                if (e.CellValue != null)
                {
                    if (e.CellValue.ToString().ToUpper() != "KG")
                    {
                        e.Cell.BackColor = Color.Red;
                    }
                }
        }

        protected void cmdPantheonArtikli_Init(object sender, EventArgs e)
        {
            ASPxComboBox cbx = sender as ASPxComboBox;
            GridViewEditItemTemplateContainer container = cbx.NamingContainer as GridViewEditItemTemplateContainer;
            string dobavitelj = (string)container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "Dobavitelj.NazivPrvi");
            string naziv = (string)container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "Naziv");
            naziv = "LEPENKA";
            cbx.DataSource = CheckModelValidation(GetDatabaseConnectionInstance().GetProductBySupplierAndName(dobavitelj, naziv));
        }

        protected void ASPxGridViewOrder_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;

            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null)
            {
                List<InquiryPositionArtikelModel> updateList = model.PovprasevanjePozicijaArtikel ?? new List<InquiryPositionArtikelModel>();
                InquiryPositionArtikelModel inquiryPosArtikel = null;

                Type myType = typeof(InquiryPositionArtikelModel);
                List<PropertyInfo> myPropInfo = myType.GetProperties().ToList();

                //Spreminjanje zapisov v gridu


                foreach (DictionaryEntry obj in e.Keys)//we set table ID
                {
                    PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                    if (info != null)
                    {
                        inquiryPosArtikel = updateList.Where(ips => ips.PovprasevanjePozicijaArtikelID == (int)obj.Value).FirstOrDefault();
                        break;
                    }
                }

                foreach (DictionaryEntry obj in e.NewValues)
                {
                    PropertyInfo info = myPropInfo.Where(prop => prop.Name.Equals(obj.Key.ToString())).FirstOrDefault();

                    if (info != null)
                    {
                        int tempID = CommonMethods.ParseInt(obj.Value);
                        var artikel_P = inquiryPosArtikel.ArtikliPantheon.Where(ap => ap.TempID == tempID).FirstOrDefault();
                        if (artikel_P != null)
                        {
                            inquiryPosArtikel.IzbraniArtikelNaziv_P = artikel_P.Naziv;
                            inquiryPosArtikel.IzbraniArtikelIdent_P = artikel_P.StevilkaArtikel;

                            // ponastavimo kolekcijo zaradi tega, da gre na drug izdelek pri preverjanju
                            inquiryPosArtikel.ArtikliPantheon = new List<ProductModel> { artikel_P };
                        }
                    }
                }

                model.PovprasevanjePozicijaArtikel = updateList;

                GetInquiryDataProvider().SetInquiryModel(model);

                gridView.CancelEdit();
                e.Cancel = true;
            }
        }

        private int GetFirstIDWithMultiplePantheonArtikels()
        {
            model = GetInquiryDataProvider().GetInquiryModel();

            if (model != null && model.PovprasevanjePozicijaArtikel != null)
            {
                var item = model.PovprasevanjePozicijaArtikel.Where(ppa => ppa.ArtikliPantheon != null && ppa.ArtikliPantheon.Count > 1 && ppa.IzbraniArtikelNaziv_P == null).FirstOrDefault();

                if (item != null)
                    return item.PovprasevanjePozicijaArtikelID;
            }
            return -1;
        }
    }
}