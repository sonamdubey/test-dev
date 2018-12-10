IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetUsedCarStockDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetUsedCarStockDetails]
GO

	CREATE PROCEDURE [dbo].[BA_GetUsedCarStockDetails]
	@StockId INT
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT BSD.BrokerId, BSD.CarMakeId AS MakeId, BSD.CarModelId AS ModelId, BSD.CarVersionId AS VersionId, BSD.Color, BSD.Comments, 
			BSD.EntryDate, BSD.FuelTypeId, BSD.Kms, BSD.ModifyDate, BSD.OwnerTypeId, BSD.MakeYear, BSD.TransmissionId, BSD.FuelTypeId AS FuelType,
			BSD.OwnerTypeId AS OwnerType, BSD.Price AS Price, CV.Name AS VersionName, CM.Name AS ModelName, Ck.Name AS MakeName, BS.PageView AS PageView
			 FROM BA_StockDetails AS BSD 
			 INNER JOIN CarVersions AS CV ON CV.ID = BSD.CarVersionId
			 INNER JOIN CarModels AS CM ON CM.Id = CV.CarModelId
			 INNER JOIN CarMakes AS CK ON CK.ID = Cm.CarMakeId
			 INNER JOIN BA_Stock AS BS ON BS.ID = BSD.StockId
			WHERE BSD.StockId = @StockId AND BSD.IsActive = 1 ;

--Get the Images
SELECT  BS.ID AS StockImageId FROM BA_StockImage AS BS
INNER JOIN BA_ImageSize AS BI ON BS.ID = BI.StockImageId AND BI.IsReplicated = 1 AND BI.StatusId = 3 AND BS.IsActive = 1 
WHERE BS.StockId = @StockId 

END
