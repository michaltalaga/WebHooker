# WebHooker
C# .NET SignalR Web Hooks Forwarding Server

Allows receiving web hooks on your local dev machine similar to ngrok.

Main difference is: WebHooker is .NET based and has only 1 feature - forwards web requests to a specified end point. 

Benefit over ngrok is that WebHooker supports host headers for free:
  
  webhookerclient -t http://yourhost:port


# Usage

Tool consists of a Server and a Client. 
To use a WebHookerClient you need a public WebHookerServer. Either host it yourself or use the default one.

Running WebHookerClient.exe will give you a public url you can use. All traffic coming to that url will be forwarded to your local address.
