IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwSMSSent' AND
     DROP VIEW dbo.vwSMSSent
GO

	Create view vwSMSSent 
AS 
SELECT
		Number,
		Message,
		ServiceType,
		SMSSentDateTime,
		Successfull,
		ReturnedMsg,
		SMSPageUrl,
		ServiceProvider,
		IsSMSSent
FROM SMSSentArchive17062013 WITH (NOLOCK)
UNION ALL
		SELECT
		Number,
		Message,
		ServiceType,
		SMSSentDateTime,
		Successfull,
		ReturnedMsg,
		SMSPageUrl,
		ServiceProvider,
		IsSMSSent
FROM SMSSent WITH (NOLOCK)
