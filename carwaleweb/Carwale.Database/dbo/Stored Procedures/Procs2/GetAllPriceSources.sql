IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllPriceSources]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetAllPriceSources]
GO

	
-- =============================================
-- Author:		Vicky Lund
-- Create date: 07/06/2016
-- EXEC [GetAllPriceSources]
-- =============================================
CREATE PROCEDURE [dbo].[GetAllPriceSources]
AS
BEGIN
	SELECT PSM.Id
		,PSM.[Name]
	FROM PriceSourceMaster PSM WITH(NOLOCK)
	WHERE PSM.IsActive = 1
END

