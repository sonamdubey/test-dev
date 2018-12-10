IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[MG_LiveStock_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[MG_LiveStock_SP]
GO

	CREATE   Procedure [dbo].[MG_LiveStock_SP]
AS

--delete the data from the magazine stock
Delete From MG_LiveStock

INSERT INTO MG_LiveStock
	SELECT 
		Si.Id InquiryId, 
		(Cm.Name +' '+ Cmo.Name +' '+ Cv.Name) AS Car,
		Cm.Id AS MakeId, Cmo.Id AS ModelId, Cv.Id AS VersionId,
		Si.Color, 
		Si.Price, 
		Si.Kilometers, 
		Si.MakeYear, 
		Ds.Organization Owner,
		Ds.MobileNo Mobile, 
		Ds.PhoneNo Phone, 
		'1' IsDealer, 
		Ct.Name City, 
		Ct.Id CityId,
		S.Name AS State,
		S.Id AS StateId,
		GETDATE() AS UpdateDateTime
	FROM 
		SellInquiries AS Si, 
		Dealers Ds,
		CarMakes AS Cm, 
		CarModels AS Cmo, 
		CarVersions AS Cv, 
		Cities Ct,
		LiveListings AS VJ,
		States AS S
	WHERE
		VJ.SellerType = 1 AND
		Si.ID = VJ.InquiryId AND
		Si.DealerId = Ds.Id AND 
		Si.CarVersionId = Cv.Id AND 
		Cv.CarModelId = Cmo.Id AND 
		Cmo.CarMakeId = Cm.Id AND
		Ds.CityId = Ct.Id AND
		S.ID = Ct.StateId
	UNION All
	SELECT 
		Csi.Id InquiryId, 
		(Cm.Name +' '+ Cmo.Name +' '+ Cv.Name) AS Car,
		Cm.Id AS MakeId, Cmo.Id AS ModelId, Cv.Id AS VersionId,
		Csi.Color, 
		Csi.Price, 
		Csi.Kilometers, 
		Csi.MakeYear, 
		Cs.Name Owner,
		Cs.Mobile Mobile, 
		Cs.Phone1 Phone, 
		'0' IsDealer, 
		Ct.Name City, 
		Ct.Id CityId,
		S.Name AS State,
		S.Id AS StateId,
		GETDATE() AS UpdateDateTime
	FROM 
		CustomerSellInquiries AS Csi,
		CustomerSellInquiryCities Cst, 
		Customers Cs,
		CarMakes AS Cm, 
		CarModels AS Cmo, 
		CarVersions AS Cv, 
		Cities Ct,
		States AS S,
		LiveListings AS VJ
	WHERE 
		VJ.SellerType = 2 AND
		Csi.ID = VJ.InquiryId AND
		Csi.CustomerId = Cs.Id AND 
		Csi.Id = Cst.SellInquiryId AND 
		Cst.CityId = Ct.Id AND 
		Csi.CarVersionId = Cv.Id AND 
		Cv.CarModelId = Cmo.Id AND 
		Cmo.CarMakeId = Cm.Id AND
		S.ID = Ct.StateId
