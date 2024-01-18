# Xeggex
Library for accessing the [Xeggex Exchange](https://xeggex.com?ref=64eda7d04fb60b78dea302a9) api  
  
Simple using:  
Create connector for Xeggex
```
var connector = new XeggexConnection(apiKey, apiSecret);
```  
* connector.Account - Contains account balance
* connector.Exchange - Exchange operations for you
* connector.ExchangeData - Market data