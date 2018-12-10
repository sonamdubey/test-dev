IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCarStocks]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCarStocks]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 26-may-14
-- Description:	Get the Stocks and images for the broker.
-- Modified: Ranjeet ||18-Jun || 
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetUsedCarStocks]
	@BrokerId INT
AS
BEGIN
	
	SET NOCOUNT ON;

	--SELECT  BS.ID AS StockId, BS.Kms AS Kms, BS.Color AS Color,BS.FuelTypeId AS FuelType, BS.OwnerTypeId AS OwnerType,
	--BS.Comments AS Comments, 
	-- BS.TransmissionId AS TransmissionId FROM BA_Stock AS BS WHERE BS.BrokerId = @BrokerId AND IsActive = 1 ;

	 SET NOCOUNT ON;
	SELECT BSD.BrokerId, BSD.CarMakeId AS MakeId, BSD.CarModelId AS ModelId, BSD.CarVersionId AS VersionId, BSD.Color, BSD.Comments, 
			BSD.EntryDate, BSD.FuelTypeId, BSD.Kms, BSD.ModifyDate, BSD.OwnerTypeId, BSD.MakeYear, BSD.TransmissionId, BSD.FuelTypeId AS FuelType,
			BSD.OwnerTypeId AS OwnerType, BSD.Price AS Price, CV.Name AS VersionName, CM.Name AS ModelName, Ck.Name AS MakeName, BS.PageView AS PageView, BS.ID AS StockId
			 FROM BA_StockDetails AS BSD  WITH(NOLOCK)
			 INNER JOIN CarVersions AS CV  WITH(NOLOCK) ON CV.ID = BSD.CarVersionId
			 INNER JOIN CarModels AS CM  WITH(NOLOCK) ON CM.Id = CV.CarModelId
			 INNER JOIN CarMakes AS CK  WITH(NOLOCK) ON CK.ID = Cm.CarMakeId
			 INNER JOIN BA_Stock AS BS  WITH(NOLOCK) ON BS.ID = BSD.StockId AND BSD.IsActive = 1
			WHERE BSD.BrokerId = @BrokerId ;

--Get the Images
SELECT  BS.ID AS StockImageId, BT.Id AS StockId FROM BA_StockImage AS BS  WITH(NOLOCK)
INNER JOIN BA_ImageSize AS BI  WITH(NOLOCK) ON BS.ID = BI.StockImageId AND BI.IsReplicated = 1 AND BI.StatusId = 3 AND BS.IsActive = 1 
INNER JOIN BA_Stock AS BT  WITH(NOLOCK) ON BT.ID = BS.StockId AND BT.BrokerId = @BrokerId AND BT.IsActive  =1 



END
