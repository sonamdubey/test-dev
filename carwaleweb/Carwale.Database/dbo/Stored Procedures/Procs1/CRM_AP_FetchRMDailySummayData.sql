IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_AP_FetchRMDailySummayData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_AP_FetchRMDailySummayData]
GO

	
-- =============================================
-- Author:		Dilip Vasu
-- Modifier:	1.Deepak Tripathi(23rd of Aug 2011)  2.Amit Kumar (12th nov 2012) (Added Type = 6 for potentially lost leads)
-- Create date: 18th Aug 2011
-- Description:	This proc selects Count of Assigned, PQ (Requested and Completed),PQ Pending, TD (Requested and Completed), Booked
-- =============================================

 CREATE PROCEDURE [dbo].[CRM_AP_FetchRMDailySummayData]
(	
	@ManagerId VARCHAR(MAX),
	@Type SMALLINT	
)
AS
BEGIN
	DECLARE @varsql VARCHAR(MAX)
	DECLARE @MonthVal VARCHAR(10)
	DECLARE @YerVal VARCHAR(10)
	DECLARE @MonthValChanged VARCHAR(10)
	DECLARE @YerValChanged VARCHAR(10)
	DECLARE @IsDiffYear BIT
	
	DECLARE @LastMonth VARCHAR(2)
	DECLARE @LastYear VARCHAR(5)
	
	CREATE TABLE #tempDay( ItemId NUMERIC)
	CREATE TABLE #tempMonth( ItemId NUMERIC)
	CREATE TABLE #tempYear( ItemId NUMERIC)

	IF (YEAR(GETDATE()) = YEAR(DATEADD(mm, -1,GETDATE())) AND YEAR(GETDATE()) = YEAR(DATEADD(mm, -2,GETDATE())))
		BEGIN
			SET @IsDiffYear = 0
			SET @MonthVal = CAST(MONTH(GETDATE()) - 1 AS VARCHAR(2))  + ',' + CAST(MONTH(GETDATE()) - 2 AS VARCHAR(2))
			SET @YerVal = YEAR(GETDATE())
		END
	ELSE
		BEGIN
			SET @IsDiffYear = 1
			SET @MonthVal = MONTH(DATEADD(mm, -1,GETDATE())) 
			SET @YerVal = YEAR(DATEADD(mm, -1,GETDATE()))
			SET @MonthValChanged = MONTH(DATEADD(mm, -2,GETDATE()))
			SET @YerValChanged = YEAR(DATEADD(mm, -2,GETDATE()))
		END
		
	
	SET @LastMonth = MONTH(DATEADD(mm, -1,GETDATE()))
	SET @LastYear = YEAR(DATEADD(mm, -1,GETDATE()))
	
	PRINT @LastMonth
	PRINT @LastYear
	PRINT @IsDiffYear
	PRINT @MonthVal
	PRINT @YerVal
	PRINT @MonthValChanged
	PRINT @YerValChanged
	
	IF @Type = 1	--Assigned
		BEGIN
		
			SET @varsql =  'SELECT COUNT(DISTINCT CDA.Id) Cnt, RM.RMId, ''1'' AS Type FROM CRM_CarDealerAssignment AS CDA WITH(NOLOCK), 
							NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
							WHERE RM.DealerId = CDA.DealerId AND RM.RMId IN (' + @ManagerId + ') 
							AND RM.IsExecutive = 0 AND RM.DealerId = ND.DId
							AND DAY(CDA.CreatedOn) = DAY(GETDATE())
							AND MONTH(CDA.CreatedOn) = MONTH(GETDATE())
							AND YEAR(CDA.CreatedOn) = YEAR(GETDATE())
							GROUP BY RM.RMId
						   
							UNION ALL 
							   
							SELECT COUNT(DISTINCT CDA.Id) Cnt, RM.RMId, ''2'' AS Type FROM CRM_CarDealerAssignment AS CDA WITH(NOLOCK), 
							NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
							WHERE RM.DealerId = CDA.DealerId AND RM.RMId IN ('+@ManagerId+') 
							AND RM.IsExecutive = 0 AND RM.DealerId = ND.DId'
							
							IF (DAY(GETDATE()) > 1)				 
								SET @varsql = @varsql + ' AND DAY(CDA.CreatedOn) <> DAY(GETDATE())	AND MONTH(CDA.CreatedOn) = MONTH(GETDATE()) AND YEAR(CDA.CreatedOn) = YEAR(GETDATE())'
							ELSE
								SET @varsql = @varsql + ' AND MONTH(CDA.CreatedOn) = '+ @LastMonth  +' AND YEAR(CDA.CreatedOn) = '+ @LastYear  +''
								
							SET @varsql = @varsql + ' GROUP BY RM.RMId 
							
							UNION ALL 
							  
							SELECT COUNT(DISTINCT CDA.Id) Cnt, RM.RMId, ''3'' AS Type FROM CRM_CarDealerAssignment AS CDA WITH(NOLOCK), 
							NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
							WHERE RM.DealerId = CDA.DealerId AND RM.RMId IN ('+@ManagerId+') 
							AND RM.IsExecutive = 0 AND RM.DealerId = ND.DId'
						
							IF (@IsDiffYear = 0)				 
								SET @varsql = @varsql + ' AND MONTH(CDA.CreatedOn) IN ('+ @MonthVal +') AND YEAR(CDA.CreatedOn) = '+ @YerVal +''
							ELSE 	
								SET @varsql = @varsql + ' AND ((MONTH(CDA.CreatedOn) = '+ @MonthVal +' AND YEAR(CDA.CreatedOn) = '+ @YerVal +')
													OR (MONTH(CDA.CreatedOn) = '+ @MonthValChanged +' AND YEAR(CDA.CreatedOn) = '+ @YerValChanged +'))'
													
							SET @varsql = @varsql +     ' GROUP BY RM.RMId'	
				
			
		END
	ELSE IF @Type = 2    --PQ Pending
		BEGIN
	
		SET @varsql = ' SELECT COUNT(DISTINCT CCPL.id) Cnt, RM.RMId 
						FROM CRM_CarPQLog CCPL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
						WHERE CCPL.CBDId = CDA.CBDId AND CCPL.IsPQRequested = 1 AND RM.DealerId = ND.DId
						AND (CCPL.IsPQCompleted = 0 OR CCPL.IsPQCompleted IS NULL) 
						AND (CCPL.IsPQNotRequired = 0 OR CCPL.IsPQNotRequired IS NULL) 
						AND RM.DealerId = CDA.DealerId AND RM.IsExecutive = 0 
						AND RM.RMId IN ('+@ManagerId+')
						AND DATEDIFF(day, CCPL.PQRequestDate, GETDATE()) > 2 GROUP BY RM.RMId'
		END
	ELSE IF @Type = 3    --Others Like 36-PQCompleted, 44-PQRequested, 7-TDRequested, 14-TDCOmpleted, CarBooked-16
		BEGIN
		
		SET @varsql = 'SELECT COUNT(DISTINCT CCBL.ItemId) Cnt, CCBL.EventType, RM.RMId, ''1'' AS Type 
						FROM CRM_EventLogs CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
						WHERE CCBL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
						AND RM.RMId IN ('+@ManagerId+') AND CCBL.EventType IN(7,14,16,44,36) AND RM.IsExecutive = 0 
						AND DAY(CCBL.EventDatePart) = DAY(GETDATE())
						AND MONTH(CCBL.EventDatePart) = MONTH(GETDATE())
						AND YEAR(CCBL.EventDatePart) = YEAR(GETDATE())						
						GROUP BY RM.RMId, CCBL.EventType
						
						UNION ALL 
						
						SELECT COUNT(DISTINCT CCBL.ItemId) Cnt, CCBL.EventType, RM.RMId, ''2'' AS Type 
						FROM CRM_EventLogs CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
						WHERE CCBL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
						AND RM.RMId IN ('+@ManagerId+') AND CCBL.EventType IN(7,14,16,44,36) AND RM.IsExecutive = 0'
						
						IF (DAY(GETDATE()) > 1)				 
							SET @varsql = @varsql + ' AND DAY(CCBL.EventDatePart) <> DAY(GETDATE())	AND MONTH(CCBL.EventDatePart) = MONTH(GETDATE()) AND YEAR(CCBL.EventDatePart) = YEAR(GETDATE())'
						ELSE
							SET @varsql = @varsql + ' AND MONTH(CCBL.EventDatePart) = '+ @LastMonth  +' AND YEAR(CCBL.EventDatePart) = '+ @LastYear  +''
								
						SET @varsql = @varsql + ' GROUP BY RM.RMId, CCBL.EventType  
					   						
						UNION ALL 
						
						SELECT COUNT(DISTINCT CCBL.ItemId) Cnt, CCBL.EventType, RM.RMId, ''3'' AS Type 
						FROM CRM_EventLogs CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK), NCS_SubDealerOrganization AS ND WITH(NOLOCK)
						WHERE CCBL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
						AND RM.RMId IN ('+@ManagerId+') AND CCBL.EventType IN(7,14,16,44,36) AND RM.IsExecutive = 0'
					
						IF (@IsDiffYear = 0)				 
							SET @varsql = @varsql + ' AND MONTH(CCBL.EventDatePart) IN ('+ @MonthVal +') AND YEAR(CCBL.EventDatePart) = '+ @YerVal +''
						ELSE 	
							SET @varsql = @varsql + ' AND ((MONTH(CCBL.EventDatePart) = '+ @MonthVal +' AND YEAR(CCBL.EventDatePart) = '+ @YerVal +')
													OR (MONTH(CCBL.EventDatePart) = '+ @MonthValChanged +' AND YEAR(CCBL.EventDatePart) = '+ @YerValChanged +'))'
												
						SET @varsql = @varsql + ' GROUP BY RM.RMId, CCBL.EventType'
			
		END
	
	ELSE IF @Type = 4
		BEGIN
			SET @varsql = 'SELECT COUNT(CCTL.id) Cnt, NRD.RMId, ''1'' AS Type
						FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
						INNER JOIN NCS_RMDealers NRD WITH(NOLOCK) ON CDA.DealerId = NRD.DealerId
						INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
						INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CBD.ID
						WHERE (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL) 
						AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)
						AND NRD.RMId IN ('+@ManagerId+')
						AND CCTL.TDRequestDate > GETDATE()
						AND DAY(CCTL.CreatedOn) = DAY(GETDATE())
						AND MONTH(CCTL.CreatedOn) = MONTH(GETDATE())
						AND YEAR(CCTL.CreatedOn) = YEAR(GETDATE())
						GROUP BY NRD.RMId
						
						UNION ALL
						
						SELECT COUNT(CCTL.id) Cnt, NRD.RMId, ''2'' AS Type
						FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
						INNER JOIN NCS_RMDealers NRD WITH(NOLOCK) ON CDA.DealerId = NRD.DealerId
						INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
						INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CBD.ID
						WHERE (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL) 
						AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)
						AND NRD.RMId IN ('+@ManagerId+')
						AND CCTL.TDRequestDate > GETDATE()'
						
						IF (DAY(GETDATE()) > 1)				 
							SET @varsql = @varsql + ' AND DAY(CCTL.CreatedOn) <> DAY(GETDATE())	AND MONTH(CCTL.CreatedOn) = MONTH(GETDATE()) AND YEAR(CCTL.CreatedOn) = YEAR(GETDATE())'
						ELSE
							SET @varsql = @varsql + ' AND MONTH(CCTL.CreatedOn) = '+ @LastMonth  +' AND YEAR(CCTL.CreatedOn) = '+ @LastYear  +''
								
						SET @varsql = @varsql + ' GROUP BY NRD.RMId 
						
						UNION ALL
						
						SELECT COUNT(CCTL.id) Cnt, NRD.RMId, ''3'' AS Type
						FROM CRM_CarDealerAssignment CDA WITH(NOLOCK)
						INNER JOIN NCS_RMDealers NRD WITH(NOLOCK) ON CDA.DealerId = NRD.DealerId
						INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
						INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CBD.ID
						WHERE (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL) 
						AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)
						AND NRD.RMId IN ('+@ManagerId+')
						AND CCTL.TDRequestDate > GETDATE()'
						
						IF (@IsDiffYear = 0)				 
							SET @varsql = @varsql + ' AND MONTH(CCTL.CreatedOn) IN ('+ @MonthVal +') AND YEAR(CCTL.CreatedOn) = '+ @YerVal +''
						ELSE 	
							SET @varsql = @varsql + ' AND ((MONTH(CCTL.CreatedOn) = '+ @MonthVal +' AND YEAR(CCTL.CreatedOn) = '+ @YerVal +')
														OR (MONTH(CCTL.CreatedOn) = '+ @MonthValChanged +' AND YEAR(CCTL.CreatedOn) = '+ @YerValChanged +'))'
														
						SET @varsql = @varsql +     ' GROUP BY NRD.RMId'
		END
	ELSE IF @Type = 5
		BEGIN
			DECLARE @varsqlDay VARCHAR(MAX)
			SET @varsqlDay = ''
			DECLARE @varsqlMonth VARCHAR(MAX)
			SET @varsqlMonth = ''
			DECLARE @varsqlYear VARCHAR(MAX)
			SET @varsqlYear = ''
			
			
			SET @varsqlDay = 'INSERT INTO #tempDay
							SELECT CEL.ItemId
							FROM CRM_EventLogs CEL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK),
							NCS_SubDealerOrganization AS ND WITH(NOLOCK)
							WHERE CEL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
							AND RM.RMId IN ('+@ManagerId+') AND CEL.EventType IN(16) AND RM.IsExecutive = 0
							AND DAY(CEL.EventDatePart) = DAY(GETDATE())
							AND MONTH(CEL.EventDatePart) = MONTH(GETDATE())
							AND YEAR(CEL.EventDatePart) = YEAR(GETDATE())'
				
			EXEC (@varsqlDay)
			PRINT @varsqlDay
			
			SET @varsqlMonth = 'INSERT INTO #tempMonth
								SELECT CEL.ItemId
								FROM CRM_EventLogs CEL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK),
								NCS_SubDealerOrganization AS ND WITH(NOLOCK)
								WHERE CEL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
								AND RM.RMId IN ('+@ManagerId+') AND CEL.EventType IN(16) AND RM.IsExecutive = 0'
								
								IF (DAY(GETDATE()) > 1)				 
									SET @varsqlMonth = @varsqlMonth + ' AND DAY(CEL.EventDatePart) <> DAY(GETDATE()) AND MONTH(CEL.EventDatePart) = MONTH(GETDATE()) AND YEAR(CEL.EventDatePart) = YEAR(GETDATE())'
								ELSE
									SET @varsqlMonth = @varsqlMonth + ' AND MONTH(CEL.EventDatePart) = '+ @LastMonth  +' AND YEAR(CEL.EventDatePart) = '+ @LastYear  +''
							
			
			EXEC (@varsqlMonth)
			PRINT @varsqlMonth
			
			SET @varsqlYear = '	INSERT INTO #tempYear
								SELECT CEL.ItemId	FROM CRM_EventLogs CEL WITH(NOLOCK), CRM_CarDealerAssignment CDA WITH(NOLOCK), NCS_RMDealers AS RM WITH(NOLOCK),
								NCS_SubDealerOrganization AS ND WITH(NOLOCK) WHERE CEL.ItemId = CDA.CBDId AND RM.DealerId = CDA.DealerId AND RM.DealerId = ND.DId
								AND RM.RMId IN ('+@ManagerId+') AND CEL.EventType IN(16) AND RM.IsExecutive = 0'
							
								IF (@IsDiffYear = 0)				 
									SET @varsqlYear = @varsqlYear + ' AND MONTH(CEL.EventDatePart) IN ('+ @MonthVal +') AND YEAR(CEL.EventDatePart) = '+ @YerVal +''
								ELSE 	
									SET @varsqlYear = @varsqlYear + ' AND ((MONTH(CEL.EventDatePart) = '+ @MonthVal +' AND YEAR(CEL.EventDatePart) = '+ @YerVal +')
															OR (MONTH(CEL.EventDatePart) = '+ @MonthValChanged +' AND YEAR(CEL.EventDatePart) = '+ @YerValChanged +'))'
			
			EXEC (@varsqlYear)
			PRINT @varsqlYear				
			
			SET @varsql =  'SELECT COUNT(DISTINCT CCBL.CBDId) Cnt, RM1.RMId, ''1'' AS Type
							FROM CRM_CarBookingLog AS CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA1 WITH(NOLOCK), NCS_RMDealers AS RM1 WITH(NOLOCK)
							WHERE
							CCBL.CBDId = CDA1.CBDId AND CDA1.DealerId = RM1.DealerId
							AND CCBL.IsBookingNotPossible = 1
							AND RM1.RMId IN ('+@ManagerId+')
							AND CCBL.CBDId NOT In(SELECT CBDId FROM CRM_CarBookingLog WITH(NOLOCK) WHERE CBDId = CCBL.CBDId AND IsBookingCompleted = 1)
							AND CCBL.CBDId IN(SELECT ItemId FROM #tempDay)
							GROUP BY RM1.RMId
							
							UNION ALL
							
							SELECT COUNT(DISTINCT CCBL.CBDId) Cnt, RM1.RMId, ''2'' AS Type
							FROM CRM_CarBookingLog AS CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA1 WITH(NOLOCK), NCS_RMDealers AS RM1 WITH(NOLOCK)
							WHERE
							CCBL.CBDId = CDA1.CBDId AND CDA1.DealerId = RM1.DealerId
							AND CCBL.IsBookingNotPossible = 1
							AND RM1.RMId IN ('+@ManagerId+')
							AND CCBL.CBDId NOT In(SELECT CBDId FROM CRM_CarBookingLog WITH(NOLOCK) WHERE CBDId = CCBL.CBDId AND IsBookingCompleted = 1)
							AND CCBL.CBDId IN(SELECT ItemId FROM #tempMonth)
							GROUP BY RM1.RMId
							
							UNION ALL
							
							SELECT COUNT(DISTINCT CCBL.CBDId) Cnt, RM1.RMId, ''3'' AS Type
							FROM CRM_CarBookingLog AS CCBL WITH(NOLOCK), CRM_CarDealerAssignment CDA1 WITH(NOLOCK), NCS_RMDealers AS RM1 WITH(NOLOCK)
							WHERE
							CCBL.CBDId = CDA1.CBDId AND CDA1.DealerId = RM1.DealerId
							AND CCBL.IsBookingNotPossible = 1 AND RM1.RMId IN ('+@ManagerId+') AND CCBL.CBDId NOT IN (SELECT CBDId FROM CRM_CarBookingLog WITH(NOLOCK) WHERE CBDId = CCBL.CBDId AND IsBookingCompleted = 1)
							AND CCBL.CBDId IN(SELECT ItemId FROM #tempYear)
							GROUP BY RM1.RMId'
				
		END
	
	ELSE IF @Type = 6
		BEGIN
			SET @varsql = 'SELECT COUNT(PLC.Id) Cnt,RM.RMId ,''1'' Type FROM CRM_PotentiallyLostCase PLC 
							INNER JOIN NCS_RMDealers AS RM WITH(NOLOCK) ON RM.DealerId = PLC.DealerId
							INNER JOIN NCS_SubDealerOrganization AS ND WITH(NOLOCK) ON RM.DealerId = ND.DId
							WHERE DAY(PLC.TaggedOn) = DAY(GETDATE())
							AND MONTH(PLC.TaggedOn)= MONTH(GETDATE())
							AND YEAR(PLC.TaggedOn) = YEAR(GETDATE())
							AND RM.RMId IN ('+@ManagerId+') AND IsActionTaken = 0 
							GROUP BY RM.RMId
							
							UNION ALL
						
							SELECT COUNT(PLC.Id) Cnt,RM.RMId , ''2'' Type FROM CRM_PotentiallyLostCase PLC 
							INNER JOIN NCS_RMDealers AS RM WITH(NOLOCK) ON RM.DealerId = PLC.DealerId
							INNER JOIN NCS_SubDealerOrganization AS ND WITH(NOLOCK) ON RM.DealerId = ND.DId 
							WHERE  RM.RMId IN ('+@ManagerId+') AND IsActionTaken = 0 '
						IF (DAY(GETDATE()) > 1)				 
							SET @varsql = @varsql + ' AND DAY(PLC.TaggedOn) <> DAY(GETDATE())	AND MONTH(PLC.TaggedOn) = MONTH(GETDATE()) AND YEAR(PLC.TaggedOn) = YEAR(GETDATE())'
						ELSE
							SET @varsql = @varsql + ' AND MONTH(PLC.TaggedOn) = '+ @LastMonth  +' AND YEAR(PLC.TaggedOn) = '+ @LastYear  + ' '
							
							SET @varsql = @varsql + ' GROUP BY RM.RMId 
						
							UNION ALL
							
							SELECT COUNT(PLC.Id) Cnt,RM.RMId , ''3'' Type FROM CRM_PotentiallyLostCase PLC 
							INNER JOIN NCS_RMDealers AS RM WITH(NOLOCK) ON RM.DealerId = PLC.DealerId
							INNER JOIN NCS_SubDealerOrganization AS ND WITH(NOLOCK) ON RM.DealerId = ND.DId 
							WHERE  RM.RMId IN ('+@ManagerId+') AND IsActionTaken = 0 ' 
							IF (@IsDiffYear = 0)				 
								SET @varsql = @varsql + ' AND MONTH(PLC.TaggedOn) IN ('+ @MonthVal +') AND YEAR(PLC.TaggedOn) = '+ @YerVal +''
							ELSE 	
								SET @varsql = @varsql + ' AND ((MONTH(PLC.TaggedOn) = '+ @MonthVal +' AND YEAR(PLC.TaggedOn) = '+ @YerVal +')
															OR (MONTH(PLC.TaggedOn) = '+ @MonthValChanged +' AND YEAR(PLC.TaggedOn) = '+ @YerValChanged +')) '
							SET @varsql = @varsql + ' GROUP BY RM.RMId '
						
		END

	PRINT @varsql
	EXEC(@varsql)
	
		DROP TABLE #tempDay
		DROP TABLE #tempMonth
		DROP TABLE #tempYear

END
 
 









