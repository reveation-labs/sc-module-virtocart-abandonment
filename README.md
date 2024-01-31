# CartAbandonmentReminder

## Overview

In E-Commerce, Cart abandonment is an important concern. There are reasons for cart to be abandoned in online shopping and businesses should address this before loosing customers. There are ongoing challenges that require strategic improvemnet to reduce cart abandonment. So here, we are providing a strategy to reduce cart abandonment by sending an email notification to the concerned customer who forgot to proceed with items present in the cart. Leverage this finest feature to improve sales and attract buyers.

In Virto Platform, this module is present as Abandoned Carts in more option.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/825a963f-1f9b-4330-b78d-547df536be65)

In main blade it will show customer and store details for which carts have been added.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/b8ab3283-4e8a-427a-be93-a6a5514ca5b9)

On selecting any customer, it will list out all the available items in the cart with details.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/9fd0cf35-5bb8-4e94-9815-a688e8dde8dc)


## Setup
This new feature require a seamless one time setup in Virto Platform settings. Best part is being configurable and can be enable disable anytime as per need.

### Enabling Cart Reminder feature
Search for Cart Abandonment Reminder and enable options as required. In case to stop reminding through email notification, just disable 'Enable Cart Reminder' from below settings.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/3453fae0-0285-4b57-99e1-7c933c9108e0)

### Cart Reminder General Settings
Below is the general settings of cart reminder, provide cron time and cron expression which will run the service in the background as per scheduled timing and send notifications to the configured email addresses.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/bfa008f7-8a29-474e-afac-c6077266db0a)

### Email Notification Setup
Navigate to Notifications module and select Cart Reminder Email Notification.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/44c018d7-7302-40f7-8126-ef823a510249)

Enable Active option.
Provide CC and BCC addresses if required.
Add Template for Subject and Body of email notifications.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/68682f74-bb14-4ef2-943c-111985681eab)

### Verify Email Notification

After setting up cart reminder module, user can verify email notifications in Notification Activity Feed for status of each generated notifications.
For Error status, check what is in the error section and resolve it accordingly.

![image](https://github.com/reveation-labs/virto-cart-abandonment-module/assets/115815461/d96877a3-fbb9-4ba9-931b-801d4170634a)


## License

Copyright (c) Sharpcode Solutions. All rights reserved.

Licensed under the GNU Affero General Public License v3.0. You may not use this file except in compliance with the License.

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
