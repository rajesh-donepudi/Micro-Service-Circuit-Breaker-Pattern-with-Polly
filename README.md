# STARSHIP SYSTEM Architecture

![Untitled Diagram drawio](https://user-images.githubusercontent.com/61657297/229997042-015dfade-9f55-4fb2-9650-abde378d1450.png)

## Circuit Breaker Pattern

* The microservice circuit breaker pattern is a design pattern used in distributed systems.
* It detects and isolates faults in a service to prevent cascading failures.
* A circuit breaker component monitors the availability of a service and prevents further calls if the service is unresponsive.
* The pattern incorporates other design patterns such as bulkheads, timeouts, and retries.
* Bulkheads isolate different parts of the system to prevent one failure from affecting other parts.
* Timeouts and retries are mechanisms used to handle temporary failures by limiting the time a system waits for a response and automatically retrying failed requests.
* The microservice circuit breaker pattern is essential for building reliable and fault-tolerant distributed systems.

## Pros
The circuit breaker pattern provides several advantages for building resilient and reliable distributed systems:

* Fault tolerance: The circuit breaker pattern helps to prevent cascading failures in a distributed system by isolating and containing failures.

* Fail-fast: The circuit breaker pattern enables fast failure detection and helps to prevent system overloading by quickly responding to failures.

* Graceful degradation: The pattern provides a graceful degradation of services by failing over to alternative services or fallback mechanisms, ensuring that the system remains functional even when some services are down.

* Increased stability: The circuit breaker pattern helps to increase system stability by reducing the number of failed requests and improving the overall system performance.

* Reduced downtime: The pattern helps to reduce downtime by quickly detecting and recovering from failures, ensuring that the system is always available.

* Easy maintenance: The pattern provides an easy way to manage and maintain a distributed system by providing a clear separation of concerns between different services and components.

* Overall, the circuit breaker pattern is an essential tool for building resilient and reliable distributed systems that can withstand failures and continue to function even when some services are down.

## Cons

While the circuit breaker pattern provides several advantages for building resilient distributed systems, there are also some potential disadvantages:

* Increased complexity: Implementing the circuit breaker pattern can add additional complexity to the system, particularly when dealing with multiple services and components.

* Overhead: The circuit breaker pattern introduces additional overhead to the system, particularly when monitoring service availability and responding to failures.

* False positives: In some cases, the circuit breaker may trip unnecessarily, blocking requests to a service even when it is available, leading to increased latency and reduced system performance.

* Lack of visibility: The circuit breaker pattern can make it more difficult to identify the root cause of failures, particularly if multiple services or components are involved.

* Difficult to configure: Setting appropriate thresholds and timeouts for the circuit breaker can be challenging, requiring careful tuning and testing to achieve optimal performance.

Overall, while the circuit breaker pattern provides important benefits for building resilient distributed systems, it is important to carefully consider the potential disadvantages and trade-offs involved in its implementation.

## [Polly](https://github.com/App-vNext/Polly)
Polly Circuit Breaker Pattern is a .NET library that provides an implementation of the circuit breaker pattern for managing and handling faults in distributed systems. The library is designed to help developers create robust and fault-tolerant applications by providing a set of policies for handling different types of failures, including timeouts, exceptions, and network failures.

## Example

The starship system is an example of a Micro Service system in which different services communicate with each other in a resilient way.

### Overview of services

EARTH Service, MOON Service, and MARS Service are independent services of the StarShip system.

The Earth Service requests to fetch weather forecast information from MOON Service.
In the event of failure of MOON service, the earth will communicate to MARS Service to fetch weather forecast information.
