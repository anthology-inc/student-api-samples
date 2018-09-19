# Introduction
This repository includes a set of CampusNexus integration samples to help you start working with the CampusNexus APIs.

## DotNetEventHandlers
For those who prefer semicolons and curly brackets over the drag & drop visual experience provided by CampusNexus Workflow, DotNetEventHandlers is a sample .NET class library that introduces how to create event handlers in C#.  For this example, we create two event handlers against the Task entity:
1. Saving - verifies that the user added a note to the task.
2. Constructed - applies default values to new tasks.

To get started:
1. Clone the repository and open **DotNetEventHandlers.sln** in Visual Studio
2. Build the solution
3. Copy DontNetEventHandlers.dll to the \bin folder of your web application

## OData Query Examples
These samples will get you started consuming the CampusNexus Query API using various programming languages.

### DotNetODataClient
DotNetODataClient is a sample solution to get you stated on consuming the CampusNexus Query API using .NET.
To get started:
1. Clone the repository and open **DotNetODataClient.sln** in Visual Studio
2. Open Program.cs and set the **rootUri**, **userName** and **password** variables associated with your envrionment
3. Run the Project

### PythonODataClient
PythonODataClient demonstrates usage of the CampusNexus OData Query API using Python.
To get started:
1. Clone the repository
2. Install the python requests module `pip install requests`
3. Open pythonodataclient.py and set the **root_uri**, **username** and **password** variables to match your environment.
4. Run the program: python pythonodataclient.py

### RubyODataClient
RubyODataClient demonstrates usage of the CampusNexus OData Query API using Ruby.
To get started:
1. Clone the repository
2. Install Dependencies (faraday, faraday_middleware)

This project uses bundler to manage dependencies. You can easily install them:

`bundle install --path=vendor/bundle`

(Alternatively, use gem install)

3. Open odataclient.rb and set the **root_uri**, **username** and **password** variables to match your environment.
4. Run: `bundle exec ruby odataclient.rb`

## Web Command API Examples
These samples will get you started consuming the CampusNexus Command API using various programming languages.

### DotNetWebApiClient
DotNetWebApiClient is a sample solution to get you stated on consuming the CampusNexus Command API using .NET.
To get started:
1. Clone the repository and open **DotNetWebApiClient.sln** in Visual Studio
2. Open Program.cs and set the **rootUri**, **userName** and **password** variables associated with your envrionment
3. Run the Project

### PythonWebApiClient
PythonWebApiClient demonstrates usage of the CampusNexus Command Web API using Python.
To get started:
1. Clone the repository
2. Install the python requests module `pip install requests`
3. Open pythonwebapiclient.py and set the **root_uri**, **username** and **password** variables to match your environment.
4. Run the program: python pythonwebapiclient.py

### RubyWebApiClient
RubyWebApiClient demonstrates usage of the CampusNexus Command API using Ruby.
To get started:
1. Clone the repository
2. Install Dependencies (faraday, faraday_middleware, ruby_dig)

This project uses bundler to manage dependencies. You can easily install them:

`bundle install --path=vendor/bundle`

(Alternatively, use gem install)

3. Open webapiclient.rb and set the **root_uri**, **username** and **password** variables to match your environment.
4. Run: `bundle exec ruby webapiclient.rb`

