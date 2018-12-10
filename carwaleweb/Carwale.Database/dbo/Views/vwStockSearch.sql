IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'vwStockSearch' AND
     DROP VIEW dbo.vwStockSearch
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Author:  Surendra                   
-- Create date: 5th Sept,2012                   
-- Description: this view will be used in Stock Search page            
-- =============================================      r             
CREATE VIEW [dbo].[vwStockSearch]
AS
SELECT S.versionid AS CarVersionId
	,S.Id AS StockId
	,'Dealer' seller
	,1 sellertype
	,MK.NAME makename
	,MK.id MakeId
	,CM.NAME modelname
	,CM.id ModelId
	,CV.NAME versionname
	,A.NAME areaname
	,S.price Price
	,S.kms kilometers
	,S.makeyear MakeYear
	,S.colour color
	,C.NAME City
	,C.id cityid
	,(
		SELECT COUNT(p.Id)
		FROM TC_CarPhotos p
		WHERE IsActive = 1
			AND p.StockId = S.Id
		) AS PhotoCount
	,CP.ImageUrlThumbSmall frontimagepath
	,S.CertificationId
	,CV.carfueltype
	,CV.cartransmission
	,CC.AdditionalFuel
	,CP.HostUrl + DirectoryPath AS hosturl
	,D.StateId
	,CV.bodystyleid
	,S.BranchId
	,C.Longitude
	,C.Lattitude
FROM tc_stock S WITH (NOLOCK)
INNER JOIN TC_CarCondition CC WITH (NOLOCK) ON S.Id = CC.StockId
LEFT JOIN TC_CarPhotos CP WITH (NOLOCK) ON S.Id = CP.StockId
	AND CP.IsMain = 1
	AND CP.IsActive = 1
INNER JOIN carversions CV WITH (NOLOCK) ON S.versionid = CV.id
INNER JOIN carmodels CM WITH (NOLOCK) ON CV.carmodelid = CM.id
INNER JOIN carmakes MK WITH (NOLOCK) ON CM.carmakeid = MK.id
INNER JOIN dealers D WITH (NOLOCK) ON S.branchid = D.id
INNER JOIN cities C WITH (NOLOCK) ON D.cityid = C.id
INNER JOIN Areas A WITH (NOLOCK) ON D.AreaId = A.ID
WHERE S.StatusId = 1
	AND D.IsTCDealer = 1
	AND D.IsDealerActive = 1
	AND S.IsActive = 1
	AND S.IsApproved = 1
