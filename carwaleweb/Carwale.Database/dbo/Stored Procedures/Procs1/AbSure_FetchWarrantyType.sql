IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_FetchWarrantyType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_FetchWarrantyType]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Jan 10,2015
-- Description:	This SP fetch Warranty Types
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_FetchWarrantyType]
@IsVisible BIT = 0
AS
BEGIN
		SET NOCOUNT ON;
		SELECT	AbSure_WarrantyTypesId WarrantyTypeId,Warranty Warranty
		FROM	AbSure_WarrantyTypes
		WHERE   IsVisible = @IsVisible
   
END

---------------------------------------------------------------------------------------------------------


