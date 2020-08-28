<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" CodeBehind="OrderPos_popup.aspx.cs" Inherits="GrafolitPDO.Pages.Order.OrderPos_popup" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ MasterType VirtualPath="~/Popup.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolderPopup" runat="server">
    <script type="text/javascript">
        var hasStartEditing = false;
        var postbackInitiated = false;

        function CheckFieldValidation() {
            var process = false;
            var lookUpItems = [];
            var comboBoxItems = [clientComboBoxOddelek];

            var inputItems = [txtName, txtProductSearch, txtOrderQ, txtUnitOfMeasure];
            var dateItems = null;

            process = InputFieldsValidation(lookUpItems, inputItems, dateItems, /*memoItems*/ null, comboBoxItems, null);

            if (btnConfirm.GetText() == 'Izbriši')
                process = true;

            return process;
        }

        function ActionButton_Click(s, e) {
            var process = CheckFieldValidation();

            if (process) {
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
            }
            else
                e.processOnServer = false;
        }

        function CallbackPanel_EndCallback(s, e) {

        }

        function btnSearch_Click(s, e) {
            CallbackPanel.PerformCallback("StartSearchPopup");
        }

        function btnSearchSupplier_Click(s, e) {
            var process = true;

            if (process) {
                CallbackPanel.PerformCallback("StartSearchSupplierPopup");
            }
            else
                e.processOnServer = false;
        }

        function txtProductSearch_KeyUp(s, e) {
            if (s.GetText().length >= 3)
                btnSearch.SetEnabled(true);
            else
                btnSearch.SetEnabled(false);
        }

        function StartSearchByKeyboardKey(s, e) {
            if (e.htmlEvent.keyCode == 13 && s.GetText().length >= 3)//Enter
            {
                CallbackPanel.PerformCallback("StartSearchPopup");
            }
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'ProductSearch':
                            PopupControlSearchProduct.Hide();
                            btnSearch.SetEnabled(true);
                            txtProductSearch.Focus();
                            CallbackPanel.PerformCallback("ProductSelected");
                            break;
                        case 'SupplierSearch':
                            var seleSupVal = '<%= GetStringValueFromSession(GrafolitPDO.Common.Enums.InquirySession.ReturnSupplierVal) %>';
                            txtDobavitelj.SetText(seleSupVal);                            
                            PopupControlSearchSupplier.Hide();
                            CallbackPanel.PerformCallback("SupplierSelected");                                                                                 
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'ProductSearch':
                            PopupControlSearchProduct.Hide();
                            btnSearch.SetEnabled(true);
                            txtProductSearch.Focus();
                            break;
                        case 'SupplierSearch':
                            PopupControlSearchSupplier.Hide();
                            //btnSearch.SetEnabled(true);
                            txtDobavitelj.Focus();
                            break;
                    }
                    break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolderPopup" runat="server">
    <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel" OnCallback="CallbackPanel_Callback" runat="server">
        <ClientSideEvents EndCallback="CallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>

                <div class="row m-0 pb-3 pt-3">
                    <div class="col-lg-8 mb-2 mb-lg-0 ">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 35px;">
                                <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="Oddelek : *"></dx:ASPxLabel>
                            </div>
                            <div class="col-6 p-0">
                                <dx:ASPxComboBox ID="ComboBoxOddelek" runat="server" ValueType="System.String" DropDownStyle="DropDownList"
                                    IncrementalFilteringMode="StartsWith" TextField="Naziv" ValueField="OddelekID"
                                    EnableSynchronization="False" Width="100%" OnDataBinding="ComboBoxOddelek_DataBinding"
                                    Font-Size="14px" Font-Names="Segoe UI" CssClass="text-box-input" ClientInstanceName="clientComboBoxOddelek">
                                    <ClearButton DisplayMode="OnHover" />
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 10px;">
                                <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="PRIKAZI KUPCA : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-0 p-0">
                                <dx:ASPxCheckBox ID="CheckBoxPrikaziKupca" runat="server"></dx:ASPxCheckBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-12 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0 mr-2">
                                <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="OPISNI NAZIV : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxTextBox runat="server" ID="txtName" ClientInstanceName="txtName" MaxLength="300"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-6 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 15px;">
                                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="DOBAVITELJ : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxTextBox runat="server" ID="txtDobavitellj" ClientInstanceName="txtDobavitelj" MaxLength="300"
                                    Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                            <div class="col-3 p-0">
                                <dx:ASPxButton ID="btmSearchSupplier" runat="server" AutoPostBack="false" ClientInstanceName="btnSearch"
                                    Height="25" Width="50" UseSubmitBehavior="false">
                                    <Paddings Padding="0" />
                                    <Image Url="../../Images/search.png" UrlHottracked="../../Images/searchHover.png" UrlDisabled="../../Images/searchDisabled.png" />
                                    <ClientSideEvents Click="btnSearchSupplier_Click" />
                                </dx:ASPxButton>
                            </div>
                            <dx:ASPxPopupControl ID="PopupControlSearchSupplier" runat="server" ContentUrl="../Inquiry/SearchSupplier_popup.aspx"
                                ClientInstanceName="PopupControlSearchSupplier" Modal="True" HeaderText="DOBAVITELJ"
                                CloseAction="CloseButton" Width="800px" Height="635px" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                AllowResize="true" ShowShadow="true"
                                OnWindowCallback="PopupControlSearchSupplier_WindowCallback">
                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                <ContentStyle BackColor="#F7F7F7">
                                    <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                                </ContentStyle>
                            </dx:ASPxPopupControl>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-9 mb-2 mb-lg-0">
                        <div class="row m-0 mb-2 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 35px;">
                                <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="ARTIKEL : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-8 p-0 mr-3">
                                <dx:ASPxTextBox runat="server" ID="txtProductSearch" ClientInstanceName="txtProductSearch" MaxLength="500"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" NullText="Poišči artikel...">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyUp="txtProductSearch_KeyUp" KeyPress="StartSearchByKeyboardKey" />
                                </dx:ASPxTextBox>
                            </div>
                            <div class="col-1 p-0">
                                <dx:ASPxButton ID="btnSearch" runat="server" AutoPostBack="false" ClientInstanceName="btnSearch"
                                    Height="25" Width="50" UseSubmitBehavior="false" ClientEnabled="false">
                                    <Paddings Padding="0" />
                                    <Image Url="../../Images/search.png" UrlHottracked="../../Images/searchHover.png" UrlDisabled="../../Images/searchDisabled.png" />
                                    <ClientSideEvents Click="btnSearch_Click" />
                                </dx:ASPxButton>
                            </div>
                            <dx:ASPxPopupControl ID="PopupControlSearchProduct" runat="server" ContentUrl="SearchProduct_popup.aspx"
                                ClientInstanceName="PopupControlSearchProduct" Modal="True" HeaderText="ARTIKEL"
                                CloseAction="CloseButton" Width="1000px" Height="640px" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                AllowResize="true" ShowShadow="true"
                                OnWindowCallback="PopupControlSearchProduct_WindowCallback">
                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                <ContentStyle BackColor="#F7F7F7">
                                    <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                                </ContentStyle>
                            </dx:ASPxPopupControl>
                        </div>
                    </div>

                    <div class="col-lg-3 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 43px;">
                                <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="CENA : " Font-Size="12px" ClientVisible="false"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxTextBox runat="server" ID="txtPrice" ClientInstanceName="txtPrice" MaxLength="25"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" ClientVisible="false">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-3 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 28px;">
                                <dx:ASPxLabel ID="ASPxLabel10" runat="server" Text="KOLIČINA : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-7 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtOrderQ" ClientInstanceName="txtOrderQ" MaxLength="23"
                                    Font-Bold="true" CssClass="text-box-input" Font-Size="13px" Width="100%">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0 mr-2">
                                <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="ENOTA MERE : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-3 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtEnotaMere" ClientInstanceName="txtUnitOfMeasure" MaxLength="30"
                                    Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-5 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center justify-content-end">
                            <div class="col-0 p-0 mr-2">
                                <dx:ASPxLabel ID="ASPxLabel12" runat="server" Text="Rabat :" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-3 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtRabate" ClientInstanceName="txtRabate" MaxLength="30"
                                    Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-3 mb-2 mb-lg-0 ">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 43px;">
                                <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Datum dobave : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxDateEdit ID="DateEditSupplyDate" runat="server" EditFormat="Date" Width="100%"
                                    CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                    ClientInstanceName="DateEditSupplyDate">
                                    <FocusedStyle CssClass="focus-text-box-input" />
                                    <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                    <DropDownButton Visible="true"></DropDownButton>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-12 mb-2 mb-lg-0 ">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 43px;">
                                <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="OPOMBE naročila : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxMemo ID="memOpombaNarocilnica" runat="server" Width="100%" Rows="2" MaxLength="5000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>

                <h5 class="font-italic"><i class='fas fa-table'></i>Podatki povpraševanja</h5>
                <div class="row m-0 pb-3">
                    <div class="col-lg-6 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 20px;">
                                <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="KOLIČINA 1 : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-4 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtQuantity1" ClientInstanceName="txtQuantity1" MaxLength="23"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center justify-content-end">
                            <div class="col-0 p-0 mr-2">
                                <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="ENOTA MERE 1 : *" Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-3 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtUnitOfMeasure1" ClientInstanceName="txtUnitOfMeasure1" MaxLength="30"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-6 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 28px;">
                                <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="KOLIČINA 2 : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-4 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtQuantity2" ClientInstanceName="txtQuantity2" MaxLength="23"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                    <ClientSideEvents KeyPress="isNumberKey_decimal" />
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center justify-content-end">
                            <div class="col-0 p-0" style="margin-right: 17px">
                                <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="ENOTA MERE 2 : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col-3 p-0">
                                <dx:ASPxTextBox runat="server" ID="txtUnitOfMeasure2" ClientInstanceName="txtUnitOfMeasure2" MaxLength="30"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true"
                                    CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3">
                    <div class="col-lg-12 mb-2 mb-lg-0 ">
                        <div class="row m-0 align-items-center">
                            <div class="col-0 p-0" style="margin-right: 43px;">
                                <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBE : " Font-Size="12px"></dx:ASPxLabel>
                            </div>
                            <div class="col p-0">
                                <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="5" MaxLength="5000" CssClass="text-box-input" AutoCompleteType="Disabled"
                                    ClientEnabled="false" BackColor="LightGray" Font-Bold="true">
                                    <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                </dx:ASPxMemo>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-0 pb-3 ">
                    <div class="col-lg-12 mb-2 mb-lg-0">
                        <div class="row m-0 align-items-center justify-content-end">
                            <div class="col-0 p-0 pr-2">
                                <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false"
                                    Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnCancel_Click">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../Images/cancelPopup.png" />
                                </dx:ASPxButton>
                            </div>
                            <div class="col-0 p-0">
                                <dx:ASPxButton ID="btnConfirm" runat="server" Text="Potrdi" AutoPostBack="false"
                                    Height="25" Width="110" UseSubmitBehavior="false" OnClick="btnConfirm_Click" ClientInstanceName="btnConfirm">
                                    <Paddings PaddingLeft="10" PaddingRight="10" />
                                    <Image Url="../../Images/addPopup.png" />
                                    <ClientSideEvents Click="ActionButton_Click" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>

            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</asp:Content>
