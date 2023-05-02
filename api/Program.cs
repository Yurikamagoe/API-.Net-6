using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => new {Name ="Yuri Kamagoe", Age = 26});
app.MapGet("/addHeader", (HttpResponse response) => {
 response.Headers.Add("Teste", "Kamagoe");
 return new {Name ="Yuri Kamagoe", Age = 26};   
});


//Parameter Through QueryString
app.MapGet("/getBook", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});

//Parameter Through Route
app.MapGet("/getBook/{code}", ([FromRoute] string code) => {
    Book book = BookCollection.GetBy(code);
    return book;
});

//Parameter through Header
app.MapGet("/getBookByHeader", (HttpRequest request) => {
    return request.Headers["book-code"].ToString();
});

//Save book 
app.MapPost("/saveBook", (Book book) =>{
   BookCollection.Add(book);
   return new {Success = "True", Response = "Método executado com sucesso"};
});

//Edit book properties
app.MapPut("/editBook", (Book book) =>{
   Book savedBook = BookCollection.GetBy(book.Code);
   savedBook.Title = book.Title;
   savedBook.Code = book.Code;
   savedBook.Type = book.Type;
   return savedBook;
});

//Delete book from list
app.MapDelete("/removeBook/{code}", ([FromRoute] string code) => {
   Book bookToRemove = BookCollection.GetBy(code);
   BookCollection.Delete(bookToRemove);  
   return new {Success = "True", Response = "Elemento removido com sucesso"}; 
});

app.Run();

//Simulador de base de dados
public static class BookCollection{
    public static List<Book> Books { get; set; }

    public static void Add (Book book){
        if(Books == null)
            Books = new List<Book>();

        Books.Add(book);
    }

    public static Book GetBy (string code){
        return Books.FirstOrDefault(b => b.Code == code);
    }

    public static void Delete (Book book){        
        Books.Remove(book);
    }

}

public class Book {
    public string  Title { get; set; }
    public string Type { get; set; }
    public string Code { get; set; }
}