// See https://aka.ms/new-console-template for more information
using Azure.Core;
using Bogus;
using BookShop.Application.Common.Rules;
using BookShop.Application.Features.Book.Commands.Create;
using BookShop.Application.Features.Book.Queries.GetSummaries;
using BookShop.Domain.Common.Entity;
using BookShop.Domain.Constants;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Persistance;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.WebEncoders.Testing;
using Newtonsoft.Json;
using System.Buffers.Text;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

Console.WriteLine("Hello, World!");

foreach (var item in PermissionConstants.GetAll())
{
    Console.WriteLine(item);
}

Console.WriteLine();

//GetBookSummariesQuery getBookSummariesQuery = new GetBookSummariesQuery();

//JsonSerializerSettings settings = new JsonSerializerSettings()
//{
//    NullValueHandling = NullValueHandling.Ignore,
//};
//settings.NullValueHandling = NullValueHandling.Ignore;
//var myJson = JsonConvert.SerializeObject(getBookSummariesQuery, settings);
//Console.WriteLine(myJson);
//string strr = Convert.ToBase64String(Encoding.UTF8.GetBytes(myJson));
//Console.WriteLine(); 
//Console.WriteLine(strr);











//string plainText = "HTTPS://BookShop.IR/books/title?fdsfsfsdfsd&dgsigjiojg&ssdlfjl?=dsjfljsdfljfG464HDFH6DHDFFF89DFJDLKsdfsfshfdsofijifsfhuifiuhqxmohifuhoixogxuynguyxngs";
//var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

//string ascciEncodedString = Convert.ToBase64String(plainTextBytes);

//var b2 = Encoding.UTF8.GetBytes(ascciEncodedString);

//string e2 = Convert.ToBase64String(b2);

//var b3 = Encoding.UTF8.GetBytes(e2);

//string e3 = Convert.ToBase64String(b3);

//Console.WriteLine(ascciEncodedString);
//Console.WriteLine();
//Console.WriteLine(e2);
//Console.WriteLine();
//Console.WriteLine(e3);





//var x = Math.Ceiling((decimal)92 / 10);
//int y = 92 / 10;
//Console.WriteLine(x);
//Console.WriteLine(y);





//TestRepo testRepo = new TestRepo();
//TestEntity testEntity = new TestEntity();

//testRepo.SetId(testEntity);

//public class TestEntity : Entity<Guid>
//{
//}
//public class TestRepo : GenericRepo<TestEntity, Guid>
//{
//}
//public class GenericRepo<TEntity,TKey> 
//    where TEntity : Entity<TKey> 
//    where TKey : struct 
//{

//    public void SetId(TEntity entity)
//    {
//        Console.WriteLine($"Before Set, Id: {entity.Id}");

//        if (typeof(TKey) == typeof(Guid))
//            entity.Id = (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));

//        if (typeof(TKey) == typeof(byte))
//            entity.Id = (TKey)Convert.ChangeType(100, typeof(TKey));

//        if (typeof(TKey) == typeof(int))
//            entity.Id = (TKey)Convert.ChangeType(200, typeof(TKey));

//        if (typeof(TKey) == typeof(long))
//            entity.Id = (TKey)Convert.ChangeType(300, typeof(TKey));

//        Console.WriteLine($"After Set, Id: {entity.Id}");
//    }


//}


























//BookShopDbContext bookShopDbContext = new BookShopDbContext();


//User? user = null;

////user = bookShopDbContext.Users.FromSqlRaw("Select * From Users Where Id = '4159f964-ee71-4219-ad82-3ad49b92bcfe'").FirstOrDefault();

////user = bookShopDbContext.Database.SqlQueryRaw<User>("Select From Users Where Id = '4159f964-ee71-4219-ad82-3ad49b92bcfe'").FirstOrDefault();

////user = bookShopDbContext.Users.Find("4159f964-ee71-4219-ad82-3ad49b92bcfe");


////Console.WriteLine(bookShopDbContext.Model.FindEntityType(typeof(User)).GetTableName());

//Console.WriteLine(user?.Name ?? "Null");


////SqlConnection sqlConnection = new SqlConnection("Server=.;Database=BookShopDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");
////sqlConnection.Open();
////SqlCommand sqlCommand = new SqlCommand("Select * From Users Where Id = '4159f964-ee71-4219-ad82-3ad49b92bcfe'", sqlConnection);
////var x = sqlCommand.ExecuteReader();
////while (x.Read())
////{
////    user = x.OfType<User>().FirstOrDefault();
////    Console.WriteLine(user?.Name ?? "Null");
////    Console.WriteLine($"{x.GetValue(1)} - {x.GetValue(2)} - {x.GetValue(3)} - {x.GetValue(4)} - {x.GetValue(5)} - ");
////}












//Person person = new Person();

//person.SetPerson(a =>
//{
//    a.Age = 22;
//});

//Console.WriteLine("----------------------------------------");
//Console.WriteLine(person.Age);


//public  class Person
//{
//    public int Age { get; set; } = 0;
//    public Action<Person> Action { get; set; }

//    public Person GetFromAction()
//    {
//        return this;
//    }


//    public void SetPerson(Action<Person> action)
//    {
//        Action = action;

//        Console.WriteLine(this.Age);

//        Action.Invoke(this);

//        Console.WriteLine(this.Age);
//    }
//}