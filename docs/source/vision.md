# Vision

<!-- Копия этого документа находится в ../README.md -->

## Введение

В этом документе описываются цели и возможности продукта, рамки, в которых ограничивается продукт, и рассматриваются
характеристики, требования. Описание продукта, приведенное здесь, является связующем звеном между разработчиком и
заказчиком.

## Позиционирование

Безликие и плоские карты OpenStreetMap снижают уровень впечатления от их использования и кажутся «сырыми» и уставшими.
Предприятия, использующие OSM, могут потенциальных клиентов, потому что ухудшается не только впечатление от
использования такой картой, но и способность ориентироваться из-за отсутствия понимания ландшафта или высот зданий, а
увеличивается количество людей, которые просто не смогут добраться до пункта их назначения.

Трёхмерный рендеринг OpenStreetMap решает эту проблему. Данный продукт помогает в понимании расположения того или иного
здания или дороги, а также в представлении разных географических мест, что может положительно подействовать на доходы
использующих это решение предприятий.

## Обзор продукта

Продукт представляет собой решение для 3D рендеринга OpenStreetMap Data.

### Описание продукта

Конечным результатом продукта является два приложения: серверное для хранения и обработки данных и клиентское,
работающее на Unity, используемое для рендеринга объектов. Он позволяет использовать не блёклые, невзрачные и порой даже
непонятные 2D карты с «видом сверху», а привлекательную и объемную карту, которая поможет представить, как выглядит
место, куда пользователь хочет попасть.

<img src="source\diagramApplicationDescription.png" alt="Диаграмма описания работы приложения" title="Диаграмма описания работы приложения">

### Характеристики и возможности продукта

Продукт должен:

* запрашивать данные с зеркал OSM,
* обрабатывать данные OSM PBF в более удобный для использования и анализа формат,
* запрашивать карту высот,
* хранить и обновлять все запрошенные данные,
* предоставлять и рендерить хранимые данные.

### Ограничения

Продукт учавствует только в рендеринге карты и не изменяет данные OpenStreetMap. Продукт отрисовывает только здания и
дороги. Остальные объекты могут присутствовать, но они не являются основными или обязательными. Также должен происходить
рендеринг местности, но генерация ни текстуры зданий, ни текстуры ландшафта не является основной задачей продукта.

## Другие требования

### Зависимости

* Unity 2020 LTS
* Go 1.17.6
* Procedural Toolkit 0.2.3
* PostgreSQL 14
* Docker 20.10.2

### Системные требования

Минимальные системные требования ограничиваются системными требованиями Unity, как клиентского приложения:

| | Windows | macOS | Linux |
|---|---|---|---|
| Версия OS | Windows 7 (SP1+), Windows 10 или Windows 11, только 64-bit версия. | High Sierra 10.13+ (Intel editor) Big Sur 11.0 (Apple silicon Editor) | Ubuntu 20.04, Ubuntu 18.04 или CentOS 7 |
| CPU | Архитектура X64 с поддержкой набора инструкций SSE2 | Архитектура X64 с поддержкой набора инструкций SSE2 (процессоры Intel) Apple M1 или выше (процессоры Apple) | Архитектура X64 с поддержкой набора инструкций SSE2 |
| Graphics API | DX10, DX11 или DX12-capable GPU | Metal-capable Intel или AMD GPUs | Графические процессоры Nvidia и AMD с поддержкой OpenGL 3.2+ или Vulkan. |

Также необходимо серверное приложение, развернутое на сервере с достаточным объемом хранилища данных.

## Требования к производительности

Продукт должен предоставлять четкую картинку с приемлемым количеством кадров в секунду. Должны отсутствовать ошибки или
недочеты, которые могут привести к сбоям в работе продукта или полностью или частично не позволяют пользоваться им.

## Требования к документации

Документация, в полной мере описывающая работу продукта, должна состоять как минимум из трёх артефактов:

* README.md — документ, описывающий продукт, содержание репозитория, включающий в себя инструкцию по запуску приложения;
* Техническое задание — документ, содержащий детальное описание требований к продукту и его реализации;
* Справочник API/Руководство пользователя — документы, описывающие работу и взаимодействие с продуктом.

## Глоссарий

* [OpenStreetMap](#openstreetmap)
* [Unity](#unity)

### OpenStreetMap

OpenStreetMap (OSM) — проект, который создаёт и предоставляет свободные географические данные, дает возможность
создавать карты всего мира любому человеку, кто этого хочет.

### Unity

Unity —  межплатформенная среда разработки компьютерных игр, разработанная американской компанией Unity Technologies.