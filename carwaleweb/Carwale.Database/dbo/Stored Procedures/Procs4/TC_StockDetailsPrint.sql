IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockDetailsPrint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockDetailsPrint]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 22-07-2015
-- Description:	Print Stock Details
-- Modified By Vivek Gupta on 11-08-2015, removed http
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockDetailsPrint] 
	@StockId BIGINT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @BranchId INT ,@VersionId INT, @TransmissionType INT

	SET @BranchId = (SELECT BranchId FROM TC_Stock WITH(NOLOCK) WHERE Id = @StockId)
	SET @VersionId = (SELECT VersionId FROM TC_Stock WITH(NOLOCK) WHERE Id = @StockId)
	SET @TransmissionType = (SELECT CarTransmission FROM CarVersions CV WITH(NOLOCK)  WHERE CV.ID = @VersionId) 

    SELECT 
	V.Car, 
	St.Price, 
	St.Kms, 
	St.MakeYear,
	st.Colour,
	cc.Owners,
	cc.Insurance,
	@TransmissionType AS TransmissionType,
	cc.CityMileage AS FuelEconomy,
	cc.Comments AS SellerNote,
	CC.Features_SafetySecurity,
	cc.Features_Comfort,
	cc.Features_Others,
	cc.ACCondition,
	CC.EngineCondition,
	cc.SuspensionsCondition,
	cc.ExteriorCondition,
	cc.BrakesCondition,
	cc.BatteryCondition,
	cc.TyresCondition,
	cc.OverallCondition,
	cc.ElectricalsCondition,
	cc.SeatsCondition,
	cc.InteriorCondition,
		(SELECT HostUrl + ISNULL(DirectoryPath,'') + ImageUrlFull AS MainPhotoUrl 
		 FROM TC_CarPhotos WITH (NOLOCK) 
		 WHERE StockId = @StockId 
		 AND IsActive=1 
		 AND IsMain = 1) 
		 AS MainPhotoUrl
	
	FROM TC_Stock St WITH (NOLOCK) 
	INNER JOIN TC_CarCondition Cc WITH (NOLOCK) ON Cc.StockId = St.Id 
	JOIN vwAllMMV V WITH(NOLOCK) ON V.VersionId = st.VersionId
                               
	WHERE St.Id = @StockId 
	--AND St.BranchId=@BranchId
	
	SELECT Organization,ContactPerson,ContactEmail,MobileNo, Address1 + ' ' + Address2 + ' ' + Pincode AS Address FROM Dealers WITH(NOLOCK) WHERE ID = @BranchId
	
    SELECT HostUrl + ISNULL(DirectoryPath,'') + ImageUrlFull AS PhotoUrl FROM TC_CarPhotos WITH (NOLOCK) WHERE StockId = @StockId AND IsActive=1 AND IsMain <> 1

    	
		
	EXEC CD.GetCarSpecsByVersionID @VersionId	
				
	EXEC CD.GetCarFeaturesByVersionID @VersionId

	

END










/****** Object:  StoredProcedure [dbo].[TC_INQLeadSave]    Script Date: 8/14/2015 3:46:30 PM ******/
SET ANSI_NULLS ON
