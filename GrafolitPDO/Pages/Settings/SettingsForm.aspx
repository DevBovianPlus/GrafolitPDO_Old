<%@ Page Title="Povpraševanje" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SettingsForm.aspx.cs" Inherits="GrafolitPDO.Pages.Settings.SettingsForm" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        var postbackInitiated = false;
        function CheckFieldValidation() {
            var process = false;

            var inputItems = [clientTxtInquiryNum, clientTxtPrefix];
            var memoItems = null;
            var dateItems = null;

            process = InputFieldsValidation(null, inputItems, dateItems, memoItems, /*comboBoxItems*/null, null);

            if (clientBtnSaveChanges.GetText() == 'Izbriši')
                process = true;

            return process;
        }

        function ActionButton_Click(s, e) {
            var process = CheckFieldValidation();

            if (process) {
                LoadingPanel.Show();
                SettingsCallbackPanel.PerformCallback('SaveSettings');
            }
            else
                e.processOnServer = false;
        }

        function SettingsCallbackPanel_EndCallback(s, e) {
            LoadingPanel.Hide();
            ShowModal('Nastavitve shranjene!', 'Uspešno ste shranili nastavitve.');
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

        function ShowModal(title, message) {
            $('.modal-title').empty();
            $('.modal-title').append(title);

            $('.modal-body').empty();
            $('.modal-body').append(message);

            $('#modal').modal("show");
        }

        function HideModal() {
            $('#modal').modal("hide");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <dx:ASPxCallbackPanel ID="SettingsCallbackPanel" runat="server" ClientInstanceName="SettingsCallbackPanel" OnCallback="SettingsCallbackPanel_Callback">
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="SettingsCallbackPanel_EndCallback" />
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
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="ŠTEVILKA POVPRAŠEVANJA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtInquiryNum" ClientInstanceName="clientTxtInquiryNum"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents KeyPress="isNumberKey_int" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="PREDPONA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtPrefix" ClientInstanceName="clientTxtPrefix"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 65px;">
                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="POŠILJANJE POŠTE : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxCheckBox ID="CheckBoxMailSending" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="STRŽNIK SMTP : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtSMTPServer" ClientInstanceName="txtSMTPServer"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 40px;">
                                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Text="ŠIFRIRANJE POŠTE SSL : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxCheckBox ID="CheckBoxSSLEncrypting" runat="server" ToggleSwitchDisplayMode="Always"></dx:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 mb-2 mb-lg-0">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 15px;">
                                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="VRATA : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxTextBox runat="server" ID="txtPort" ClientInstanceName="txtPort"
                                            CssClass="text-box-input" Font-Size="13px" Width="100%" AutoCompleteType="Disabled" Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                            <ClientSideEvents KeyPress="isNumberKey_int" />
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 125px;">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="OPOMBE : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="MemoNotes" runat="server" Width="100%" Rows="6" MaxLength="1000" CssClass="text-box-input" AutoCompleteType="Disabled"
                                            Font-Bold="true">
                                            <FocusedStyle CssClass="focus-text-box-input"></FocusedStyle>
                                        </dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <%-- Gumbi za Povpraševanje --%>
                                <div class="row m-0 align-items-center justify-content-start">
                                    <div class="col-0 p-0 pr-2">
                                        <dx:ASPxButton ID="btnSaveChanges" runat="server" Text="Shrani" AutoPostBack="false"
                                            Height="25" Width="110" ClientInstanceName="clientBtnSaveChanges" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                                            <ClientSideEvents Click="ActionButton_Click" />
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
</asp:Content>
