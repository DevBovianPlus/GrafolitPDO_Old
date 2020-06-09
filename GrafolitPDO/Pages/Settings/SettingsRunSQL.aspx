<%@ Page Title="Povpraševanje" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SettingsRunSQL.aspx.cs" Inherits="GrafolitPDO.Pages.Settings.SettingsRunSQL" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        

 
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
                            <h6>Zaženi SQL</h6>
                            <a data-toggle="collapse" href="#collapseBasicData" aria-expanded="true" aria-controls="collapseBasicData"><i style="font-size: 24px; color: #209FE8;" class='fas fa-angle-down'></i></a>
                        </div>
                    </div>
                    <div class="card-body" style="background-color: #eef2f5d6;">                                                    
                        <div class="row m-0 pb-3">
                            <div class="col-lg-6">
                                <div class="row m-0 align-items-center">
                                    <div class="col-0 p-0" style="margin-right: 125px;">
                                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="SQL Stavek : "></dx:ASPxLabel>
                                    </div>
                                    <div class="col p-0">
                                        <dx:ASPxMemo ID="memSQL" runat="server" Width="100%" Rows="6" MaxLength="1000" CssClass="text-box-input" AutoCompleteType="Disabled"
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
                                        <dx:ASPxButton ID="btnRunSQL" runat="server" Text="Run" AutoPostBack="false" OnClick="btnRunSQL_Click"
                                            Height="25" Width="110" ClientInstanceName="clientBtnRun" UseSubmitBehavior="false">
                                            <Paddings PaddingLeft="10" PaddingRight="10" />
                                            <Image Url="../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
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
