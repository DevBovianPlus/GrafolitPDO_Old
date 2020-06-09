using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DatabaseWebService.ModelsPDO.Inquiry;
using System.Collections.Generic;
using DatabaseWebService.Models.Client;
using GrafolitPDO.Helpers;
using static DatabaseWebService.Common.Enums.Enums;
using static GrafolitPDO.Common.Enums;
using GrafolitPDO.Common;

/// <summary>
/// Summary description for InquiryForSupplierReport
/// </summary>
public class InquiryForSupplierReport : DevExpress.XtraReports.UI.XtraReport
{
    #region Variables
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageHeaderBand PageHeader;
    private PageFooterBand PageFooter;
    private XRPictureBox xrPictureBox1;
    private XRLabel lblGreetingsText;
    private XRLabel lblInquiry;
    private XRLabel lblTimeStamp;
    private XRLabel lblSupplierEmail;
    private XRLabel lblSupplierPhone;
    private XRLabel lblSupplierAddress;
    private XRLabel lblSupplierName;
    private XRPageInfo xrPageInfo;
    private XRPictureBox xrPictureBox2;
    private XRLabel lblSigniture;
    private XRLabel lblBestRegardsText;
    private XRTable ProductTable;
    private XRTableRow xrTableRow1;
    private XRTableCell tchMaterial;
    private XRTableCell tchQnt;
    private XRTableCell thcNotes;
    private XRLabel lblBuyer;

    private decimal dKolicina;
    private string sMerskaEnota;
    private string sKolicinaFull;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    #endregion

    private string GetSheetsNameByLanguage(Language enLang)
    {
        switch (enLang)
        {
            case Language.ANG:
                return "SH";
                break;
            case Language.HRV:
                return "POL";
                break;
            case Language.SLO:
                return "POL";
                break;
            default:
                break;
        }

        return "SHEETS";
    }

