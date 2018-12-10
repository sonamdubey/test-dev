IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[CRM_WeeklyReportDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[CRM_WeeklyReportDetails]
GO

	


--Name of SP/Function				: CarWale.dbo.CRM_WeeklyReport
--Applications using SP				: CRM, Dealer and RM Panel 
--Modules using the SP				: WeeklyReportLead.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 02-Feb-2012
--Modification history				: 1. 

CREATE PROCEDURE [CRM].[CRM_WeeklyReportDetails]
 @Type		SMALLINT,
 @Status	SMALLINT,
 @Eagerness	SMALLINT,
 @SiteId	SMALLINT,
 @DealerId	VARCHAR(MAX),	
 @FromDate	DATETIME,
 @ToDate	DATETIME,
 @Model		NUMERIC
 

 AS

 BEGIN
	SET NOCOUNT ON
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''
	--PRINT (@SingleQuotesTwice)
	--PRINT (@SingleQuotes)
	
	IF @Type = 1	--Assigned
	BEGIN
		SET @varsql = 'SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, CC.Email,CC.Mobile, 
		CC.Landline,  C.Name CityName, (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
		ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,
		CDA.Comments,CDA.LostDate, CDA.LostName,CDA.ReasonLost,'+@SingleQuotesTwice+' Pending,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions'
		IF (@SiteId = 2)		--Dealer Panel
			SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
		SET @varsql += ' FROM Cities C WITH(NOLOCK) 
		INNER JOIN(vwMMV VW WITH(NOLOCK)
		INNER JOIN (NCS_Dealers ND WITH(NOLOCK)
		INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID)  ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id 
		INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id) ON CC.CityId = C.Id'
		IF(@Eagerness <> 0)
			SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId'
		
		IF (@SiteId = 2)		--Dealer Panel
			BEGIN
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
					LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
			END
		SET @varsql += ' WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+' 
			AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
		
		IF(@Status = 1)--Lost
			SET @varsql += ' AND CDA.Status IN (41,60,61)'		
		ELSE IF(@Status = 2)--Not Interested
			SET @varsql += ' AND CDA.Status IN (40,42,50)'
		
		IF(@Eagerness <> 0)
			SET @varsql += dbo.GetEagerness(@Eagerness)
		
		IF(@Model <> -1)
			SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			
		IF(@Status = 2)--Not Interested
		BEGIN
			SET @varsql +=  ' UNION ALL  
				SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
				'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending, 
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions'
				IF (@SiteId = 2)		--Dealer Panel
				SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
				SET @varsql += ' FROM NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID
				INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
				INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
				INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
				INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
				LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID'
				IF(@Eagerness <> 0)
					SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId'
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
				LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId
				WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
				AND (CCBD.BookingStatusId <> 16 OR CCBD.BookingStatusId IS NULL) 				 
				AND LeadStageId = 3 AND Status = -1
				AND CDA.DealerId IN ('+@DealerId+')'
				IF(@Model <> -1)
					SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
					
				IF(@Eagerness <> 0)
					SET @varsql += dbo.GetEagerness(@Eagerness)
				
			END
			SET @varsql += ' ORDER BY CreatedOn'
					
	END
	ELSE IF @Type = 2	--Active
	BEGIN
		SET @varsql =  'SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
				'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending, 
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions'
				IF (@SiteId = 2)		--Dealer Panel
					SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
			SET @varsql += ' FROM NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
				INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
				INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id'
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId'
			SET @varsql += ' INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
				LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID'
				IF (@SiteId = 2)		--Dealer Panel
				BEGIN
					SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
						LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
				END
			SET @varsql += ' INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id 
				WHERE CL.LeadStageId <> 3 AND CDA.Status = -1 
				AND (CCBD.BookingStatusId <> 16 OR CCBD.BookingStatusId IS NULL) 
				AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
				AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model <> -1)
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF(@Eagerness <> 0)
				SET @varsql += dbo.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CreatedOn'
		
	END
	
	IF @Type = 3	--PQ
	BEGIN
		SET @varsql = 'SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
			CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
			(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
			ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
			'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,
			ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions'
			IF (@Status = 0)		--PQ Requested
				BEGIN
					SET @varsql += ', CASE WHEN (CCPL.IsPQCompleted = 1 OR CCPL.IsPQNotRequired = 1) THEN 0
						ELSE DATEDIFF(day, CCPL.PQRequestDate, GETDATE()) END Pending'
				END                
            ELSE IF (@Status = 3)	--PQ Pending
                SET @varsql += ', DATEDIFF(day, CCPL.PQRequestDate, GETDATE()) Pending'
            ELSE					--PQ (NotPossible, Completed)
				SET @varsql += ', '+@SingleQuotesTwice+' Pending'
			IF (@SiteId = 2)		--Dealer Panel
					SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
		SET @varsql += ' FROM Cities C WITH(NOLOCK)
			INNER JOIN (vwMMV VW INNER JOIN	(NCS_Dealers ND WITH(NOLOCK)
			INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
			INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CDA.CBDId
			INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCPL.CBDId) ON VW.VersionId = CBD.VersionId'
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId'
		SET @varsql += ' INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId) ON CC.CityId = C.Id'
			IF (@SiteId = 2)		--Dealer Panel
			BEGIN
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
					LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
			END
			SET @varsql += ' LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCPL.PQNRDispositionId = CED.Id AND CED.IsActive = 1
			WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
			AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model <> -1)
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF (@Status = 1)--PQ Completed
                SET @varsql += ' AND CCPL.IsPQCompleted = 1'
            ELSE IF (@Status = 2)--PQ NotRequired
                SET @varsql += ' AND CCPL.IsPQNotRequired = 1'
            ELSE IF (@Status = 3)--PQ Pending
				BEGIN
					SET @varsql += ' AND (CCPL.IsPQCompleted = 0 OR CCPL.IsPQCompleted IS NULL)
						AND (CCPL.IsPQNotRequired = 0 OR CCPL.IsPQNotRequired IS NULL)'
				END
			IF(@Eagerness <> 0)
				SET @varsql += dbo.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CreatedOn'		
	END
	IF @Type = 4	--TD
		BEGIN		
			SET @varsql = 'SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer,CL.CreatedOn,
				'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions' 
				IF (@Status = 0)		--TD Requested
					BEGIN
						SET @varsql += ', CASE WHEN (CCTL.IsTDCompleted = 1 OR CCTL.ISTDNotPossible = 1) THEN 0 
						ELSE DATEDIFF(day, CCTL.TDRequestDate, GETDATE()) END Pending'
					END                
				ELSE IF (@Status = 3)	--TD Pending
					SET @varsql += ', DATEDIFF(day, CCTL.TDRequestDate, GETDATE()) Pending'
				ELSE					--TD (NotPossible, Completed)
					SET @varsql += ', '+@SingleQuotesTwice+' Pending'
				IF (@SiteId = 2)		--Dealer Panel
					SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
				SET @varsql += ' FROM vwMMV VW
				INNER JOIN (NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
				INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCTL.CBDId) ON VW.VersionId = CBD.VersionId'
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId'					
				
			SET @varsql += ' INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
				INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
				INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id				
				LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCTL.TDNPDispositionId = CED.Id AND CED.IsActive = 1'
				IF (@SiteId = 2)		--Dealer Panel
				BEGIN
					SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
						LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
				END
				SET @varsql += ' WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
				AND CDA.DealerId IN ('+@DealerId+')'
				IF(@Model <> -1)
					SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
				IF (@Status = 1)--TD Completed
					SET @varsql += ' AND CCTL.IsTDCompleted = 1'
				ELSE IF (@Status = 2)--TD NotPossible
					SET @varsql += ' AND CCTL.ISTDNotPossible = 1'
				ELSE IF (@Status = 3)--TD Pending
					BEGIN
					SET @varsql += ' AND (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL)
						AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)
						AND CCTL.TDRequestDate <= ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1)+ @SingleQuotes
					END
				ELSE IF (@Status = 4)--TD Future
				BEGIN
				SET @varsql += ' AND (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL)
					AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)
					AND CCTL.TDRequestDate > ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1)+ @SingleQuotes
				END
				IF(@Eagerness <> 0)
					SET @varsql += dbo.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CDA.CreatedOn'		
		END	
	IF @Type = 5	--Booked
		BEGIN
			SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName,
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId, ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
				'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
				CL.CreatedOn,'+@SingleQuotesTwice+' AS Dispositions,'+@SingleQuotesTwice+' Pending,CBD.Id'
			IF (@SiteId = 2)		--Dealer Panel
				SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
				SET @varsql += ' FROM NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
				INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CDA.CBDId
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
				INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId'
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId'
				SET @varsql += ' INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
				INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID
				INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id'
			IF (@SiteId = 2)		--Dealer Panel
				BEGIN
					SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
						LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
				END
				SET @varsql += ' WHERE CCBD.BookingStatusId = 16 
				AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
				AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model <> -1)
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF(@Eagerness <> 0)
				SET @varsql += dbo.GetEagerness(@Eagerness)
				SET @varsql += ' ORDER BY CreatedOn'
		END	
	IF @Type = 6	--VIN
		BEGIN
			SET @varsql = 'SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
			CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
			(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
			ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
			'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
			CL.CreatedOn, '+@SingleQuotesTwice+' AS Dispositions, DATEDIFF(day, CCBD.BookingDate, GETDATE()) Pending'
			IF (@SiteId = 2)		--Dealer Panel
				SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
			SET @varsql += ' FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
			INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON CDA.DealerId = ND.Id
			INNER JOIN CRM_CarBookingData AS CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
			INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
			INNER JOIN vwMMV AS VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
			INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId
			INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id'
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId'
			SET @varsql += ' LEFT JOIN CRM_CarDeliveryData AS CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId'
			IF (@SiteId = 2)		--Dealer Panel
			BEGIN
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
					LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
			END
			SET @varsql += ' WHERE CDA.Status NOT IN (40,41,42,50,60,61) 
			AND CDA.CBDId NOT IN 
			(SELECT CBDId FROM CRM_CarInvoices WITH(NOLOCK) 
			WHERE InvoiceId IN 
			(SELECT id FROM CRM_ADM_Invoices WITH(NOLOCK) WHERE MakeId = 15)) 
			AND ( BookingStatusId = 16 OR DeliveryStatusId = 20) 
			AND IsNull(CCDD.ChasisNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
			AND IsNull(CCDD.EngineNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
			AND IsNull(CCDD.RegistrationNumber,'+@SingleQuotesTwice+') = '+@SingleQuotesTwice+' 
			AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(8), @FromDate, 1) +@SingleQuotes+'
			AND ' +@SingleQuotes+ CONVERT(CHAR(8), @ToDate, 1) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model <> -1)
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF(@Eagerness <> 0)
				SET @varsql += dbo.GetEagerness(@Eagerness)
				SET @varsql += ' ORDER BY CreatedOn'
		END
	EXEC (@varsql)
	PRINT (@varsql)
 END
	
	


