IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddBuyerInqFromFollowupLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddBuyerInqFromFollowupLoad]
GO

	-- Created By:	Binu
-- Create date: 24 Jul 2012
-- Description:	loading  Stock from follow up page in edit mode
-- =============================================
CREATE PROCEDURE [dbo].[TC_AddBuyerInqFromFollowupLoad]
@TC_InquiriesId BIGINT, 
@BranchId BIGINT
AS       
BEGIN
	SELECT Id,(CASE WHEN Name='SUV/MUV' THEN 'SUV' WHEN Name='Station Wagon' THEN 'Wagon' ELSE Name END)AS Name FROM CarBodyStyles WHERE  ID<>9 
	
	SELECT FuelTypeId, FuelTypeName FROM TC_CarFuelType FT WHERE IsActive=1
	
	IF(@TC_InquiriesId IS NOT NULL)
	BEGIN
		SELECT BIWS.TC_InquiriesId, BIWS.BodyType,BIWS.MinPrice, BIWS.MaxPrice, BIWS.FromMakeYear,BIWS.ToMakeYear,BIWS.FuelType,
		BIWS.ModelIds,BIWS.ModelNames
		FROM TC_Inquiries INQ
		INNER JOIN TC_BuyerInqWithoutStock BIWS WITH(NOLOCK)ON BIWS.TC_InquiriesId=INQ.TC_InquiriesId
		WHERE INQ.TC_InquiriesId=@TC_InquiriesId AND INQ.BranchId=@BranchId
	END
END
