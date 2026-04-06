RabbitMQ prototype integration

Det her er den minimale prototype der passer til Franks besked:

1. PizzaGrandiosa backend publisher en besked til RabbitMQ når en SalesOrder bliver oprettet.
2. RabbitMQ kører i docker compose.
3. PizzaRabbitMqClient er en grafisk WPF Windows-klient der lytter på køen `salesorder-created`.

Vigtig praktisk note:
En normal WPF-klient kan ikke startes inde i samme Linux-baserede docker compose setup som resten af løsningen.
Så den realistiske aflevering er normalt:
- `docker compose up` starter postgres + rabbitmq + backend + webapi + web
- WPF-klienten ligger med i solutionen og startes fra Visual Studio på Windows

Sådan tester du:
1. Kør `docker compose up --build`
2. Åbn RabbitMQ UI på http://localhost:15672 (guest/guest)
3. Start `PizzaRabbitMqClient` fra Visual Studio
4. Opret en sales order mod backend, fx POST til http://localhost:8080/api/salesorder/
5. Se at ordren dukker op i WPF-klienten

Eksempel JSON til POST:
{
  "customerId": 1,
  "orderType": "TakeAway",
  "isAccepted": false,
  "isPosted": false,
  "date": "2026-04-06T19:00:00Z",
  "salesLines": []
}
