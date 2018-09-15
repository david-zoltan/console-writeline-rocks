# Console.WriteLine() Rocks

"Console.WriteLine() Rocks" is a one-line alternative to complex logging solutions for .NET developers.
It lets you see in a remote browser what your .NET applications print on their console, by redirecting the Console.WriteLine() messages to a unique-URL webpage.

## Sample Usage
### 1. Install the NuGet package
    PM> Install-Package ConsoleWriteLineRocksSDK

### 2. Pick a unique name, for example: _YOUR-SECRET-ID_

This will serve as a feed name. Your applications will Console.WriteLine() into that feed. You can see what is written into that feed by opening http://console.writeline.rocks/feed/YOUR-SECRET-ID in a browser.


### 3. Create a Console Application

    using System;
    using ConsoleWriteLineRocksSDK;

    class Program
    {
        static void Main(string[] args)
        {
            Console.SetOut( ConsoleWriteLineRocks.Feed("YOUR-SECRET-ID") ); 
            // you can use any feed name you like (it's a good idea to choose a unique one)
            
            Console.WriteLine("Hello World!");

            Console.ReadKey(); 
            // it takes a few millisecs to capture Console.WriteLine() output
            // not a problem in production environments with long running apps
        }
    }

### 4. Open http://console.writeline.rocks/feed/YOUR-SECRET-ID in a browser
You will see the Console.WriteLine() messages appear here. Only those messages will appear which are printed _after_ you have opened the browser.

### 5. Run your application
Your application's Console.WriteLine() messages will be printed into the browser instead of the application's local console.

## Free & Anonymous

It is absolutely anonymous and free to use. No registration or credit card is required. "Console.WriteLine() Rocks" writes no logs.

## Print Debugging

"Console.WriteLine() Rocks" acknowledges the fact that "print debugging" is still a favorite tracing technique used by many developers today (even if they don't admit it publicly).
By adding just one line of code to your .NET applications, "Console.WriteLine() Rocks" enables you to see all those console "print debug" messages in a browser.

## Architecture

"Console.WriteLine() Rocks" consists of
* a free online service hosted in the cloud (it's like an online console that you can check in a browser),
* and a NuGet package that lets you integrate your application with the online service.

## Technology

The technology behind "Console.WriteLine() Rocks" is
* async HTTP posts, to send the Console.WriteLine() messages to the "Console.WriteLine() Rocks" backend for further distribution,
* Server Side Events for distributing those messages to the browsers,
* implemented with ASP.NET Core,
* currently hosted on Ubuntu Linux,
* no databases - "Console.WriteLine() Rocks" stores nothing on its servers.

## More information

* Website: http://console.writeline.rocks/
* NuGet package: https://www.NuGet.org/packages/ConsoleWriteLineRocksSDK/
* GitHub: https://github.com/david-zoltan/console-writeline-rocks
