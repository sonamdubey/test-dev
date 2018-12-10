IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarMake]
GO

	-- =============================================
-- Author:		Binumon George
-- Create date: 11 Jan 2012
-- Description:	This procedure is used to get carmake
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarMake]
AS
BEGIN
	SELECT ID, Name FROM CarMakes WITH(NOLOCK) WHERE IsDeleted = 0 ORDER BY Name
END