using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
  public class Tusc
  {
    private List<User> users;
    private List<Product> products;

    public Tusc(List<User> users, List<Product> products)
    {
      this.users = users;
      this.products = products;
    }

    public void Start()
    {
      showWelcome();

      User currentUser = performUserLogin(users);
      if (currentUser != null)
      {
        showLoginSuccess(currentUser);
        showBalance(currentUser);
        processUserSelections(currentUser);
      }

      showGoodbye();
    }

    private void processUserSelections(User currentUser)
    {
      while (true)
      {
        showProductList();

        int selectedProduct = getProductSelection();
        if (userSelectedExit(selectedProduct))
        {
          onExit();
          return;
        }
        else
        {
          if (!processProductSelection(currentUser, selectedProduct))
          {
            continue;
          }
        }
      }
    }

    private bool processProductSelection(User currentUser, int selectedProduct)
    {
      showProductSelectionSuccess(currentUser, selectedProduct);

      int selectedQuantity = getQuantitySelection();
      double costToPurchase = products[selectedProduct].Price * selectedQuantity;
      if (currentUser.balance < costToPurchase)
      {
        showInsufficientBalanceMessage();
        return false;
      }

      int remainingQuantity = products[selectedProduct].Quantity;
      if (selectedQuantity > remainingQuantity)
      {
        showInsufficientQuantityMessage(selectedProduct);
        return false;
      }

      if (selectedQuantity > 0)
      {
        processPurchase(currentUser, selectedProduct, selectedQuantity);
      }
      else
      {
        showSelectedQuantityNegativeMessage();
      }

      return true;
    }

    private void onExit()
    {
      updateBalances();
      updateQuantities();
      showGoodbye();
    }

    private void processPurchase(User currentUser, int selectedProduct, int selectedQuantity)
    {
      currentUser.balance -= products[selectedProduct].Price * selectedQuantity;
      products[selectedProduct].Quantity -= selectedQuantity;

      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("You bought " + selectedQuantity + " " + products[selectedProduct].Name);
      Console.WriteLine("Your new balance is " + currentUser.balance.ToString("C"));
      Console.ResetColor();
    }

    private static void showSelectedQuantityNegativeMessage()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine();
      Console.WriteLine("Purchase cancelled");
      Console.ResetColor();
    }

    private void showInsufficientQuantityMessage(int selection)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine();
      Console.WriteLine("Sorry, " + products[selection].Name + " is out of stock");
      Console.ResetColor();
    }

    private static void showInsufficientBalanceMessage()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine();
      Console.WriteLine("You do not have enough money to buy that.");
      Console.ResetColor();
    }

    private void showProductSelectionSuccess(User currentUser, int selection)
    {
      Console.WriteLine();
      Console.WriteLine("You want to buy: " + products[selection].Name);
      Console.WriteLine("Your balance is " + currentUser.balance.ToString("C"));
    }

    private static int getQuantitySelection()
    {
      Console.WriteLine("Enter amount to purchase:");
      string answer = Console.ReadLine();
      int qty = Convert.ToInt32(answer);
      return qty;
    }

    private void updateQuantities()
    {
      string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
      File.WriteAllText(@"Data/Products.json", json2);
    }

    private void updateBalances()
    {
      string json = JsonConvert.SerializeObject(users, Formatting.Indented);
      File.WriteAllText(@"Data/Users.json", json);
    }

    private static int getProductSelection()
    {
      Console.WriteLine("Enter a number:");
      string answer = Console.ReadLine();
      return Convert.ToInt32(answer) - 1;
    }

    private void showProductList()
    {
      Console.WriteLine();
      Console.WriteLine("What would you like to buy?");

      int productIndex = 0;
      foreach (Product product in products)
      {
        productIndex++;
        Console.WriteLine(productIndex + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
      }
      Console.WriteLine(products.Count + 1 + ": Exit");
    }

    private static void showBalance(User user)
    {
      Console.WriteLine();
      Console.WriteLine("Your balance is " + user.balance.ToString("C"));
    }

    private static void showLoginSuccess(User user)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine();
      Console.WriteLine("Login successful! Welcome " + user.username + "!");
      Console.ResetColor();
    }

    private static User performUserLogin(List<User> users)
    {
      User user = UserAuthentication.getUserByUsername(users, getUsername());

      if (user != null)
      {
        user = UserAuthentication.validatePassword(user, getPassword());
      }

      return user;
    }

    private static string getPassword()
    {
      Console.WriteLine("Enter Password:");
      return Console.ReadLine();
    }

    private static string getUsername()
    {
      Console.WriteLine("Enter Username:");
      return Console.ReadLine();
    }

    private static void showGoodbye()
    {
      Console.WriteLine();
      Console.WriteLine("Press Enter key to exit");
      Console.ReadLine();
    }

    private static void showWelcome()
    {
      Console.WriteLine("Welcome to TUSC");
      Console.WriteLine("---------------");
      Console.WriteLine();
    }

    private bool userSelectedExit(int selection)
    {
      return (selection == products.Count);
    }
  }
}
