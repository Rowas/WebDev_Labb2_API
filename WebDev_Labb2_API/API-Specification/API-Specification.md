# API-specifikation för WebDev_Labb2_API

## Innehåll
1. [Allmän information](#allmän-information)
2. [Login API](#login-api)
3. [Orders API](#orders-api)
4. [Products API](#products-api)
5. [Customers API](#customers-api)
6. [Datamodeller](#datamodeller)
7. [Statuskoder och felhantering](#statuskoder-och-felhantering)
8. [Säkerhet och Auktorisering](#säkerhet-och-auktorisering)

## Allmän information
Alla API-anrop använder JSON för både förfrågnings- och svarsdata. API:et använder MongoDB som databas och är byggt med ASP.NET Core. Basvägen för API:et är inte angiven i endpoint-adresserna nedan.

API:et tillåter CORS (Cross-Origin Resource Sharing) från alla ursprung (*) med stöd för:
- Alla headers
- Alla HTTP-metoder

### Versionshantering
API:et är för närvarande i sin första version och använder inte explicit versionshantering i URL:erna.

## Login API

### Logga in
- **Endpoint:** `POST /Login`
- **Beskrivning:** Autentiserar en användare och returnerar en JWT-token
- **Auktorisering:** Ingen auktorisering krävs (publik endpoint)
- **Förfrågningskropp:**
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Statuskoder:**
  - `200 OK`: Inloggning lyckades
  - `400 Bad Request`: Felaktiga uppgifter
    - Felmeddelande: `{"message": "Kunden hittades inte eller lösenordet var felaktigt"}`
  - `500 Internal Server Error`: Serverfel
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "token": "JWT-token"
  }
  ```

### Validera token
- **Endpoint:** `GET /Login/validate`
- **Beskrivning:** Kontrollerar om en JWT-token är giltig
- **Headers:**
  - `Authorization`: Bearer {token}
- **Statuskoder:**
  - `200 OK`: Token är giltig
  - `401 Unauthorized`: Token är ogiltig eller saknas
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "user": {
      "username": "string",
      "userlevel": "string",
      "firstname": "string",
      "lastname": "string",
      "email": "string"
    }
  }
  ```

## Orders API

### Hämta alla beställningar
- **Endpoint:** `GET /Orders`
- **Beskrivning:** Hämtar en lista med alla beställningar i systemet, sorterade efter beställningsdatum
- **Auktorisering:** Kräver Admin-roll
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren har inte Admin-behörighet
  - `500 Internal Server Error`: Serverfel

### Hämta användarens beställningar
- **Endpoint:** `GET /Orders/user/{username}`
- **Beskrivning:** Hämtar alla beställningar för en specifik användare
- **Auktorisering:** Kräver autentisering, användare kan endast se sina egna ordrar
- **Parametrar:**
  - `username` (sträng, obligatorisk): Användarnamn
- **Statuskoder:**
  - `200 OK`: Beställningar hittade
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren försöker se någon annans ordrar
  - `500 Internal Server Error`: Serverfel

### Hämta specifik beställning
- **Endpoint:** `GET /Orders/{orderId}`
- **Beskrivning:** Hämtar en specifik beställning baserat på beställningsnummer
- **Auktorisering:** Kräver autentisering, användare kan endast se sina egna ordrar
- **Parametrar:**
  - `orderId` (sträng, obligatorisk): Unikt ordernummer
- **Statuskoder:**
  - `200 OK`: Beställning hittad
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren försöker se någon annans order
  - `404 Not Found`: Beställning ej hittad
  - `500 Internal Server Error`: Serverfel

### Skapa ny beställning
- **Endpoint:** `POST /Orders`
- **Beskrivning:** Skapar en ny beställning i systemet
- **Auktorisering:** Kräver autentisering
- **Förfrågningskropp:**
  ```json
  {
    "order_id": "string",
    "item_list": [
      {
        "sku": "int",
        "quantity": "int"
      }
    ]
  }
  ```
- **Statuskoder:**
  - `201 Created`: Beställning skapad
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `500 Internal Server Error`: Serverfel

### Uppdatera orderstatus
- **Endpoint:** `PATCH /Orders/{orderId}/status`
- **Beskrivning:** Uppdaterar status för en specifik order
- **Auktorisering:** Kräver Admin-roll
- **Parametrar:**
  - `orderId` (sträng, obligatorisk): Ordernummer
- **Förfrågningskropp:** String med ny status
- **Statuskoder:**
  - `200 OK`: Status uppdaterad
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren har inte Admin-behörighet
  - `404 Not Found`: Order hittades inte
  - `500 Internal Server Error`: Serverfel

### Hämta ordrar inom datumintervall
- **Endpoint:** `GET /Orders/date-range`
- **Beskrivning:** Hämtar ordrar inom ett specifikt datumintervall
- **Auktorisering:** Kräver Admin-roll
- **Query-parametrar:**
  - `startDate` (datum, obligatorisk): Startdatum (åååå-mm-dd)
  - `endDate` (datum, obligatorisk): Slutdatum (åååå-mm-dd)
- **Statuskoder:**
  - `200 OK`: Ordrar hittade
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren har inte Admin-behörighet
  - `500 Internal Server Error`: Serverfel
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "orders": [/* samma format som ovan */]
  }
  ```

## Products API

### Hämta alla produkter
- **Endpoint:** `GET /Products`
- **Beskrivning:** Hämtar en lista med alla produkter
- **Auktorisering:** Ingen auktorisering krävs (publik endpoint)
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `500 Internal Server Error`: Serverfel

### Hämta specifik produkt
- **Endpoint:** `GET /Products/{sku_or_name}`
- **Beskrivning:** Hämtar en produkt baserat på SKU eller namn
- **Auktorisering:** Ingen auktorisering krävs (publik endpoint)
- **Parametrar:**
  - `sku_or_name` (sträng, obligatorisk): Produkt-SKU (numeriskt) eller produktnamn
- **Statuskoder:**
  - `200 OK`: Produkt hittad
  - `404 Not Found`: Produkt ej hittad
  - `500 Internal Server Error`: Serverfel

### Hämta produkter efter kategori
- **Endpoint:** `GET /Products/category/{category}`
- **Beskrivning:** Hämtar produkter baserat på kategori
- **Auktorisering:** Ingen auktorisering krävs (publik endpoint)
- **Parametrar:**
  - `category` (sträng, obligatorisk): Produktkategori
- **Statuskoder:**
  - `200 OK`: Produkter hittade
  - `404 Not Found`: Inga produkter hittades i kategorin
  - `500 Internal Server Error`: Serverfel

### Lägg till produkt
- **Endpoint:** `POST /Products`
- **Beskrivning:** Skapar en ny produkt
- **Auktorisering:** Kräver Admin-roll
- **Förfrågningskropp:** Products-objekt
- **Statuskoder:**
  - `201 Created`: Produkt skapad
  - `400 Bad Request`: Produkten finns redan
    - Felmeddelande: `{"message": "Product already exists."}`
  - `500 Internal Server Error`: Serverfel

### Uppdatera produkt
- **Endpoint:** `PUT /Products`
- **Beskrivning:** Uppdaterar en existerande produkt
- **Auktorisering:** Kräver Admin-roll
- **Förfrågningskropp:** Products-objekt med attribut som ska uppdateras
- **Statuskoder:**
  - `200 OK`: Produkt uppdaterad
  - `404 Not Found`: Produkt hittades inte
  - `500 Internal Server Error`: Serverfel

### Ta bort produkt
- **Endpoint:** `DELETE /Products`
- **Beskrivning:** Tar bort en produkt från systemet
- **Auktorisering:** Kräver Admin-roll
- **Query-parametrar:**
  - `sku` (heltal, obligatorisk): Produkt-SKU att ta bort
- **Statuskoder:**
  - `204 No Content`: Produkt borttagen
  - `404 Not Found`: Produkt hittades inte
  - `500 Internal Server Error`: Serverfel

## Customers API

### Hämta alla kunder
- **Endpoint:** `GET /Customers`
- **Beskrivning:** Hämtar en lista med alla kunder
- **Auktorisering:** Kräver Admin-roll
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren har inte Admin-behörighet
  - `500 Internal Server Error`: Serverfel

### Hämta specifik kund
- **Endpoint:** `GET /Customers/{email}`
- **Beskrivning:** Hämtar en kund baserat på e-postadress
- **Auktorisering:** Kräver autentisering, användare kan endast se sin egen profil
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Statuskoder:**
  - `200 OK`: Kund hittad
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren försöker se någon annans profil
  - `404 Not Found`: Kund ej hittad
  - `500 Internal Server Error`: Serverfel

### Lägg till kund
- **Endpoint:** `POST /Customers`
- **Beskrivning:** Skapar en ny kund
- **Förfrågningskropp:** Customers-objekt
- **Statuskoder:**
  - `201 Created`: Kund skapad
  - `400 Bad Request`: Kunden finns redan eller användarnamnet är upptaget
    - Felmeddelande: `{"message": "Customer already exists."}` 
  - `500 Internal Server Error`: Serverfel

### Uppdatera kund
- **Endpoint:** `PATCH /Customers/{email}`
- **Beskrivning:** Uppdaterar en existerande kund
- **Auktorisering:** Kräver autentisering, användare kan endast uppdatera sin egen profil
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Förfrågningskropp:** Customers-objekt med attribut som ska uppdateras
- **Statuskoder:**
  - `200 OK`: Kund uppdaterad
  - `400 Bad Request`: Kunden hittades inte
    - Felmeddelande: `{"message": "Customer not found"}`
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren försöker uppdatera någon annans profil
  - `500 Internal Server Error`: Serverfel

### Ta bort kund
- **Endpoint:** `DELETE /Customers/{email}`
- **Beskrivning:** Tar bort en kund från systemet
- **Auktorisering:** Kräver Admin-roll
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Statuskoder:**
  - `200 OK`: Kund borttagen
  - `400 Bad Request`: Kunden hittades inte
    - Felmeddelande: `{"message": "Customer not found"}`
  - `401 Unauthorized`: Användaren är inte autentiserad
  - `403 Forbidden`: Användaren har inte Admin-behörighet
  - `500 Internal Server Error`: Serverfel

## Datamodeller

### Orders
```json
{
  "Id": "ObjectId (genereras automatiskt)",
  "username": "string",
  "order_id": "string",
  "order_date": "åååå-mm-dd",
  "delivery_date": "åååå-mm-dd",
  "status": "string",
  "item_list": [
    {
      "sku": "int",
      "quantity": "int"
    }
  ]
}
```

### Products
```json
{
  "Id": "ObjectId (genereras automatiskt)",
  "sku": "int",
  "price": "double",
  "in_stock": "bool",
  "name": "string",
  "appearance": "string eller null",
  "atomic_mass": "double",
  "boil": "double eller null",
  "category": "string",
  "density": "double eller null",
  "discovered_by": "string eller null",
  "melt": "double eller null",
  "molar_heat": "double eller null",
  "named_by": "string eller null",
  "number": "double",
  "period": "int eller null",
  "group": "int eller null",
  "phase": "string",
  "source": "string eller null",
  "bohr_model_image": "string eller null",
  "bohr_model_3d": "string eller null",
  "spectral_img": "string eller null",
  "summary": "string",
  "symbol": "string",
  "xpos": "int eller null",
  "ypos": "int eller null",
  "wxpos": "int eller null",
  "wypos": "int eller null",
  "shells": "array av int eller null",
  "electron_configuration": "string eller null",
  "electron_configuration_semantic": "string eller null",
  "electron_affinity": "double eller null",
  "electronegativity_pauling": "double eller null",
  "ionization_energies": "array av double eller null",
  "cpk_hex": "string",
  "block": "string"
}
```

### Customers
```json
{
  "Id": "ObjectId (genereras automatiskt)",
  "username": "string",
  "password": "string (hashas innan lagring, returneras aldrig)",
  "userlevel": "string (automatiskt 'customer')",
  "firstname": "string",
  "lastname": "string",
  "email": "string",
  "mobile_number": "string",
  "delivery_adress": {
    "street": "string",
    "post_code": "string",
    "city": "string",
    "country": "string"
  }
}
```

## Statuskoder och felhantering

### Vanliga statuskoder
- `200 OK`: Förfrågan lyckades
- `201 Created`: Resurs skapad
- `204 No Content`: Resurs borttagen
- `400 Bad Request`: Felaktig förfrågan, t.ex. ogiltig data eller entitet finns redan/hittades inte
- `401 Unauthorized`: Användaren är inte autentiserad
- `403 Forbidden`: Användaren saknar behörighet
- `404 Not Found`: Entiteten finns inte
- `500 Internal Server Error`: Serverfel

### Standardformat för lyckat svar
```json
{
  "message": "Success",
  "data_eller_entitet": { ... }
}
```

### Standardformat för felsvar
```json
{
  "message": "Felbeskrivning"
}
```

### Felhantering
- Vid serverfel (500) returneras ett standardiserat felmeddelande: `{"message": "Ett internt fel har inträffat"}`
- Vid 401 Unauthorized returneras när token saknas eller är ogiltig
- Vid 403 Forbidden returneras när användaren saknar tillräckliga behörigheter
- Vid 404 Not Found returneras när resursen inte hittas
- Vid 400 Bad Request returneras ett beskrivande felmeddelande i standardformat

## Säkerhet och Auktorisering

### JWT Authentication
- Alla skyddade endpoints kräver en giltig JWT-token i Authorization-headern
- Format: `Authorization: Bearer {token}`
- Token innehåller följande claims:
  - `sub`: Användarens username
  - `jti`: Unikt token-ID
  - `role`: Användarens roll (Admin/Customer)
  - `username`: Användarens username
  - `isAdmin`: true/false
  - `permissions`: full_access/limited_access
- Token har en giltighetstid på 30 minuter
- Token valideras automatiskt av JWT middleware

### Token Validering
- Kontrollerar token-signatur
- Verifierar utfärdare (issuer)
- Verifierar mottagare (audience)
- Kontrollerar utgångsdatum
- Validerar alla claims

### Roller och Behörigheter
- **Admin:**
  - Full åtkomst till alla endpoints
  - Kan se och hantera alla användare och ordrar
  - Kan hantera produktsortimentet
- **Customer:**
  - Kan se och hantera sina egna ordrar
  - Kan se produkter
  - Kan uppdatera sin egen profil 