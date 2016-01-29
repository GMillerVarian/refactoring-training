using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
  public class UserAuthentication
  {
    public static User getUserByUsername(List<User> users, string username)
    {
      User foundUser = null;

      if (!string.IsNullOrEmpty(username))
      {
        foreach (User user in users)
        {
          if (user.username == username)
          {
            foundUser = user;
          }
        }
      }

      if (foundUser == null)
      {
        showInvalidUsernameMessage();
      }

      return foundUser;
    }

    private static void showInvalidUsernameMessage()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine();
      Console.WriteLine("You entered an invalid user.");
      Console.ResetColor();
    }

    public static User validatePassword(User user, string password)
    {
      User validUser = null;

      if (user.password == password)
      {
        validUser = user;
      }
      else
      {
        showInvalidPasswordMessage();
      }

      return validUser;
    }

    private static void showInvalidPasswordMessage()
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine();
      Console.WriteLine("You entered an invalid password.");
      Console.ResetColor();
    }
  }
}
