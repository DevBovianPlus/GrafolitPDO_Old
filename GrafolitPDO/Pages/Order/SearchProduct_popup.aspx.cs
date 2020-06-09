using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsPDO.Inquiry;
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
    public partial class SearchProduct_popup : ServerMasterPage
    {
        string searchString = "";
        string sSelectedSupplier = "";
        List<ProductModel> model;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) RedirectHome();

            searchString = GetStringValueFromSession(Enums.CommonSession.SearchString);
            sSelectedSupplier = GetStringValueFromSession(Enums.InquirySession.SelectedSupplierPopup);
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
            model = CheckModelValidation(GetDatabaseConnectionInstance().GetProductBySupplierAndName(sSelectedSupplier, searchString));
            GetOrderDataProvider().SetSearchedProductListModel(model);

            ASPxGridViewProductSearchResult.DataBind();
        }

        #region Helper methods
        private void RemoveSessionsAndClosePopUP(bool confirm = false)
        {
            string confirmCancelAction = "Preklici";

            if (confirm)
                confirmCancelAction = "Potrdi";

            RemoveSession(Enums.CommonSession.SearchString);
            RemoveSession(Enums.OrderSession.ProductListModel);
            RemoveSession(Enums.InquirySession.SelectedSupplierPopup);

            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopUpHandler('{0}','{1}');", confirmCancelAction, "ProductSearch"), true);
        }
        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RemoveSessionsAndClosePopUP();
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            List<int> selectedProductID = ASPxGridViewProductSearchResult.GetSelectedFieldValues("TempID").OfType<int>().ToList();
            if (selectedProductID.Count == 1)
            {
                var selectedProduct = GetOrderDataProvider().GetSearchedProductListModel().Where(sp => sp.TempID == selectedProductID[0]).FirstOrDefault();

                if (selectedProduct != null)
                    GetOrderDataProvider().SetSelectedSearchedProduct(selectedProduct);

                RemoveSessionsAndClosePopUP(true);
            }
        }

        protected void ASPxGridViewProductSearchResult_DataBinding(object sender, EventArgs e)
        {
            model = GetOrderDataProvider().GetSearchedProductListModel();

            if (model != null)
            {
                (sender as ASPxGridView).DataSource = model;
                if (model.Count > 0)
                {
                    ASPxGridViewProductSearchResult.FocusedRowIndex = 0;                    
                }
            }

            ASPxGridViewProductSearchResult.Settings.GridLines = GridLines.Both;
        }

        protected void ASPxGridViewProductSearchResult_DataBound(object sender, EventArgs e)
        {
      
            if (model.Count > 0)
            {
                ASPxGridViewProductSearchResult.Selection.SelectRow(0);
            }
        }
    }
}