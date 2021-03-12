<%@ Page Title="Naročila" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OrderTable.aspx.cs" Inherits="GrafolitPDO.Pages.Order.OrderTable" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

         $(document).ready(function () {
            
            var submitOrder = GetUrlQueryStrings()['submitOrder'];
            if (submitOrder == '1') {
                ShowModal("Odlično!", "Uspešno ste ustvarili novo naročilnico!");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.submitOrder;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });

        function DoubleClick(s, e) {
            gridOrder.GetRowValues(gridOrder.GetFocusedRowIndex(), 'NarociloID', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            gridOrder.PerformCallback('DblClick;' + value);
        }

        function gridOrder_EndCallback(s, e) { }

        function gridOrder_SelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0) {
                var role = '<%= GrafolitPDO.Helpers.PrincipalHelper.IsUserAdmin()%>';
                var role2 ='<%= GrafolitPDO.Helpers.PrincipalHelper.IsUserSuperAdmin()%>';
                btnClearStatus.SetVisible(true);
                btnSendOrder.SetVisible(true);

                if ((role == "False") && (role2 == "False")) {
                    btnClearStatus.SetEnabled(false);
                }

            }
            else {
                btnClearStatus.SetVisible(false);
                btnSendOrder.SetVisible(false);
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
    <dx:ASPxGridView ID="ASPxGridViewOrder" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridOrder"
        OnDataBinding="ASPxGridViewOrder_DataBinding" Width="100%"
        KeyFieldName="NarociloID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewOrder_CustomCallback"
        OnHtmlRowPrepared="ASPxGridViewOrder_HtmlRowPrepared" OnCommandButtonInitialize="ASPxGridViewOrder_CommandButtonInitialize">
        <ClientSideEvents RowDblClick="DoubleClick" EndCallback="gridOrder_EndCallback" SelectionChanged="gridOrder_SelectionChanged" />
        <Paddings Padding="0" />
        <Settings ShowVerticalScrollBar="True"
            ShowFilterBar="Auto" ShowFilterRow="True" VerticalScrollableHeight="400" AutoFilterCondition="Contains"
            ShowFilterRowMenu="True" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
        <SettingsPager PageSize="50" ShowNumericButtons="true">
            <PageSizeItemSettings Visible="true" Items="50,80,100" Caption="Zapisi na stran : " AllItemText="Vsi">
            </PageSizeItemSettings>
            <Summary Visible="true" Text="Vseh zapisov : {2}" EmptyText="Ni zapisov" />
        </SettingsPager>
        <SettingsBehavior AllowFocusedRow="true" AllowSelectSingleRowOnly="true" />
        <Styles Header-Wrap="True">
            <Header Paddings-PaddingTop="5" HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></Header>
            <FocusedRow BackColor="#d1e6fe" Font-Bold="true" ForeColor="#606060"></FocusedRow>
        </Styles>
        <SettingsText EmptyDataRow="Trenutno ni podatka o naročilih." />
        <Columns>
            <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="50px" Caption="Izberi"></dx:GridViewCommandColumn>--%>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="PovprasevanjeID" Visible="false" Width="3%" />
            <dx:GridViewDataTextColumn Caption="ID" FieldName="NarociloID" Visible="false" Width="3%" />

            <dx:GridViewDataTextColumn Caption="Številka pov." FieldName="PovprasevanjeStevilka" Width="8%" />
            
            <dx:GridViewDataDateColumn Caption="Datum naročila" FieldName="ts" PropertiesDateEdit-DisplayFormatString="dd.MM yyyy HH:mm:ss" Width="15%" />

            <dx:GridViewDataTextColumn Caption="Številka naročila Pantheon" FieldName="NarociloStevilka_P" Width="15%" />

            <dx:GridViewDataTextColumn Caption="Dobavitelj" FieldName="DobaviteljID.NazivPrvi" Width="28%" />

            <dx:GridViewDataDateColumn Caption="Datum dobave" FieldName="DatumDobave" Width="12%" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy" />

            <dx:GridViewDataTextColumn Caption="Status" FieldName="StatusModel.Naziv" Width="20%" />
            <dx:GridViewDataTextColumn Caption="StatusKoda" FieldName="StatusModel.Koda" Width="15%" Visible="false" />

            <dx:GridViewDataTextColumn Caption="Opombe" FieldName="Opombe" Width="40%" />

            <dx:GridViewDataTextColumn Caption="Ustvaril" FieldName="NarociloIzdelal.Priimek" Width="10%" />

            <dx:GridViewDataTextColumn Caption="#" FieldName="P_UnsuccCountCreatePDFPantheon" Width="2%" />

        </Columns>
        <SettingsResizing ColumnResizeMode="NextColumn" Visualization="Live" />
    </dx:ASPxGridView>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="DeleteButtonElements">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false" OnClick="btnDelete_Click"
                    Height="25" Width="50" ClientVisible="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                </dx:ASPxButton>
            </span>

        </div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnClearStatus" runat="server" Text="Resetiraj" AutoPostBack="false"
                    Height="25" Width="90" ClientInstanceName="btnClearStatus" ClientVisible="false" OnClick="btnClearStatus_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/newOrder.png" UrlHottracked="../../Images/newOrderHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnSendOrder" runat="server" Text="Pošlji naročilnico" AutoPostBack="false"
                    Height="43" Width="90" ClientInstanceName="btnSendOrder" ClientVisible="false" OnClick="btnSendOrder_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/sendMailToCarriers.png" UrlHottracked="../../Images/sendMailToCarriersHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false" OnClick="btnAdd_Click"
                    Height="25" Width="90" ClientVisible="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnEdit" runat="server" Text="Podrobno" AutoPostBack="false" OnClick="btnEdit_Click"
                    Height="25" Width="90">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>
</asp:Content>
