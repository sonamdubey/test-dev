IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VW_LiveListingsDailyLog' AND
     DROP VIEW dbo.VW_LiveListingsDailyLog
GO

	

 


/*************************************************************************************************************
       Description: This view shows all the details of LiveListingsDailyLog 31-07-2013 onwards  

       Created On: 16-11-2015
       Created By: Kundan Dombale

       History:
       
       Edited By               		 EditedON               		 Description
       ---------                	----------               			 -----------
      
       
**********************************************************************************************************/
	CREATE VIEW  [dbo].[VW_LiveListingsDailyLog]
		AS
	SELECT [AsOnDate]
		  ,[ProfileId]
		  ,[SellerType]
		  ,[Seller]
		  ,[Inquiryid]
		  ,[MakeId]
		  ,[MakeName]
		  ,[ModelId]
		  ,[ModelName]
		  ,[VersionId]
		  ,[VersionName]
		  ,[StateId]
		  ,[StateName]
		  ,[CityId]
		  ,[CityName]
		  ,[AreaId]
		  ,[AreaName]
		  ,[Lattitude]
		  ,[Longitude]
		  ,[MakeYear]
		  ,[Price]
		  ,[Kilometers]
		  ,[Color]
		  ,[Comments]
		  ,[EntryDate]
		  ,[LastUpdated]
		  ,[PackageType]
		  ,[ShowDetails]
		  ,[Priority]
		  ,[PhotoCount]
		  ,[FrontImagePath]
		  ,[CertificationId]
		  ,[AdditionalFuel]
		  ,[IsReplicated]
		  ,[HostURL]
		  ,[CalculatedEMI]
		  ,[Score]
		  ,[Responses]
		  ,[CertifiedLogoUrl]
		  ,[Owners]
		  ,[InsertionDate]
		  ,[ClassifiedExpiryDate]
		  ,[IsPremium]
		  ,[VideoCount]
		  ,[SortScore]
		  ,[DealerId]
		  ,[CustomerPackageId]
	  FROM [CWArchiveTables].[dbo].[LiveListingsDailyLogFrom31072013To30062015] WITH (NOLOCK)
	  
	       UNION ALL

	SELECT [AsOnDate]
		  ,[ProfileId]
		  ,[SellerType]
		  ,[Seller]
		  ,[Inquiryid]
		  ,[MakeId]
		  ,[MakeName]
		  ,[ModelId]
		  ,[ModelName]
		  ,[VersionId]
		  ,[VersionName]
		  ,[StateId]
		  ,[StateName]
		  ,[CityId]
		  ,[CityName]
		  ,[AreaId]
		  ,[AreaName]
		  ,[Lattitude]
		  ,[Longitude]
		  ,[MakeYear]
		  ,[Price]
		  ,[Kilometers]
		  ,[Color]
		  ,[Comments]
		  ,[EntryDate]
		  ,[LastUpdated]
		  ,[PackageType]
		  ,[ShowDetails]
		  ,[Priority]
		  ,[PhotoCount]
		  ,[FrontImagePath]
		  ,[CertificationId]
		  ,[AdditionalFuel]
		  ,[IsReplicated]
		  ,[HostURL]
		  ,[CalculatedEMI]
		  ,[Score]
		  ,[Responses]
		  ,[CertifiedLogoUrl]
		  ,[Owners]
		  ,[InsertionDate]
		  ,[ClassifiedExpiryDate]
		  ,[IsPremium]
		  ,[VideoCount]
		  ,[SortScore]
		  ,[DealerId]
		  ,[CustomerPackageId]
	  FROM [dbo].[LiveListingsDailyLog] WITH (NOLOCK)


