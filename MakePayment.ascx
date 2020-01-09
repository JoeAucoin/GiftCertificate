<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MakePayment.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.MakePayment" %>




<div style="font-size:1.4em; color:Red;"><asp:Label ID="lblDebug" runat="server" Text=""  /></div>



<h2>Review Order</h2>

<h3>Amount of Certificate: $<asp:Label ID="lblCertAmount" runat="server" Text="lblCertAmount" /></h3>

<h4>Recipient</h4>
<div><asp:Label ID="lblRecipient" runat="server" Text="lblRecipient" /></div>

<h4>Purchaser</h4>
<div><asp:Label ID="lblPurchaser" runat="server" Text="lblPurchaser" /></div>


<div style="display:none;">
<h4>Mailing To</h4>
<div><asp:Label ID="lblMailingTo" runat="server" Text="lblMailingTo" /></div>
</div>

<h4>Special Instructions/Notes</h4>
<div><asp:Label ID="lblNotes" runat="server" Text="lblNotes" />
    <br />&nbsp;<br />
</div>

<p class="text-center">

<asp:HyperLink ID="flowGo" runat="server" CssClass="btn btn-primary btn-lg" Text="Complete Payment"></asp:HyperLink>
&nbsp;&nbsp;&nbsp;&nbsp;
<asp:LinkButton ID="LinkButtonMakeCorrections" runat="server" 
    CssClass="btn btn-default btn-lg" onclick="LinkButtonMakeCorrections_Click">Make Corrections</asp:LinkButton></p>
