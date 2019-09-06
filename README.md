# Teco
A little template engine for .NET, a tool intended to compile information message templates (for logs, notifications, emails etc) into IL code, which can be used to render a result message with the data substituted from an object model.

Teco is capable to:
* work with .NET Framework 3.5 and above, .NET Core 2.0 and above;
* produce messages in text and HTML modes;
* produce either separate dynamic methods for each template or classes with a banch of methods or properties for few templates;
* save compiled assemblies on a physical location (only for .NET Framework);
* support flow control commands and collection enumeration commands inside a template.

## Usage
Teco template is an ordinary text or HTML markup with flow control and substitution commands. Substitution commands are represented by a single tag, other commands are represented by a pair of tags.

In general, substitution tags and an flow control start tags have the next structure:

`{` *command* `/` *modifiers* `:` *value* `:` *format* or *variable name* `}`

End tags are represented by the next sequence:

`{/` *command* `}`

where

| Component | Description |
|---|---|
| *command* | A corresponsing keyword. A complete list is below. |
| *modifiers* | Flags represented by single characters, supported by some commands. Modifiers change command's behaviour to a limited extent. If modifiers are not used then `/` character must be omitted. |
| *value* | A path to a model property in a form of string which is consisted of names of model properties separated by `.` character. This path is used to obtain an operating value from a model. Getting a value using an integer or string indexer by using brackets is also supported. Some commands support a using of constants instead of paths. In this case a constant must be prepended with a `@` character. Characters `{`, `}`, `:` must be doubled. |
| *format* | A format string which is supported by some commands. It is effectively equal to format string used by `System.String.Format(...)` method. This string can contain any characters but `{` and `}`. If format is not used then corresponding `:` character must be omitted. |
| *variable name* | A variable name for *each* command. This name must be started with a latin letter or underscore and can contain latin letters, digits and underscores. |

### Commands

| Keyword | Constant support | Format support | Description |
|---------|:----------------:|:--------------:|-------------|
| text     |                  |       +        | Substitutes a value from a model. In HTML mode special characters will be replaced with character references. |
| html     |                  |       +        | Substitutes a value from a model. In HTML mode special characters will be passed as is. |
| url      |                  |                | Substitutes a value from a model encoding some characters with percent encoding. Encoding is perfomed with a reference to RFC 3986. By default, all the characters not included in reserved and unreserved character sets will be encoded. With `p` modifier all the reserved characters will be also encoded. With `a` modifier none of characters code of which is 0x80 and above will be encoded.
| each     |                  |                | Enumerates an array or a collection applying to each element a nested template. In a context of this command all the properties of a model will be relative to an current element. Additionally, there will be available few extra variables: `this`, `thisIndex`, `thisNum`, `thisCount`. They will contain a current element, its index in a collection, its number (one-based) and total count of elements in a collection (if the collection supports corresponding property) respectively. The name of a variable to a current element can be specified explicitly. In this case names of other variables will be also changed respectively. |
| if       |                  |                | Obtains a value from a model and process nested part of template if this value is effectively true. A table of *true* values depending on types is below. |
| ifnot    |                  |                | Obtains a value from a model and process nested part of template if this value is effectively false. A table of *false* values depending on types is below. |
| when     |        +         |       +        | Obtains a value from a model, applies a format to it if specified and stores it. Right inside of this command can be only placed next commands: eq, begins, contains, ends, else. While processing each of the nested commands its value is compared with a value from *when* command. Depending on the comparison result the command branch is processed or not. Once one branch is processed the following nested commands are ignored. |
| eq       |        +         |       +        | Obtains a value from a model, applies a format to it if specified and compares it with a value of the parent *when* command. If values are equal then a nested template is processed. With `i` modifier comparison is case insensetive. |
| begins   |        +         |       +        | Obtains a value from a model, applies a format to it if specified and compares it with a value of the parent *when* command. If parent command value starts with current command value then a nested template is processed. With `i` modifier comparison is case insensetive. |
| contains |        +         |       +        | Obtains a value from a model, applies a format to it if specified and compares it with a value of the parent *when* command. If parent command value contains current command value then a nested template is processed. With `i` modifier comparison is case insensetive. |
| ends     |        +         |       +        | Obtains a value from a model, applies a format to it if specified and compares it with a value of the parent *when* command. If parent command value ends with current command value then a nested template is processed. With `i` modifier comparison is case insensetive. |
| else     |                  |                | Processes a nested template part. |

### True and false values

| Type of a model property                                                                          | True                                          | False                                |
|---------------------------------------------------------------------------------------------------|-----------------------------------------------|--------------------------------------|
| `System.Boolean`                                                                                  | value is `true`                               | value is `false`                     |
| `System.Int16`, `System.UInt16`, `System.Int32`, `System.UInt32`, `System.Int64`, `System.UInt64` | value is not `0`                              | value is `0`                         |
| `System.Nullable< T >`                                                                            | `HasValue` property is `true`                 | `HasValue` property is `false`       |
| `System.String`                                                                                   | value is not `null` and its length is not `0` | value is `null` or its length is `0` |
| any other reference type                                                                          | value is not `null`                           | value is `null`                      |
| any other value type                                                                              | always                                        | never                                |

