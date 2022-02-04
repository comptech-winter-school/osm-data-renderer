FROM golang:alpine as builder

RUN apk --no-cache add git

WORKDIR /app/osm-render-back/

COPY . .

WORKDIR /app/osm-render-back/cmd/server

RUN go mod vendor

RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o main

FROM alpine:latest

RUN apk --no-cache add ca-certificates

RUN mkdir /app
WORKDIR /app
COPY --from=builder /app/osm-render-back/cmd/server .
COPY --from=builder /app/osm-render-back/ .
CMD ["./main"]
