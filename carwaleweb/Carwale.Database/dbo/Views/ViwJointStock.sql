IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'ViwJointStock' AND
     DROP VIEW dbo.ViwJointStock
GO

	
CREATE  VIEW [dbo].[ViwJointStock] AS
SELECT
CONVERT(bit, 1) AS IsDealer,
SI.ID AS InquiryId,
'D' + Convert(Varchar, SI.ID) AS ProfileId,
SI.CarVersionId AS VersionId,
D.CityId AS CityId,
D.AreaId AS AreaId,
SI.Price AS Price,
SI.MakeYear AS MakeYear,
SI.Kilometers AS Kilometers,
SI.Color,
SI.Comments,
SI.LastUpdated AS LastUpdated,
SI.EntryDate,
SI.PackageType,
(CASE SI.PackageType WHEN 8 THEN 0 ELSE 1 END) AS ShowDetails,
(CASE SI.PackageType 
	 WHEN 7 THEN 2  -- Dealer Shwroom, Priority = 2
	 WHEN 5 THEN 3  -- Dealer paid listing based,  Priority = 3
	 WHEN 6 THEN 3  -- Dealer paid slot based,  Priority = 3 
	 WHEN 9 THEN 3  -- Dealer unlimitted, Priority = 3 
	 WHEN 8 THEN 4  -- Dealer unpaid, Priority = 4 
 END ) AS Priority

FROM 
SellInquiries AS SI,
Dealers AS D
WHERE
D.ID = SI.DealerId AND
SI.StatusId = 1 AND D.Status = 0 AND
SI.PackageExpiryDate >= convert(varchar, getdate() ,101 )
UNION ALL
SELECT 
CONVERT(bit, 0) AS IsDealer,
CSI.ID AS InquiryId,
'S' + Convert(Varchar, CSI.ID) AS ProfileId,
CSI.CarVersionId AS VersionId,
CSC.CityId AS CityId,
-1 AS AreaId,
CSI.Price AS Price,
CSI.MakeYear AS MakeYear,
CSI.Kilometers AS Kilometers,
CSI.Color,
CSI.Comments,
DateAdd(D, -30, CSI.ClassifiedExpiryDate) AS LastUpdated,
CSI.EntryDate AS EntryDate,
CSI.PackageType,
(CASE CSI.PackageType WHEN 2 THEN 1 ELSE 0 END) AS ShowDetails,
(CASE CSI.PackageType 
	 WHEN 2 THEN 1  -- Customer paid, Priority = 1
	 WHEN 1 THEN 5  -- Customer not paid,  Priority = 5
 END ) AS Priority
FROM 
CustomerSellInquiries AS CSI,
Customers AS C,
CustomerSellInquiryCities AS CSC
WHERE
C.IsFake = 0 AND
C.ID = CSI.CustomerId AND
CSC.SellInquiryId = CSI.ID AND
CSI.IsApproved = 1 AND
CSI.IsFake = 0 AND
CSI.StatusId IN (SELECT IsActive FROM InquiryStatus) AND
CSI.ClassifiedExpiryDate >= convert(varchar, getdate() ,101 )


