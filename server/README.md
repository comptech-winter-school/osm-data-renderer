# Серверное приложение OpenStreetMap Data Renderer

Это серверное приложение отвечает за:

* получение и кеширование данных с OpenStreetMap,
* передачу клиенту и поиск объектов (зданий, дорог),
* передачу данных топологии местности.

## Установка и настройка

Запуск приложения и `postgres`:

1. Скопируйте `.env-example` и переименовать его в `.env`;
2. Выполнить команду `docker-compose up --build`.

### Зависимости

* Docker 20.10.12,
* PostgreSQL 12.9,
* Postgis 3.0.
