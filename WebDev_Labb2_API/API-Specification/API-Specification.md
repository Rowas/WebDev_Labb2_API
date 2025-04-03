# API-specifikation för WebDev_Labb2_API

## Innehåll
1. [Allmän information](#allmän-information)
2. [Login API](#login-api)
3. [Orders API](#orders-api)
4. [Products API](#products-api)
5. [Customers API](#customers-api)
6. [Datamodeller](#datamodeller)
7. [Statuskoder och felhantering](#statuskoder-och-felhantering)

## Allmän information
Alla API-anrop använder JSON för både förfrågnings- och svarsdata. API:et använder MongoDB som databas och är byggt med ASP.NET Core. Basvägen för API:et är inte angiven i endpoint-adresserna nedan.

## Login API

### Logga in
- **Endpoint:** `POST /Login`
- **Beskrivning:** Autentiserar en användare och returnerar en JWT-token
- **Förfrågningskropp:**
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Statuskoder:**
  - `200 OK`: Inloggning lyckades
  - `400 Bad Request`: Felaktiga uppgifter
    - Felmeddelande: `{"message": "Invalid username or password"}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "token": "JWT-token",
    "user": {
      "username": "string",
      "userlevel": "string",
      "firstname": "string",
      "lastname": "string",
      "email": "string"
    }
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
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat:**
  ```json
  [
    {
      "Id": "ObjectId",
      "username": "användarnamn",
      "order_id": "ordernummer",
      "order_date": "2023-04-15",
      "delivery_date": "2023-04-20",
      "status": "levererad",
      "item_list": [
        {
          "sku": 1001,
          "quantity": 2
        }
      ]
    }
  ]
  ```

### Hämta specifik beställning
- **Endpoint:** `GET /Orders/{order_id}`
- **Beskrivning:** Hämtar en specifik beställning baserat på beställningsnummer
- **Parametrar:**
  - `order_id` (sträng, obligatorisk): Unikt ordernummer 
- **Statuskoder:**
  - `200 OK`: Beställning hittad
  - `404 Not Found`: Beställning ej hittad (returnerar null)
  - `500 Internal Server Error`: Serverfel (returnerar null)

### Skapa ny beställning
- **Endpoint:** `POST /Orders`
- **Beskrivning:** Skapar en ny beställning i systemet
- **Förfrågningskropp:** Orders-objekt (se datamodeller)
- **Statuskoder:**
  - `200 OK`: Beställning skapad
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat:**
  ```json
  {
    "message": "Success",
    "receivedOrder": {
      "Id": "genererad-ObjectId",
      "username": "användarnamn",
      "order_id": "ordernummer",
      "order_date": "2023-04-15",
      "delivery_date": "2023-04-20",
      "status": "pågående",
      "item_list": [
        {
          "sku": 1001,
          "quantity": 2
        }
      ]
    }
  }
  ```

## Products API

### Hämta alla produkter
- **Endpoint:** `GET /Products`
- **Beskrivning:** Hämtar en lista med alla produkter sorterade efter produktnummer
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `500 Internal Server Error`: Serverfel (returnerar null)

### Hämta specifik produkt
- **Endpoint:** `GET /Products/{sku_or_name}`
- **Beskrivning:** Hämtar en produkt baserat på SKU eller namn
- **Parametrar:**
  - `sku_or_name` (sträng, obligatorisk): Produkt-SKU (numeriskt) eller produktnamn
- **Statuskoder:**
  - `200 OK`: Produkt hittad
  - `404 Not Found`: Produkt ej hittad (returnerar null)
  - `500 Internal Server Error`: Serverfel (returnerar null)

### Uppdatera produkt
- **Endpoint:** `PATCH /Products/{ProdToUpdate}`
- **Beskrivning:** Uppdaterar en existerande produkt
- **Parametrar:**
  - `ProdToUpdate` (sträng, obligatorisk): SKU eller namn på produkten
- **Förfrågningskropp:** Products-objekt med attribut som ska uppdateras
- **Statuskoder:**
  - `200 OK`: Produkt uppdaterad
  - `400 Bad Request`: Produkten hittades inte
    - Felmeddelande: `{"message": "Product not found."}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "product": {
      "Id": "ObjectId",
      "sku": 1001,
      "price": 199.99,
      "in_stock": true,
      "name": "Syre",
      ...
    }
  }
  ```

### Lägg till produkt
- **Endpoint:** `POST /Products`
- **Beskrivning:** Skapar en ny produkt
- **Förfrågningskropp:** Products-objekt
- **Statuskoder:**
  - `200 OK`: Produkt skapad
  - `400 Bad Request`: Produkten finns redan
    - Felmeddelande: `{"message": "Product already exists."}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "receivedProduct": {
      "Id": "genererad-ObjectId",
      "sku": 1001,
      "price": 199.99,
      ...
    }
  }
  ```

### Ta bort produkt
- **Endpoint:** `DELETE /Products/{sku}`
- **Beskrivning:** Tar bort en produkt från systemet
- **Parametrar:**
  - `sku` (heltal, obligatorisk): Produkt-SKU att ta bort
- **Statuskoder:**
  - `200 OK`: Produkt borttagen
  - `400 Bad Request`: Produkten hittades inte
    - Felmeddelande: `{"message": "Product not found."}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "product": {
      "Id": "ObjectId",
      "sku": 1001,
      ...
    }
  }
  ```

## Customers API

### Hämta alla kunder
- **Endpoint:** `GET /Customers`
- **Beskrivning:** Hämtar en lista med alla kunder
- **Statuskoder:**
  - `200 OK`: Listan returneras (även om den är tom)
  - `500 Internal Server Error`: Serverfel (returnerar null)

### Hämta specifik kund
- **Endpoint:** `GET /Customers/{email}`
- **Beskrivning:** Hämtar en kund baserat på e-postadress
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Statuskoder:**
  - `200 OK`: Kund hittad
  - `404 Not Found`: Kund ej hittad (returnerar null)
  - `500 Internal Server Error`: Serverfel (returnerar null)

### Lägg till kund
- **Endpoint:** `POST /Customers`
- **Beskrivning:** Skapar en ny kund
- **Förfrågningskropp:** Customers-objekt
- **Statuskoder:**
  - `200 OK`: Kund skapad
  - `400 Bad Request`: Kunden finns redan eller användarnamnet är upptaget
    - Felmeddelande: `{"message": "Customer already exists."}` eller
    - Felmeddelande: `{"message": "Username already exists."}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "customer": {
      "Id": "genererad-ObjectId",
      "username": "användarnamn",
      "userlevel": "customer",
      "firstname": "Förnamn",
      "lastname": "Efternamn",
      "email": "e-post@exempel.se",
      "mobile_number": "0701234567",
      "delivery_adress": {
        "street": "Gatan 1",
        "post_code": "12345",
        "city": "Stad",
        "country": "Land"
      }
    }
  }
  ```

### Uppdatera kund
- **Endpoint:** `PATCH /Customers/{email}`
- **Beskrivning:** Uppdaterar en existerande kund
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Förfrågningskropp:** Customers-objekt med attribut som ska uppdateras
- **Statuskoder:**
  - `200 OK`: Kund uppdaterad
  - `400 Bad Request`: Kunden hittades inte
    - Felmeddelande: `{"message": "Customer not found"}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "customer": {
      "Id": "ObjectId",
      "username": "användarnamn",
      ...
    }
  }
  ```

### Ta bort kund
- **Endpoint:** `DELETE /Customers/{email}`
- **Beskrivning:** Tar bort en kund från systemet
- **Parametrar:**
  - `email` (sträng, obligatorisk): Kundens e-postadress
- **Statuskoder:**
  - `200 OK`: Kund borttagen
  - `400 Bad Request`: Kunden hittades inte
    - Felmeddelande: `{"message": "Customer not found"}`
  - `500 Internal Server Error`: Serverfel (returnerar null)
- **Svarsformat (lyckat):**
  ```json
  {
    "message": "Success",
    "customer": {
      "Id": "ObjectId",
      "username": "användarnamn",
      ...
    }
  }
  ```

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
- `400 Bad Request`: Felaktig förfrågan, t.ex. ogiltig data eller entitet finns redan/hittades inte
- `404 Not Found`: Entiteten finns inte 
- `500 Internal Server Error`: Serverfel, oftast på grund av databasproblem

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
- Vid serverfel (500) returneras `null` istället för ett felmeddelande
- Vid 404 Not Found returneras `null` istället för ett felmeddelande
- Vid 400 Bad Request returneras ett felmeddelande i standardformat 