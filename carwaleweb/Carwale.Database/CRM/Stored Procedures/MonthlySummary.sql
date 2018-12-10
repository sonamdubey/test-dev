IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[MonthlySummary]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[MonthlySummary]
GO

	-- Description	:	Get Report of Monthly on the basis of Model and Make
-- Author		:	Dilip V. 04-Mar-2012
-- Modifier		:	1. Dilip V. 19-Jun-2012 (Added PQ, TD and Booking)
--				:	2. Amit Kumar 14th feb 2013(replaced vwMMV by CRM.vwMMV)
--				:	3. Amit Kumar 7th feb 2013(added query for deleted leads)
--				:   4. Chetan Navin 27 May 2014 (Added parametes stateId and cityId)	
-- EXEC CRM.MonthlySummary 5,2014,9,Null,NULL,NULL,1,Null
CREATE PROCEDURE [CRM].[MonthlySummary]	
	@Month	SMALLINT,
	@Year	SMALLINT,
	@MakeId BIGINT,
	@ModelId VARCHAR(MAX) = NULL,
	@HeadAgency BIGINT,
	@Agency BIGINT,
	@StateId BIGINT,
	@CityId VARCHAR(MAX) = NULL
	
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE	@varsql				VARCHAR(MAX)
	
	BEGIN		
		
		--Lead Assigned
		SET @varsql =	'SELECT  COUNT(DISTINCT CL.ID) Cnt, CL.LeadStatusId, CII.ProductStatusId, 
						CII.ClosingProbability, Day(CL.CreatedOnDatePart) AS DayOfEvent
                        FROM CRM_CarDealerAssignment CDA WITH (NOLOCK)
                        INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CDA.CBDId = CBD.ID
                        INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
						INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
                        INNER JOIN CRM.vwMMV VW ON VW.VersionId = CBD.VersionId
						INNER JOIN dbo.vwCity VC ON VC.CityId = CC.CityId
                        INNER JOIN CRM_InterestedIn CII WITH(NOLOCK) ON CII.LeadId = CL.Id'
		
		IF(@HeadAgency != 0)
			BEGIN			
			SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
							INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
			END			
		SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
						AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year) + '
						AND CL.LeadStatusId = 2 AND CII.ProductTypeId = 1'
		
		SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)
		
		IF (@ModelId IS NOT NULL)
			SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
		ELSE
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

		IF (@CityId IS NOT NULL)
			SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
		ELSE IF(@StateId IS NOT NULL)
			SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)
		
		SET @varsql += ' GROUP BY CL.LeadStatusId,CII.ProductStatusId, CII.ClosingProbability, Day(CL.CreatedOnDatePart)'
		
		PRINT (@varsql);
		EXEC (@varsql);
		
		--Lead Count for All
		SET @varsql = ' SELECT COUNT(DISTINCT CL.id) AS Cnt, Day(CL.CreatedOnDatePart) AS DayOfEvent , CL.LeadStageId LeadStageId,
		CL.Owner, CL.LeadStatusId 
		FROM  CRM_Customers C WITH(NOLOCK)
		INNER JOIN CRM_Leads AS CL WITH(NOLOCK) ON CL.CNS_CustId = C.ID
		INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CL.Id = CBD.LeadId
		INNER JOIN CRM.vwMMV VW ON VW.VersionId = CBD.VersionId 
		INNER JOIN dbo.vwCity VC ON VC.CityId = C.CityId '
		
		IF(@HeadAgency != 0)
			BEGIN			
			SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
							INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
			END			
		SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
						AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year)
		
		 SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)
		 		
		IF (@ModelId IS NOT NULL)
			SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
		ELSE 
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

		IF (@CityId IS NOT NULL)
			SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
		ELSE IF(@StateId IS NOT NULL) 
			SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)
		
		SET @varsql += ' GROUP BY Day(CL.CreatedOnDatePart), CL.LeadStageId, CL.Owner, CL.LeadStatusId 
		ORDER BY DayOfEvent DESC'
		
		PRINT (@varsql);
		EXEC (@varsql);
		
		--Gets Count of PQ (Requested, Completed, Not Required)
		SET @varsql =  'SELECT COUNT(DISTINCT CCPL.ID) Cnt ,CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired, 
		Day(CL.CreatedOnDatePart) AS DayOfEvent
		FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID
		INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
		INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
		INNER JOIN CRM.vwMMV VW ON CBD.VersionId = VW.VersionId
		INNER JOIN dbo.vwCity VC ON VC.CityId = CC.CityId
		INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CBD.ID'
		IF(@HeadAgency != 0)
			BEGIN			
			SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
							INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
			END
		SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
						AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year)

		SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)

		IF (@ModelId IS NOT NULL)
			SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
		ELSE
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

		IF (@CityId IS NOT NULL)
			SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
		ELSE IF(@StateId IS NOT NULL) 
			SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)
	
		SET @varsql += ' GROUP BY CCPL.IsPQRequested,CCPL.IsPQCompleted,CCPL.IsPQNotRequired,Day(CL.CreatedOnDatePart)'
		PRINT (@varsql)
		EXEC(@varsql)
	
		--Gets Count of TD (Requested, Completed, Not Required)
		SET @varsql = 'SELECT COUNT(DISTINCT CCTL.ID) Cnt ,CCTL.IsTDCompleted, CCTL.ISTDNotPossible,
			CCTL.IsTDRequested, Day(CL.CreatedOnDatePart) AS DayOfEvent
			FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
			INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
			INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
			INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
			INNER JOIN CRM.vwMMV VW ON CBD.VersionId = VW.VersionId
			INNER JOIN dbo.vwCity VC ON VC.CityId = CC.CityId '
		IF(@HeadAgency != 0)
			BEGIN			
			SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
							INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
			END			
			SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
						AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year)
						
		SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)
		
		IF (@ModelId IS NOT NULL)
			SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
		ELSE
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)
			
		
		IF (@CityId IS NOT NULL)
			SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
		ELSE IF(@StateId IS NOT NULL) 
			SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)

			SET @varsql += ' GROUP BY CCTL.IsTDCompleted, CCTL.ISTDNotPossible,
			CCTL.IsTDRequested,Day(CL.CreatedOnDatePart)'
		PRINT (@varsql)
		EXEC(@varsql)
		
		--Gets Count of Booking (Requested, Completed, Not Possible)
		SET @varsql = 'SELECT COUNT(DISTINCT CCBL.ID) Cnt ,CCBL.IsBookingCompleted, CCBL.IsBookingNotPossible,
			CCBL.IsBookingRequested, Day(CL.CreatedOnDatePart) AS DayOfEvent
			FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
			INNER JOIN CRM_CarBookingLog CCBL WITH(NOLOCK) ON CCBL.CBDId = CDA.CBDId
			INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID 
			INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
			INNER JOIN CRM.vwMMV VW ON CBD.VersionId = VW.VersionId 
			INNER JOIN dbo.vwCity VC ON VC.CityId = CC.CityId'
		IF(@HeadAgency != 0)
			BEGIN			
			SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
							INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
			END			
			SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
						AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year)
						
		SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)
		
		IF (@ModelId IS NOT NULL)
			SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
		ELSE 
			SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)
			
		
		IF (@CityId IS NOT NULL)
			SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
		ELSE IF(@StateId IS NOT NULL) 
			SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)

			SET @varsql += ' GROUP BY CCBL.IsBookingCompleted, CCBL.IsBookingNotPossible,
			CCBL.IsBookingRequested,Day(CL.CreatedOnDatePart)'
		PRINT (@varsql)
		EXEC(@varsql)
		
		-- Get Count of deleted and assigned lead
		SET @varsql = 'SELECT DISTINCT CL.ID LeadId,COUNT(DISTINCT CBD.LeadId) Cnt, DAY(CL.CreatedOnDatePart) AS DayOfEvent,CL.LeadStatusId,  
						max(case CBD.IsDeleted  when 1 then 1 else 0 end) IsDeleted, CII.ProductStatusId,CII.ClosingProbability ,
						max(case CBD.IsDealerAssigned  when 1 then 1 else 0 end) IsDealerAssigned
						FROM CRM_CarBasicData CBD WITH (NOLOCK)
						INNER JOIN CRM_Leads CL WITH (NOLOCK) ON CL.ID = CBD.LeadId
						INNER JOIN CRM_Customers AS CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
						INNER JOIN CRM_InterestedIn CII WITH(NOLOCK) ON CII.LeadId = CL.Id AND CII.ProductTypeId = 1
						INNER JOIN CRM.vwMMV VW WITH (NOLOCK) ON CBD.VersionId = VW.VersionId 
						INNER JOIN dbo.vwCity VC ON VC.CityId = CC.CityId '
			IF(@HeadAgency != 0)
						BEGIN			
						SET @varsql +=	' INNER JOIN CRM_LeadSource AS CLS WITH(NOLOCK) ON CBD.LeadId = CLS.LeadId
										INNER JOIN LA_Agencies AS LA WITH(NOLOCK) ON LA.Id = CLS.SourceId'
						END			
						SET @varsql +=	' WHERE MONTH(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(2), @Month) + '
									AND YEAR(CL.CreatedOnDatePart) = ' + CONVERT(CHAR(4), @Year)
									
					SET @varsql += CRM.ConditionMonthlySummary(@HeadAgency,@Agency)
					
					IF (@ModelId IS NOT NULL)
						SET @varsql += ' AND VW.ModelId IN ('+@ModelId+')'
					ELSE 
						SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @MakeId, 101)

					IF (@CityId IS NOT NULL)
						SET @varsql += ' AND VC.CityId IN ('+@CityId+')'
					ELSE IF(@StateId IS NOT NULL) 
						SET @varsql += ' AND VC.StateId = ' + CONVERT(CHAR(10), @StateId, 101)

			SET @varsql+=' GROUP BY CBD.LeadId,CL.CreatedOnDatePart,CL.LeadStatusId,CII.ProductStatusId,CII.ClosingProbability,CL.ID'
		
		PRINT (@varsql)
		EXEC(@varsql)
	END
	
END














