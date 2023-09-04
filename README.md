# CountryExplorer

## Application Description

**CountryExplorer** is a web-based tool that provides information about countries around the world. It allows users to search for countries by name, filter them by population, sort them by name, and even limit the number of results displayed. With an easy-to-use interface, users can explore a wealth of information about countries, including their common names, populations, and more.

Whether you're a traveler planning your next adventure, a student working on a geography project, or just curious about the world, CountryExplorer is your go-to resource for country-related information.

## Running the Application Locally

To run CountryExplorer locally, follow these steps:

1. **Clone the Repository:**
https://github.com/matacarlosj/cmata.git


2. **Navigate to the Project Directory:**
cd cmata

3. **Install Dependencies:**
dotnet restore

4. **Build the Application:**
dotnet build

5. **Run the Application:**
dotnet run

6. **Access the Application:**
Open a web browser and navigate to usually `https://localhost:7104/` to access the api swagger.

## Using the Developed Endpoint

### Filtering Countries by Name

- **Endpoint:**
GET /api/countries/filterByName?search={name}
- **Example 1:**
GET /api/countries/filterByName?search=Spain
- **Example 2:**
GET /api/countries/filterByName?search=Germany


### Sorting Countries by Name

- **Endpoint:**
GET /api/countries/sortByName?sortOrder={ascend/descend}
- **Example (Ascending Order):**
GET /api/countries/sortByName?sortOrder=ascend
- **Example (Descending Order):**
GET /api/countries/sortByName?sortOrder=descend


### Limiting the Number of Records

- **Endpoint:**
GET /api/countries/limit?recordLimit={limit}
- **Example 1 (Limit to 5 Records):**
GET /api/countries/limit?recordLimit=5
- **Example 2 (Limit to 2 Records):**
GET /api/countries/limit?recordLimit=2


### Filtering Countries by Population

- **Endpoint:**
GET /api/countries/filterByPopulation?population={population}
- **Example 1 (Population Less Than 10 Million):**
GET /api/countries/filterByPopulation?population=10000000
- **Example 2 (Population Less Than 1 Million):**
GET /api/countries/filterByPopulation?population=1000000
