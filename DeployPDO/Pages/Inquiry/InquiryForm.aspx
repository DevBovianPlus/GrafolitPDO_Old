<%@ Page Title="Povpraševanje" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="InquiryForm.aspx.cs" Inherits="GrafolitPDO.Pages.Inquiry.InquiryForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">
        var submitConfirmed = false;

        $(document).ready(function () {

            $('#btnSubmitInquiry').on('click', function () {
                submitConfirmed = true;
                clientBtnSubmitInquiry.DoClick();
            });

            $('#btnFinishInquiry').on('click', function () {
                submitConfirmed = true;
                clientBtnFinishInquiry.DoClick();
            });


            var sessionValue = '<%= Session[GrafolitPDO.Common.Enums.InquirySession.SuppliersValidOnSubmittingInquiry.ToString()] %>';

            if (sessionValue != undefined && sessionValue != "") {
                ShowModal('Dobavitelji!', sessionValue);
                btnSubmitInquiryClick = false;
            }
        });


        var postbackInitiated = false;
        function CheckFieldValidation() {
            var process = false;
            var lookUpItems = [lookUpBuyer];
            //var comboBoxItems = [clientComboBoxTip];
            var inputItems = [clientTxtInquiryName];
            var memoItems = null;
            var dateItems = null;

            process = InputFieldsValidation(lookUpItems, inputItems, dateItems, memoItems, /*comboBoxItems*/null, null);

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


        function SubmitInquiry_Click(s, e) {
            var process = CheckFieldValidation();
            if (gridInquiryPosition.GetVisibleRowsOnPage() <= 0) {

                ShowModal('Artikli!', 'V povpraševanje je potrebno vnesti artikle.');
                process = false;
            }

            if (process) {
                ShowModal('Želite oddati povpraševanje!', 'Ali ste prepričani da želite poslati povpraševanja izbranim dobaviteljem?', true);
            }

            if (submitConfirmed) {
                LoadingPanel.Show();
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
                submitConfirmed = false;
            }
            else
                e.processOnServer = false;
        }

        function FinishInquiry_Click(s, e) {
            var process = CheckFieldValidation();
            if (gridInquiryPosition.GetVisibleRowsOnPage() <= 0) {

                ShowModalFinish('Artikli!', 'V povpraševanje je potrebno vnesti artikle.');
                process = false;
            }

            if (process) {
                ShowModalFinish('Želite oddati povpraševanje!', 'Ali ste prepričani da želite zaključiti povpraševanje? POZOR: Vaše povpraševanje ne bo poslano dobaviteljem.', true);
            }

            if (submitConfirmed) {
                LoadingPanel.Show();
                e.processOnServer = !postbackInitiated;
                postbackInitiated = true;
                submitConfirmed = false;
            }
            else
                e.processOnServer = false;
        }

        function PopupHandeling_Click(s, e) {
            var parameter = HandleUserActionsOnTabs(gridInquiryPosition, clientBtnAddPos, clientBtnEditPos, clientBtnDeletePos, s);
            var process = true;

            process = CheckFieldValidation();

            if (process) {
                LoadingPanel.Show();
                InquiryCallbackPanel.PerformCallback(parameter);
            }
        }

        function InquiryCallbackPanel_EndCallback(s, e) {
            LoadingPanel.Hide();
        }

        function OnClosePopUpHandler(command, sender) {
            switch (command) {
                case 'Potrdi':
                    switch (sender) {
                        case 'InquiryPos':
                            PopupControlInquiryPos.Hide();
                            gridInquiryPosition.Refresh();
                            break;
                    }
                    break;
                case 'Preklici':
                    switch (sender) {
                        case 'InquiryPos':
                            PopupControlInquiryPos.Hide();
                            gridInquiryPosition.Refresh();
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

        function ShowModalFinish(title, message, yeNoModal) {
            $('.modal-title').empty();
            $('.modal-title').append(title);

            $('.modal-body').empty();
            $('.modal-body').append(message);

            if (yeNoModal)
                $('#questionModalFinish').modal("show");
            else
                $('#modal').modal("show");
        }

        function HideModalFinish(yeNoModal) {

            if (yeNoModal)
                $('#questionModalFinish').modal("hide");
            else
                $('#modal').modal("hide");
        }

        function lookUpBuyer_ValueChanged(s, e) {
            lookUpBuyer.GetGridView().GetRowValues(lookUpBuyer.GetGridView().GetFocusedRowIndex(), 'NazivPrvi', OnGetRowValuesBuyer);
        }
        function OnGetRowValuesBuyer(values) {
            nazivKupca = values;

            clientTxtInquiryName.SetText(nazivKupca);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="InquiryCallbackPanel" runat="server" ClientInstanceName="InquiryCallbackPanel" OnCallback="InquiryCallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="InquiryCallbackPanel_EndCallback" />
        <PanelCollection>
            <dx:PanelContent>
                <div class="card">
                    <div class="card-header" style="background-color: #FAFCFE">
                        <div class="d-flex justify-content-between align-items-center">
                            <h6>Osnovni podatki</h6>
                            <a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                        </div>
                    </div>
                    <div class="card-body" style="background-color: #eef2f5d6;">
                        <div class="row m-0 pb-3">
                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="ŠTEVILKA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtInquiryNum" ClientInstanceName="clientTxtInquiryNum"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" ClientEnabled="false" BackColor="LightGray" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3"></div>
                            <div class="col-lg-3"></div>
                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="STATUS : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <%--<dx:ASPxTextBox runat="server" ID="txtStatus" ClientInstanceName="clientTxtStatus"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" ClientEnabled="false" BackColor="LightGray" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>--%>
                                        <dx:ASPxGridLookup ID="GridLookupStatus" runat="server" ClientInstanceName="lookUpStatus"
                                            KeyFieldName="StatusPovprasevanjaID" TextFormatString="{0}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px" ClientEnabled="false"
                                            OnDataBinding="GridLookupStatus_DataBinding"
                                            IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            <%--<ClientSideEvents ValueChanged="lookUpBuyer_ValueChanged" />--%>
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                                                <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" AutoFilterCondition="Contains"></Settings>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="40%"></dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Koda" FieldName="Koda" Width="30%"></dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 20px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="KUPEC : *"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxGridLookup ID="GridLookupBuyer" runat="server" ClientInstanceName="lookUpBuyer"
                                            KeyFieldName="TempID" TextFormatString="{0}" CssClass="text-box-input"
                                            Paddings-PaddingTop="0" Paddings-PaddingBottom="0" Width="100%" Font-Size="13px"
                                            OnLoad="ASPxGridLookupLoad_WidthLarge" OnDataBinding="GridLookupBuyer_DataBinding"
                                            IncrementalFilteringMode="Contains">
                                            <ClearButton DisplayMode="OnHover" />
                                            <ClientSideEvents ValueChanged="lookUpBuyer_ValueChanged" />
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <GridViewStyles>
                                                <Header CssClass="gridview-no-header-padding" ForeColor="Black"></Header>
                                            </GridViewStyles>
                                            <GridViewProperties>
                                                <SettingsBehavior EnableRowHotTrack="True" AllowEllipsisInText="true" AllowDragDrop="false" />
                                                <SettingsPager ShowSeparators="True" NumericButtonCount="3" EnableAdaptivity="true" />
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" ShowVerticalScrollBar="True"
                                                    ShowHorizontalScrollBar="true" VerticalScrollableHeight="200" AutoFilterCondition="Contains"></Settings>
                                            </GridViewProperties>
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="Naziv" FieldName="NazivPrvi" Width="40%"></dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" Width="30%"></dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn Caption="Telefon" FieldName="Telefon" Width="30%"></dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 20px;">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="NAZIV : *"></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtInquiryName" ClientInstanceName="clientTxtInquiryName"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" MaxLength="250" AutoCompleteType="Disabled">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-12 mb-2 mb-lg-0 ">
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="DATUM PREDVIDENE DOBAVE :"></dx:ASPxLabel>
                                    </div>
                                    <div class="col-1 p-0">
                                        <dx:ASPxDateEdit ID="DateEditSupplyDate" runat="server" EditFormat="Date" Width="100%"
                                            CssClass="text-box-input date-edit-padding" Font-Size="13px"
                                            ClientInstanceName="dateEditSupplyDate">
                                            <FocusedStyle CssClass="focus-text-box-input" />
                                            <CalendarProperties TodayButtonText="Danes" ClearButtonText="Izbriši" />
                                            <DropDownButton Visible="true"></DropDownButton>
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-12">
                                <div class="jumbotron pb-0 pt-2 mb-0" style="background-color: #e9ecef00">
                                    <h5 class="font-italic"><i class='fas fa-table'></i>Seznam artiklov</h5>
                                    <hr class="mb-4 w-100">
                                    <div class="row m-0 pb-3">
                                        <div class="col-12 mb-2 mb-lg-0 p-0">
                                            <dx:ASPxGridView ID="ASPxGridViewInquiryPosition" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridInquiryPosition"
                                                OnDataBinding="ASPxGridViewInquiryPosition_DataBinding" Width="100%"
                                                KeyFieldName="PovprasevanjePozicijaID" CssClass="gridview-no-header-padding"
                                                OnHtmlRowPrepared="ASPxGridViewInquiryPosition_HtmlRowPrepared" OnDataBound="ASPxGridViewInquiryPosition_DataBound">
                                                <ClientSideEvents RowDblClick="PopupHandeling_Click" />
                                                <Paddings Padding="0" />
                                                <Settings ShowVerticalScrollBar="True"
                                                    ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="300" AutoFilterCondition="Contains"
                                                    ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                <SettingsPager PageSize="50" ShowNumericButtons="true">
                                                    <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                    </PageSizeItemSettings>
                                                    <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                </SettingsPager>
                                                <SettingsBehavior AllowFocusedRow="true" />
                                                <Styles Header-Wrap="True">
                                                    <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                    <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                </Styles>
                                                <SettingsText EmptyDataRow="Trenutno ni podatka o artiklih. Dodaj novega." />
                                                <Columns>
                                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="PovprasevanjePozicijaID" Visible="false" SortOrder="Descending" />

                                                    <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="DobaviteljNaziv_P" Width="8%" />

                                                    <dx:GridViewDataTextColumn Caption="Artikli" FieldName="Artikli" Width="35%" />



                                                </Columns>
                                                <Templates>
                                                    <DetailRow>
                                                        <div class="row2">
                                                            <div class="col-xs-8">
                                                                <dx:ASPxGridView ID="ASPxGridViewSelectedArtikel" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridSelectedArtikel"
                                                                    OnBeforePerformDataSelect="ASPxGridViewSelectedArtikel_BeforePerformDataSelect" OnDataBinding="ASPxGridViewSelectedArtikel_DataBinding" Width="100%"  KeyFieldName="PovprasevanjePozicijaArtikelID" CssClass="gridview-no-header-padding">
                                                                    <Paddings Padding="0" />
                                                                <%--    <Settings ShowVerticalScrollBar="True"
                                                                        ShowFilterBar="Auto" ShowFilterRow="false" ShowFilterRowMenu="false" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                                                               --%>     <SettingsPager PageSize="50" ShowNumericButtons="false">
                                                                        <PageSizeItemSettings Visible="false" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
                                                                        </PageSizeItemSettings>
                                                                        <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
                                                                    </SettingsPager>
                                                                    <%--<SettingsBehavior AllowFocusedRow="true" />--%>
                                                                    <Styles Header-Wrap="True">
                                                                        <Header Paddings-PaddingTop="2" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
                                                                        <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
                                                                    </Styles>
                                                                    <SettingsText EmptyDataRow="Trenutno ni podatka o Artikel. Dodaj novega."/>                                                                    
                                                                   
                                                                    <Columns>
                                                                      

                                                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="PovprasevanjePozicijaArtikelID" Visible="false" SortOrder="Ascending" EditFormSettings-Visible="False" />

                                                                        <dx:GridViewDataTextColumn Caption="Kategorija" FieldName="KategorijaNaziv" Width="7%" EditFormSettings-Visible="False" />

                                                                        <dx:GridViewDataTextColumn Caption="Artikel" FieldName="Naziv" Width="30%" />

                                                                        <dx:GridViewDataTextColumn Caption="Kol. 1" FieldName="Kolicina1" Width="7%" EditFormSettings-Visible="true" PropertiesTextEdit-DisplayFormatString="{0:n2}" />

                                                                        <dx:GridViewDataTextColumn Caption="EM 1" FieldName="EnotaMere1" Width="5%" EditFormSettings-Visible="true" />

                                                                        <dx:GridViewDataTextColumn Caption="Kol. 2" FieldName="Kolicina2" Width="7%" EditFormSettings-Visible="true" PropertiesTextEdit-DisplayFormatString="{0:n2}" />

                                                                        <dx:GridViewDataTextColumn Caption="EM 2" FieldName="EnotaMere2" Width="5%" EditFormSettings-Visible="true" />

                                                                        <dx:GridViewDataTextColumn Caption="Opomba poz." FieldName="OpombaNarocilnica" Width="18%" EditFormSettings-Visible="true" />

                                                                    </Columns>
                                                                    <SettingsResizing ColumnResizeMode="NextColumn" Visualization="Live" />
                                                                </dx:ASPxGridView>
                                                            </div>
                                                        </div>
                                                    </DetailRow>
                                                </Templates>
                                                <SettingsDetail ShowDetailRow="true"/>
                                            </dx:ASPxGridView>

                                            <dx:ASPxPopupControl ID="PopupControlInquiryPos" runat="server" ContentUrl="InquiryPos_popup.aspx"
                                                ClientInstanceName="PopupControlInquiryPos" Modal="True" HeaderText="ARTIKEL"
                                                CloseAction="CloseButton" Width="1600px" Height="890px" PopupHorizontalAlign="WindowCenter"
                                                PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" AllowDragging="true" ShowSizeGrip="true"
                                                AllowResize="true" ShowShadow="true"
                                                OnWindowCallback="PopupControlInquiryPos_WindowCallback">
                                                <ClientSideEvents CloseButtonClick="OnPopupCloseButtonClick" />
                                                <ContentStyle BackColor="#F7F7F7">
                                                    <Paddings PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px"></Paddings>
                                                </ContentStyle>
                                            </dx:ASPxPopupControl>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <%-- Gumbi za Povpraševanje --%>
                                <div class="row m-0 align-items-center justify-content-start">
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Prekliči" AutoPostBack="false" OnClick="btnCancel_Click"
                                            Height="25" Width="110" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/cancel.png" UrlHottracked="../../Images/cancelHover.png" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Shrani" AutoPostBack="false" OnClick="btnSaveChanges_Click"
                                            Height="25" Width="110" ClientInstanceName="clientBtnSaveChanges" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                            <ClientSideEvents Click="ActionButton_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnSubmit" runat="server" Text="Oddaj povpraševanje" AutoPostBack="false" OnClick="btnSubmit_Click"
                                            Height="25" Width="110" ClientInstanceName="clientBtnSubmitInquiry" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/submitInquiry.png" UrlHottracked="../../Images/submitInquryHover.png" />
                                            <ClientSideEvents Click="SubmitInquiry_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnFinish" runat="server" Text="Oddaj naročilo" AutoPostBack="false" OnClick="btnFinishInquiry_Click"
                                            Height="25" Width="110" ClientInstanceName="clientBtnFinishInquiry" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/prevzem.png" UrlHottracked="../../Images/prevzemHover.png" />
                                            <ClientSideEvents Click="FinishInquiry_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <%-- Gumbi za artikle (povpraševanje pozicije) --%>
                                <div class="row m-0 align-items-center justify-content-end">
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnCopyPos" runat="server" Text="Kopiraj" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnCopy" UseSubmitBehavior="false" OnClick="btnCopyPos_Click">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/copy.png" UrlHottracked="../../Images/copyHover.png" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnAddPos" runat="server" Text="Dodaj" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnAddPos" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/addPopupMain.png" UrlHottracked="../../Images/addPopupMainHover.png" />
                                            <ClientSideEvents Click="PopupHandeling_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnEditPos" runat="server" Text="Spremeni" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnEditPos" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/editPopupMain.png" UrlHottracked="../../Images/editPopupMainHover.png" />
                                            <ClientSideEvents Click="PopupHandeling_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnDeletePos" runat="server" Text="Izbriši" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnDeletePos" UseSubmitBehavior="false">
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
    <div class="modal fade" id="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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
                    <button type="button" id="btnSubmitInquiry" class="btn btn-primary" data-dismiss="modal">Da, želim</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Prekliči</button>
                </div>

            </div>
        </div>
    </div>

    <!-- The Question Modal -->
    <div class="modal fade" id="questionModalFinish" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
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
                    <button type="button" id="btnFinishInquiry" class="btn btn-primary" data-dismiss="modal">Da, želim</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Prekliči</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
