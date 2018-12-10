IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_RManagersSMS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_RManagersSMS]
GO

	





-- Created By : Jayant Mhatre
-- Create date: 26-07-2011
-- Modified By: Dilip V. 15-Feb-2011 (MTD) 
-- Description:	This procedure is used in Automatic Process CRMRManagersSMS Page 
--			  : to send SMS to R-Manager about count of Leads(Assigned, PQ, TD, Booked)

CREATE PROCEDURE [dbo].[CRM_RManagersSMS]
	
AS
			
	BEGIN
	SET NOCOUNT ON

	DECLARE @RowsToProcess  INT,
			@CurrentRow     INT,
			@RMId		    INT,
			@Month			INT,
			@Today			INT
	DECLARE @tempNodeCode	TABLE ( MonthCnt INT,TodayCnt INT,EventType INT,NodeCode VARCHAR(100))
	DECLARE @tempEvent	TABLE (MonthCnt NUMERIC,TodayCnt NUMERIC,EventType INT)
	CREATE TABLE #tempRMId (RowID		INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
							RMId		NUMERIC,
							Mobile		VARCHAR(50),
							Name	   	VARCHAR(100),
							AssignMonth	NUMERIC,
							AssignToday	NUMERIC,
							PQMonth		NUMERIC,
							PQToday		NUMERIC,
							TDMonth		NUMERIC,
							TDToday		NUMERIC,
							BkMonth		NUMERIC,
							BkToday		NUMERIC)
	
	
	INSERT INTO #tempRMId (RMId,Mobile,Name)
	SELECT  M.Id,M.MobileNo,M.Name FROM NCS_RManagers AS M WITH(NOLOCK) WHERE M.Type=0 AND M.IsActive=1 AND MakeGroupId IN(1,12)  --6 For TataSelect RMID and Mobile no
	SET @RowsToProcess = @@ROWCOUNT--Get Count of Total RM
	
	--Select Count with respective Event Type and Node Code
	INSERT INTO @tempNodeCode
	SELECT COUNT(DISTINCT ItemId),SUM (CASE WHEN DAY(EventOn) = DAY(GETDATE()) THEN 1 ELSE 0 END), EventType,NodeCode
	FROM(
	SELECT CEL.ItemId,CEL.EventOn,CEL.EventType,NRM.NodeCode,
       ROW_NUMBER() OVER(PARTITION BY CEL.ItemId, CEL.EventType ORDER BY CEL.EventOn DESC ) AS rownum 
	FROM CRM_EventLogs CEL WITH (NOLOCK)
	INNER JOIN (
				(NCS_DealerOrganization NDO WITH(NOLOCK)
						INNER JOIN (
									NCS_RManagers AS NRM WITH(NOLOCK) 
									INNER JOIN NCS_RMDealers AS NRD WITH(NOLOCK) ON NRD.RMId = NRM.Id
									INNER JOIN NCS_SubDealerOrganization SDO WITH(NOLOCK) ON SDO.DId = NRD.DealerId
									) ON NDO.ID = SDO.OId
				)
				INNER JOIN CRM_CarDealerAssignment AS CDA WITH(NOLOCK) ON CDA.DealerId = SDO.DId
				)
				ON CEL.ItemId = CDA.CBDId
	INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CEL.ItemId = CBD.ID
	WHERE NRM.Type = 0				  
	AND MONTH(CEL.EventDatePart) = MONTH(GETDATE()) 
	AND YEAR (CEL.EventDatePart) = YEAR (GETDATE())
	AND CEL.EventType IN (39,44,7,16)
	AND NDO.IsCWExecutive = 0 AND NDO.IsActive=1 AND NRM.IsActive = 1 
	AND NRD.Type = 0
	)a
	WHERE rownum = 1
	GROUP BY EventType,NodeCode
		
		--Get Count of Corresponding RM using NodeCode
		SET @CurrentRow = 0
		WHILE @CurrentRow < @RowsToProcess
		BEGIN
			SET @CurrentRow = @CurrentRow + 1
			SELECT @RMId = RMId FROM #tempRMId WHERE RowID = @CurrentRow
				--Insert Monthly and Todays Count with EventType
				INSERT INTO @tempEvent
				SELECT SUM(MonthCnt),SUM(TodayCnt),EventType 
				FROM @tempNodeCode 
				WHERE NodeCode LIKE (SELECT REPLACE (NodeCode, '_','[_]') FROM NCS_RManagers WITH(NOLOCK) WHERE Id = @RMId)+'%'
				GROUP BY EventType
				
			SET @Month = 0; SET @Today = 0;
			SELECT @Month = MonthCnt,@Today = TodayCnt FROM @tempEvent WHERE EventType = 39	--EventType of Assigned Lead
			UPDATE #tempRMId SET #tempRMId.AssignMonth = @Month,#tempRMId.AssignToday = @Today WHERE RMId = @RMId
			SET @Month = 0; SET @Today = 0;
			SELECT @Month = MonthCnt,@Today = TodayCnt FROM @tempEvent WHERE EventType = 44	--PQ
			UPDATE #tempRMId SET #tempRMId.PQMonth = @Month,#tempRMId.PQToday = @Today WHERE RMId = @RMId
			SET @Month = 0; SET @Today = 0;
			SELECT @Month = MonthCnt,@Today = TodayCnt FROM @tempEvent WHERE EventType = 7	--TD
			UPDATE #tempRMId SET #tempRMId.TDMonth = @Month,#tempRMId.TDToday = @Today WHERE RMId = @RMId
			SET @Month = 0; SET @Today = 0;
			SELECT @Month = MonthCnt,@Today = TodayCnt FROM @tempEvent WHERE EventType = 16	--Booked
			UPDATE #tempRMId SET #tempRMId.BkMonth = @Month,#tempRMId.BkToday = @Today WHERE RMId = @RMId
			DELETE @tempEvent
			
		END
		SELECT RMId,Mobile,Name,AssignMonth,AssignToday,PQMonth,PQToday,TDMonth,TDToday,BkMonth,BkToday FROM #tempRMId
DROP TABLE #tempRMId
		
END



	







