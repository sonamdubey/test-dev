IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwTempCustomers' AND
     DROP VIEW dbo.vwTempCustomers
GO

	CREATE VIEW [dbo].[vwTempCustomers]
AS
SELECT 
Id	,
Name	,
Address	,
Area	,
City	,
State	,
PinCode	,
phone1	,
phone2	,
Industry	,
Designation	,
Organization	,
DOB	,
ContactHours	,
ContactMode	,
CurrentVehicle	,
InternetUsePlace	,
CarwaleContact	,
InternetUseTime	,
ReceiveNewsletters	,
RegistrationDateTime	
 FROM TempCustomers
