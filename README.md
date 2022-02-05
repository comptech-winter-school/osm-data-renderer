# OpenStreetMap Data Renderer

Этот репозиторий содержит реализацию проекта «OpenStreetMap Data Renderer» в рамках зимней школы [CompTech School 2022](https://comptechschool.com/).
Решение представляет собой серверное приложение для получения координат и метаинформации объектов из баз данных
OpenStreetMap и клиентское Unity-приложение, которое помогает в представлении разных географических мест, отрисовывая
безликие карты OpenStreetMap с "видом сверху" в красивую и сочную 3d картинку.

## Назначение

Безликие и плоские карты OpenStreetMap снижают уровень впечатления от их использования и кажутся «сырыми» и уставшими.
Предприятия, использующие OSM, могут потерять потенциальных клиентов, потому что ухудшается не только впечатление от
использования такой картой, но и способность ориентироваться из-за отсутствия понимания ландшафта или высот зданий, а
увеличивается количество людей, которые просто не смогут добраться до пункта их назначения.

Трёхмерный рендеринг OpenStreetMap решает эту проблему. Данный Продукт помогает в понимании расположения того или иного
здания или дороги, а также в представлении разных географических мест, что может положительно подействовать на доходы
использующих это решение предприятий.

## Принцип работы

Продукт позволяет использовать не блёклые, невзрачные и порой даже непонятные 2D карты с «видом сверху», а
привлекательную и объемную карту, которая поможет представить, как выглядит место, куда пользователь хочет попасть.
Конечным результатом Продукта является два приложения:

* серверное, необходимое для хранения и обработки данных,
* клиентское, работающее на Unity, используемое для рендеринга объектов.

<img src="docs/pictures/diagramApplicationDescription.png" alt="Диаграмма описания работы приложения" title="Диаграмма описания работы приложения">

## Структура репозитория

* [`client`](https://github.com/comptech-winter-school/osm-data-renderer/tree/main/client) — клиентское приложение,
* [`docs`](https://github.com/comptech-winter-school/osm-data-renderer/tree/main/docs) — проектная документация,
* [`server`](https://github.com/comptech-winter-school/osm-data-renderer/tree/main/server) — серверное приложение.

## Установка и настройка

Процесс установки и настройки серверного и клиентского приложения подробно описан в соотвествующих директориях.

### Зависимости

Зависимости подробно описаны в соотвествующих директориях.

## Куратор

| [<img src="https://avatars.githubusercontent.com/u/936289?v=4" width="130">](https://github.com/small-jeeper)<br>[Anthony Kireev](https://github.com/small-jeeper) |
|---|
| Антон Киреев |
| TechLead, Avito |

## Команда

| [<img src="https://avatars.githubusercontent.com/u/62282276?v=4">](https://github.com/NacRyTchUk)<br>[NacRyTchUk](https://github.com/NacRyTchUk) | [<img src="https://avatars.githubusercontent.com/u/67387536?v=4">](https://github.com/annstasi/)<br>[annstasi](https://github.com/annstasi/) | [<img src="https://avatars.githubusercontent.com/u/58635649?v=4">](https://github.com/cirno42)<br>[cirno42](https://github.com/cirno42/) | [<img src="https://avatars.githubusercontent.com/u/91747573?v=4">](https://github.com/StarHamster)<br>[StarHamster](https://github.com/StarHamster) | [<img src="https://avatars.githubusercontent.com/u/57074999?v=4">](https://github.com/HaumiRiff)<br>[HaumiRiff](https://github.com/HaumiRiff) | [<img src="https://avatars.githubusercontent.com/u/19913836?v=4">](https://github.com/Vov-etc)<br>[Vov-etc](https://github.com/Vov-etc) | [<img src="https://avatars.githubusercontent.com/u/78679173?v=4">](https://github.com/bernmarx)<br>[bernmarx](https://github.com/bernmarx) |
|---|---|---|---|---|---|---|
| Алексей Авершин | Анастасия Феофанова | Андрей Николотов | Антон Капустинский | Антон Семенов | Владимир Хачатуров | Даниил Стрелкин |
| Back-End Developer | Front-End Developer | Back-End Developer | Технический писатель | Back-End Developer | Front-End Developer | Front-End Developer |
