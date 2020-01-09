using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Configuration;
//using PayPal.AdaptivePayments.Model;
//using PayPal.AdaptivePayments;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using GIBS.Modules.GiftCertificate.Components;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common;
using System.Globalization;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using PdfSharp;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using PdfSharp.Pdf.IO;

namespace GIBS.Modules.GiftCertificate
{
    public partial class MakeCertificate : PortalModuleBase
    {

        int itemId = Null.NullInteger;
        string _CertReturnAddress = "";
        string _CertBannerText = "";
        string _CertFooterText = "";
        string _CertNotes = "";
        string _CertWatermark = "";
        string _CertLogo = "";
        public bool _isAuthorized = false;
        public string _PdfFilesFolder = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            HyperLinkManageCertificates.NavigateUrl = EditUrl("List");
            LoadSettings();

            if (_isAuthorized)
            {

            }
            else
            {
                Response.Redirect(Globals.NavigateURL("Access Denied"), true);
            }

            if (!IsPostBack)
            {
                try
                {



                    if (Request.QueryString["ItemId"] != null)
                    {
                        itemId = Int32.Parse(Request.QueryString["ItemId"]);
                        
                  //     this.CreateLink(item, true);
                    }




                }
                catch (Exception ex)
                {
                    Exceptions.ProcessModuleLoadException(this, ex);
                }
            }
        }


