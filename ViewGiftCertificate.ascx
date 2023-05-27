<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGiftCertificate.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.ViewGiftCertificate" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>


<style type="text/css">
    
     
     input[type=text], input[type=number], input[type=select], input[type=email], input[type=password], input[type=tel] {
  height: 40px;
  line-height: 40px; font-size: 17px;
  
  border: 1px solid #bbb;
}

.myRequiredFields
{
	 display:inline-block;
	 
	
	}

input[type=number]
{
	text-align:center; 
	margin-bottom:18px;
	border-radius:5px;
	}

  .ddlstyle
        {
            border:1px solid #7d6754;
            height: 40px;
            vertical-align: top;
            border-radius:5px;
            padding:6px;
            -webkit-appearance: none; 
             font-size:14px;
            text-indent: 0.01px;/*In Firefox*/
            text-overflow: ''; /*In Firefox*/
        }
     
     
</style>


<script type="text/javascript">
    // txtMailToName



    $(function () {
        $('input[id$=<%=txtToName.ClientID%>]').keyup(function () {
            var txtClone = $(this).val();
            $('input[id$=<%=txtMailToName.ClientID%>]').val(txtClone);
            Page_ClientValidate();
        });
    });
    
    //This function enable or disable the validator based on the user
    // MailToGroup
    // RequiredFieldCity RequiredFieldState
//    function EnableValidator() {
//        if (document.getElementById('<%=rdoRecipient.ClientID%>').checked) {
//            var txtClone = $("#<%=txtToName.ClientID%>").val();
//            $('input[id$=<%=txtMailToName.ClientID%>]').val(txtClone);

//        }
//        else {
//            var txtClone = $("#<%=txtFromName.ClientID%>").val();
//            $('input[id$=<%=txtMailToName.ClientID%>]').val(txtClone);
//        }
//    }      
</script> 

<div align="right"><asp:HyperLink ID="HyperLinkManageCertificates" runat="server" Visible="false" CssClass="btn btn-default"><span class="glyphicon glyphicon-chevron-right"></span> Manage Gift Certificates</asp:HyperLink></div>



<p><asp:label id="lblModuleInstructions" runat="server" 
        meta:resourcekey="lblModuleInstructionsResource1"></asp:label></p>
<div class="dnnForm" id="form-demo">

