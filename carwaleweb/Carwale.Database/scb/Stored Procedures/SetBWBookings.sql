IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[scb].[SetBWBookings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [scb].[SetBWBookings]
GO

	CREATE PROCEDURE scb.SetBWBookings
AS
INSERT INTO [scb].[BikeWaleDailyTracker](TrackerType,TrackerDate,TrackerCount)
SELECT TrackerType,TrackerDate,SUM(TrackerCount)
FROM(
SELECT  2 as TrackerType,CAST(RequestedDate as date) TrackerDate,COUNT(TB.TC_NewCarBookingId) as TrackerCount
FROM TC_NewCarBooking as TB
   JOIN TC_NewCarInquiries as TI on TI.TC_NewCarInquiriesId=TB.TC_NewCarInquiriesId
       JOIN TC_InquiriesLead as TL on TL.TC_InquiriesLeadId=TI.TC_InquiriesLeadId
       JOIN Dealers as D on D.ID=TL.BranchId and D.ApplicationId=2 -- Bikewale
GROUP BY cast(RequestedDate as date)
UNION
SELECT 2 as TrackerType,CAST(PGT.EntryDateTime AS DATE) AS Trackerdate,COUNT(PGT.ID) as TrackerCount
FROM pgtransactions PGT
INNER JOIN BikeWale..Customers C ON PGT.ConsumerId = C.Id
INNER JOIN BikeWale..PQ_NewBikeDealerPriceQuotes DPQ ON PGT.id = DPQ.TransactionId
INNER JOIN BikeWale..BikeVersions BV ON BV.id = PGT.CarId
INNER JOIN BikeWale..BikeModels BMO ON BMO.id = BV.BikeModelId
INNER JOIN BikeWale..BikeMakes BM ON BMO.BikeMakeId = BM.id
INNER JOIN Dealers D ON DPQ.DealerId = D.id
WHERE PGT.ApplicationId = 2
	AND PGT.TransactionCompleted = 1
	AND PGT.PackageId = 67
	AND PGT.Amount >= 100
GROUP BY CAST(PGT.EntryDateTime AS DATE) 
) Bookings
GROUP BY TrackerType,TrackerDate
