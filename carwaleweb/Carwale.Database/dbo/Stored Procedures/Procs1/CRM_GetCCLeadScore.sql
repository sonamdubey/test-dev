IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetCCLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetCCLeadScore]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th Feb 2014
-- Description:	To get the actual and planned booking,TD,active lead status on quarterly basis
-- Modifier:   1. Ruchira Patil on 6th May 2014 (Modified the TestDrive query-How many requested in that date range and out of them how many got delivered)
--			   2. Ruchira Patil on 14th May 2014(Added DEaler BenchMark Query and modified active lead status query)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetCCLeadScore]
	AS
	BEGIN

		--TO GET ACTUAL BOOKING    
		SELECT CAST(CBL.BookingCompletedEventOn AS DATE) AS BookDate,MONTH(CBL.BookingCompletedEventOn) AS BookMonth,YEAR(CBL.BookingCompletedEventOn) AS BookYear,
		COUNT(DISTINCT CBL.CBDId) AS TotalBooking,VM.MakeId AS MakeId
		FROM CRM_CarBasicData CBD WITH (NOLOCK) 
		INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		INNER JOIN CRM_CarBookingLog CBL WITH (NOLOCK) ON CBD.Id = CBL.CBDId  
		WHERE VM.MakeId IN (9,15,16) 
		AND CBL.IsBookingCompleted = 1 
		AND CBL.BookingCompletedEventOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
		GROUP BY CAST(CBL.BookingCompletedEventOn AS DATE),MONTH(CBL.BookingCompletedEventOn),YEAR(CBL.BookingCompletedEventOn),VM.MakeId

		--TO GET ACTUAL TD
		SELECT CAST(CTD.TDCompletedEventOn AS DATE) AS TDDate,MONTH(CTD.TDCompletedEventOn) AS TDMonth,YEAR(CTD.TDCompletedEventOn) AS TDYear,
		SUM(CASE ISNULL(IsTDRequested,0) WHEN 1 THEN 1 ELSE 0 END) AS Requested,
		SUM(CASE ISNULL(IsTDCompleted,0) WHEN 1 THEN 1 ELSE 0 END) AS Completed,
		VM.MakeId AS MakeId
		FROM CRM_CarBasicData CBD WITH (NOLOCK) 
		INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		INNER JOIN CRM_CarTDLog CTD WITH (NOLOCK) ON CBD.Id = CTD.CBDId  
		WHERE VM.MakeId IN (9,15,16) 
		AND CTD.TDCompletedEventOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
		GROUP BY CAST(CTD.TDCompletedEventOn AS DATE),MONTH(CTD.TDCompletedEventOn),YEAR(CTD.TDCompletedEventOn),VM.MakeId 

		--SELECT CAST(CTD.TDCompletedEventOn AS DATE) AS TDDate,MONTH(CTD.TDCompletedEventOn) AS TDMonth,YEAR(CTD.TDCompletedEventOn) AS TDYear,
		--COUNT(DISTINCT CTD.CBDId) AS TotalTD,VM.MakeId AS MakeId
		--FROM CRM_CarDealerAssignment CDA WITH (NOLOCK)
		--INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CDA.CBDId
		--INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		--INNER JOIN CRM_CarTDLog CTD WITH (NOLOCK) ON CDA.CBDId = CTD.CBDId  
		--WHERE VM.MakeId IN (SELECT DISTINCT MakeId FROM @tblTargetCount) 
		--AND CTD.IsTDCompleted = 1 
		--AND CTD.TDCompletedEventOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
		--GROUP BY CAST(CTD.TDCompletedEventOn AS DATE),MONTH(CTD.TDCompletedEventOn),YEAR(CTD.TDCompletedEventOn),VM.MakeId


		--TO GET ACTUAL ACTIVE LEAD STATUS 
		SELECT CAST(CSL.UpdatedOn AS DATE) AS LeadStatusDate, MONTH(CSL.UpdatedOn) AS LeadStatusMonth,YEAR(CSL.UpdatedOn) AS LeadStatusYear,
		COUNT(DISTINCT CSL.CBDId) AS TotalStatus,VM.MakeId AS MakeId,VM.Make AS Make
		FROM CRM_CarStatusUpdateLog CSL WITH (NOLOCK)
		JOIN CRM_SubDisposition CSD WITH(NOLOCK) ON CSD.Id = CSL.SubDispositionId
		JOIN CRM_Dispositions CD WITH (NOLOCK) ON CSD.DispId = CD.Id
		JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CSL.CBDId
		INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		WHERE VM.MakeId IN (9,15,16)  
		AND CD.Id IN(2,3,4,5)-- ('Booked a car','Interested to Buy','Not interested to buy','Postpone Purchase') 
		AND CSL.UpdatedOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
		GROUP BY CAST(CSL.UpdatedOn AS DATE),MONTH(CSL.UpdatedOn),YEAR(CSL.UpdatedOn),VM.MakeId,VM.Make

		--SELECT CAST(CSL.UpdatedOn AS DATE) AS LeadStatusDate, MONTH(CSL.UpdatedOn) AS LeadStatusMonth,YEAR(CSL.UpdatedOn) AS LeadStatusYear,
		--COUNT(DISTINCT CSL.CBDId) AS TotalStatus,VM.MakeId AS MakeId
		--FROM CRM_CarStatusUpdateLog CSL WITH (NOLOCK)
		--INNER JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID = CSL.CBDId
		--INNER JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		--INNER JOIN CRM_SubDisposition CSD WITH(NOLOCK) ON CSD.Id = CSL.SubDispositionId 
		--WHERE VM.MakeId IN (SELECT DISTINCT MakeId FROM @tblTargetCount) 
		----AND CSD.IsConnected = 1
		--AND CSL.UpdatedOn BETWEEN CAST(DATEADD(q, DATEDIFF(q, 0, GETDATE()), 0) AS DATE) AND CAST(DATEADD(d, -1, DATEADD(q, DATEDIFF(q, 0, GETDATE()) + 1, 0)) AS DATE)
		--GROUP BY CAST(CSL.UpdatedOn AS DATE),MONTH(CSL.UpdatedOn),YEAR(CSL.UpdatedOn),VM.MakeId

		--DEALER BENCHMARK
		--SELECT *,CAST(((CAST(Tab.Booked AS DECIMAL(5,2))/CAST(Tab.Assigned AS DECIMAL(5,2)))*100) AS DECIMAL(5,2)) AS Conversion FROM (
		SELECT DISTINCT VM.Make AS Make,Vm.MakeId AS MakeId,COUNT(CDA.CBDId) AS Assigned,COUNT(DISTINCT CBL.CBDId) AS Booked
		FROM CRM_CarDealerAssignment CDA WITH (NOLOCK)
		JOIN NCS_Dealers ND WITH (NOLOCK) ON ND.Id = CDA.DealerId
		JOIN CRM_CarBasicData CBD WITH (NOLOCK) ON CBD.ID=CDA.CBDId
		JOIN vwMMV VM WITH (NOLOCK) ON CBD.VersionId = VM.VersionId
		LEFT JOIN CRM_CarBookingLog CBL WITH (NOLOCK) ON CDA.CBDId = CBL.CBDId AND CBL.IsBookingCompleted=1 
		WHERE VM.MakeId IN(9,15,16) --('Mahindra','Skoda','Tata') 
		AND CDA.CreatedOn BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-4, 0) AND GETDATE()
		GROUP BY VM.Make,VM.MakeId
		--HAVING Count(distinct CBD.LeadId) > 9 
		--)Tab

		--TO GET TARGET COUNT
		SELECT CT.Brand AS MakeId,CM.Name AS Make,CAST(Date AS DATE) AS TargetDate,--MONTH(Date) AS TargetMonth,YEAR(Date) AS TargetYear,
		CT.Value Target,CT.Type,CT.TargetPeriod
		FROM CRM_Targets CT WITH(NOLOCK)
		INNER JOIN CarMakes CM WITH(NOLOCK) ON CM.ID = CT.Brand
		WHERE YEAR(CT.Date) = YEAR(GETDATE()) 
	END

