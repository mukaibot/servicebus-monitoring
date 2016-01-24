# ServiceBus Monitoring
A .NET 4.5 console app to monitor the number of messages in your ServiceBus queues. You nominate a threshold (ie 10 messages), and receive an email if there are more messages than this in the queue.

## Getting started

You need the following:
* At least one queue
* A SAS token with the "Management" access right
* Credentials to an SMTP server

**Using the app**
1. Copy the sample `App.config.sample` to `App.config`
1. Update the values as necessary
1. Run `ServiceBusMonitoring.exe` and check that everything is healthy (perhaps create a dummy queue, and send some sample messages to test notification)
1. Use Task Scheduler or similar to run this on a regular basis

## Contributing to this project

Pull requests are welcome. I have tried to make the alerting functionality extendable, because a lot of people use things like Slack, HipChat etc. If you'd like to contribute a new Alerter, please implement the `IAlerter` interface in the `Alerters` namespace.
