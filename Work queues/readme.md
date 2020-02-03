# Rabbit MQ
## Work queue
Distributing tasks among workers (the competing consumers pattern)
The assumption behind a work queue is that each task is delivered to exactly one worker.

- We have one publisher and one consumer app
- So if we run one publisher and 5 consumer apps. And will publiblish messages, every  message published will be consumed by consumer app in order , also known as round-robin.

- For example: We have run one publihser and 3 consumers
Publisher publish a message
First consumer handle it
Publisher publish one more message
Second consumer handle it
Publisher publish one more message
Third consumer handle it
Publisher publish one more message
First consumer handle it
and so on...