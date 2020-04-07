This project is responsible for scheduling the payment requests made by the client into a queue.
This queue will later be consumed by a worker project which can be horizontally scaled to process
payment requets in parallel.