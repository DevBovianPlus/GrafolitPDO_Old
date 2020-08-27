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

namespace GrafolitPDO.Pages.Order
{
    public partial class OrderPos_popup : ServerMasterPage
    {
        int InqueryPosAction = -1;
        int InqueryPosID = 0;
        int InqueryPosArtikelID = 0;
        int selOddelekID = 0;
        bool enablEedit;
        InquiryFullModel model;
        InquiryPositionArtikelModel selArtikel;
        InquiryPositionModel selPosition;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            InqueryPosAction = CommonMethods.ParseInt(GetStringValueFromSession(Enums.CommonSession.UserActionPopUp));
            InqueryPosID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.InquirySession.InquiryPositionID));
            enablEedit = CommonMethods.ParseBool(GetStringValueFromSession(Enums.OrderSession.EnableEdit));
            selOddelekID = CommonMethods.ParseInt(GetStringValueFromSession("OddelekID"));
            InqueryPosArtikelID = CommonMethods.ParseInt(GetStringValueFromSession(Enums.InquirySession.InquiryPositionArtikelID));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (InqueryPosAction == (int)Enums.UserAction.Edit || InqueryPosAction == (int)Enums.UserAction.Delete)
                {
                    if (InqueryPosID > 0)
                    {
                        selArtikel = GetInquiryDataProvider().GetInquiryModel().PovprasevanjePozicijaArtikel.Where(ppa => ppa.PovprasevanjePozicijaArtikelID == InqueryPosArtikelID).FirstOrDefault();
                        if (selArtikel != null)
                        {
                            selPosition = GetInquiryDataProvider().GetInquiryModel().PovprasevanjePozicija.Where(pp => pp.PovprasevanjePozicijaID == selArtikel.PovprasevanjePozicijaID).FirstOrDefault();
                        
                            GetInquiryDataProvider().SetInquiryPositionArtikelModel(selArtikel);
                        }
                        if (selPosition != null)
                        {
                            GetInquiryDataProvider().SetInquiryPositionModel(selPosition);
                        }

                        if (GetInquiryDataProvider().GetInquiryModel() != null)
                            model = GetInquiryDataProvider().GetInquiryModel();
                        else
                        {
                            //model = GetOrderDataProvider().GetOrderModel().NarociloPozicija_PDO.Where(np => np.NarociloPozicijaID == InqueryPosID).FirstOrDefault();
                        }

                        if (model != null)
                        {
                            GetInquiryDataProvider().SetInquiryModel(model);
                            FillForm();
                            btnSearch.ClientEnabled = true;
                        }

                        if (SessionHasValue("PageName"))
                        {
                           string sPageName = GetValueFromSession("PageName").ToString();
                            if (sPageName == "Order/OrderTable.aspx")
                            {
                                btnConfirm.ClientEnabled = false;
                            }
                        }
                    }
                }
                /*else if (inquiryPosAction == (int)Enums.UserAction.Add)
                {
                    SetFromDefaultValues();
                }*/
                UserActionConfirmBtnUpdate(btnConfirm, InqueryPosAction, true);
            }
        }

        private void FillForm()
        {

            ComboBoxOddelek.DataBind();



            if (selArtikel != null)
            {
                txtName.Text = selArtikel.Naziv;
                txtDobavitellj.Text = selArtikel.DobaviteljNaziv_PA != null ? selArtikel.DobaviteljNaziv_PA : "";
                txtProductSearch.Text = (selArtikel.IzbraniArtikelNaziv_P != null) ? selArtikel.IzbraniArtikelNaziv_P : selArtikel.Naziv;
                txtOrderQ.Text = (selArtikel.KolicinavKG > 0)  ? selArtikel.KolicinavKG.ToString() : selArtikel.Kolicina1.ToString();
                if (selArtikel.ArtikelCena > 0)
                    txtPrice.Text = selArtikel.ArtikelCena.ToString("N3");

                txtQuantity1.Text = selArtikel.Kolicina1.ToString("N2");
                txtUnitOfMeasure1.Text = selArtikel.EnotaMere1;
                txtQuantity2.Text = selArtikel.Kolicina2.ToString("N2");
                txtUnitOfMeasure2.Text = selArtikel.EnotaMere2;
                memOpombaNarocilnica.Text = selArtikel.Opombe; 


                ComboBoxOddelek.SelectedIndex = model.OddelekID > 0 ? ComboBoxOddelek.Items.IndexOfValue(model.OddelekID.ToString()) : 0;
                memOpombaNarocilnica.Text = MemoNotes.Text = selArtikel.OpombaNarocilnica;
                MemoNotes.Text = selArtikel.OpombaNarocilnica;
                selArtikel.EnotaMere = (selArtikel.EnotaMere == null) ? "kg" : selArtikel.EnotaMere;
                txtEnotaMere.Text = selArtikel.EnotaMere;

                DateEditSupplyDate.Value = (selArtikel.DatumDobavePos != null) ? selArtikel.DatumDobavePos.ToString() : "";
            }

           
        }

       

        private bool AddOrEditEntityObject(bool add = false)
        {
            model = GetInquiryDataProvider().GetInquiryModel();

            model.tsIDOsebe = PrincipalHelper.GetUserPrincipal().ID;
            model.tsUpdateUserID = PrincipalHelper.GetUserPrincipal().ID;
            selArtikel = GetInquiryDataProvider().GetInquiryPositionArtikelModel();
            if (selArtikel != null)
            {                
                var selectedProduct = GetOrderDataProvider().GetSelectedSearchedProduct();
                if (selectedProduct != null)
                {
                    selArtikel.IzbraniArtikelNaziv_P = selectedProduct.Naziv;
                    selArtikel.IzbraniArtikelIdent_P = selectedProduct.StevilkaArtikel;
                }

                selArtikel.ArtikelCena = CommonMethods.ParseDecimal(txtPrice.Text);
                selArtikel.KolicinavKG = CommonMethods.ParseDecimal(txtOrderQ.Text);
                selArtikel.EnotaMere = txtEnotaMere.Text;
                selArtikel.Rabat = CommonMethods.ParseDecimal(txtRabate.Text) > 0 ? CommonMethods.ParseDecimal(txtRabate.Text) : 0;
                selArtikel.PrikaziKupca = CheckBoxPrikaziKupca.Checked;

                selArtikel.OddelekID = CommonMethods.ParseInt(ComboBoxOddelek.Value);
                selArtikel.Opombe = MemoNotes.Text;
                selArtikel.OpombaNarocilnica = memOpombaNarocilnica.Text;
                selArtikel.PrikaziKupca = CheckBoxPrikaziKupca.Checked;
                selArtikel.DatumDobavePos = Convert.ToDateTime (DateEditSupplyDate.Value);

                selArtikel.DobaviteljNaziv_PA = txtDobavitellj.Text;
                ClientFullModel Supplier = CheckModelValidation(GetDatabaseConnectionInstance().GetClientByNameOrInsert(selArtikel.DobaviteljNaziv_PA));
                if (Supplier != null)
                {
                    selArtikel.Dobavitelj = Supplier;
                    selArtikel.DobaviteljID = Supplier.idStranka;
                }
            }

            if (model != null)
            {
                GetInquiryDataProvider().SetInquiryModel(model);
                model = CheckModelValidation(GetDatabaseConnectionInstance().SaveInquiryPurchase(model));
            }

                return true;

        }




        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.UserActionPopUp);
            RemoveSession(Enums.OrderSession.OrderPositionID);
            RemoveSession(Enums.OrderSession.OrderPositionModel);
            RemoveSession(Enums.OrderSession.EnableEdit);
            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);
            RemoveSession(Enums.OrderSession.SelectedSearchedProduct);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "OrderPos"), true);
        }

        private ClientFullModel GetSelectedSupplier(int id)
        {
            if (model == null)
                model = GetInquiryDataProvider().GetInquiryModel();

            if (selPosition.Dobavitelj != null)
                return selPosition.Dobavitelj;

            return null;
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            ProcessUserAction();
        }

        private bool DeleteObject()
        {
            return false;//CheckModelValidation(GetDatabaseConnectionInstance().DeleteInquiryPosition(inquiryPosID));
        }

        private void ProcessUserAction()
        {
            bool isValid = false;
            bool confirm = false;

            switch (InqueryPosAction)
            {
                case (int)Enums.UserAction.Add:
                    isValid = AddOrEditEntityObject(true);
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Edit:
                    isValid = AddOrEditEntityObject();
                    confirm = true;
                    break;
                case (int)Enums.UserAction.Delete:
                    isValid = DeleteObject();
                    confirm = true;
                    break;
            }

            if (isValid)
            {
                RemoveSessionsAndClosePopUP(confirm);
            }
            else
                ShowClientPopUp("Something went wrong. Contact administrator", 1);
        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "StartSearchPopup")
            {
                AddValueToSession(Enums.CommonSession.SearchString, txtProductSearch.Text.Trim());
                AddValueToSession(Enums.InquirySession.SelectedSupplierPopup, txtDobavitellj.Text.Trim());
                PopupControlSearchProduct.ShowOnPageLoad = true;
            }
            else if (e.Parameter == "ProductSelected")
            {
                var selectedProduct = GetOrderDataProvider().GetSelectedSearchedProduct();

                if (selectedProduct != null)
                {
                    txtProductSearch.Text = selectedProduct.Naziv;
                    txtDobavitellj.Text = selectedProduct.Dobavitelj;
                }
            }
            else if (e.Parameter == "SupplierSelected")
            {
                string sSupplier = GetStringValueFromSession(Enums.InquirySession.ReturnSupplierVal);
                List<ClientSimpleModel> model;
                txtDobavitellj.Text = sSupplier;
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetSupplierByName(sSupplier.Trim()));
                if (model.Count == 1)
                {
                    AddSupplierToForm(model[0]);
                }
            }
            else if (e.Parameter == "StartSearchSupplierPopup")
            {
                AddValueToSession(Enums.CommonSession.SearchString, txtDobavitellj.Text.Trim());

                List<ClientSimpleModel> model;
                model = CheckModelValidation(GetDatabaseConnectionInstance().GetSupplierByName(txtDobavitellj.Text.Trim()));

                if (model.Count > 1)
                {
                    PopupControlSearchSupplier.ShowOnPageLoad = true;
                }
                else if (model.Count == 1)
                {
                    AddSupplierToForm(model[0]);
                    //GridLookupKontaktnaOSeba.DataBind();
                }
                else if (model.Count == 0)
                {
                    PopupControlSearchSupplier.ShowOnPageLoad = true;
                }
            }
        }

        private void AddSupplierToForm(ClientSimpleModel cSupplier)
        {
            ClientFullModel Supplier = CheckModelValidation(GetDatabaseConnectionInstance().GetClientByNameOrInsert(cSupplier.NazivPrvi));

            selArtikel = GetInquiryDataProvider().GetInquiryPositionArtikelModel();
            
            if (selArtikel != null)
            {
                selArtikel.Dobavitelj = Supplier;
                selArtikel.IzbranDobaviteljID = Supplier.idStranka;                
                txtDobavitellj.Text = Supplier.NazivPrvi;                                
            }
            
        }

        protected void PopupControlSearchProduct_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.OrderSession.ProductListModel);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);
        }

        protected void PopupControlSearchSupplier_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);
            RemoveSession(Enums.InquirySession.SupplierListModel);
        }
        protected void ComboBoxOddelek_DataBinding(object sender, EventArgs e)
        {
            List<DepartmentModel> types = CheckModelValidation(GetDatabaseConnectionInstance().GetDepartments());
            (sender as ASPxComboBox).DataSource = SerializeToDataTable(types, "OddelekID", "Naziv");
        }
    }
}