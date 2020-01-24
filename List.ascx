<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="List.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.List" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css" />

<script type="text/javascript">

    $(function () {
        $("#txtStartDate").datepicker({
            numberOfMonths: 1,
            // minDate: 0,
            dateFormat: 'mm/dd/yy',
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        $("#txtEndDate").datepicker({
            dateFormat: 'mm/dd/yy',
            showButtonPanel: false
        });
    });

 </script>

<style type="text/css">
     .dnnFormItem.dnnFormHelp { margin-top: 2em; }


</style>



<p>

    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" 
        onselectedindexchanged="RadioButtonList1_SelectedIndexChanged" 
        RepeatDirection="Horizontal" Visible="false">
        <asp:ListItem Value="All">Show All</asp:ListItem>
        <asp:ListItem Selected="True" Value="0">Pending</asp:ListItem>
        <asp:ListItem Value="1">Fulfilled</asp:ListItem>
    </asp:RadioButtonList>
</p>


<div class="dnnForm" id="form-settings">

    <fieldset>

        <div class="dnnFormItem">	
            <dnn:Label ID="lblStartDate" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtStartDate" Suffix=":"  />
            <asp:TextBox ID="txtStartDate" runat="server" Width="220px" ClientIDMode="Static" /> &nbsp; <asp:Button ID="btnUpdateReport" resourcekey="btnUpdateReport" CausesValidation="False" runat="server" CssClass="btn btn-default" 
                Text="Update" onclick="btnUpdateReport_Click" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEndDate" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtEndDate" Suffix=":" />
            <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" Width="220px" />
        </div>			
	</fieldset>
</div>	

<div class="row">
    <div class="col-md-10 col-md-offset-1">
        <div class="table-responsive">
            <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True"
                DataKeyNames="ItemID" OnRowDeleting="GridView1_RowDeleting" 
                OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound"
                ShowFooter="True" EmptyDataText="No Records" EmptyDataRowStyle-HorizontalAlign="Center"
                AutoGenerateColumns="False"
                CssClass="table table-striped table-bordered table-list table-condensed">

                <Columns>


                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" CommandArgument='<%# Eval("ItemID") %>' CommandName="Edit" runat="server">View</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonPrint" CommandArgument='<%# Eval("ItemID") %>' CommandName="Update" runat="server">Print</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:BoundField HeaderText="Purchaser" DataField="FromName"></asp:BoundField>
                    <asp:BoundField HeaderText="Recieient" DataField="ToName"></asp:BoundField>
                  

                    <asp:TemplateField HeaderText="Amount" SortExpression="total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate><b><asp:Label ID="lbltotal" runat="server" /></b></FooterTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# String.Format("{0:c}", Eval("CertAmount")) %>' />
                        </ItemTemplate>
                        <ItemStyle Width="100px" />

                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Paid" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="Image2" runat="server" ImageUrl='<%# (Eval("PaypalPaymentState").Equals("approved") ? "~/DesktopModules/GIBS/GiftCertificate/Images/yes.png" : "~/DesktopModules/GIBS/GiftCertificate/Images/no.png")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Processed" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# (Eval("isProcessed").Equals(true) ? "~/DesktopModules/GIBS/GiftCertificate/Images/yes.png" : "~/DesktopModules/GIBS/GiftCertificate/Images/no.png")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderStyle-HorizontalAlign="Center" HeaderText="Created On" DataField="CreatedDate"></asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" OnClientClick="return confirm('This action is irreversible!    Are you sure you want to delete this record?');"
                                CommandArgument='<%# Eval("ItemID") %>'
                                CommandName="Delete" runat="server" meta:resourcekey="LinkButton1Resource1">
             Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <dnn:PagingControl id="PagingControl1" runat="server" Visible="False"></dnn:PagingControl>

        </div>
    </div>
</div>