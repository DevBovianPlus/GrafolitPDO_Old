<%@ Page Title="Naročilo dobavitelju" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OrderForm.aspx.cs" Inherits="GrafolitPDO.Pages.Order.OrderForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        var postbackInitiated = false;
        var submitOrder = false;
        var CreateOrderInPantheon = false;

        $(document).ready(function () {
            $('#btnSubmitOrder').on('click', function () {
                submitOrder = true;
                if (CreateOrderInPantheon) {
                    clientBtnCreateOrder.DoClick();
                    CreateOrderInPantheon = false;
                }
                else
                    clientBtnSendToOrderDep.DoClick();
            });
        });


        function CheckFieldValidation() {
            var process = false;
            var lookUpItems = [];
            var inputItems = [];
            var dateEditItems = [DateEditSupplyDate];
            var memoItems = null;
            var comboBoxItems = [clientComboBoxOddelek];


            process = InputFieldsValidation(lookUpItems, inputItems, dateEditItems, memoItems, comboBoxItems, null);

            if (clientBtnSaveChanges.GetText() == 'Izbriši')
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

        function SendToOrderDep_Click(s, e) {
            var process = CheckFieldValidation();

            if (!submitOrder && process) {
                ShowModal('Želite oddati naročilo v nabavo!', 'Ali ste prepričani da želite oddati naročilo v nabavo?', true);
            }

            if (process && submitOrder) {
                LoadingPanel.Show();
                OrderCallbackPanel.PerformCallback('SubmitOrderToSupplierDep');
                submitOrder = false;
            }
            else
                e.processOnServer = false;
        }

        function CreateOrder_Click(s, e) {
            var process = CheckFieldValidation();

            if (!submitOrder && process) {
                ShowModal('Želite izdelati naročilo!', 'Ali ste prepričani da želite izdelati naročilo v PANTHEON?', true);
                CreateOrderInPantheon = true;
            }

            if (process && submitOrder) {
                LoadingPanel.Show();
                OrderCallbackPanel.PerformCallback('CreateOrderInPanheon');
                submitOrder = false;
            }
            else
                e.processOnServer = false;
        }


        function CheckArtikels_Click(s, e) {
            LoadingPanel.Show();
            OrderCallbackPanel.PerformCallback('CheckArtikels');
            submitOrder = false;
        }

        function gridOrder_SelectionChanged(s, e) { }

        function DateEditSupplyDate_ValueChanged(s, e) {
            LoadingPanel.Hide();
            OrderCallbackPanel.PerformCallback('SupplyDateChanged');
            submitOrder = false;
        }

        function PopupHandeling_Click(s, e) {
            var parameter = HandleUserActionsOnTabs(gridOrder, clientBtnAddPos, clientBtnEditPos, clientBtnDeletePos, s);
            var process = true;

            if (parameter == "1")
                process = CheckFieldValidation();

            if (process)
                OrderCallbackPanel.PerformCallback(parameter);
        }

        function OrderCallbackPanel_EndCallback(s, e) {
            LoadingPanel.Hide();

            if (s.cpOrderPositionValidationError != "" && s.cpOrderPositionValidationError != undefined) {
                ShowModal('Pozicija naročila!', 'Pozicije naročil ne vsebujejo podatkov, ki so potrebni za oddajo naročila!')
                delete (s.cpOrderPositionValidationError);
            }

            if (s.cpNoSelectedValidationError != "" && s.cpNoSelectedValidationError != undefined) {
                ShowModal('Pozicija naročila!', 'Vsaj ena pozija mora biti izbrana!')
                delete (s.cpNoSelectedValidationError);
            }

        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'OrderPos':
                            PopupControlOrderPos.Hide();
                            OrderCallbackPanel.PerformCallback("RefreshOrderPositions");
                            gridOrder.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'OrderPos':
                            PopupControlOrderPos.Hide();
                            break;
                    }
                    break;
            }
        }

        function ShowModal(title, message, yeNoModal) {
            $('.modal-title').empty();
            $('.modal-title').append(title);

            $('.modal-body').empty();
            $('.modal-body').append(message);

            if (yeNoModal)
                $('#questionModal').modal("show");
            else
                $('#modal').modal("show");
        }

        function HideModal(yeNoModal) {

            if (yeNoModal)
                $('#questionModal').modal("hide");
            else
                $('#modal').modal("hide");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="OrderCallbackPanel" runat="server" ClientInstanceName="OrderCallbackPanel" OnCallback="OrderCallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="OrderCallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="card">
                    <div class="card-header" style="background-color: #FAFCFE">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6>Artikli za dobavitelja</h6>
                            <%--<a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>--%>
                        </div>
                    </div>
                    <div class="card-body p-0" style="background-color: #eef2f5d6;">
                        <div class="row m-0 pb-3">
                            <div class="col-12 mb-2 p-0">
                                <dx:ASPxGridView ID="ASPxGridViewOrder" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrder"
                                    OnDataBinding="ASPxGridViewOrder_DataBinding" Width="100%"
                                    KeyFieldName="PovprasevanjePozicijaArtikelID" CssClass="gridview-no-header-padding"
                                    OnHtmlRowPrepared="ASPxGridViewOrder_HtmlRowPrepared" OnHtmlDataCellPrepared="ASPxGridViewOrder_HtmlDataCellPrepared" OnCellEditorInitialize="ASPxGridViewOrder_CellEditorInitialize"
                                    OnRowUpdating="ASPxGridViewOrder_RowUpdating">
                                    <ClientSideEvents SelectionChanged="gridOrder_SelectionChanged" RowDblClick="PopupHandeling_Click" />
                                    <Paddings Padding="0" />
                                    <Settings ShowVerticalScrollBar="True"
                                        ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300" AutoFilterCondition="Contains" UseFixedTableLayout="true" 
                                        ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                    <SettingsPager PageSize="50" ShowNumericButtons="true">
                                        <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                        </PageSizeItemSettings>
                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline" BatchEditSettings-StartEditAction="Click" />
                                    <SettingsText EmptyDataRow="Trenutno ni podatkov. Dodaj novega." CommandBatchEditUpdate="Spremeni" CommandBatchEditCancel="Prekliči" CommandEdit="Uredi" CommandCancel="Prekliči" CommandUpdate="Spremeni" />
                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                                    <SettingsBehavior AllowEllipsisInText="true" AllowFocusedRow="true" />

                                    <Styles Header-Wrap="True">
                                        <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                    </Styles>
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowEditButton="true" ShowNewButtonInHeader="false" Width="70px" Caption="Uredi" />
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="50px" Caption="Izberi"></dx:GridViewCommandColumn>

                                        <%--<dx:GridViewDataTextColumn Caption="ID" FieldName="PovprasevanjePozicijaArtikelID" Visible="false" SortOrder="Descending" />--%>

                                        <dx:GridViewDataTextColumn Caption="Izbran dobavitelj" FieldName="DobaviteljNaziv_PA" Width="7%" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Kategorija" FieldName="KategorijaNaziv" Width="3%" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Popraševanje naziv art." FieldName="Naziv" Width="12%" EditFormSettings-Visible="false" />
                                        <%--<dx:GridViewDataTextColumn Caption="Naziv artikla - PANTHEON" FieldName="IzbraniArtikelNaziv_P" Width="15%" EditFormSettings-Visible="true" />--%>

                                        <dx:GridViewDataComboBoxColumn FieldName="IzbraniArtikelNaziv_P" Caption="Naziv artikla - PANTHEON" Width="12%" EditFormSettings-Visible="true">
                                            <PropertiesComboBox EnableSynchronization="false" IncrementalFilteringMode="StartsWith"
                                                ValueField="TempID" TextField="Naziv" ValueType="System.Int32" DataSecurityMode="Default">
                                                <%--<ClientSideEvents EndCallback="onCityEndCallback" />--%>
                                            </PropertiesComboBox>
                                        </dx:GridViewDataComboBoxColumn>


                                        <dx:GridViewDataTextColumn Name="Kol1" Caption="Kol1" FieldName="Kolicina1" Width="4%" EditFormSettings-Visible="false" PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                                        <dx:GridViewDataTextColumn Name="EM1" Caption="EM1" FieldName="EnotaMere1" Width="2%" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataTextColumn Name="Kol2" Caption="Kol2" FieldName="Kolicina2" Width="4%" EditFormSettings-Visible="false" PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                                        <dx:GridViewDataTextColumn Name="EM2" Caption="EM2" FieldName="EnotaMere2" Width="2%" EditFormSettings-Visible="false" />                                           
                                        <dx:GridViewDataTextColumn Name="KolicinavKG" Caption="KolicinavKG" FieldName="KolicinavKG" Width="3%" Visible="false" EditFormSettings-Visible="false" PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                                        <dx:GridViewDataTextColumn Name="NabCena" Caption="Nab. cena" FieldName="ArtikelCena" Width="5%" Visible="false" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataTextColumn Name="EnotaM" Caption="Enota mere" FieldName="EnotaMere" Width="3%" Visible="false" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataTextColumn Name="Rabat" Caption="Rabat" FieldName="Rabat" Width="3%" Visible="false" EditFormSettings-Visible="false" />
                                        <dx:GridViewDataDateColumn Name="DatumDobavePos" Caption="Datum dob." FieldName="DatumDobavePos" Width="3%" Visible="true" EditFormSettings-Visible="false" PropertiesDateEdit-DisplayFormatString="dd.MM.yyyy" />

                                        <%--<dx:GridViewDataTextColumn Caption="Naročilo" FieldName="NarociloStevilka_P" Width="15%" />--%>
                                    </Columns>

                                    <SettingsResizing ColumnResizeMode="NextColumn" Visualization="Live" />
                                </dx:ASPxGridView>

                                <dx:ASPxPopupControl ID="PopupControlOrderPos" runat="server" ContentUrl="OrderPos_popup.aspx"
                                    ClientInstanceName="PopupControlOrderPos" Modal="True" HeaderText="POZICIJA NAROČILNICE"
                                    CloseAction="CloseButton" Width="1100px" Height="750px" PopupHorizontalAlign="WindowCenter"
                                    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                    AllowResize="true" ShowShadow="true"
                                    OnWindowCallback="PopupControlOrderPos_WindowCallback">
                                    <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                    <ContentStyle BackColor="#F7F7F7">
                                        <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                                    </ContentStyle>
                                </dx:ASPxPopupControl>

                            </div>
                        </div>

                        <div class="row m-0 pb-3 d-none">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Size="12px" Text="YAMADA 1 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada1" ClientInstanceName="txtYamada1"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Size="12px" Text="YAMADA 2 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada2" ClientInstanceName="txtYamada2"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Size="12px" Text="YAMADA 3 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada3" ClientInstanceName="txtYamada3"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3 d-none">
                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Size="12px" Text="YAMADA 4 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada4" ClientInstanceName="txtYamada4"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Size="12px" Text="YAMADA 5 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada5" ClientInstanceName="txtYamada5"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-4 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Size="12px" Text="YAMADA 6 : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtYamada6" ClientInstanceName="txtYamada6"
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
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Font-Size="12px" Text="DATUM DOBAVE : *"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-1 p-0">
                                        <dx:ASPxDateEdit ID="DateEditSupplyDate" runat="server" EditFormat="Date" Width="100%"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                            ClientInstanceName="DateEditSupplyDate">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                            <ClientSideEvents ValueChanged="DateEditSupplyDate_ValueChanged" />
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-4">
                            <div class="row m-0 align-items-center" style="align-items: center">
                                <div class="col-0 p-0" style="margin-right: 40px;">
                                    <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Oddelek : *"></dx:ASPxLabel>
                                </div>
                                <div class="col p-0">
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

                        <div class="row m-2 pb-4">
                            <div class="col-lg-12 mb-2 mb-lg-0 ">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 43px;">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="OPOMBE : " Font-Size="12px"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="6" MaxLength="6000" CssClass="text-box-input" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <%-- Gumbi za Naročilo --%>
                                <div class="row m-0 align-items-center justify-content-start">
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false" OnClick="btnCancel_Click"
                                            Height="25" Width="110" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/cancel.png" UrlHottracked="../../Images/cancelHover.png" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0">
                                        <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Shrani" AutoPostBack="false" OnClick="btnSaveChanges_Click"
                                            Height="25" Width="110" ClientInstanceName="clientBtnSaveChanges" ClientVisible="false" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                            <ClientSideEvents Click="ActionButton_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnCheckArtikels" runat="server" Text="Preveri artikle" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnCheckArtikels" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/prevzem.png" UrlHottracked="../../Images/prevzemHover.png" />
                                            <ClientSideEvents Click="CheckArtikels_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnSendToOrderDep" runat="server" Text="Pošlji v nabavo" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnSendToOrderDep" UseSubmitBehavior="false" ClientVisible="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/submitInquiry.png" UrlHottracked="../../Images/submitInquryHover.png" />
                                            <ClientSideEvents Click="SendToOrderDep_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnCreateOrder" runat="server" Text="Izdelaj naročilo" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnCreateOrder" UseSubmitBehavior="false" ClientVisible="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/submitInquiry.png" UrlHottracked="../../Images/submitInquryHover.png" />
                                            <ClientSideEvents Click="CreateOrder_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <%-- Gumbi za artikle (naročila pozicije) --%>
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnAddPos" runat="server" Text="Dodaj" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnAddPos" UseSubmitBehavior="false" ClientVisible="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/addPopupMain.png" UrlHottracked="../../Images/addPopupMainHover.png" />
                                            <ClientSideEvents Click="PopupHandeling_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnEditPos" runat="server" Text="Spremeni pozicijo" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnEditPos" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/editPopupMain.png" UrlHottracked="../../Images/editPopupMainHover.png" />
                                            <ClientSideEvents Click="PopupHandeling_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnDeletePos" runat="server" Text="Izbriši" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnDeletePos" UseSubmitBehavior="false" ClientVisible="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/trashPopupMain.png" UrlHottracked="../../Images/trashPopupMainHover.png" />
                                            <ClientSideEvents Click="PopupHandeling_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <!-- The Modal -->
    <div class="modal fade" id="modal">
        <div class="modal-dialog">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title"></h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                </div>

                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Zapri</button>
                </div>

            </div>
        </div>
    </div>

    <!-- The Question Modal -->
    <div class="modal fade" id="questionModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h4 class="modal-title"></h4>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>

                <!-- Modal body -->
                <div class="modal-body">
                </div>

                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" id="btnSubmitOrder" class="btn btn-primary" data-dismiss="modal">Da, želim</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Prekliči</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
