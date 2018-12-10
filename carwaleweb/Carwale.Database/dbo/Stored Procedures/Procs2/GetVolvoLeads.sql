IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetVolvoLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetVolvoLeads]
GO

	
--Created By: Manish Chourasiya on 12-05-2014 for sending data through automated mail to Rajan.
--Description: Leads for Volvo
CREATE PROCEDURE [dbo].[GetVolvoLeads] 
AS 
 BEGIN 
		Declare @startDate datetime
		Declare @endDate datetime

		set @StartDate = convert(datetime,convert(varchar(10),GETDATE()-1,120)+ ' 00:00:00')	
		set @endDate = convert(datetime,convert(varchar(10),GETDATE()-1,120)+ ' 23:59:59');

		

		SELECT DISTINCT C.Id AS CustomerId
			,C.NAME AS CustomerName
			,C.email AS Email
			,C.Mobile AS Mobile
			,CT.NAME AS City
			,vw.make AS CarMake
			,vw.Model AS CarModels
			,vw.Version AS CarVersions
			,n.ReqDateTimeDatePart As ReqDateTime
			,N.BuyTime AS BuyTime
			 ,'S60' As ReportName
		FROM NewCarPurchaseInquiries AS N WITH (NOLOCK)
		JOIN vwMMV AS vw WITH (NOLOCK) ON vw.VersionId = N.CarVersionId
										AND vw.ModelId IN (310,113,1,79)
		JOIN customers AS C WITH (NOLOCK) ON C.Id = N.CustomerId
		inner JOIN Cities AS CT WITH (NOLOCK) ON C.CityId = CT.ID
		WHERE n.RequestDateTime BETWEEN @startDate  AND @endDate
		--2) Query for Volvo-XC60
		UNION ALL
			SELECT DISTINCT C.Id AS CustomerId
				,C.NAME  AS CustomerName
				,C.email AS Email
				,C.Mobile AS Mobile
				,CT.NAME AS City
				,vw.make AS CarMake
				,vw.Model AS CarModels
				,vw.Version AS CarVersions
				,n.ReqDateTimeDatePart As ReqDateTime
				,N.BuyTime  AS BuyTime
				,'XC60' As ReportName
			FROM NewCarPurchaseInquiries AS N WITH (NOLOCK)
			JOIN vwMMV AS vw    WITH (NOLOCK) ON vw.VersionId = N.CarVersionId
											  AND vw.ModelId IN (293,128,238,85)
			JOIN customers AS C  WITH (NOLOCK) ON C.Id = N.CustomerId
			JOIN Cities AS CT   WITH (NOLOCK) ON C.CityId = CT.ID
			WHERE n.RequestDateTime  BETWEEN @startDate AND @endDate
END 