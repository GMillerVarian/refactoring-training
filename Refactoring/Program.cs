using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Refactoring
{
  public class Program
  {
    public static void Main(string[] args)
    {
      List<User> users = getUsers();
      List<Product> products = getProducts();

      Tusc tusc = new Tusc(users, products);
      tusc.Start();
    }

    private static List<Product> getProducts()
    {
      return JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data/Products.json"));
    }

    private static List<User> getUsers()
    {
      return JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data/Users.json"));
    }
  }
}