<asp:Label ID="lblFormMessage" runat="server" CssClass="dnnFormMessage dnnFormInfo" ResourceKey="lblFormMessage" Visible="False" />

    <fieldset>

        <div class="dnnFormItem">
        <dnn:label id="lblCertAmount" ControlName="txtCertAmount" runat="server" />
            <asp:DropDownList ID="ddlQuantity" runat="server" Visible="false">
            <asp:ListItem Text="1" Value="1" />
            <asp:ListItem Text="2" Value="2" />
            <asp:ListItem Text="3" Value="3" />
            <asp:ListItem Text="4" Value="4" />
            <asp:ListItem Text="5" Value="5" />
            <asp:ListItem Text="6" Value="6" />
            <asp:ListItem Text="7" Value="7" />
            <asp:ListItem Text="8" Value="8" />
            <asp:ListItem Text="9" Value="9" />
            <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        <asp:TextBox ID="txtCertAmount" runat="server" Width="80px" type="number" /> <span class="myRequiredFields"><asp:RequiredFieldValidator 
                ID="RequiredFieldValidator1" runat="server" Display="Dynamic" 
                ErrorMessage="* Required" ControlToValidate="txtCertAmount"  /><asp:RegularExpressionValidator
                    ID="RegularExpressionValidator1" runat="server" Display="Dynamic" 
                ErrorMessage="Numeric Only" ControlToValidate="txtCertAmount" 
                ValidationExpression="^\$?\d+(\.(\d{2}))?$" /></span>
        </div>

        <div class="dnnFormItem">
		<dnn:label id="lblToName" runat="server" />
		<asp:TextBox ID="txtToName" runat="server" CssClass="dnnFormRequired"  /> <asp:RequiredFieldValidator ID="RequiredFieldToName" runat="server" 
                ErrorMessage="* Required" ControlToValidate="txtToName" Display="Dynamic"  />
        </div>
		
		<div class="dnnFormItem">
		<dnn:label id="lblFromName" runat="server" />
        <asp:TextBox ID="txtFromName" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldFromName" runat="server" 
                ErrorMessage="* Required" ControlToValidate="txtFromName" Display="Dynamic" CssClass="myRequiredFields" />
        </div>
		
       <div class="dnnFormItem">
            <dnn:label id="lblFromEmail" runat="server" />
            <asp:TextBox ID="txtFromEmail" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                runat="server" ErrorMessage="* Required"  ControlToValidate="txtFromEmail" Display="Dynamic" CssClass="myRequiredFields" /><asp:RegularExpressionValidator
                    ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ControlToValidate="txtFromEmail" CssClass="myRequiredFields" ErrorMessage="Provide a Valid E-Mail" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"></asp:RegularExpressionValidator>
        </div>	
				
		<div class="dnnFormItem">
            <dnn:label id="lblFromPhone" runat="server" />
            <asp:TextBox ID="txtFromPhone" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator6"
                runat="server" ErrorMessage="* Required" ControlToValidate="txtFromPhone" Display="Dynamic" CssClass="myRequiredFields" />
        </div>


        <div>


		<div class="dnnFormItem">
		<dnn:label id="lblMailTo" runat="server" Text="Mail To:" />
		<asp:RadioButton ID="rdoRecipient" Checked="True" runat="server" 
                onclick="EnableValidator();" Text=" Recipient . . . or" GroupName="MailToGroup" CssClass="normalRadioButton" /> &nbsp; <asp:RadioButton ID="rdoPurchaser" runat="server" onclick="EnableValidator();" 
                Text=" send it to me." CssClass="normalRadioButton" GroupName="MailToGroup"  />
        </div>
		<div class="dnnFormItem">
            <dnn:label id="lblMailToName" runat="server" Text="Mailing To:" />
            <asp:TextBox ID="txtMailToName" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                runat="server" ErrorMessage="* Required" ControlToValidate="txtMailToName" Display="Dynamic" CssClass="myRequiredFields" Enabled="false" />
        </div>
		
		<div class="dnnFormItem">
		<dnn:label id="lblToAddress" runat="server" />
		<asp:TextBox ID="txtToAddress" runat="server" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ErrorMessage="* Required" ControlToValidate="txtToAddress" Display="Dynamic" CssClass="myRequiredFields" Enabled="false" />
        </div>
        		
	<div class="dnnFormItem">
	 <dnn:label id="lblToAddress1" runat="server" />
        <asp:TextBox ID="txtToAddress1" runat="server" />
     </div>
      
    <div class="dnnFormItem">  
        <dnn:label id="lblToCityStateZip" runat="server"></dnn:label>
        <asp:TextBox ID="txtToCity" runat="server" Width="20%" />&nbsp;<asp:DropDownList 
                ID="ddlStatesRecipient" runat="server" CssClass="ddlstyle" Width="10%">
</asp:DropDownList>&nbsp;<asp:TextBox ID="txtToZip" runat="server" Width="10%" />
        <asp:RequiredFieldValidator ID="RequiredFieldCity" runat="server" 
                ErrorMessage="* " ControlToValidate="txtToCity" 
                meta:resourcekey="RequiredFieldCityResource1" Enabled="false"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldState" runat="server" 
                ErrorMessage="* " ControlToValidate="ddlStatesRecipient" 
                meta:resourcekey="RequiredFieldStateResource1" Enabled="false"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldZip" runat="server" 
                ErrorMessage="* Required" ControlToValidate="txtToZip" meta:resourcekey="RequiredFieldZipResource1" Enabled="false"></asp:RequiredFieldValidator>
	</div>
		

</div>

   		<div class="dnnFormItem">

  <dnn:label id="lblSpecialInstructions" runat="server" 
        Text="Special Instructions" />

<asp:TextBox ID="txtNotes" runat="server" Height="200px" TextMode="MultiLine" />
	
	
        </div>
		
    </fieldset>

</div>
    <asp:HiddenField ID="txtItemID" runat="server" />

    <p class="text-center">
        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary btn-lg" 
                ResourceKey="btnSave" onclick="btnSave_Click" />
        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-default btn-lg" CausesValidation="False" Visible="false" 
        ResourceKey="btnCancel" onclick="btnCancel_Click" />
    </p>