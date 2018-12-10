IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarPriceByCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarPriceByCity]
GO

	-- =============================================      
-- Author:  <Vikas J>
-- Create date: <18/02/2013>      
-- Description: <Returns the mim and max price of all the version available for the provided modelId and city from the used car listings.
-- Modified by: Manish on 18-06-2014 added with (nolock keyword)
-- =============================================      
CREATE PROCEDURE [dbo].[GetUsedCarPriceByCity]  -- exec GetUsedCarPriceByCity 222,1
	@ModelId INT  , --Model Id of car viewed by user
	@CityId  INT    --City for which used car price is required
	AS
BEGIN
	
	--Returns the mim and max price of all the version available for the provided modelId and city from the used car listings.
	SELECT YEAR(LL.MakeYear) as YearOfManufacture, LL.ModelId, round(MIN(LL.Price)/100000,2) as MinPrice, round(MAX(LL.Price)/100000,2) as MaxPrice 
	FROM LiveListings LL  WITH (NOLOCK)
	WHERE LL.CityId=@CityId 
	AND LL.ModelId=@ModelId
	AND (
			(YEAR(LL.MakeYear)>=YEAR(GETDATE())-2 AND MONTH(GETDATE())>6) 
			OR 
			(YEAR(LL.MakeYear)>=YEAR(GETDATE())-3 AND YEAR(LL.MakeYear)<YEAR(GETDATE()) AND MONTH(GETDATE())<=6)
		)
	GROUP BY YEAR(LL.MakeYear), LL.ModelId	;
	
END 

