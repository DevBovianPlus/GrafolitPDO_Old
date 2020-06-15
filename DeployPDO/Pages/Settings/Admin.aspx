<%@ Page Title="Povpraševanje" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="GrafolitPDO.Pages.Settings.Admin" %>

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
                            <h6>PDO - Povpraševanje dobaviteljem</h6>
                            <a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                        </div>
                    </div>
                    <div class="card-body" style="background-color: #eef2f5d6;">

                        <div class="row m-0 pb-3">
                            <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Izdelaj Naročilnico PDF in pošlji</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Izdelaj Naročilnico PDF in pošlji" data-toggle="popover" data-trigger="hover" data-content="Izdelaj Naročilnico PDF in pošlji">
                                            <dx:ASPxButton ID="btnIzdelajNarocilnicoInPoslji" runat="server" Text="Izdelaj in Pošji" Width="100" Theme="Moderno" OnClick="btnIzdelajNarocilnicoInPoslji_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Izvedi PDF kriranje na Pantheon</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Izvedi PDF kriranje na Pantheon" data-toggle="popover" data-trigger="hover" data-content="Run Pantheon">
                                            <dx:ASPxTextBox runat="server" ID="txtFile" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxTextBox runat="server" ID="txtArgs" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxButton ID="btnRunPantheon" runat="server" Text="Izvedi Pantheon" Width="100" Theme="Moderno" OnClick="btnRunPantheon_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Log datoteke</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Prodobi log file iz aplikacije in web servica" data-toggle="popover" data-trigger="hover" data-content="Izdelaj Naročilnico PDF in pošlji">
                                            <dx:ASPxButton ID="btnGetLogs" runat="server" Text="Log datoteke" Width="100" OnClick="btnGetLogs_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                             <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Change DB config</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Change DB config" data-toggle="popover" data-trigger="hover" data-content="Change DB config">
                                            <dx:ASPxTextBox runat="server" ID="txtConfigName" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxTextBox runat="server" ID="txtConfigValue" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                            <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Zamenjaj vrednost" Width="100" Theme="Moderno" OnClick="btnChangeConfig_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3 no-padding-left">
                                <div>
                                    <h5 class="no-margin"><em>Get config Value by name</em></h5>
                                </div>
                                <div class="panel panel-default" style="margin-top: 2px;">
                                    <div class="panel-body">
                                        <div style="display: inline-block;" title="Change DB config" data-toggle="popover" data-trigger="hover" data-content="Change DB config">
                                            <dx:ASPxTextBox runat="server" ID="txtConfigNameRet" CssClass="text-box-input" Font-Size="14px" Width="100%" Font-Bold="true">
                                                </dx:ASPxTextBox>
                                           
                                            <dx:ASPxButton ID="btnGetConfigValue" runat="server" Text="Dobi vrednost" Width="100" Theme="Moderno" OnClick="btnGetConfigVal_Click"
                                                AutoPostBack="false" UseSubmitBehavior="false">                                                
                                            </dx:ASPxButton>

                                            <dx:ASPxLabel ID="lblRezultat" runat="server" Font-Bold="true" Text=""></dx:ASPxLabel>
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
