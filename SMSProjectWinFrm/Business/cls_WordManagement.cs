using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using MSWord = Microsoft.Office.Interop.Word;

namespace SMSProjectWinFrm.Business
{
    public class cls_WordManagement
    {
        public bool ChangeHeaderInfo(cls_Contact contact, string wordDocumentFilePath)
        {
            try
            {
                object replaceAll = MSWord.WdReplace.wdReplaceAll;
                object missing = System.Reflection.Missing.Value;
                MSWord.Application application = new MSWord.Application();
                MSWord.Document document = application.Documents.Open(wordDocumentFilePath);
                //Add header into the document
                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    //headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    //headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    //headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    //headerRange.Font.Size = 10;
                    
                    headerRange.Find.Text = "{0}";
                    headerRange.Find.Replacement.Text = contact.FirstName + " " + contact.LastName;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{1}";
                    //CultureInfo culture = new CultureInfo("fa-IR");
                    headerRange.Find.Replacement.Text = contact.Birthday;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{2}";
                    headerRange.Find.Replacement.Text = contact.SSID;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{3}";
                    headerRange.Find.Replacement.Text = contact.FatherName;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{4}";
                    headerRange.Find.Replacement.Text = contact.PatientID;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{5}";
                    headerRange.Find.Replacement.Text = contact.Email;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{6}";
                    headerRange.Find.Replacement.Text = "";
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{7}";
                    headerRange.Find.Replacement.Text = contact.Mobile;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    headerRange.Find.Text = "{8}";
                    headerRange.Find.Replacement.Text = contact.Address;
                    headerRange.Find.Execute(ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref replaceAll, ref missing, ref missing, ref missing, ref missing);

                    //string Str = headerRange.Text;
                    //headerRange.Text = string.Format(Str, 
                    //    contact.FirstName + " " + contact.LastName, 
                    //    contact.Birthday, 
                    //    contact.SSID, 
                    //    contact.FatherName, 
                    //    contact.PatientID,
                    //    contact.Email,
                    //    "",
                    //    contact.Mobile,
                    //    contact.Address
                    //    );
                }
                document.Save();
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                application.Quit(ref missing, ref missing, ref missing);
                application = null;
            }
            catch (Exception)
            {
                ;
            }
            return false;
        }
    }
}
