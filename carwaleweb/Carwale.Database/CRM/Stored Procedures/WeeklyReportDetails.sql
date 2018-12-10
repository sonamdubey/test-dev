IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[WeeklyReportDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[WeeklyReportDetails]
GO

	


--Name of SP/Function                 : CarWale.dbo.WeeklyReportDetails
--Applications using SP               : CRM, Dealer and RM Panel 
--Modules using the SP                : WeeklyReportLead.cs
--Technical department                : Database
--Summary                             : PQ, TD, Booking ,VIN details
--Author							  : Dilip V. 02-Feb-2012
--Modification history                : 1. Dilip V. 18-Apr-2012 (In TD (Pending & Future), TD Direct should not come, so checked TD Direct status)
--                                    : 2. Dilip V. 19-Apr-2012 (Removed field CCTL.IsTDDirect instead check IsTDRequested)
--                                    : 3. Dilip V. 24-Apr-2012 (In Active Case replaced CDA.Id Alias name DAId)
--                                    : 4. Dilip V. 02-May-2012 (If MakeId is passed check)
--									  : 5. Amit Kumar 23rd july 2013(put ISTDDirect = 1)
--									  : 6. Amit Kumar 19th dec 2013(added lostmake ,lostmodel,lostversion)
--									  : 7. Chetan Navin 25th June 2014 (Added dealer coordinator and reasonal manager) 
--									  : 8. Chetan Navin 23 July 2014 (Added Sales Executive)

CREATE PROCEDURE [CRM].[WeeklyReportDetails]
 @Type        SMALLINT,
 @Status      SMALLINT,
 @Eagerness   SMALLINT,
 @SiteId      SMALLINT,
 @DealerId    VARCHAR(MAX),    
 @FromDate    DATETIME,
 @ToDate      DATETIME,
 @Model       NUMERIC,
 @Make        NUMERIC=NULL

 AS

 BEGIN
    SET NOCOUNT ON
    DECLARE	@varsql					VARCHAR(MAX) = '',
			@varsqlForUnion			VARCHAR(MAX) = '',
            @SingleQuotesTwice		VARCHAR(2) = '''''',
            @SingleQuotes			VARCHAR(1) = ''''
    IF @Type = 1    --Assigned
    BEGIN
        SET @varsql = 'SELECT DISTINCT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName,
        CC.Email,CC.Mobile, CC.Landline, C.Name CityName, CDA.CreatedOn AS DealerAssigned, 
        (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
        ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn, CDA.Comments,CDA.LostDate,
        VW1.Make+ + '+@SingleQuotes+' '+@SingleQuotes+' +VW1.Model + + '+@SingleQuotes+' '+@SingleQuotes+' +VW1.Version LostName,VW1.Make LostMake,VW1.Model LostModel,VW1.Version LostVersion,
        CDA.ReasonLost,'+@SingleQuotesTwice+' Pending,
        ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,
        CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name AS CarCity ,        
        '+@SingleQuotesTwice+' PQRequestDate ,'+@SingleQuotesTwice+' PQCompleteDate,'+@SingleQuotesTwice+' TDRequestDate ,'+@SingleQuotesTwice+' TDCompleteDate,CBD.ID CBDID,
        CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
        WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
        ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
		FROM Cities C WITH(NOLOCK) 
        INNER JOIN(vwMMV VW WITH(NOLOCK)
        INNER JOIN (NCS_Dealers ND WITH(NOLOCK)
        INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
        LEFT JOIN CRM_DALostBrand CDLB WITH(NOLOCK) ON CDLB.CDAId = CDA.Id
        LEFT JOIN vwMMV VW1 ON VW1.VersionId = CDLB.VersionId
        INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID)  ON CBD.VersionId = VW.VersionId
        INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id 
        INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id) ON CC.CityId = C.Id   
        INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId AND CII.ProductTypeId = 1
        LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
        LEFT JOIN Cities AS CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
		LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
        LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
		LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
		LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
		LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
        WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+' 
        AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
        AND CDA.DealerId IN ('+@DealerId+')'
         
        IF(@Status = 3)--Potentially Lost
            SET @varsql += ' AND CDA.Status IN (65)'
        ELSE IF(@Status = 1)--Lost
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
        IF(@Status <> 2)
            SET @varsql += ' ORDER BY CDA.CreatedOn'
        IF(@Status = 2)--Not Interested  CDA.CreatedOn AS DealerAssigned,CONVERT(VARCHAR(15),CDA.CreatedOn,106) AS DealerAssigned,
        BEGIN
        SET @varsqlForUnion =  ' UNION ALL  
        SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, CC.Email,CC.Mobile,
        CC.Landline, C.Name CityName,CDA.CreatedOn AS DealerAssigned,
        (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
        ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
        '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion, 
        ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,
        CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name As CarCity ,
        '+@SingleQuotesTwice+' PQRequestDate ,'+@SingleQuotesTwice+' PQCompleteDate,'+@SingleQuotesTwice+' TDRequestDate ,'+@SingleQuotesTwice+' TDCompleteDate,CBD.ID CBDID,                 
        CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
        WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
		FROM NCS_Dealers ND WITH(NOLOCK)
        INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
        INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.ID
        INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
        INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
        INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
        INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
        LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID 
        INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId AND ProductTypeId = 1 
        LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
        LEFT JOIN Cities AS CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID 
		LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id   
		LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
		LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
		LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id   
		LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id                     
        WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
        AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
        AND (CCBD.BookingStatusId <> 16 OR CCBD.BookingStatusId IS NULL)                  
        AND LeadStageId = 3 AND Status = -1
        AND CDA.DealerId IN ('+@DealerId+')'
        IF(@Model = -1)
            BEGIN
                IF(@Make IS NOT NULL)
                    SET @varsqlForUnion += ' AND VW.MakeId = ' + CONVERT(CHAR(10), @Make, 101)
            END
        ELSE
            SET @varsqlForUnion += ' AND VW.ModelId = ' + CONVERT(CHAR(10), @Model, 101)
            
        IF(@Eagerness <> 0)
            SET @varsqlForUnion += CRM.GetEagerness(@Eagerness)
            SET @varsqlForUnion += ' ORDER BY CDA.CreatedOn'
        END
        
                    
    END
    ELSE IF @Type = 2    --Active
    BEGIN
        SET @varsql =  'SELECT CDA.Id DAId, CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
                CC.Email, CC.Mobile, CC.Landline, C.Name CityName, CDA.CreatedOn AS DealerAssigned,
                (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
                ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
                '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' Pending, '+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
                ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, '+@SingleQuotesTwice+' AS Dispositions,CBD.ID CBDID,
                 CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name As CarCity ,
                CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
                WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
                ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
				FROM NCS_Dealers ND WITH(NOLOCK)
                INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON CDA.DealerId = ND.Id
                INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
                INNER JOIN vwMMV VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
                INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id   
                INNER JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CL.ID = CII.LeadId AND ProductTypeId = 1
                INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id 
                LEFT JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CBD.ID
                LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
                LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
                INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id 
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id
                LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
				LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
                WHERE CL.LeadStageId <> 3 AND CDA.Status = -1 
                AND (CCBD.BookingStatusId <> 16 OR CCBD.BookingStatusId IS NULL) 
                AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
                AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
            SET @varsql += ' ORDER BY CDA.CreatedOn'
        
    END
    
    IF @Type = 3    --PQ
    BEGIN
        SET @varsql = 'WITH CP AS ( SELECT CC.Id,CCPL.Id CCPLId,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
            CC.Email, CC.Mobile, CC.Landline, C.Name CityName, CDA.CreatedOn AS DealerAssigned, 
            CCPL.PQRequestDate ,CCPL.PQCompleteDate,CCTL.TDRequestDate ,CCTL.TDCompleteDate,
            
            ROW_NUMBER() OVER (PARTITION BY CCPL.Id ORDER BY CCPL.PQRequestDate DESC) AS RowNumber,
            
            (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
            ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, CL.CreatedOn,'+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,
            '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,CBD.ID CBDID,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
            ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions'
            IF (@Status = 0)        --PQ Requested
                BEGIN
                    SET @varsql += ', CASE WHEN (CCPL.IsPQCompleted = 1 OR CCPL.IsPQNotRequired = 1) THEN 0
                        ELSE DATEDIFF(day, CCPL.PQRequestDate, GETDATE()) END Pending'
                END                
            ELSE IF (@Status = 3)    --PQ Pending
                SET @varsql += ', DATEDIFF(day, CCPL.PQRequestDate, GETDATE()) Pending'
            ELSE                    --PQ (NotPossible, Completed)
                SET @varsql += ', '+@SingleQuotesTwice+' Pending'
            
        SET @varsql += ', CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name As CarCity,        
        CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
        WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
        ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
		FROM Cities C WITH(NOLOCK)
        INNER JOIN (vwMMV VW INNER JOIN    (NCS_Dealers ND WITH(NOLOCK)
        INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
        INNER JOIN CRM_CarPQLog CCPL WITH(NOLOCK) ON CCPL.CBDId = CDA.CBDId
        INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCPL.CBDId) ON VW.VersionId = CBD.VersionId             
        LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId 
        LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId AND ProductTypeId = 1
        INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
        INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId) ON CC.CityId = C.Id
        LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
        LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
        LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCPL.PQNRDispositionId = CED.Id AND CED.IsActive = 1
		LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id
        LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
		LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
		LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
		LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
        WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
        AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
            SET @varsql += ' ) SELECT DISTINCT Id,CCPLId, CustomerName, Email,Mobile,Landline,CityName,DealerAssigned,CarName,Did,DealerCode,ReasonLost,
				LeadId,Dealer,CreatedOn,Comments,LostDate,LostName,Pending,Dispositions,SourceName,CarCity,CBDID, LostMake, LostModel, LostVersion,
				PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability,DCName,RMName,Executive
				FROM CP
				WHERE RowNumber = 1
				ORDER BY CreatedOn'        
    END
    IF @Type = 4    --TD
        BEGIN        
            
            SET @varsql = 'WITH CP AS ( SELECT CC.Id, CCTL.Id CCTLId,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
                CC.Email, CC.Mobile, CC.Landline, C.Name CityName, CDA.CreatedOn AS DealerAssigned, 
                CCPL.PQRequestDate,CCPL.PQCompleteDate,CCTL.TDRequestDate,CCTL.TDCompleteDate,
                ROW_NUMBER() OVER (PARTITION BY CCTL.Id ORDER BY CCTL.TDRequestDate DESC) AS RowNumber,
                (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
                ND.ID Did,CL.Id AS LeadId,ND.Name Dealer,CL.CreatedOn,
                '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,'+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
                ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode, CED.Name AS Dispositions' 
                IF (@Status = 0)        --TD Requested
                    BEGIN
                        SET @varsql += ', CASE WHEN (CCTL.IsTDCompleted = 1 OR CCTL.ISTDNotPossible = 1 OR CCTL.IsTDDirect = 1) THEN 0 
                        ELSE DATEDIFF(day, CCTL.TDRequestDate, GETDATE()) END Pending'
                    END                
                ELSE IF (@Status = 3)    --TD Pending
                    SET @varsql += ', DATEDIFF(day, CCTL.TDRequestDate, GETDATE()) Pending'
                ELSE                    --TD (NotPossible, Completed)
                    SET @varsql += ', '+@SingleQuotesTwice+' Pending'
                
                SET @varsql += ', CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName,
                CAC.Name As CarCity ,CBD.ID CBDID,CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
                WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
                ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
				FROM vwMMV VW
                INNER JOIN (NCS_Dealers ND WITH(NOLOCK)
                INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
                INNER JOIN CRM_CarTDLog CCTL WITH(NOLOCK) ON CCTL.CBDId = CDA.CBDId
                INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CBD.ID = CCTL.CBDId) ON VW.VersionId = CBD.VersionId   
                LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId 
                LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CBD.LeadId = CII.LeadId AND ProductTypeId = 1
                INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CBD.LeadId = CL.Id
                INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.Id
                INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id                
                LEFT JOIN CRM_ETDispositions CED WITH(NOLOCK) ON CCTL.TDNPDispositionId = CED.Id AND CED.IsActive = 1
                LEFT OUTER JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
                LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
                LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
				LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
                WHERE CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
                AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
                AND CDA.DealerId IN ('+@DealerId+')'
                
                IF (@Status <> 5)--TD Direct
                BEGIN
                    SET @varsql += ' AND CCTL.IsTDRequested = 1'
                END
                
                
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
                
                ELSE IF (@Status = 5)--TD Direct
                BEGIN
                    SET @varsql += ' AND CCTL.IsTDDirect=1'
                END
                
                ELSE IF (@Status = 3)--TD Pending
                BEGIN
                    SET @varsql += ' AND (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL)
                        AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)'                        
                END
                ELSE IF (@Status = 4)--TD Future
                BEGIN
                    SET @varsql += ' AND (CCTL.IsTDCompleted = 0 OR CCTL.IsTDCompleted IS NULL)
                        AND (CCTL.ISTDNotPossible = 0 OR CCTL.ISTDNotPossible IS NULL)'                    
                END
                
                IF(@Eagerness <> 0)
                    SET @varsql += CRM.GetEagerness(@Eagerness)
                SET @varsql += ' ) SELECT DISTINCT Id, CCTLId,CustomerName, Email,Mobile,Landline,CityName,DealerAssigned,CarName,Did,DealerCode,ReasonLost,
				LeadId,Dealer,CreatedOn,Comments,LostDate,LostName,Pending,Dispositions,SourceName,CarCity,CBDID, LostMake, LostModel, LostVersion,
				PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability,DCName,RMName,Executive
				FROM CP
				WHERE RowNumber = 1'
                
                IF (@Status = 3)--TD Pending
                BEGIN
                    SET @varsql += ' AND CONVERT(CHAR(10),TDRequestDate, 120) <= ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120)+ @SingleQuotes
                END
                ELSE IF (@Status = 4)--TD Future
                BEGIN
                    SET @varsql += ' AND CONVERT(CHAR(10),TDRequestDate, 120) > ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120)+ @SingleQuotes
                END    
                
                SET @varsql += ' ORDER BY CreatedOn'  
        END  
        
    IF @Type = 5    --Booked
        BEGIN
            SET @varsql = 'WITH CP AS ( SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
                CC.Email, CC.Mobile, CC.Landline, C.Name CityName,CDA.CreatedOn AS DealerAssigned,
                CCPL.PQRequestDate ,CCPL.PQCompleteDate,CCTL.TDRequestDate ,CCTL.TDCompleteDate,
				ROW_NUMBER() OVER (PARTITION BY CBD.ID ORDER BY CDA.CreateOnDatePart DESC) AS RowNumber,
                (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
                ND.ID Did,CL.Id AS LeadId, ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
                '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
                CL.CreatedOn,'+@SingleQuotesTwice+' AS Dispositions,'+@SingleQuotesTwice+' Pending,CBD.Id CBDID,
                CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name As CarCity,
                CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
                WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
                ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
				FROM NCS_Dealers ND WITH(NOLOCK)
                INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
                INNER JOIN CRM_CarBookingData CCBD WITH(NOLOCK) ON CCBD.CarBasicDataId = CDA.CBDId
                INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
                INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId                 
                LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId 
                LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId 
                LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId AND ProductTypeId =1
                INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
                INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID
                INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
                LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
                LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
				LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
                LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID 
				LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
				LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
				LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
                WHERE CCBD.BookingStatusId = 16 
                AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
                AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
                
            SET @varsql += ' ) SELECT DISTINCT Id,CustomerName, Email,Mobile,Landline,CityName,DealerAssigned,CarName,Did,DealerCode,ReasonLost,
				LeadId,Dealer,CreatedOn,Comments,LostDate,LostName,Pending,Dispositions,SourceName,CarCity,CBDID,LostMake,LostModel,LostVersion,
				PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability,DCName,RMName,Executive
				FROM CP
				WHERE RowNumber = 1
				ORDER BY CreatedOn'
        END    
    IF @Type = 6    --VIN
        BEGIN
            SET @varsql = 'WITH CP AS ( SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
            CC.Email, CC.Mobile, CC.Landline, C.Name CityName, CDA.CreatedOn AS DealerAssigned,
            CCPL.PQRequestDate,CCPL.PQCompleteDate,CCTL.TDRequestDate,CCTL.TDCompleteDate,
            ROW_NUMBER() OVER (PARTITION BY CDA.ID ORDER BY CDA.CreateOnDatePart DESC) AS RowNumber,
            (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName, 
            ND.ID Did,CL.Id AS LeadId,ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
            '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
            CL.CreatedOn, '+@SingleQuotesTwice+' AS Dispositions, DATEDIFF(day, CCBD.BookingDate, GETDATE()) Pending,
            CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName, CAC.Name As CarCity,CBD.Id CBDID,
            CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
            WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
            ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
			FROM CRM_CarDealerAssignment CDA WITH(NOLOCK) 
            INNER JOIN NCS_Dealers ND WITH(NOLOCK) ON CDA.DealerId = ND.Id
            INNER JOIN CRM_CarBookingData AS CCBD WITH(NOLOCK) ON CDA.CBDId = CCBD.CarBasicDataId 
            INNER JOIN CRM_CarBasicData AS CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id 
            INNER JOIN vwMMV AS VW WITH(NOLOCK) ON CBD.VersionId = VW.VersionId
            INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
            INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CC.ID = CL.CNS_CustId
            INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id            
            LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId 
            LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId
            LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId AND ProductTypeId = 1
            LEFT JOIN CRM_CarDeliveryData AS CCDD WITH(NOLOCK) ON CDA.CBDId = CCDD.CarBasicDataId
            LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
            LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
			LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
            LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID
			LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
			LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
			LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
            WHERE CDA.Status NOT IN (40,41,42,50,60,61) 
            AND CDA.CBDId NOT IN 
            (SELECT CBDId FROM CRM_CarInvoices WITH(NOLOCK) 
            WHERE InvoiceId IN 
            (SELECT id FROM CRM_ADM_Invoices WITH(NOLOCK) WHERE MakeId = 15)) 
            AND ( BookingStatusId = 16 OR DeliveryStatusId = 20) 
            AND IsNull(CCDD.ChasisNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
            AND IsNull(CCDD.EngineNumber, '+@SingleQuotesTwice+') = '+@SingleQuotesTwice+'
            AND IsNull(CCDD.RegistrationNumber,'+@SingleQuotesTwice+') = '+@SingleQuotesTwice+' 
            AND CDA.CreateOnDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
            AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
            SET @varsql += ' ) SELECT DISTINCT Id,CustomerName, Email,Mobile,Landline,CityName,DealerAssigned,CarName,Did,DealerCode,ReasonLost,
				LeadId,Dealer,CreatedOn,Comments,LostDate,LostName,Pending,Dispositions,SourceName,CarCity,CBDID,LostMake,LostModel,LostVersion,
				PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability,DCName,RMName,Executive
				FROM CP
				WHERE RowNumber = 1
				ORDER BY CreatedOn'  
        END
    
    IF @Type = 7    --Car Booked record from Event
        BEGIN
                SET @varsql = 'WITH CP AS ( SELECT CC.Id,(CC.FirstName + '+@SingleQuotes+' '+@SingleQuotes+' + CC.LastName) CustomerName, 
                    CC.Email, CC.Mobile, CC.Landline, C.Name CityName,CDA.CreatedOn AS DealerAssigned,
                    (VW.Make + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Model + '+@SingleQuotes+' '+@SingleQuotes+' + VW.Version) CarName,'+@SingleQuotesTwice+' LostMake,'+@SingleQuotesTwice+' LostModel,'+@SingleQuotesTwice+' LostVersion,
                    ND.ID Did,CL.Id AS LeadId, ND.Name Dealer, '+@SingleQuotesTwice+' Comments,'+@SingleQuotesTwice+' LostDate, CAC.Name As CarCity,
                    '+@SingleQuotesTwice+' LostName,'+@SingleQuotesTwice+' ReasonLost,ISNULL(ND.DealerCode,'+@SingleQuotes+'--'+@SingleQuotes+') AS DealerCode,
                    CL.CreatedOn, '+@SingleQuotesTwice+' AS Dispositions,'+@SingleQuotesTwice+' Pending,CBD.Id CBDID,
                    CASE CBD.SourceCategory WHEN 3 THEN LA.Organization ELSE '+@SingleQuotes+'CarWale'+@SingleQuotes+' END AS SourceName ,
                    CCPL.PQRequestDate ,CCPL.PQCompleteDate,CCTL.TDRequestDate ,CCTL.TDCompleteDate,
					ROW_NUMBER() OVER (PARTITION BY CEL.ItemId ORDER BY CEL.EventOn DESC) AS RowNumber,
					CASE WHEN CII.ClosingProbability = 1 THEN '+@SingleQuotes+'Very Hot'+@SingleQuotes+' WHEN CII.ClosingProbability = 2 THEN '+@SingleQuotes+'Hot'+@SingleQuotes+'
                    WHEN CII.ClosingProbability = 3 THEN '+@SingleQuotes+'Normal'+@SingleQuotes+' WHEN CII.ClosingProbability = 4 THEN '+@SingleQuotes+'Low'+@SingleQuotes+' END AS ClosingProbability
                    ,OU.UserName AS DCName,NRM.Name AS RMName ,CCDS.Name AS Executive
					FROM NCS_Dealers ND WITH(NOLOCK)
                    INNER JOIN CRM_CarDealerAssignment CDA WITH(NOLOCK) ON ND.ID = CDA.DealerId
                    INNER JOIN CRM_CarBasicData CBD WITH(NOLOCK) ON CDA.CBDId = CBD.Id
                    INNER JOIN CRM_EventLogs CEL WITH (NOLOCK) ON CBD.ID = CEL.ItemId
                    INNER JOIN vwMMV VW ON VW.VersionId = CBD.VersionId                     
                    LEFT JOIN CRM_CarTDLog CCTL WITH (NOLOCK) ON CBD.ID = CCTL.CBDId 
                    LEFT JOIN CRM_CarPQLog CCPL WITH (NOLOCK) ON CBD.ID = CCPL.CBDId 
                    LEFT JOIN CRM_InterestedIn AS CII WITH(NOLOCK) ON CII.LeadId = CBD.LeadId AND ProductTypeId = 1
                    INNER JOIN CRM_Leads CL WITH(NOLOCK) ON CL.ID = CBD.LeadId
                    INNER JOIN CRM_Customers CC WITH(NOLOCK) ON CL.CNS_CustId = CC.ID
                    INNER JOIN Cities C WITH(NOLOCK) ON CC.CityId = C.Id
                    LEFT JOIN Cities CAC WITH (NOLOCK) ON CBD.CityId = CAC.ID
                    LEFT JOIN LA_Agencies AS LA WITH (NOLOCK) ON LA.Id = CBD.SourceId
					LEFT JOIN CRM_ADM_DCDealers CAD WITH(NOLOCK) ON CAD.DealerId = ND.Id 
                    LEFT JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = CAD.DCID
					LEFT JOIN NCS_RMDealers NRD WITH (NOLOCK) ON ND.Id = NRD.DealerId AND NRD.Type = 1
					LEFT JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id  
					LEFT JOIN CRM_CDA_DealerSalesExecutive CCDS WITH(NOLOCK) ON CDA.SalesExecutiveId = CCDS.Id
                    WHERE CEL.EventType = 16
                    AND CEL.EventDatePart BETWEEN ' +@SingleQuotes+ CONVERT(CHAR(10), @FromDate, 120) +@SingleQuotes+'
                    AND ' +@SingleQuotes+ CONVERT(CHAR(10), @ToDate, 120) +@SingleQuotes+ '
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
                    SET @varsql += ' ) SELECT DISTINCT Id,CustomerName, Email,Mobile,Landline,CityName,DealerAssigned,CarName,Did,DealerCode,ReasonLost,
				LeadId,Dealer,CreatedOn,Comments,LostDate,LostName,Pending,Dispositions,SourceName,CarCity,CBDID,LostMake,LostModel,LostVersion,
				PQRequestDate,PQCompleteDate,TDRequestDate,TDCompleteDate,ClosingProbability,DCName,RMName,Executive
				FROM CP
				WHERE RowNumber = 1
				ORDER BY CreatedOn'                    
        END
        
    EXEC (@varsql + @varsqlForUnion)
    PRINT (@varsql + @varsqlForUnion)
 END