    public InquiryForSupplierReport(GroupedInquiryPositionsBySupplier model)
    {
        InitializeComponent();

        bool showBuyer = false;

        lblSupplierName.Text = model.Supplier.NazivPrvi;
        lblSupplierAddress.Text = model.Supplier.Naslov;
        lblSupplierPhone.Text = model.Supplier.Telefon;
        lblSupplierEmail.Text = model.Supplier.Email;
        string langStr = (model.Supplier.JezikID > 0) ? model.Supplier.Jezik.Koda : Language.SLO.ToString();

        Language enMLanguage = (Language)(Enum.Parse(typeof(Language), langStr));

        lblTimeStamp.Text = "Žalec, " + DateTime.Now.ToString("dd. MMMM yyyy");

        lblInquiry.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.INQUIRY);
        thcNotes.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.NOTES);
        tchMaterial.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.MATERIAL);
        tchQnt.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.QUANTITY);
        
        lblGreetingsText.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.GREETINGS); //"Hello, \r\n\r\n We kindly ask for the best delivery date for the material:";
        lblBestRegardsText.Text = TranslateHelper.GetTranslateValueByContentAndLanguage(enMLanguage, ReportContentType.REGARDS); //"Thank you and best regards,";
        lblSigniture.Text = PrincipalHelper.GetUserPrincipal().Signature;

        foreach (var item in model.InquiryPositionsArtikel)
        {
            XRTableRow row = new XRTableRow();
            XRTableCell cell = new XRTableCell();

            cell.WidthF = 280f;
            //cell.BackColor = Color.AliceBlue;
            cell.Text = item.Naziv;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);
            item.EnotaMere1 = (item.EnotaMere2 != null && item.EnotaMere1.ToString() == "POL" || item.EnotaMere1.ToString() == "pol") ? item.EnotaMere1 = GetSheetsNameByLanguage(enMLanguage) : item.EnotaMere1;
            item.EnotaMere2 = (item.EnotaMere2 == null ? "" : item.EnotaMere2);
            item.EnotaMere2 = (item.EnotaMere2 != null && item.EnotaMere2.ToString() == "POL" || item.EnotaMere2.ToString() == "pol") ? item.EnotaMere2 = GetSheetsNameByLanguage(enMLanguage) : item.EnotaMere2;
            dKolicina = (item.Kolicina1 > 0) ? item.Kolicina1 : item.Kolicina2;
            sMerskaEnota = (item.EnotaMere1.Length > 0) ? item.EnotaMere1 : item.EnotaMere2;

            if ((item.Kolicina1 > 0) && (item.Kolicina2 > 0))
                sKolicinaFull = item.Kolicina1 + " " + item.EnotaMere1 + " / " + item.Kolicina2 + " " + item.EnotaMere2;
            else
                sKolicinaFull = dKolicina + " " + sMerskaEnota;

            cell = new XRTableCell();
            cell.WidthF = 90f;
            //cell.BackColor = Color.AliceBlue;
            cell.Text = sKolicinaFull;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            cell = new XRTableCell();
            cell.WidthF = 257f;            
            cell.Text = item.OpombaNarocilnica;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            cell.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            row.Cells.Add(cell);

            if ((bool)model.KupecViden)
                showBuyer = true;

            ProductTable.Rows.Add(row);
        }

        if (showBuyer && model.Buyer != null)
        {
            lblBuyer.Text = "BUYER : " + model.Buyer.NazivPrvi;
        }
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.lblBuyer = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSigniture = new DevExpress.XtraReports.UI.XRLabel();
            this.lblBestRegardsText = new DevExpress.XtraReports.UI.XRLabel();
            this.ProductTable = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tchMaterial = new DevExpress.XtraReports.UI.XRTableCell();
            this.tchQnt = new DevExpress.XtraReports.UI.XRTableCell();
            this.thcNotes = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblGreetingsText = new DevExpress.XtraReports.UI.XRLabel();
            this.lblInquiry = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTimeStamp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierEmail = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierPhone = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierName = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProductTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblBuyer,
            this.lblSigniture,
            this.lblBestRegardsText,
            this.ProductTable,
            this.lblGreetingsText,
            this.lblInquiry,
            this.lblTimeStamp,
            this.lblSupplierEmail,
            this.lblSupplierPhone,
            this.lblSupplierAddress,
            this.lblSupplierName});
            this.Detail.HeightF = 541.6667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblBuyer
            // 
            this.lblBuyer.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblBuyer.LocationFloat = new DevExpress.Utils.PointFloat(0F, 362.9583F);
            this.lblBuyer.Multiline = true;
            this.lblBuyer.Name = "lblBuyer";
            this.lblBuyer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblBuyer.SizeF = new System.Drawing.SizeF(277.0833F, 23F);
            this.lblBuyer.StylePriority.UseFont = false;
            this.lblBuyer.StylePriority.UseTextAlignment = false;
            this.lblBuyer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblSigniture
            // 
            this.lblSigniture.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblSigniture.LocationFloat = new DevExpress.Utils.PointFloat(450.75F, 454F);
            this.lblSigniture.Multiline = true;
            this.lblSigniture.Name = "lblSigniture";
            this.lblSigniture.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSigniture.SizeF = new System.Drawing.SizeF(256.25F, 23F);
            this.lblSigniture.StylePriority.UseFont = false;
            this.lblSigniture.StylePriority.UseTextAlignment = false;
            this.lblSigniture.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblBestRegardsText
            // 
            this.lblBestRegardsText.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblBestRegardsText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 401.2917F);
            this.lblBestRegardsText.Multiline = true;
            this.lblBestRegardsText.Name = "lblBestRegardsText";
            this.lblBestRegardsText.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblBestRegardsText.SizeF = new System.Drawing.SizeF(277.0833F, 23F);
            this.lblBestRegardsText.StylePriority.UseFont = false;
            this.lblBestRegardsText.StylePriority.UseTextAlignment = false;
            this.lblBestRegardsText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // ProductTable
            // 
            this.ProductTable.LocationFloat = new DevExpress.Utils.PointFloat(0F, 324.625F);
            this.ProductTable.Name = "ProductTable";
            this.ProductTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.ProductTable.SizeF = new System.Drawing.SizeF(707F, 25F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tchMaterial,
            this.tchQnt,
            this.thcNotes});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tchMaterial
            // 
            this.tchMaterial.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.tchMaterial.Multiline = true;
            this.tchMaterial.Name = "tchMaterial";
            this.tchMaterial.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tchMaterial.StylePriority.UseBorders = false;
            this.tchMaterial.StylePriority.UseTextAlignment = false;
            this.tchMaterial.Text = "MATERIAL";
            this.tchMaterial.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tchMaterial.Weight = 1.3397126334961262D;
            // 
            // tchQnt
            // 
            this.tchQnt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.tchQnt.Multiline = true;
            this.tchQnt.Name = "tchQnt";
            this.tchQnt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.tchQnt.StylePriority.UseBorders = false;
            this.tchQnt.StylePriority.UseTextAlignment = false;
            this.tchQnt.Text = "QUANTITY";
            this.tchQnt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tchQnt.Weight = 0.57294642599882584D;
            // 
            // thcNotes
            // 
            this.thcNotes.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.thcNotes.Multiline = true;
            this.thcNotes.Name = "thcNotes";
            this.thcNotes.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.thcNotes.StylePriority.UseBorders = false;
            this.thcNotes.StylePriority.UseTextAlignment = false;
            this.thcNotes.Text = "NOTES";
            this.thcNotes.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.thcNotes.Weight = 1.0873409405050483D;
            // 
            // lblGreetingsText
            // 
            this.lblGreetingsText.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblGreetingsText.LocationFloat = new DevExpress.Utils.PointFloat(0F, 227.6667F);
            this.lblGreetingsText.Multiline = true;
            this.lblGreetingsText.Name = "lblGreetingsText";
            this.lblGreetingsText.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblGreetingsText.SizeF = new System.Drawing.SizeF(707F, 66.75002F);
            this.lblGreetingsText.StylePriority.UseFont = false;
            // 
            // lblInquiry
            // 
            this.lblInquiry.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
            this.lblInquiry.LocationFloat = new DevExpress.Utils.PointFloat(0F, 185.3333F);
            this.lblInquiry.Multiline = true;
            this.lblInquiry.Name = "lblInquiry";
            this.lblInquiry.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblInquiry.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblInquiry.StylePriority.UseFont = false;
            this.lblInquiry.Text = "INQUIRY";
            // 
            // lblTimeStamp
            // 
            this.lblTimeStamp.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblTimeStamp.LocationFloat = new DevExpress.Utils.PointFloat(540.3333F, 147.2084F);
            this.lblTimeStamp.Multiline = true;
            this.lblTimeStamp.Name = "lblTimeStamp";
            this.lblTimeStamp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTimeStamp.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblTimeStamp.StylePriority.UseFont = false;
            // 
            // lblSupplierEmail
            // 
            this.lblSupplierEmail.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplierEmail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 83.99998F);
            this.lblSupplierEmail.Multiline = true;
            this.lblSupplierEmail.Name = "lblSupplierEmail";
            this.lblSupplierEmail.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSupplierEmail.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblSupplierEmail.StylePriority.UseFont = false;
            // 
            // lblSupplierPhone
            // 
            this.lblSupplierPhone.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplierPhone.LocationFloat = new DevExpress.Utils.PointFloat(0F, 107F);
            this.lblSupplierPhone.Multiline = true;
            this.lblSupplierPhone.Name = "lblSupplierPhone";
            this.lblSupplierPhone.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSupplierPhone.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblSupplierPhone.StylePriority.UseFont = false;
            // 
            // lblSupplierAddress
            // 
            this.lblSupplierAddress.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplierAddress.LocationFloat = new DevExpress.Utils.PointFloat(0F, 60.99999F);
            this.lblSupplierAddress.Multiline = true;
            this.lblSupplierAddress.Name = "lblSupplierAddress";
            this.lblSupplierAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSupplierAddress.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblSupplierAddress.StylePriority.UseFont = false;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.lblSupplierName.LocationFloat = new DevExpress.Utils.PointFloat(0F, 38.00001F);
            this.lblSupplierName.Multiline = true;
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSupplierName.SizeF = new System.Drawing.SizeF(166.6667F, 23F);
            this.lblSupplierName.StylePriority.UseFont = false;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 3F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.PageHeader.HeightF = 136.4583F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrPictureBox1.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrPictureBox1.ImageUrl = "~/Images/ReportImages/ReportGlave.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(707F, 136.4583F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo,
            this.xrPictureBox2});
            this.PageFooter.HeightF = 107.7916F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(707.0001F, 15.70832F);
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPageInfo.TextFormatString = "Page {0} of {1}";
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.xrPictureBox2.ImageUrl = "~/Images/ReportImages/ReportNoga.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 15.70829F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(707F, 84.16672F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // InquiryForSupplierReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.PageFooter});
            this.Margins = new System.Drawing.Printing.Margins(60, 60, 0, 3);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this.ProductTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}

