IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetModelForPriceQuotes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetModelForPriceQuotes]
GO
	/*
    Author:Vishal Srivastava AE-1830
	Date:30/09/2013
	Purpose:To get the modelid and modelname from the database for dropdown
*/

CREATE PROCEDURE [dbo].[TC_GetModelForPriceQuotes]
@MakeId INT
AS
BEGIN

SELECT DISTINCT ModelId, Model FROM vwMMV WHERE MakeId=@MakeId AND IsModelNew=1;

END
