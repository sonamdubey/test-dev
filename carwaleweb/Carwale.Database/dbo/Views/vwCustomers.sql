IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwCustomers' AND
     DROP VIEW dbo.vwCustomers
GO

	--select * from Customers

create view [dbo].[vwCustomers]
as
select Id	,
Name	,
Address	,
StateId	,
CityId	,
AreaId	,
PrimaryPhone	,
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
RegistrationDateTime	,
IsEmailVerified	,
IsVerified	,
TempId	,
IsFake	,
IsTelephonic	,
Comment	,
EEmpType	,
SourceId	,
MailerCount	,
LastCampaign	,
LastEMailOn	
 from Customers
