<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MakeCertificate.ascx.cs" Inherits="GIBS.Modules.GiftCertificate.MakeCertificate" %>

<div>
<asp:Button ID="Button1" runat="server" Text="Make PDF" CssClass="btn btn-primary btn-default"
    onclick="Button1_Click" />
</div>

<div style="padding-top:20px; padding-bottom:20px;"><asp:HyperLink ID="HyperLinkPDF" Visible="false" runat="server" CssClass="btn btn-success" Target="_blank"><span class="glyphicon glyphicon-ok"></span> View PDF</asp:HyperLink></div>

<div align="right"><asp:HyperLink ID="HyperLinkManageCertificates" runat="server" CssClass="btn btn-default"> <span class="glyphicon glyphicon-chevron-left"></span>Return To List</asp:HyperLink></div>

