## Arquitectura del Sistema Antifraude

El sistema antifraude está basado en una arquitectura de microservicios y emplea una estrategia de comunicación asíncrona basada en eventos usando Apache Kafka.

TransactionService: expone un endpoint REST para crear transacciones, las almacena en una base de datos PostgreSQL y publica un evento en Kafka cuando se crea una nueva transacción.

AntiFraudService: es un servicio sin endpoints REST, que escucha los eventos de transacciones nuevas desde Kafka. Evalúa si la transacción es fraudulenta y actualiza su estado mediante una llamada al TransactionService.

Esta arquitectura desacopla los servicios y permite escalar el antifraude sin afectar al sistema transaccional.