### Examples

Suppose we have a template and a model class an instance of which will contain a data later. To create a method corresponding to these template and class we have to create an instance of `Teco.RuntimeCompiler` class. Using the instance of this class we should call one of `Compile` method overloads and pass the template, model type and output format there. The method returns a delegate that can be used to create any number of messages according to the template and format specified.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Teco;

class OrderItem {
    public string SKU { get; set; }
    public string Title { get; set; }
    public double Cost { get; set; }
    public int Qty { get; set; }
    public double Total {
        get { return Cost * Qty; }
    }
}

class Model {
    public string Customer { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public double Total {
        get { return OrderItems != null ? OrderItems.Sum(item => item.Total) : 0.0; }
    }
}

class Program {
    static void Main(string[] args) {
        var template = @"
          <h1>Dear {text:Customer}.</h1>
          <p>
            {if:OrderItems.Count}
              <table>
                <tr>
                  <th>Product</th>
                  <th>Cost</th>
                  <th>Qty</th>
                  <th>Total</th>
                </tr>
                {each:OrderItems}
                  <tr>
                    <td><a href='http://host/product?SKU={url/p:SKU}'>{text:Title}</a></td>
                    <td>{text:Cost:$0.00}</td>
                    <td>{text:Qty}</td>
                    <td>{text:Total:$0.00}</td>
                  </tr>
                {/each}
              </table>
              <br />
              Total: {text:Total:$0.00}
            {/if}
            {ifnot:OrderItems.Count}
                The order list is empty.
            {/ifnot}
          </p>
        ";

        var compiler = new RuntimeCompiler(new CompilerOptions());
        var getMessage = compiler.Compile<Model>(template, OutputFormat.Html);

        var model1 = new Model {
            Customer = "John Smith",
            OrderItems = new List<OrderItem> {
                new OrderItem { SKU = "8536914", Title = "Ice Cream", Cost = 2.9, Qty = 2 },
                new OrderItem { SKU = "4397104", Title = "Orange Juice", Cost = 5.0, Qty = 1 },
                new OrderItem { SKU = "9150185", Title = "Cheeseburger", Cost = 3.0, Qty = 2 }
            }
        };
        String message1 = getMessage(model1, CultureInfo.CurrentCulture);
        Console.WriteLine(message1);

        var model2 = new Model {
            Customer = "John Doe",
            OrderItems = new List<OrderItem> {
                new OrderItem { SKU = "5209602", Title = "Apples", Cost = 1.0, Qty = 4 },
                new OrderItem { SKU = "8962957", Title = "Raspberry", Cost = 2.0, Qty = 3 },
                new OrderItem { SKU = "1052987", Title = "Chicken", Cost = 5.0, Qty = 1 }
            }
        };
        String message2 = getMessage(model2, CultureInfo.CurrentCulture);
        Console.WriteLine(message2);
    }
}
```

If there is a need to group few templates then `Teco.AssemblyCompiler` class can be used. It has `AddType` method to create a single class with a banch of properties or methods returning corresponding messages.
It's also more covenient to pass an interface type which a future class will impliment, so there will be no need to use a reflection to get those properties and methods from an instance of the created class. 
After properties and methods were added `GetCompiledTypes` method is used to compile templates into a class. To instantiate this class and get an interface provided can be used `Teco.AssemblyCompiler.CreateTypeInstance` static method.
The example below demonstrates this feature. Also note that model type and interface type must be accessible from an assembly that will be generated, so it maked as `public`.

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;
using Teco;

public interface IEmail {
    string From { get; }
    string To { get; }
    string Subject { get; }
    string Body { get; }
}

public class AccountData {
    public string AccountId { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
}

class Program {
    static void Main(string[] args) {
        var compiler = new AssemblyCompiler(new AssemblyCompilerOptions() { OutputFormat = OutputFormat.Text, ModelType = typeof(AccountData) });
        var typeCtx = compiler.AddType("EmailGen", typeof(IEmail));
        typeCtx.AddProperty("From", "ouremail@host");
        typeCtx.AddProperty("To", "{text:CustomerEmail}");
        typeCtx.AddProperty("Subject", "Account #{text:AccountId}");
        typeCtx.AddProperty("Body", "Dear {text:CustomerName}. Here is information about your account #{text:AccountId}...");
        Type emailGenType = compiler.GetCompiledTypes()[0];

        List<AccountData> accounts = new List<AccountData>() {
            new AccountData { AccountId = "241", CustomerEmail = "foo@host", CustomerName = "Foo" },
            new AccountData { AccountId = "672", CustomerEmail = "bar@host", CustomerName = "Bar" },
            new AccountData { AccountId = "591", CustomerEmail = "teco@host", CustomerName = "Teco" }
        };
        foreach (AccountData acc in accounts) {
            IEmail email = AssemblyCompiler.CreateTypeInstance<IEmail>(emailGenType, acc, CultureInfo.CurrentCulture);
            Console.WriteLine("From: {0}\tTo: {1}\tSubject: {2}\tBody:{3}", email.From, email.To, email.Subject, email.Body);
        }
    }
}
```

##License

This project is under [MIT License](LICENSE)