using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => new {Name ="Yuri Kamagoe", Age = 26});
app.MapGet("/addHeader", (HttpResponse response) => {
 response.Headers.Add("Teste", "Kamagoe");
 return new {Name ="Yuri Kamagoe", Age = 26};   
});


// //Parameter Through QueryString
// app.MapGet("/getBook", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
//     return dateStart + " - " + dateEnd;
// });


// //Parameter through Header
// app.MapGet("/getBookByHeader", (HttpRequest request) => {
//     return request.Headers["book-code"].ToString();
// });

//Get book
app.MapGet("/books/{code}", ([FromRoute] string code) => {
    Book book = BookCollection.GetBy(code);
    if(book != null)
        return Results.Ok(book);
    else
        return Results.NotFound();
});

//Save book 
app.MapPost("/books", (Book book) =>{
   BookCollection.Add(book);
//    return new {Success = "True", Response = "Método executado com sucesso"};
      return Results.Created("$/books/{book.Code}", new {Success = "True", Response = "Método executado com sucesso"});
});

//Edit book properties
app.MapPut("/books", (Book book) =>{
   Book savedBook = BookCollection.GetBy(book.Code);
   savedBook.Title = book.Title;    
   savedBook.Code = book.Code;
   savedBook.Type = book.Type;
   return Results.Ok(savedBook);
});

//Delete book from list
app.MapDelete("/books/{code}", ([FromRoute] string code) => {
   Book bookToRemove = BookCollection.GetBy(code);
   BookCollection.Delete(bookToRemove);  
   return Results.Ok();
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