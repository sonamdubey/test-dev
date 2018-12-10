IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetCarsByVins]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetCarsByVins]
GO

	-- =============================================
-- Author:		Purohith Guguloth
-- Create date: 29th july, 2016
-- Description:	This is used to get car details for VinIds  
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_GetCarsByVins] 
	@VinIds VARCHAR(100) = NULL,
	@StockId INT = 0
AS
BEGIN
	
	SET NOCOUNT ON;
	if (@VinIds IS NOT NULL)
	BEGIN
  		Select 
			STUFF((
				SELECT DISTINCT ', ' + CAST(CT.ID AS VARCHAR)
				FROM TC_Deals_StockPrices DSP WITH (NOLOCK)
								 INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = DSP.CityId
								 WHERE dsp.TC_Deals_StockId = ds.id
				FOR XML PATH('')
				,TYPE
				).value('.', 'NVARCHAR(MAX)'), 1, 1, ' ') AS
				CityId,

			MMV.MakeId,MMV.ModelId,MMV.VersionId
			From TC_Deals_Stock DS WITH(NOLOCK)
			INNER Join TC_Deals_StockVIN DSV WITH(NOLOCK) on DS.Id = DSV.TC_Deals_StockId
			INNER Join vwMMV MMV WITH(NOLOCK) on DS.CarVersionId = MMV.VersionId
			Where DSV.TC_DealsStockVINId in (select * from fnSplitCSV(@VinIds))
	END
	Else If (@StockId > 0)
	Begin
		Select 
			STUFF((
				SELECT DISTINCT ', ' + CAST(CT.ID AS VARCHAR)
				FROM TC_Deals_StockPrices DSP WITH (NOLOCK)
								 INNER JOIN Cities CT WITH (NOLOCK) ON CT.ID = DSP.CityId
								 WHERE dsp.TC_Deals_StockId = ds.id
				FOR XML PATH('')
				,TYPE
				).value('.', 'NVARCHAR(MAX)'), 1, 1, ' ') AS
				CityId,

			MMV.MakeId,MMV.ModelId,MMV.VersionId
			From TC_Deals_Stock DS WITH(NOLOCK)
			INNER Join TC_Deals_StockVIN DSV WITH(NOLOCK) on DS.Id = DSV.TC_Deals_StockId
			INNER Join vwMMV MMV WITH(NOLOCK) on DS.CarVersionId = MMV.VersionId
			Where DSV.TC_Deals_StockId = @StockId
		End
END

