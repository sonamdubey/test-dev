IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReportDateBasedDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReportDateBasedDetails]
GO

	

--Name of SP/Function				: CarWale.CRM.WeeklyReportDateBasedDetails
--Applications using SP				: CRM
--Modules using the SP				: WeeklyReportDateBasedDetails.cs
--Technical department				: Database
--Summary							: PQ, TD, Booking ,VIN details
--Author							: Dilip V. 26-Jun-2012
--Modification history				: Amit Kumar 24th  july 2013 (Added TDDirect)
--									: Chetan Navin 25th June 2014 (Added dealer coordinator and reasonal manager) 

CREATE PROCEDURE [CRM].[WeeklyReportDateBasedDetails]
 @Type		SMALLINT,
 @Status	SMALLINT,
 @Eagerness	SMALLINT,
 @SiteId	SMALLINT,
 @DealerId	VARCHAR(MAX),	
 @FromDate	DATETIME,
 @ToDate	DATETIME,
 @Model		NUMERIC,
 @Make		NUMERIC=NULL
 

 AS

 BEGIN
	SET NOCOUNT ON
	
	DECLARE	@varsql				VARCHAR(MAX),
			@SingleQuotesTwice	VARCHAR(2) = '''''',
			@SingleQuotes		VARCHAR(1) = ''''
			
	
	IF @Type = 1	--Assigned
	BEGIN
		SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, CC.Email,CC.Mobile, 
		CC.Landline,  C.Name CityName, (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
		ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,
		CDA.Comments,CDA.LostDate, VW1.Make+ + '+@SingleQuotes+' '+@SingleQuotes+' +VW1.Model + + '+@SingleQuotes+' '+@SingleQuotes+' +VW1.Version LostName,CDA.ReasonLost,'+@SingleQuotesTwice+' Pending,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName'
		IF (@SiteId = 2)		--Dealer Panel
			SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
		SET @varsql += ' FROM Cities C WITH(NOLOCK) 
		INNER JOIN (vwMMV VW WITH(NOLOCK)
		INNER JOIN (NCS_Dealers ND WITH(NOLOCK)
		INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
		LEFT JOIN CRM_DALostBrand CDLB ON CDLB.CDAId = CDA.Id
		LEFT JOIN vwMMV VW1 ON VW1.VersionId = CDLB.VersionId
		INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID)  ON CBD.VersionId = VW.VersionId
		INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id 
		INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id) ON CC.CityId = C.Id
		LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
        LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
		LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
        LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id '
		IF(@Eagerness <> 0)
			SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId'
		
		IF (@SiteId = 2)		--Dealer Panel
			BEGIN
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
					LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
			END
		SET @varsql += ' WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
		AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
		
		IF(@Status = 1)--Lost
			SET @varsql += ' AND CDA.Status IN (41,60,61)'		
		ELSE IF(@Status = 2)--Not Interested
			SET @varsql += ' AND CDA.Status IN (40,42,50)'
		
		IF(@Eagerness <> 0)
			SET @varsql += CRM.GetEagerness(@Eagerness)
		
		IF(@Model = -1)
			BEGIN
				IF(@Make IS NOT NULL)
					SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
			END
		ELSE
			SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			
		IF(@Status = 2)--Not Interested
		BEGIN
			SET @varsql +=  ' UNION ALL  
				SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
				'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending, 
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName'
				IF (@SiteId = 2)		--Dealer Panel
				SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
				SET @varsql += ' FROM NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID
				INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
				INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
				INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
				INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
				LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID 
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
				LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id'
				IF(@Eagerness <> 0)
					SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId'
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
				LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId
				WHERE CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
				AND (CCBD.BookingStatusId <> 16 OR CCBD.BookingStatusId IS NULL) 				 
				AND LeadStageId = 3 AND Status = -1
				AND CDA.DealerId IN ('+@DealerId+')'
				IF(@Model = -1)
					BEGIN
						IF(@Make IS NOT NULL)
							SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
					END
				ELSE
					SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
					
				IF(@Eagerness <> 0)
					SET @varsql += CRM.GetEagerness(@Eagerness)
				
			END
			SET @varsql += ' ORDER BY CL.CreatedOn'
					
	END
	ELSE IF @Type = 2	--Active
	BEGIN
		SET @varsql =  'SELECT DISTINCT CDA.Id DAId, CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
				'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending, 
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName'
				IF (@SiteId = 2)		--Dealer Panel
					SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
			SET @varsql += ' FROM NCS_Dealers ND WITH(NOLOCK)
				INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
				INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
				INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id 
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
				LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH(NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH(NOLOCK) ON NRD.RMId = NRM.Id '
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
				AND CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
				AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model = -1)
				BEGIN
					IF(@Make IS NOT NULL)
						SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
				END
			ELSE
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF(@Eagerness <> 0)
				SET @varsql += CRM.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CL.CreatedOn'
		
	END
	
	IF @Type = 3	--PQ
	BEGIN
		SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
			CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
			(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
			ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CCPL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
			'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,
			ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName'
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
			INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCPL.CBDId) ON VW.VersionId = CBD.VersionId
			LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
			LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
			LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
			LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id '
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
			WHERE CCPL.PQRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
			AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model = -1)
				BEGIN
					IF(@Make IS NOT NULL)
						SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
				END
			ELSE
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
				SET @varsql += CRM.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CCPL.CreatedOn'		
	END
	IF @Type = 4	--TD
		BEGIN		
			SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
				CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
				(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
				ND.ID Did,CL.Id AS LeadId,ND.Name Dealer,CCTL.CreatedOn,
				'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,
				ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName' 
				IF (@Status = 0)		--TD Requested
					BEGIN
						SET @varsql += ', CASE WHEN (CCTL.IsTDCompleted = 1 OR CCTL.ISTDNotPossible = 1 OR CCTL.IsTDDirect=1) THEN 0 
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
				INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCTL.CBDId) ON VW.VersionId = CBD.VersionId '
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId'					
				
			SET @varsql += ' INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
				INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
				INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id				
				LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCTL.TDNPDispositionId = CED.Id AND CED.IsActive = 1 
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
				LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id '
				IF (@SiteId = 2)		--Dealer Panel
				BEGIN
					SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
						LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
				END
				SET @varsql += ' WHERE CCTL.TDRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
				AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
				AND CDA.DealerId IN ('+@DealerId+')'
				IF (@Status <> 4)--TD Direct
					SET @varsql += ' AND CCTL.IsTDRequested = 1'
				
				IF(@Model = -1)
					BEGIN
						IF(@Make IS NOT NULL)
							SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
					END
				ELSE
					SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
				IF (@Status = 1)--TD Completed
					SET @varsql += ' AND CCTL.IsTDCompleted = 1'
				ELSE IF (@Status = 2)--TD NotPossible
					SET @varsql += ' AND CCTL.ISTDNotPossible = 1'
				ELSE IF (@Status = 4)--TD Direct
					SET @varsql += ' AND CCTL.IsTDDirect = 1'
				
				ELSE IF (@Status = 3)--TD Pending
					BEGIN
					SET @varsql += ' AND (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL)
						AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)'						
					END
				IF(@Eagerness <> 0)
					SET @varsql += CRM.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CCTL.CreatedOn'		
		END	
	IF @Type = 5	--Booked
		BEGIN
			SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
			CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
			(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
			ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CCBL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
			'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,
			ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions,OU.UserName AS DCName,NRM.Name AS RMName'
			IF (@Status = 0)		--Booking Requested
				BEGIN
					SET @varsql += ', CASE WHEN (CCBL.IsBookingCompleted = 1 OR CCBL.IsBookingNotPossible = 1) THEN 0
						ELSE DATEDIFF(day, CCBL.BookingRequestDate, GETDATE()) END Pending'
				END                
            ELSE IF (@Status = 3)	--Booking Pending
                SET @varsql += ', DATEDIFF(day, CCBL.BookingRequestDate, GETDATE()) Pending'
            ELSE					--Booking (NotPossible, Completed)
				SET @varsql += ', '+@SingleQuotesTwice+' Pending'
			IF (@SiteId = 2)		--Dealer Panel
					SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
		SET @varsql += ' FROM Cities C WITH(NOLOCK)
			INNER JOIN (vwMMV VW INNER JOIN	(NCS_Dealers ND WITH(NOLOCK)
			INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
			INNER JOIN CRM_CarBookingLog CCBL WITH(NOLOCK) ON CCBL.CBDId = CDA.CBDId
			INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCBL.CBDId) ON VW.VersionId = CBD.VersionId
			LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
			LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
			LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
			LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id '
			IF(@Eagerness <> 0)
				SET @varsql += ' INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId'
		SET @varsql += ' INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId) ON CC.CityId = C.Id'
			IF (@SiteId = 2)		--Dealer Panel
			BEGIN
				SET @varsql += ' LEFT OUTER JOIN CRM_LeadSource CLS WITH (NOLOCK) ON CL.Id = CLS.LeadId
					LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CLS.SourceId'
			END
			SET @varsql += ' LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCBL.BookingNPDispositionId = CED.Id AND CED.IsActive = 1
			WHERE CCBL.BookingRequestDate BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
			AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model = -1)
				BEGIN
					IF(@Make IS NOT NULL)
						SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
				END
			ELSE
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF (@Status = 1)--Booking Completed
                SET @varsql += ' AND CCBL.IsBookingCompleted = 1'
            ELSE IF (@Status = 2)--Booking NotRequired
                SET @varsql += ' AND CCBL.IsBookingNotPossible = 1'
            ELSE IF (@Status = 3)--Booking Pending
				BEGIN
					SET @varsql += ' AND (CCBL.IsBookingCompleted = 0 OR CCBL.IsBookingCompleted IS NULL)
						AND (CCBL.IsBookingNotPossible = 0 OR CCBL.IsBookingNotPossible IS NULL)'
				END
			IF(@Eagerness <> 0)
				SET @varsql += CRM.GetEagerness(@Eagerness)
			SET @varsql += ' ORDER BY CCBL.CreatedOn'	
		END	
	IF @Type = 6	--VIN
		BEGIN
			SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
			CC.Email, CC.Mobile, CC.Landline, C.Name CityName, 
			(VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
			ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
			'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
			CL.CreatedOn, '+@SingleQuotesTwice+' AS Dispositions, DATEDIFF(day, CCBD.BookingDate, GETDATE()) Pending,OU.UserName AS DCName,NRM.Name AS RMName'
			IF (@SiteId = 2)		--Dealer Panel
				SET @varsql += ', CASE LA.Id WHEN 5 THEN '+@SingleQuotes+'Skoda-Portal'+@SingleQuotes+' WHEN 9 THEN '+@SingleQuotes+'Mahindra-Portal'+@SingleQuotes+' ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName'
			SET @varsql += ' FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
			INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON CDA.DealerId = ND.Id
			INNER JOIN CRM_CarBookingData AS CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
			INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
			INNER JOIN vwMMV AS VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
			INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
			INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId
			INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
			LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
			LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
			LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
			LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id '
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
			AND CDA.CreatedOn BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(19), @FromDate, 120) +@SingleQuotes+'
			AND ' +@SingleQuotes+ CONVERT(CHAR(19), @ToDate, 120) +@SingleQuotes+ '
			AND CDA.DealerId IN ('+@DealerId+')'
			IF(@Model = -1)
				BEGIN
					IF(@Make IS NOT NULL)
						SET @varsql += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
				END
			ELSE
				SET @varsql += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
			IF(@Eagerness <> 0)
				SET @varsql += CRM.GetEagerness(@Eagerness)
				SET @varsql += ' ORDER BY CL.CreatedOn'
		END
	PRINT (@varsql)
	EXEC (@varsql)
	
 END
	
	