## PostmanCollections
A set of [Postman](https://www.getpostman.com/) collections that assist in discovering how to invoke CampusNexus Query Model and Command Model APIs.

### Queries
Below is a list of the Postman requests found in the Queries folder of the Collection.
1. **Read the service root**
All REST APIs should have a single entry point from which a generic hypermedia client can navigate to the resources in the service. In the response we see links to three things: 1. The $metadata document that describes the schema of ther service 2. Links to various collections of objects like People and Airports 3. Links to other top-level items like Me (a singleton) and operations.
2. **Read the service metadata**
$metadata is an endpoint in OData services that contains a machine-readable description of the service model including type schemas, available operations, etc.
3. **Read an entity set**
One of the most common responses from a REST API is a collection of resources. In this case we asked for the People collection. For each response, the OData service writes a self-described response (another REST principle) by annotating the response with a context URL. This context URL tells the service that the contents of the response are a collection of things in the People entity set. The @odata.nextLink annotation is present because the server opted to split the result set across multiple pages. The client can also drive paging using $top and $skip, but server-side paging is a mitigation against DoS attacks. The value property contains the bulk of the response. Note that @odata.id and @odata.editLink should generally not be present in the payload unless they deviate from convention. In this case it appears that there is a bug in our sample service. Pretend those properties aren't there.
4. **Get a single entity from an entity set**
To get a particular entity from a collection, append a key segment. By default, key segments in OData services are bounded by parens because they may be composite keys, e.g., OrderLine(OrderId=1,LineNumber=1) or alternate keys, e.g., Person(SSN='000-00-0000') and Person(2115) both address the same resource. Some OData services use normal URL segments for key segments, e.g., Orders/1. This is not recommended because of the scenarios mentioned above.
5. **Get a primititve property**
Even when fetching a primitive property, an object wrapper is returned rather than returning the raw primitive. This is to protect against a JSON vulnerability.
6. **Navigate to related properties**
To navigate the resource graph, keep appending segments representing valid property names as defined in $metadata or in a full metadata response (see query x). In this case we have started from the service root, navigated to the entity set People, navigated to the resource keyed 'russellwhyte', navigated to the Friends property, navigated to the resource keyed 'scottketchum', and finally navigated to the AddressInfo property. Note that the @odata.context URL self-describes the payload.
7. **Filter a collection**
The $filter system query option can be used to filter any collection of resources. Note that the response to a filtered collection is a collection of the same type, regardless of the number of matched resources.
8. **Filter on a nested structure**
You can use any related properties in a filter clause by using the same segments used in the path to traverse properties.
9. **Filter using logic operators**
You can use and, or and not to create more complex filter clauses.
10. **Filter using any/all operators**
You can use any/all lambda-style filters for collection properties.
11. **Sort a collection**
You can use the $orderby system query option to specify ordering criteria. You can use many of the functions usable in $filter in $orderby as well.
12. **Client-side paging**
There are two types of paging in OData services. Servers can enable server-side paging, returning nextLinks that clients can follow to subsequent pages. Clients can drive paging using $top and $skip.
13. **Counting the elements in a collection**
If you want to know how many items meet a condition, you can use the $count path segment. Note that the Content-Type header indicates that the content is text/plain. Although it doesn't work with system query options in the reference service, $count can typically be combined with $filter.
14. **Expand related entities**
You can use the $expand system query option to include related resources. The expand clause can include many of the other system query options, enabling left-join type semantics. Also, you can expand <property>/$ref in order to get just the links to the related resources.
15. **Select the properties**
You can use the $select system query option to only return specified properties.
16. **Request full metadata**
By default OData services return an extremely compact JSON format. This happens by stripping out all of the metadata that should be calculable by "smart" OData clients. For generic hypermedia clients, you can request additional metadata by using the Accept header or $format system query option to request application/json;odata.metadata=full. In this case, we get a bunch of additional annotations in the payload indicating type information and relationships to related resources.
17. **Invoke a bound function**
  OData support bound custom functions. The bound functions are bounded to a resource. Note: OData functions CANNOT have side effect, so only GET verb is allowed.
### Commands
Below is a list of the Postman requests found in the Commands folder of the Collection.
1. **Get Commands**
  Gets a list of all Commands broken down by domain: Academics, Admissions, Career Services, Common, CRM, Financial Aid, Student Accounts and Student Services
2. **Create Course Type**
  An example of how to create a new entity, in this case a Course Type.  It is important to note that this operation will initialize the entity and set any defaults applied by event handlers; however, it will not persist any data to the database.  To save an entity you must use the SaveNew or Save commands.
3. **Save New Course Type**
  An example of how to save a newly created entity.
4. **Get Course Type**
  An example of how to retrieve an existing entity.  Due to the way optomistic concurrency is implemented, this operation is required prior to calling the Save operation.
5. **Delete Course Type**
  Deletes an existing entity.  
6. **StudentPreviousEducation.Get**
  Represents how to invoke a non-CRUD related operation.  This operation will retieve the specified Student's previous education.
7. **Cache.Remove**
  Represents how to invoke a non-CRUD related operation.  This operation will evict the cache associated with the specified cache key.
8. **Cache.Clear**
  Represents how to invoke a non-CRUD related operation.  This operation will clear the server-side cache.
