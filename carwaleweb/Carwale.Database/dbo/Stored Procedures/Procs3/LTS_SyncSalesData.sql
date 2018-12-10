IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_SyncSalesData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_SyncSalesData]
GO

	/************************************************************************************************************************************
This Stored Procedure is being created on 09 FEB 2010
Procedure Created will be scheduled to Sync data for previous day sales done through Campaign!
************************************************************************************************************************************/
CREATE PROCEDURE [dbo].[LTS_SyncSalesData]

AS

DECLARE 
	@SyncDate AS DATETIME
BEGIN
	
	SET @SyncDate = CONVERT(VarChar, GETDATE() - 1, 101)
	--For the new cars
	--For the new cars booking data
	Insert Into LTS_SalesSync ( CampaignID, CampaignCode, SyncDate, Count, Type) 
		Select
			CL.SourceId AS CampaignId,
			CL.SourceName AS CampaignCode,
			@SyncDate,
			Count(CL.LeadId) AS Cnt,
			1
		From
			CRM_EventLogs AS EL,
			CRM_CarBasicData AS CBD,
			CRM_LeadSource AS CL
		Where
			EL.EventDatePart = @SyncDate AND
			EL.EventType = 16 AND
			CBD.ID = EL.ItemId AND
			CL.LeadId = CBD.LeadId AND
			CL.CategoryId = 2 
		Group By 
			CL.SourceId,
			CL.SourceName
			
	--For Used Cars
	Insert Into LTS_SalesSync ( CampaignID, CampaignCode, SyncDate, Count, Type) 
		Select
			ST.CampaignId,
			ST.CampaignCode,
			@SyncDate,
			Count(CP.ConsumerId) AS Cnt,
			2
		From
			ConsumerPackageRequests AS CP,
			LTS_SourceTracking AS ST
		Where
			DAY(CP.EntryDate) = DAY(@SyncDate) AND
			Month(CP.EntryDate) = Month(@SyncDate) AND
			Year(CP.EntryDate) = Year(@SyncDate) AND
			CP.PackageId = 1 AND
			CP.ActualAmount > 0 AND
			CP.ConsumerId IN (Select CustomerId From LTS_ConversionTracking)
			AND
			ST.ID = (Select Top 1 STDID From LTS_ConversionTracking Where CustomerId = CP.ConsumerId Order By Id ASC)
		Group By 
			ST.CampaignCode,
			ST.CampaignId
END
