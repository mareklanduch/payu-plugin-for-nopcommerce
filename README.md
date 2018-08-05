# payu-plugin-for-nopcommerce

PayU Poland plugin for nopcommerce

## Getting Started

### Prerequisites

NopCommerce v. 4.10

### Installing

Plugin zip file can be downloaded here
[Payments.PayU.zip v. 1.1](http://www.sharpapp.net/download/1.1/Payments.PayU.zip).

You can install the plugin the same way as other plugins in nopcommerce.

In admin panel go to Configuration -> Plugins -> Local plugins and click the button "upload plugin or theme", choose zip file with plugin. If plugin is already uploaded scroll down and search for the plugin and click install.

After installation we need to configure plugin. Click configure button and provide all data which are available in your PayU account (client id, client secret, second key). When the "Use sandbox" is checked then sandbox data will be used for payments. Please note that you don't need to provide sandbox data in case you are using this plugin only in the production.

After Congiuration remember to active payment method in Configuration -> Payment -> Payment methods

## Features

1. Redirect payment 
2. Refund and partial refund
3. ~~Capture payments~~ (soon) 

## Issues

If you have found some issues please write to me, create new issue or create pull request with a solution. 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
