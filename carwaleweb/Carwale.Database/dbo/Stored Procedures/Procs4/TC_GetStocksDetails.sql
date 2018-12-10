IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStocksDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStocksDetails]
GO

	
-- =============================================  
-- Author      : Chetan Navin
-- Create date : 17th Oct 2016
-- Description : To get stocks details and their corresponding images details
-- EXEC TC_GetStocksDetails '612138',5
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetStocksDetails]
	-- Add the parameters for the stored procedure here  
	@StockIds VARCHAR(1000)
	,@BranchId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;

	-- Insert statements for procedure here  
	SELECT TST.BranchId AS SellerId
		,TST.ID
		,CV.ID AS VersionId
		,TST.Price AS Price
		,TST.Kms
		,TST.MakeYear AS MakeYear
		,TST.Colour
		,TST.LastUpdatedDate
		,TST.EntryDate
		,TST.RegNo
		,TCC.Owners
		,TCC.Insurance
		,TCC.InsuranceExpiry
		,TCC.OneTimeTax Tax
		,TCC.RegistrationPlace
		,TCC.InteriorColor
		,TCC.CityMileage
		,TCC.AdditionalFuel
		,TCC.CarDriven
		,TCC.Accidental
		,TCC.FloodAffected
		,TCC.Modifications
		,TCC.Comments
		,TST.IsFeatured
		,TST.CertificationId
		,TV.VideoUrl
	FROM TC_Stock TST WITH (NOLOCK)
	LEFT JOIN TC_CarCondition TCC WITH (NOLOCK) ON TST.Id = TCC.StockId
	LEFT JOIN TC_StockStatus TS WITH (NOLOCK) ON TS.Id = TSt.StatusId
	LEFT JOIN CarVersions CV WITH (NOLOCK) ON CV.Id = TST.VersionId
		AND CV.IsDeleted = 0
	LEFT JOIN TC_CarVideos TV WITH (NOLOCK) ON TV.StockId = TST.Id
	WHERE TST.ID IN (
			SELECT ListMember
			FROM fnSplitCSV_WithId(@StockIds)
			)
		AND TST.BranchId = @BranchId
		AND TST.IsActive = 1
		AND TST.IsApproved = 1
	ORDER BY TST.Id DESC

	SELECT TC.Id
		,TC.IsMain
		,HostUrl
		,OriginalImgPath
		,TC.StockId AS StockId
	FROM TC_CarPhotos TC WITH (NOLOCK)
	WHERE TC.IsActive = 1
		AND TC.StockId IN (
			SELECT ListMember
			FROM fnSplitCSV_WithId(@StockIds)
			)
	ORDER BY TC.StockId DESC
END