        public void MakeGiftCert(int itemID)
        {
            try
            {


                GiftCertificateInfo item;
                //load the item
                GiftCertificateController controller = new GiftCertificateController();
                item = controller.GetGiftCert(itemId);

                string CertAmountWords = "";
                string MailToAddressField = "";
                string CertAmountNumber = "";

                if (item != null)
                {

                    if (item.PaypalPaymentState.ToString().Length == 0)
                    {
                        Response.Redirect(Globals.NavigateURL("Access Denied"), true);
                    }

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                    CertAmountWords = textInfo.ToTitleCase(NumberToWords(Int32.Parse(item.CertAmount.ToString())).ToString()).ToString() + " Dollars" + Environment.NewLine;
                    CertAmountNumber = "$" + String.Format("{0:f2}", item.CertAmount).ToString();

                    MailToAddressField = item.MailTo.ToString() + Environment.NewLine
                        + item.MailToAddress.ToString() + Environment.NewLine
                        + item.MailToCity.ToString() + ", " + item.MailToState.ToString() + " " + item.MailToZip.ToString();

                    _CertNotes = item.Notes.ToString();

                }


                string myPortalName = this.PortalSettings.PortalName.ToString();

                
                
                Document document = new Document();
                document.Info.Author = "Joseph Aucoin";
                document.Info.Keywords = "Gift Certificate";
                document.Info.Title = myPortalName.ToString() + " Gift Certificate";

                // Get the A4 page size
                MigraDoc.DocumentObjectModel.Unit width, height;
                PageSetup.GetPageSize(PageFormat.Letter, out width, out height);

            


                // Add a section to the document and configure it such that it will be in the centre
                // of the page
                Section section = document.AddSection();
                section.PageSetup.PageHeight = height;
                section.PageSetup.PageWidth = width;
                section.PageSetup.LeftMargin = 20;
                section.PageSetup.RightMargin = 10;
                section.PageSetup.TopMargin = 20;   // height / 2;


               
                //++++++++++++++++++++++++++++++
                //++++++++++++++++++++++++++++++
                //++++++++++++++++++++++++++++++
                // ADD LOGO
                string myLogo = PortalSettings.HomeDirectoryMapPath + _CertLogo.ToString(); // "Images\\Chapins-Logo.png";
                MigraDoc.DocumentObjectModel.Shapes.Image image = section.Headers.Primary.AddImage(myLogo.ToString());
             
                image.LockAspectRatio = true;
                image.RelativeVertical = MigraDoc.DocumentObjectModel.Shapes.RelativeVertical.Line;
                image.RelativeHorizontal = MigraDoc.DocumentObjectModel.Shapes.RelativeHorizontal.Margin;
                image.Top = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Top;
                image.Left = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Right;
                image.WrapFormat.Style = MigraDoc.DocumentObjectModel.Shapes.WrapStyle.Through;

                MigraDoc.DocumentObjectModel.Tables.Table HeaderTable = new MigraDoc.DocumentObjectModel.Tables.Table();
                HeaderTable.Borders.Width = 0; // Default to show borders 1 pixel wide Column
                HeaderTable.LeftPadding = 10;
                HeaderTable.RightPadding = 10;
                HeaderTable.Borders.Left.Visible = false;
                HeaderTable.Borders.Right.Visible = false;

     
                // Add 1 columns
                float myColumnWidth = ((section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin) / 2);   // ((width / 2) - 10);
                MigraDoc.DocumentObjectModel.Tables.Column column0 = HeaderTable.AddColumn(myColumnWidth);

                MigraDoc.DocumentObjectModel.Tables.Row row1 = HeaderTable.AddRow();
                row1.Cells[0].Elements.AddParagraph(_CertReturnAddress.ToString());
                row1.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                
                section.Add(HeaderTable);

                /// SPACING
                MigraDoc.DocumentObjectModel.Paragraph paragraph = section.AddParagraph();
                paragraph.Format.LineSpacingRule = MigraDoc.DocumentObjectModel.LineSpacingRule.Exactly;
                paragraph.Format.LineSpacing = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(30.0);

             //   section.Add(paragraph);

                //++++++++++++++++++++++++++++++
                // ADD ANOTHER TABLE FOR MAIL TO ADDRESS
                // Create a table so that we can draw the horizontal lines
                MigraDoc.DocumentObjectModel.Tables.Table tableMailToAddress = new MigraDoc.DocumentObjectModel.Tables.Table();
                tableMailToAddress.Borders.Width = 0; // Default to show borders 1 pixel wide Column
                tableMailToAddress.LeftPadding = 20;
                tableMailToAddress.RightPadding = 10;

              //  float myColumnWidth100percent = (width - 10);
                var column = tableMailToAddress.AddColumn((section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin));
        
                double fontHeight = 20;
                Font font = new Font("Times New Roman", fontHeight);

                // Add a row with a single cell for the first line
                MigraDoc.DocumentObjectModel.Tables.Row row = tableMailToAddress.AddRow();
                MigraDoc.DocumentObjectModel.Tables.Cell cell = row.Cells[0];


                cell = row.Cells[0];

                cell.Format.Font.Color = Colors.Black;
                cell.Format.Alignment = ParagraphAlignment.Left;
                cell.Format.Font.ApplyFont(font);

                cell.AddParagraph(MailToAddressField.ToString());

                section.Add(tableMailToAddress);

                // ADD SPACER+++++++++++++++++++++++++++++++++++++++
                MigraDoc.DocumentObjectModel.Paragraph paragraph1 = section.AddParagraph();
                paragraph1.Format.LineSpacingRule = MigraDoc.DocumentObjectModel.LineSpacingRule.Exactly;
                paragraph1.Format.LineSpacing = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(20.0);



                // ADD ANOTHER TABLE FOR CERT
                // Create a table so that we can draw the horizontal lines
                MigraDoc.DocumentObjectModel.Tables.Table tableCert = new MigraDoc.DocumentObjectModel.Tables.Table();
                tableCert.Borders.Width = 0; // Default to show borders 1 pixel wide Column
                tableCert.LeftPadding = 10;
                tableCert.RightPadding = 10;
                tableCert.Borders.Right.Visible = true;

                //  float myColumnWidth100percent = (width - 10);
                float sectionWidth = section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;
                float columnWidth = sectionWidth;


                var columnCert = tableCert.AddColumn(columnWidth);
                columnCert.Format.Alignment = ParagraphAlignment.Center;

                double fontHeightCert = 30;
                Font fontCert = new Font("Times New Roman", fontHeightCert);

                // Add a row with a single cell for the first line
                MigraDoc.DocumentObjectModel.Tables.Row rowCert = tableCert.AddRow();
                MigraDoc.DocumentObjectModel.Tables.Cell cellCert = rowCert.Cells[0];

                cellCert.Format.Font.Color = Colors.Black;
                cellCert.Format.Font.ApplyFont(fontCert);
                cellCert.Borders.Left.Visible = false;
                cellCert.Borders.Right.Visible = false;
                cellCert.Borders.Bottom.Visible = false;

                cellCert.AddParagraph(_CertBannerText.ToString());

                // Add a row with a single cell for the second line
                rowCert = tableCert.AddRow();
                cellCert = rowCert.Cells[0];

                cellCert.Format.Font.Color = Colors.Black;
                cellCert.Format.Alignment = ParagraphAlignment.Center;
                cellCert.Format.Font.ApplyFont(fontCert);
                cellCert.Borders.Left.Visible = false;
                cellCert.Borders.Right.Visible = false;
                cellCert.Borders.Top.Visible = false;

                cellCert.AddParagraph(CertAmountWords.ToString());

                // 3RD LINE
                // Add a row with a single cell for the third line
                rowCert = tableCert.AddRow();
                cellCert = rowCert.Cells[0];

                
                Font fontCertDetails = new Font("Times New Roman", 20);
                cellCert.Format.Font.Color = Colors.Black;
               
                cellCert.Format.Alignment = ParagraphAlignment.Left;
                cellCert.Format.Font.ApplyFont(fontCertDetails);
                cellCert.Borders.Left.Visible = false;
                cellCert.Borders.Right.Visible = false;
                cellCert.Borders.Top.Visible = false;
        
                cellCert.Format.SpaceBefore = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(10.0);
                cellCert.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(15.0);
                cellCert.AddParagraph("Date:" + item.CreatedDate.ToShortDateString() + Environment.NewLine + "Presented to: " + item.ToName.ToString() + Environment.NewLine +
                    "From: " + item.FromName.ToString() + Environment.NewLine + "Amount: " + CertAmountNumber.ToString());


////////////////////////////////
                // 4th LINE
                // Add a row with a single cell for the forth line
                rowCert = tableCert.AddRow();
                cellCert = rowCert.Cells[0];


                Font fontCertSignature = new Font("Arial", 12);
                cellCert.Format.Font.Color = Colors.Black;
               
                cellCert.Format.Alignment = ParagraphAlignment.Left;
                cellCert.Format.Font.ApplyFont(fontCertSignature);
                cellCert.Borders.Left.Visible = false;
                cellCert.Borders.Right.Visible = false;
                cellCert.Borders.Top.Visible = false;
         
                cellCert.Format.SpaceBefore = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(10.0);
                cellCert.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(15.0);
                cellCert.AddParagraph("Authorized Signature: __________________________________________________");

                ////////////////////////////////
                // 5th LINE
                // Add a row with a single cell for the fifth line
                rowCert = tableCert.AddRow();
                cellCert = rowCert.Cells[0];


                Font fontCertInvoiceNumber = new Font("Arial", 10);
                cellCert.Format.Font.Color = Colors.Black;
               
                cellCert.Format.Alignment = ParagraphAlignment.Center;
                cellCert.Format.Font.ApplyFont(fontCertInvoiceNumber);
                cellCert.Borders.Left.Visible = false;
                cellCert.Borders.Right.Visible = false;
                cellCert.Borders.Top.Visible = false;
          //      cellCert.Format.AddTabStop("16cm", TabAlignment.Right);
                cellCert.Format.SpaceBefore = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(3.0);
                //cellCert.Format.LeftIndent = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(15.0);
                cellCert.AddParagraph("Certificate Number 00" + item.ItemId.ToString());

                document.LastSection.Add(tableCert);


                if (item.Notes.ToString().Trim().Length > 0)
                {
                    //+++++++++ ADD PARAGRAPH FOR BUYER NOTES
                    MigraDoc.DocumentObjectModel.Paragraph paragraphFooter = section.AddParagraph();
                    paragraphFooter.Format.LineSpacingRule = MigraDoc.DocumentObjectModel.LineSpacingRule.Exactly;
                    paragraphFooter.Format.LineSpacing = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(8.0);
                    paragraphFooter.Format.SpaceBefore = MigraDoc.DocumentObjectModel.Unit.FromMillimeter(10.0);
                    paragraphFooter.AddText("NOTES:" + Environment.NewLine + _CertNotes.ToString());
                }



                Paragraph footerText = new Paragraph();
                footerText.Format.Alignment = ParagraphAlignment.Center;
                footerText.AddText(_CertFooterText.ToString());
                
                section.Footers.Primary.Add(footerText);

           


                // Create a renderer
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

                // Associate the MigraDoc document with a renderer
                pdfRenderer.Document = document;

                // Layout and render document to PDF
                pdfRenderer.RenderDocument();


                string pdfFilename = PortalSettings.HomeDirectoryMapPath + _PdfFilesFolder.ToString() + "Cert" + itemId.ToString() + ".pdf";

                if (File.Exists(pdfFilename))
                {
                    File.Delete(pdfFilename);
                }

                pdfRenderer.PdfDocument.Save(pdfFilename);








                bool watermarkIsNeeded = false;
                if (_CertWatermark.ToString().Trim().Length > 1)
                {
                    watermarkIsNeeded = true;
                }

                if (watermarkIsNeeded)
                {
                    PdfDocument pdfIn = PdfReader.Open(pdfFilename, PdfDocumentOpenMode.Import);
                    PdfDocument pdfOut = new PdfDocument();

                    for (int i = 0; i < pdfIn.PageCount; i++)
                    {
                        PdfPage pg = pdfIn.Pages[i];
                        pg = pdfOut.AddPage(pg);
                        // WATERMARK TEXT
                        string draftFlagStr = _CertWatermark.ToString();

                        // Get an XGraphics object for drawing beneath the existing content
                        XGraphics gfx = XGraphics.FromPdfPage(pg, XGraphicsPdfPageOptions.Prepend);

                        // Get the size (in point) of the text
                        XFont fontWM = new XFont("Verdana", 62, XFontStyle.Bold);
                        XSize size = gfx.MeasureString(draftFlagStr, fontWM);

                        // Define a rotation transformation at the center of the page
                        gfx.TranslateTransform(pg.Width / 2, pg.Height / 2);
                        gfx.RotateTransform(-Math.Atan(pg.Height / pg.Width) * 180 / Math.PI);
                        gfx.TranslateTransform(-pg.Width / 2, -pg.Height / 2);

                        // Create a string format
                        XStringFormat format = new XStringFormat();
                        format.Alignment = XStringAlignment.Near;
                        format.LineAlignment = XLineAlignment.Near;

                        // Create a dimmed red brush
                        XBrush brush = new XSolidBrush(XColor.FromArgb(32, 0, 0, 255));

                        // Draw the string
                        gfx.DrawString(draftFlagStr, fontWM, brush,
                            new XPoint((pg.Width - size.Width) / 2, (pg.Height - size.Height) / 2),
                            format);
                    }
                    pdfOut.Save(pdfFilename);
                }











                // Save and show the document
              //  pdfRenderer.PdfDocument.Save("TestDocument.pdf");
                Process.Start("explorer.exe", pdfFilename);


                HyperLinkPDF.Visible = true;
                HyperLinkPDF.NavigateUrl = PortalSettings.HomeDirectory + _PdfFilesFolder.ToString() + "Cert" + itemId.ToString() + ".pdf";

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                
            }
        }




