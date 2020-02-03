# Publish Subscribe
### Broadcasting baby

To illustrate the pattern, we're going to build a simple logging system. It will consist of two programs -- the first will emit log messages and the second will receive and print them.

In our logging system every running copy of the receiver program will get the messages. That way we'll be able to run one receiver and direct the logs to disk; and at the same time we'll be able to run another receiver and see the logs on the screen.

Essentially, published log messages are going to be broadcast to all the receivers.


## The core idea in the messaging model in RabbitMQ is that the producer never sends any messages directly to a queue as it was in previous example (Work queue)
## Instead, the producer can only send messages to an exchange. 




- That relationship between exchange and a queue is called a binding.

