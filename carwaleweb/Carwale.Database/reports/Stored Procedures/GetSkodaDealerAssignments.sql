IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetSkodaDealerAssignments]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetSkodaDealerAssignments]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Nov 07 2011
-- Description:	Get Skoda dealer assignment count
-- [reports].[GetSkodaDealerAssignments] 2011
-- =============================================
CREATE PROCEDURE [reports].[GetSkodaDealerAssignments]
@year smallint
AS
BEGIN


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT vw.Model,
		   MONTH(CL.CreatedOnDatePart) AS MonthVal,
		   --YEAR(CL.CreatedOnDatePart) AS YearVal,
		   COUNT(DISTINCT CSD.LeadId) AS TotalLeads
	FROM CRM_SkodaDealerAssignment CSD,     
		 CRM_InterestedIn AS CI,
		 NCS_Dealers AS ND,
		 CRM_Leads AS CL
		 LEFT JOIN CRM_LeadSource AS CLS ON CLS.LeadId = CL.Id,
		 CRM_CarBasicData as CBD,
		 vwMMV as vw
	WHERE CL.ID = CSD.LeadId
	  AND CSD.DealerId = ND.Id
	  AND CI.ProductTypeId = 1
	  AND CI.LeadId = CL.ID
	  AND CBD.LeadId=CL.Id
	  AND vw.VersionId=CBD.VersionId
	  AND CSD.PushStatus = 'SUCCESS'
	  AND ClosingProbability in (1,2,3)
	  AND YEAR(CL.CreatedOnDatePart) >= @year
	  AND vw.MakeId=15
	GROUP BY vw.Model,MONTH(CL.CreatedOnDatePart),
			 YEAR(CL.CreatedOnDatePart)
	ORDER BY vw.Model,MONTH(CL.CreatedOnDatePart),
			 YEAR(CL.CreatedOnDatePart)

END

