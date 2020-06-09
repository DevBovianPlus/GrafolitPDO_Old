<%@ Page Title="Povpraševanja" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PurchaseTable.aspx.cs" Inherits="GrafolitPDO.Pages.Order.PurchaseTable" %>

<%@ MasterType VirtualPath="~/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentHolder" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var submitInquiry = GetUrlQueryStrings()['submitInquiry'];
            var submitOrder = GetUrlQueryStrings()['submitOrder'];
            
            if (submitInquiry == '1') {
                ShowModal("Odlično!", "Uspešno ste poslali povpraševanje vsem dobaviteljem!");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.submitInquiry;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
            else if (submitOrder == '1') {
                ShowModal("Odlično!", "Uspešno ste ustvarili novo naročilo!");

                //we delete successMessage query string so we show modal only once!
                var params = QueryStringsToObject();
                delete params.submitOrder;
                var path = window.location.pathname + '?' + SerializeQueryStrings(params);
                history.pushState({}, document.title, path);
            }
        });

        function DoubleClick(s, e) {
            gridInquiry.GetRowValues(gridInquiry.GetFocusedRowIndex(), 'PovprasevanjeID', OnGetRowValues);
        }

        function OnGetRowValues(value) {
            gridInquiry.PerformCallback('DblClick;' + value);
        }

        function gridInquiry_EndCallback(s, e) { }

        function gridInquiry_SelectionChanged(s, e) {
            if (s.GetSelectedRowCount() > 0) {
                btnOrder.SetVisible(true);
            }
            else {
                btnOrder.SetVisible(false);
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
    <dx:ASPxGridView ID="ASPxGridViewIPurchase" runat="server" EnableCallbackCompression="true" ClientInstanceName="gridInquiry"
        OnDataBinding="ASPxGridViewIPurchase_DataBinding" Width="100%"
        KeyFieldName="PovprasevanjeID" CssClass="gridview-no-header-padding" OnCustomCallback="ASPxGridViewIPurchase_CustomCallback"
        OnCommandButtonInitialize="ASPxGridViewIPurchase_CommandButtonInitialize"
        OnHtmlRowPrepared="ASPxGridViewIPurchase_HtmlRowPrepared">
        <ClientSideEvents RowDblClick="DoubleClick" EndCallback="gridInquiry_EndCallback" SelectionChanged="gridInquiry_SelectionChanged" />
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
        <SettingsText EmptyDataRow="Trenutno ni podatka o povpraševanjih. Dodaj novo." />
        <Columns>
            <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true" Width="80px" Caption="Izberi"></dx:GridViewCommandColumn>--%>

            <dx:GridViewDataTextColumn Caption="ID" FieldName="PovprasevanjeID" Visible="false" />

            <dx:GridViewDataTextColumn Caption="Številka povpr." FieldName="PovprasevanjeStevilka" Width="8%" />

            <dx:GridViewDataTextColumn Caption="Naziv" FieldName="Naziv" Width="30%" />

            <dx:GridViewDataTextColumn Caption="Kupec" FieldName="Kupec.NazivPrvi" Width="35%" />

            <dx:GridViewDataTextColumn Caption="Status" FieldName="StatusPovprasevanja.Naziv" Width="12%" /> 
            
            <dx:GridViewDataTextColumn Caption="StatusKoda" FieldName="StatusPovprasevanja.Koda" Width="15%" Visible="false" />

            <dx:GridViewDataDateColumn Caption="Datum oddaje" FieldName="DatumOddajePovprasevanja" Width="15%" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy"/>
            
            <dx:GridViewDataDateColumn Caption="Datum dobave" FieldName="DatumPredvideneDobave" Width="15%" PropertiesDateEdit-DisplayFormatString="dd. MMMM yyyy" />

            <dx:GridViewDataTextColumn Caption="Naročilo" FieldName="NarociloID" Width="10%" />

            <dx:GridViewDataTextColumn Caption="Zakleni" FieldName="Zakleni" Width="10%" Visible="false" />

        </Columns>
    </dx:ASPxGridView>
    <div class="AddEditButtonsWrap medium-margin-l medium-margin-r">
        <div class="DeleteButtonElements">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnDelete" runat="server" Text="Izbriši" AutoPostBack="false" OnClick="btnDelete_Click"
                    Height="25" Width="50" ClientVisible ="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/trash.png" UrlHottracked="../../Images/trashHover.png" />
                </dx:ASPxButton>
            </span>

        </div>
        <div class="AddEditButtonsElements">
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnOrder" runat="server" Text="Naročilo" AutoPostBack="false"
                    Height="25" Width="90" ClientInstanceName="btnOrder" ClientVisible="false" OnClick="btnOrder_Click">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/newOrder.png" UrlHottracked="../../Images/newOrderHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnAdd" runat="server" Text="Dodaj" AutoPostBack="false" OnClick="btnAdd_Click"
                    Height="25" Width="90" ClientVisible ="false">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/add.png" UrlHottracked="../../Images/addHover.png" />
                </dx:ASPxButton>
            </span>
            <span class="AddEditButtons">
                <dx:ASPxButton ID="btnEdit" runat="server" Text="Spremeni" AutoPostBack="false" OnClick="btnEdit_Click"
                    Height="25" Width="90">
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Image Url="../../../Images/edit.png" UrlHottracked="../../Images/editHover.png" />
                </dx:ASPxButton>
            </span>
        </div>
    </div>

    <!-- The Modal -->
    <div class="modal fade" id="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header align-items-center" style="background-color: #88e188;">
                    <i class="far fa-check-circle" style="font-size:60px; color:#FFF; position:relative; left:43%"></i>
                    <button type="button" class="close" data-dismiss="modal">×</button>
                </div>
                <h4 class="modal-title text-center"></h4>
                <!-- Modal body -->
                <div class="modal-body text-center">
                </div>

                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Zapri</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
