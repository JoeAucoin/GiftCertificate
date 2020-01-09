<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayPalParse.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.PayPalParse" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>


<style type="text/css">

.paypalResults
{
	display: block;
-webkit-box-sizing: content-box;
-moz-box-sizing: content-box;
box-sizing: content-box;
float: none;
z-index: auto;
width:320px;
height: auto;
position: static;
cursor: default;
opacity: 1;
margin: auto;
padding: 20px;
overflow: hidden;
border: 1px solid;
-webkit-border-radius: 4px;
border-radius: 4px;
font: normal 16px/1.2 "Times New Roman", Times, serif;
color: rgba(255,255,255,1);
text-align: center;
-o-text-overflow: ellipsis;
text-overflow: ellipsis;
background: #00cc66;
-webkit-box-shadow: 1px 1px 1px 0 rgba(0,0,0,0.3) ;
box-shadow: 1px 1px 1px 0 rgba(0,0,0,0.3) ;
text-shadow: 1px 1px 1px rgba(0,0,0,0.2) ;
-webkit-transition: none;
-moz-transition: none;
-o-transition: none;
transition: none;
-webkit-transform: none;
transform: none;
-webkit-transform-origin: 50% 50% 0;
transform-origin: 50% 50% 0;
	}


.hidden{
    display:none;
}

.unhidden{
    display:block;
}

</style>

<script type="text/javascript">


function unhide(divID) {
    var item = document.getElementById(divID);
    if (item) {
        if (item.className == 'hidden') {
            item.className = 'unhidden';
            
        } else {
            item.className = 'hidden';
           
        }
    } 
}


</script>

<asp:Label ID="lblDebug" runat="server" Text="" Visible="true" />

<div class="paypalResults"><asp:Label ID="lblResults" runat="server" Text="" />


</div>


<div id="show" style="margin-top:15px; text-align:center;" onclick="unhide('form-ship-address');"><a href="javascript:unhide('form-ship-address');">Change Shipping Address</a></div>






<div class="dnnForm hidden" id="form-ship-address">
    
    <div class="dnnFormItem dnnFormHelp dnnClear">
        <p class="dnnFormRequired"><asp:Label ID="Label1" runat="server" ResourceKey="Required Indicator" /></p>
    </div>
    <fieldset>
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

    </fieldset>
   
        <div style="text-align:center;"><asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-primary btn-lg" 
                ResourceKey="btnUpdateAddress" onclick="LinkButton1_Click"></asp:LinkButton></div>
       
    
</div>


<div style="width:100%; text-align:center; margin-top:20px;">
<asp:HyperLink ID="hlBuyAnother" runat="server" CssClass="btn btn-default">Buy Another</asp:HyperLink></div>
