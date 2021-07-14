namespace HtmlToPdf
{
    public interface IPdfConverter
    {
        byte[] Create(string content, object model, string[] pageBreak = null);
        byte[] CreateWithSecurity(string content, object model, string[] pageBreak = null, string pwdUser = "", string pwdOwner = "");
    }
}