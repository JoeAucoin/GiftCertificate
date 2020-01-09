<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGiftCert.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.EditGiftCert" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>


<script type="text/javascript">
    function SelectAll(id) {
        document.getElementById(id).focus();
        document.getElementById(id).select();
    }
</script>


<div style="font-size:1.5em; color: Green; text-align:center;"><asp:Label ID="lblOrderStatus" runat="server" Text="lblOrderStatus" /></div>


<div class="dnnForm" id="GIBSform-settings">


    <fieldset>


        <dnn:sectionhead id="sect1" cssclass="Head" runat="server" text="Certificate Information" section="Section1"
            includerule="False" isexpanded="True" />

        <div id="Section1" runat="server">


            <div class="dnnFormItem">
                <dnn:label id="lblIsProcessed" runat="server" />
                <asp:RadioButtonList runat="server" ID="isProcessed" CssClass="NormalTextBox" RepeatDirection="Horizontal">
                    <asp:ListItem Value="false" Text="No" />
                    <asp:ListItem Value="true" Text="Yes" />
                </asp:RadioButtonList>
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblEmailPurchaser" runat="server" />
                <asp:CheckBox ID="cbxEmailPurchaser" runat="server" />
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblCertAmount" runat="server" />
                <asp:TextBox ID="txtCertAmount" runat="server" />&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                    ErrorMessage="* Required" ControlToValidate="txtCertAmount" />
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblRecipientName" runat="server" />
                <asp:TextBox ID="txtRecipientName" runat="server" />
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblSpecialInstructions" runat="server" />
                <asp:TextBox ID="txtSpecialInstructions" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div>


        </div>


        <dnn:sectionhead id="sect2" cssclass="Head" runat="server" text="Purchaser Information" section="Section2"
            includerule="False" isexpanded="True">
        </dnn:sectionhead>

        <div id="Section2" runat="server">
            <div class="dnnFormItem">
                <dnn:label id="lblFromName" runat="server"></dnn:label>
                <asp:TextBox ID="txtFromName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldFromName" runat="server" ErrorMessage="* Required" ControlToValidate="txtFromName"></asp:RequiredFieldValidator>
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblFromPhone" runat="server"></dnn:label>
                <asp:TextBox ID="txtFromPhone" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldFromPhone" runat="server" ErrorMessage="* Required" ControlToValidate="txtFromPhone"></asp:RequiredFieldValidator>
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblFromEmail" runat="server"></dnn:label>
                <asp:TextBox ID="txtFromEmail" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldFromEmail" runat="server" ErrorMessage="* Required" ControlToValidate="txtFromEmail"></asp:RequiredFieldValidator>
            </div>

        </div>

        <dnn:sectionhead id="sect3" cssclass="Head" runat="server" text="Mailing Information" section="Section3"
            includerule="False" isexpanded="False">
        </dnn:sectionhead>

        <div id="Section3" runat="server">
            <div class="dnnFormItem">
                <dnn:label id="lblToName" runat="server"></dnn:label>
                <asp:TextBox ID="txtMailToName" runat="server" />
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblToAddress" runat="server"></dnn:label>
                <asp:TextBox ID="txtToAddress" runat="server" />
            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblToAddress1" runat="server"></dnn:label>
                <asp:TextBox ID="txtToAddress1" runat="server" />
            </div>


            <div class="dnnFormItem">
                <dnn:label id="lblToCityStateZip" runat="server"></dnn:label>
                <asp:TextBox ID="txtToCity" runat="server" Width="26%"></asp:TextBox>&nbsp;<asp:DropDownList ID="ddlStatesRecipient" runat="server" Width="90px">
                </asp:DropDownList>&nbsp;<asp:TextBox ID="txtToZip" runat="server" Width="60px"></asp:TextBox>
            </div>



        </div>



        <div class="dnnFormItem">
            <dnn:label id="lblMailingLabel" runat="server" />
            <asp:TextBox ID="txtMailingAddresses" runat="server" Height="110px" TextMode="MultiLine" ClientIDMode="Static" onClick="SelectAll('txtMailingAddresses');" />
        </div>


        <dnn:sectionhead id="sect4" cssclass="Head" runat="server" text="Paypal Information" section="Section4"
            includerule="False" isexpanded="False">
        </dnn:sectionhead>

        <div id="Section4" runat="server">
            <div class="dnnFormItem">
                <dnn:label id="lblPaypalPaymentState" runat="server" />
                <asp:TextBox ID="txtPaypalPaymentStatee" runat="server" />

            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblPP_PaymentId" runat="server" />
                <asp:TextBox ID="txtPP_PaymentId" runat="server" />

            </div>

            <div class="dnnFormItem">
                <dnn:label id="lblPP_Response" runat="server" />
                <asp:TextBox ID="txtPP_Response" runat="server" TextMode="MultiLine" />

            </div>

        </div>


    </fieldset>

</div>





<p>
    <asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" resourcekey="cmdUpdate" runat="server"  text="Update" OnClick="cmdUpdate_Click"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="dnnSecondaryAction" id="cmdDelete" resourcekey="cmdDelete" runat="server" text="Delete" causesvalidation="False" OnClick="cmdDelete_Click"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" resourcekey="cmdCancel" runat="server" causesvalidation="False" OnClick="cmdCancel_Click"></asp:linkbutton>&nbsp;
    
</p>

   
