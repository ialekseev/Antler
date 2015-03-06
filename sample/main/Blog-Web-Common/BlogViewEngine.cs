using System.Web.Mvc;

namespace Blog.Web.Common
{
    public class BlogViewEngine: RazorViewEngine
{
    public BlogViewEngine()
    {
        var viewLocationFormatArr=new string[2];
        viewLocationFormatArr[0] = "~/bin/Views/{1}/{0}.cshtml";
        viewLocationFormatArr[1] = "~/bin/Views/Shared/{1}/{0}.cshtml";        
        
        this.ViewLocationFormats = viewLocationFormatArr;         
        this.MasterLocationFormats = viewLocationFormatArr;         
        this.ViewLocationFormats = viewLocationFormatArr; 
    }
}       
}
