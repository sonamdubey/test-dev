IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwNewPurchaseCities' AND
     DROP VIEW dbo.vwNewPurchaseCities
GO

	


----------- Modified By Kundan Dombale On 04-11-2015 
---------- Added Archived table NewPurchaseCities01012014_TO_30062015 with union 

	CREATE view [dbo].[vwNewPurchaseCities]
		
		AS

		SELECT  InquiryId,
				CityId,
				City,
				EmailId,
				PhoneNo,
				Name,
				InterestedInLoan,
				MobileVerified,
				ZoneId
		FROM  NewPurchaseCities WITH (NOLOCK)
			
		UNION
		
		SELECT  InquiryId,
				CityId,
				City,
				EmailId,
				PhoneNo,
				Name,
				InterestedInLoan,
				MobileVerified,
				ZoneId

			FROM [CWArchiveTables].[dbo].[NewPurchaseCities01012014_TO_30062015] WITH (NOLOCK)

			UNION

			SELECT InquiryId,
				   CityId,
				   City,
				   EmailId,
				   PhoneNo,
                   Name,
				   NULL  InterestedInLoan,              -- dummy values
				   NULL  MobileVerified,               -- dummy values
				   NULL		ZoneId		 -- dumy values
				 FROM NewPurchaseCities_History WITH (NOLOCK)
