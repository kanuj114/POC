14-10-2024
------------
Pre-requisite: Middleware
-----------------
1. How Token Authentication is differ from securing the controller by middleware.
2. What is Token and explain its Architecture ?
3. Can you explain the algorithm of jwT for creating the Token**
4. What is JWT and Token ?
5. What are the important Package to be applied for JWT.

Mis:-
-------
1. Which class and function are responsible to generate the Token.
2. Can you explain the code for authenticating the token which is written inside the
   middleware ?
3. Implement the complete token authentication by DB-First Approach?
4. What is SALT(algorithm)?**
Ans : Have you implemented SALT(algorithm) to secure the password?

19-10-2024
----------
I have taken column for password, salt as varbinary data type.

1. what is varbinary?

cmd: dotnet add package AutoMapper -- version 13.0.1

Add Following Packages :
------------------------
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools[For Code First Approach]
dotnet add package Microsoft.EntityFrameworkCore.Design[For DB First Approach]


Topics
--------
Have you done the Mock Test?
Diff. b/w Nunit and Xunit.
Which Mock Library have you used?

Case Study:
------------
1. We designing the application based on RESTfull principle using Dotnet Core 8.0 with Mock Test.
2. I have list of books but in order to access we need to have the Authentication Mechanism.
3. Dotnet Core provides multiple ways for Authentication:
   A. Middleware  [Not Recommended]
   B. Filter      [Internally uses Session]
   C. Third Party [Not Recommended as Company level]
   D. Token Authentication by JWT Specification.

CMD: dotnet add package Microsoft.IdentityModel.Tokens --version 8.1.2
-----
Session Date: 22-10-2024
-------------------------

{
   "Token":"MySecretTokenForJwt"
},

 DB First Approach: Scaffold command is used to generate Models and EF Core data access layer.
 CMD : dotnet ef dbcontext scaffold "Data Source=Kamlakar\SQLEXPRESS;Initial Catalog=TokenDb;integrated security=true;multipleactiveresultsets=True;Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer -o Models 
    
 varbinary In database converted into Byte[] data type in Model class.  

 -> when we are working on DB First approach, the scaffold commands gives :-

  a. Structure of tables in Model.
  b. Finest EF Core.
  c. FluentApi is a design pattern meant for providing the structure of table in C# class.
  d. scaffold command provides oncofiguring function for connection string
  
  Remarks:
  ----------
   Overall, after applying scaffold, Two functions of DbContext was overridden.
   1st func. : OnModelCreating [will apply FluentAPI]
   2nd func. : Oncofiguring [Provide connection string]

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
         => optionsBuilder.UseSqlServer("Data Source=Kamlakar\\SQLEXPRESS;Initial Catalog=TokenDb;integrated security=true;multipleactiveresultsets=True;Encrypt=False");


   OnModelCreating is a function of DbContext class, needs to override and provide the code for
   structuredness/wellformness of a table known as FluentAPI.
   ModelBuilder is a class to create FluentAPI for structuredness of a table.

      When we apply code First approach, without FluentAPI, after migration, table is completely unstructuredness.
      Example :-
      - Will set NULL to all Columns
      - Also place all columns as varchar(Max).

      - No Foreign Key will be applied.

      -----------------------------------------------------------------------------------------------------------
      Dotnet Core WebApi + EF Core + FluentAPI Pattern + Migration = TblUser (Consistent with all the columns)
      -----------------------------------------------------------------------------------------------------------

      Q 1. While we are sending the data from swagger for creating the account, we are using three properties and
      password is differ by its data type and TblUser consists five properties.

      Example : Passing as string and eventually it will convert to byte[].

      Session Dated :24-10-2024
      -------------------------

   1.Now, we have created the DTO and and Models also.
   2.Now, we need to Configure the Automapper.

   Challenges:
   -----------
   a. Differ in DataTypes
   b. Number of properties are Different.
   c. Ensure that properties name should be similar otherwise, Manually Map the properties

  public class RegisterDTO
    {
        [Required]
        [StringLength (50, MinimumLength=3, ErrorMessage="Email must be at least 3 characters")]
        public string FullName { get; set; }
        [Required]
        [StringLength (50, MinimumLength = 3, ErrorMessage = "Email must be at least 3 character")]
        public string Email {get; set;}
        [Required]
        [StringLength (64, MinimumLength = 8, ErrorMessage = "You must provide password between 8 and 20 characters")]
        public string Password { get; set; }
    }

    --> Have you implemented SALT principle in your project? 
    --> When we are designing the Model ensure that we should not add any extra responsibilty, which violates the SRP.

        [StringLength (50, MinimumLength=3, ErrorMessage="Email must be at least 3 characters")]
        public string FullName { get; set; }
     
     Above code is wrongly designed.

     solution :
     ---------

     Dotnet Core Provides Model validations by "FLUENTMODELVALIDATION".

     CMD :- dotnet add package FluentValidation --version 11.10.0

     We have to apply FLUENTMODELVALIDATION on DTO not in actual Model(entity).
     Can you tell me the class name to implement FLUENTMODELVALIDATION so that we maintain the SRP and to avoid Data Annotations?

     Classname : AbstractValidator<RegisterDTO> in using FluentValidation (namespace).
     --------------------------------------------------------------------------------
     RuleFor(RegisterDTOValidator => RegisterDTOValidator.FullName).NotNull();

     How to Apply?

      Add Package:"FluentValidation.DependencyInjectionExtensions" Version="11.10.0"
      --------------------------------------------------------------------------------
     builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

   as soon as, Token Authentication complete,and we have the POCClient [Done].
   
   Now, we start the frontend Angular.

   -----------Session Dated : 04/11/2024----------- 

