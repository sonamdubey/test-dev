IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[NewPQAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[NewPQAlert]
GO

	
-- Description	:	Get Report of (Count of Distinct LeadId and PQ Alert ID for Make on that particular Month and Year passed
-- Author		:	Dilip V. 23-Feb-2012
-- Modifier		:	1.Vaibhav K. 06-Mar-2012 (Get the content from table CRM_PQAlerts)
--				:	2.Dilip V. 28-Mar-2012 (Get Count of Dealer who assigned CBDID Optimized query)
--				:	3.Amit Kumar 31st May 2012(removed CPA.ID and CDA.Id and put CBDID in place of that and added new field IsNotInterested)

CREATE PROCEDURE [CRM].[NewPQAlert]	
	@Month	SMALLINT = 0,
	@Year	SMALLINT = 0
	
AS
BEGIN
	SET NOCOUNT ON;
	
	BEGIN		
		
		SELECT  COUNT(DISTINCT CPA.LeadId) LeadCnt,COUNT(DISTINCT CPA.CBDId) Cnt,COUNT(DISTINCT CDA.CBDId) DealerCnt,VW.MakeId,VW.Make,DAY(CPA.AlertDate) Dt,CBD.IsNotInterested IsNotInterested
		FROM CRM_PQAlerts CPA WITH (NOLOCK)
		LEFT JOIN CRM_CarDealerAssignment CDA WITH (NOLOCK) ON CDA.CBDId = CPA.CBDId
		LEFT JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.Id = CPA.CBDId
		INNER JOIN vwMMV VW WITH (NOLOCK) ON VW.VersionId = CPA.VersionId
		WHERE MONTH(AlertDate) = @Month
		AND YEAR(AlertDate) = @Year
		GROUP BY VW.MakeId,VW.Make,DAY(CPA.AlertDate),CBD.IsNotInterested
		
	END
	
END
