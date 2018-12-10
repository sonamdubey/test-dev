IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FLCMailerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FLCMailerData]
GO

	CREATE PROCEDURE [dbo].[CRM_FLCMailerData]
--Name of SP/Function						: CRM_PQTakenOnce
--Applications using SP						: CRM
--Modules using the SP						: PQTaken.cs
--Technical department						: Database
--Summary                                   : PQ Taken once only
--Author                                    : AMIT Kumar 17-Jul-2012
--Modification history                      : 1. Amit Kumar 12th aug 2013 (Added constrain for Not interested)

@FromDate			DATETIME,
@ToDate				DATETIME,
@Make				BIGINT,
-----------------------------
@NotInterested		SMALLINT,
@LeadStageId		SMALLINT,
@ETSubDispositionId SMALLINT
AS
BEGIN
	WITH CTE AS 
		( 
			 SELECT ROW_NUMBER() OVER (PARTITION BY CBD.LeadId ORDER BY CBD.LeadId) AS RowNumber ,CBD.LeadId AS LeadId, 
				VW .Car AS CarName, CC.FirstName AS CustName, CC.Email, CBD.Id AS CBDId, VW.MakeId AS MakeId, CC.ID AS CustomerId,
				CASE ISNULL(CLS.CategoryId,0) WHEN 3 THEN LA.HeadAgencyId ELSE 1 END AS LeadHeadAgency
				---------------------
				,CETD.Name AS SubDispo
				---------------------
			 FROM CRM_CarBasicData CBD (NOLOCK) 
				 INNER JOIN CRM_Leads CL ( NOLOCK) ON CL.ID= CBD.LeadId 
				 INNER JOIN CRM_EventLogs CEL ( NOLOCK) ON CL.ID = CEL.ItemId AND CEL.EventType = 4
				 INNER JOIN CRM_Customers CC ( NOLOCK) ON CC. ID=CL.CNS_CustId 
				 INNER JOIN CRM.vwMMV VW (NOLOCK ) ON CBD.VersionId = VW.VersionId 
				 INNER JOIN CRM_LeadSource CLS ON CL.Id = CLS.LeadId
				 ------------------------------------------------
				 LEFT JOIN CRM_CarETDispositions CCED WITH(NOLOCK) ON CBD.LeadId = CCED.ItemId AND CCED.Type = 1
				 LEFT JOIN CRM_ETDispositions CETD WITH(NOLOCK) ON CETD.Id = CCED.DispositonId AND (@NotInterested IS NULL OR CETD.EventType =@NotInterested)-- value = 4 for not interested
				 --LEFT JOIN CRM_EventTypes ET WITH (NOLOCK) ON ET.Id = CETD.EventType 
				 ------------------------------------------------
				 LEFT JOIN LA_Agencies LA ON CLS.SourceId = LA.Id
			 --WHERE  CL.Owner <> -1 AND CL.LeadStageId=1 AND CL.CreatedOnDatePart BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate) 
			 WHERE  CL.Owner <> -1 AND CL.LeadStageId=@LeadStageId -- leadStage id = 1 for other value and 3 for NI
			  --AND (@NotInterested IS NULL OR ET.Id =@NotInterested)  -- value = 4 for not interested
			  AND (@ETSubDispositionId IS NULL OR CETD.Id = @ETSubDispositionId)--forNotInterested
			  AND CEL.EventOn BETWEEN CONVERT(DATE,@FromDate) AND CONVERT(DATE,@ToDate) 
		 ) 
		 
		SELECT CarName, CustName, Email, CBDId, LeadId, CustomerId,SubDispo FROM CTE WHERE RowNumber = 1 
		AND LeadId NOT IN(SELECT LeadId FROM CTE WHERE  RowNumber > 1)
		AND ( @Make IS NULL OR MakeId= @Make ) AND LeadHeadAgency = 1
END