        public static void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            MigraDoc.DocumentObjectModel.Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
        }

        //public string GetGiftCertRecord(int itemID)
        //{
        //    try
        //    {

        //        string CertContent = "";
        //        //check we have an item to lookup
        //        if (!Null.IsNull(itemId))
        //        {
        //            GiftCertificateInfo item;
        //            //load the item
        //            GiftCertificateController controller = new GiftCertificateController();
        //            item = controller.GetGiftCert(itemId);
        //            if (item != null)
        //            {

        //                if (item.PaypalPaymentState.ToString().Length == 0)
        //                {
        //                    Response.Redirect(Globals.NavigateURL("Access Denied"), true);
        //                }

        //                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        //                //NumberToWords(Int32.Parse(item.CertAmount.ToString())).ToString()
        //            //    string CertContent = "";

        //                CertContent = String.Format("{0:f2}", item.CertAmount) + " - " + textInfo.ToTitleCase(NumberToWords(Int32.Parse(item.CertAmount.ToString())).ToString()).ToString() + " Dollars" + Environment.NewLine;
        //          //      CertContent += item.ToName + Environment.NewLine;
        //          //      CertContent += item.FromName + Environment.NewLine + item.FromEmail + Environment.NewLine + item.FromPhone;
        //                ////lblMailingTo.Text = item.MailTo + "<br />" + item.MailToAddress;
        //                ////if (item.MailToAddress1.ToString().Length > 0)
        //                ////{
        //                ////    lblMailingTo.Text += "<br />" + item.MailToAddress1;
        //                ////}

        //                ////lblMailingTo.Text += "<br />" + item.MailToCity + ", " + item.MailToState + " " + item.MailToZip;
        //                ////string MailAddress = "";
        //                ////MailAddress = item.ToName + Environment.NewLine + item.MailToAddress + " " + item.MailToAddress1 + Environment.NewLine + item.MailToCity + ", " + item.MailToState + " " + item.MailToZip;
        //                ////lblNotes.Text = item.Notes;
                        
        //            }
        //            else
        //            {
        //                Response.Redirect(Globals.NavigateURL(), true);
                        
        //            }

                    
        //        }
        //        return CertContent.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.ProcessModuleLoadException(this, ex);
        //        return "";
        //    }
        //}

        public void LoadSettings()
        {

            GiftCertificateSettings settingsData = new GiftCertificateSettings(this.TabModuleId);

            if (settingsData.PdfFilesFolder != null)
            {
                _PdfFilesFolder = settingsData.PdfFilesFolder.ToString();
            }

            if (settingsData.ManageUserRole != null)
            {
                if (this.UserInfo.IsInRole(settingsData.ManageUserRole.ToString()))
                {
                    _isAuthorized = true;

                }

            }

            if (this.UserInfo.IsInRole("Administrators"))
            {

                _isAuthorized = true;

            }

            if (settingsData.CertReturnAddress != null)
            {
                _CertReturnAddress = settingsData.CertReturnAddress;
            }
            else
            {
                _CertReturnAddress = "Check Settings . . . ";
            }

            if (settingsData.CertBannerText != null)
            {
                _CertBannerText = settingsData.CertBannerText;
            }
            else
            {
                _CertBannerText = "Check Settings . . . ";
            }

            if (settingsData.CertFooterText != null)
            {
                _CertFooterText = settingsData.CertFooterText;
            }
            else
            {
                _CertFooterText = "Check Settings . . . ";
            }

            // _CertWatermark
            if (settingsData.CertWatermark != null)
            {
                _CertWatermark = settingsData.CertWatermark;
            }
            // _CertLogo
            if (settingsData.CertLogo != null)
            {
                _CertLogo = settingsData.CertLogo;
            }

        }


        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["ItemId"] != null)
            {
                itemId = Int32.Parse(Request.QueryString["ItemId"]);

            }

            MakeGiftCert(itemId);

        }









    }
}