public interface IAuthReporitory
{
    Task<TblUser> Register(TblUser user, string password);
    Task<TblUser> Login(string username, string password); 
    Task<bool> UserExists(string username);
}

1. WE have applied Inversion of control(IOC), for AuthRepository and TokenDbContext to achieve loose coupling Architecture.
   Now, we have to apply DI for both the classes(Service and TokenDbContext).

2. We have to create a new account per user and credentials are respectively'

byte[]? Password
-----------------
User will enter the data in string format in password.
Eg : "anuj" But in database table password column is of byte[] type.

We need to apply certain algorithm to convert string to byte[] with encoded data, this process is known as SALT.

byte[]? Salt
-------------
String to byte[] + Extra encoded data = SALT

Need to write a complex code for achieving above equation.

Now, we are creating a function for Password Hash: 
---------------------------------------------------
 private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

-----------Session Dated :- 06-11-2024 ----------

  Below are the Pre-requisite of Token Authentication 
-------------------------------------------------------
1. Types of Encryption :-
a. Symmetric : Single Private Key
Algorithm : Designed by Ron Rivest in 1987.

 Ex:- Getting PDF File : Credentials.  In order to open, need to provide Private key.
 like in AADHAR Card, we use DOB as a private key

b. Asymmetric : Private Key + Public Key
Algorithm :

[Use in case of Network Related like Banking, Insurance like OTP]

[Works on RSA [Rivest, Shamir, Adleman] algorithm] 


2. Hashing or Message Digest [No Public/Private Key] : Works on fixed value

Algorithm : 
SHA, SHA1, MD5, SHA1256
 
[Used Mainly in Database Password, File Handling, session]

3. SALT

Q.1 Have you done the Token Authentication in Dotnet core ?
Ans : Yes

Q.2 Have you save the password for the user in database table ?
Ans : Option :
 1. Encryption : a. Asymmetric [Private Key + Public Key]
                 b. Symmetric [Single Private Key]

 2. Hashing [Message Digest] ?  

 If a user is going to create an account in database, then he is eligible to get the token.
 Can you show me User Table Structure along with Model of that table ?

 Recommendation :
 -----------------

 I have password column in database table
 Now, I have the corresponding column available in Model i.e,  byte[]? Password.
 We never apply Hashing for storing the password in Database Table, Hackers can
 easily decode the password.

 Strongly Recommendation is applying : Hashing + SALT.          
 For : Anuj
 Creating source code in C# for hashing with SHA256.
 "2cf1086c22c7accb399deb0afb88decd77495d29a18a1a5eb2408dc12535aa2c"
 "2cf1086c22c7accb399deb0afb88decd77495d29a18a1a5eb2408dc12535aa2c 1986" + SALT [Additional data]

 No Reversal Possible: You cannot get real/original value.

 Table[Applying all the possibilities of that user and gives the data in Hashing] : Reimann Table
  Above is fixed value : Length : 256

3. SALT ? [Additional data/value]

--------------Session Dated : ---------------

Hashing + SALT : Continue...

Agenda
-------
Practical implementation of Password column should be Hashed + SALT

HMACSHA512()
Above class is primarily meant for generating the hash value with the help of ComputeHash function.

Token Authentication Continue...
Roles to be done: Role Based Authentication. 

Process to create Token
------------------------
steps:
******

1. We have to provide Login to Registered User. 
2. As Soon as user login successfully, we have to provide Token to logged in User.
3. We have to setup Authentication Framework for Token in program.cs 
   [Inbuilt Library is provided by Microsoft for JWT Based Token]

Latest Trend :
*************  
Q1. What are the important features of .netcore 8.0 [from MSDN].

Topics:
******
1. AOT Architecture(8.0).
2. https://weblogs.asp.net/ricardoperes/net-8-dependency-injection-changes-keyed-services
3. New way of achieving dependency injection in dotnet 8.0
4. How to maintain the log?(NLog,)

Q2. Important features of Angular 17 [from Angular.io].
Q3. SSRS (reporting service), SSAS(data mining), SSIS(for migration eg.pdf/otherformat to database).

Token Authentication continues...
******************************
Ques. Have you done authentication and authorization in dotnet core?
Ans.  Yes, we have done the authentication by JWT based Token. Later on, we can access the book controller. 

Identity also called as Claim. An identity can have multiple claims

Claim is a class having overloaded constructor to assign the claim UserId, Email.

JWT Based token works on Private key(placed in appsetting.json : "Token":"MySecretTokenForJwt")

using Microsoft.IdentityModel.Tokens; For identity
// Pre-requisites before generating Tokens

1. For Identity :- Claim class

2. To generate key: SymmetricSecurityKey class (Converted key into bytes)

3. For algorithm : SigningCredentials class.

4. SecurityTokenDescriptor: For Combining Claim + expirytime + cred
   It requires 3 things : Subject + Expiretime + SigningCredentials(creds)

5. JwtSecurityTokenHandler: Responsible for creating the Token.

using Microsoft.IdentityModel.Tokens;
-------------------------------------------------------------------------------------

Now, we need to have certain classes and functions available in certain package in Program.cs to authenticate 
the token, then you are authorise for BookApiContoller.







 