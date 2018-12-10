IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_Insert_ZicaLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_Insert_ZicaLeads]
GO

	 -- =============================================
 --Author:		Kundan 
 --Create date: 15/02/2016
 --Description:	 Pushing Zica leads from PQDealerAdLeads table to NCS_TDReq 
 -- modified by Kundan Added LeadType column On 9 March 16
 -- Avishkar Added lead click source 152 
 --=============================================
CREATE PROCEDURE [dbo].[usp_Insert_ZicaLeads]
	AS
 
BEGIN 
SET NOCOUNT ON;
		DECLARE @startDate DATETIME
		DECLARE @EndDate DATETIME	
		
		SET @StartDate = CONVERT(DATETIME,CONVERT(varchar(10),Getdate()-1,120)+ ' 00:00:00')	
		SET @EndDate = CONVERT(DATETIME,CONVERT(varchar(10),Getdate()-1,120)+ ' 23:59:59');

		INSERT INTO  NCS_TDReq (	ModelId,
									VersionId,
									LeadType,--- Added By Kundan On 9th March'16
									Name,
									Email,
									Mobile,
									CreatedOn
		                        )
							SELECT 852 AS ModelId,
								   0 AS VersionId,
								   17,
								   Name,
								   isnull(Email,''), -- modified by kundan on 12/3/2016 added isnull
								   Mobile,
								   RequestDateTime
			  FROM pqdealeradleads WITH(NOLOCK) 
			  WHERE VersionId=0 
			  AND LeadClickSource IN(130,135,136,141,142,152 )
			  AND RequestDateTime Between @startDate AND @EndDate
			  

END 
