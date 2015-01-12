Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Private db As cemeteryContext = New cemeteryContext

    Function Index() As ActionResult
        ViewData("Message") = "Modify this template to jump-start your ASP.NET MVC application."
        db.Owners.Add(New Owner() With {.Deed_Name = "Jerred Schmidt"})
        db.SaveChanges()
        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Your app description page."

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
