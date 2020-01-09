<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>



<div class="dnnForm" id="GIBSform-settings">


    <fieldset>


<dnn:sectionhead id="sectMainSettings" cssclass="Head" runat="server" text="Module Settings" section="MainSection"
	includerule="False" isexpanded="True"  />

<div id="MainSection" runat="server">

    	     <div class="dnnFormItem">
            <dnn:label id="lblPdfFilesFolder" runat="server" controlname="ddlPdfFilesFolder" suffix=":" />
            <asp:DropDownList ID="ddlPdfFilesFolder" runat="server" DataTextField="FolderPath" DataValueField="FolderPath" />

		 </div>	

	     <div class="dnnFormItem">
            <dnn:label id="lblModuleInstructions" runat="server" controlname="txtModuleInstructions" suffix=":" />
            <dnn:TextEditor id="txtModuleInstructions" HtmlEncode="false" ChooseMode="false" ChooseRender="false" DefaultMode="Rich" runat="server" Height="400" Width="100%" />
		 </div>	 

         	<div class="dnnFormItem">					
	<dnn:label id="lblNumPerPage" runat="server" controlname="ddlNumPerPage" suffix=":"></dnn:label>
	<asp:DropDownList ID="ddlNumPerPage" runat="server">
		<asp:ListItem Text="10" Value="10"></asp:ListItem>
		<asp:ListItem Text="20" Value="20"></asp:ListItem>
		<asp:ListItem Text="50" Value="50"></asp:ListItem>
		<asp:ListItem Text="100" Value="100"></asp:ListItem>
			<asp:ListItem Text="200" Value="200"></asp:ListItem>
		<asp:ListItem Text="500" Value="500"></asp:ListItem>

		</asp:DropDownList>			
				
	</div>
              
	     <div class="dnnFormItem">
            <dnn:label id="lblCertBannerText" runat="server" controlname="txtCertBannerText" suffix=":" />
            <asp:TextBox ID="txtCertBannerText" runat="server" Rows="8" MaxLength="2000" TextMode="MultiLine" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblCertFooterText" runat="server" controlname="txtCertFooterText" suffix=":" />
            <asp:TextBox ID="txtCertFooterText" runat="server" Rows="8" MaxLength="2000" TextMode="MultiLine" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblCertReturnAddress" runat="server" controlname="txtCertReturnAddress" suffix=":" />
            <asp:TextBox ID="txtCertReturnAddress" runat="server" Rows="8" MaxLength="2000" TextMode="MultiLine" />
		 </div>	  

         	     <div class="dnnFormItem">
            <dnn:label id="lblCertWatermark" runat="server" controlname="txtCertWatermark" suffix=":" />
            <asp:TextBox ID="txtCertWatermark" runat="server" MaxLength="30" />
		 </div>		
                  <div class="dnnFormItem">
            <dnn:label id="lblCertLogo" runat="server" controlname="txtCertLogo" suffix=":" />
            <asp:TextBox ID="txtCertLogo" runat="server"  />  
                     
		 </div>	

	     <div class="dnnFormItem">
            <dnn:label id="lblSpecialInstructions" runat="server" controlname="txtSpecialInstructions" suffix=":" />
            <asp:TextBox ID="txtSpecialInstructions" runat="server" Rows="8" MaxLength="2000" TextMode="MultiLine" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblEmailFrom" runat="server" controlname="txtEmailFrom" suffix=":" />
            <asp:TextBox ID="txtEmailFrom" runat="server" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblEmailNotify" runat="server" controlname="txtEmailNotify" suffix=":" />
            <asp:TextBox ID="txtEmailNotify" runat="server" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblEmailSubject" runat="server" controlname="txtEmailSubject" suffix=":" />
            <asp:TextBox ID="txtEmailSubject" runat="server" />

		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblRedirectPage" runat="server" controlname="ddlPageList" suffix=":"></dnn:label>
            <asp:DropDownList ID="ddlPageList" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblAddUserRole" runat="server" controlname="ddlRoles" suffix=":" />
            <asp:DropDownList ID="ddlRoles" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
            </asp:DropDownList>
		 </div>	  

	     <div class="dnnFormItem">
            <dnn:label id="lblManageUserRole" runat="server" controlname="ddlManageUserRole" suffix=":" />
            <asp:DropDownList ID="ddlManageUserRole" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
            </asp:DropDownList>
		 </div>	  		 

</div>        


<dnn:sectionhead id="sectPayPalSettings" cssclass="Head" runat="server" text="Paypal Settings" section="PaypalSection"
	includerule="False" isexpanded="True"></dnn:sectionhead>

<div id="PaypalSection" runat="server">
            <div class="dnnFormItem">
            <dnn:label id="lblPayPalSandboxMode" runat="server" controlname="rblPayPalSandboxMode" suffix=":" />
            <asp:RadioButtonList ID="rblPayPalSandboxMode" runat="server" RepeatDirection="Horizontal">
             <asp:ListItem Text="Sandbox" Value="True" />
             <asp:ListItem Text="Live" Value="False" />
             </asp:RadioButtonList>

		 </div>	  
		 
		 
	     <div class="dnnFormItem">
            <dnn:label id="lblPayPalPayee" runat="server" controlname="txtPayPalPayee" suffix=":" />
            <asp:TextBox ID="txtPayPalPayee" runat="server" />

		 </div>	  		 


</div>        





    </fieldset>

</div>



