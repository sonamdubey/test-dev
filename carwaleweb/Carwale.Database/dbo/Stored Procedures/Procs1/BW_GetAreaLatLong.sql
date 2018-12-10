IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetAreaLatLong]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetAreaLatLong]
GO

	-- Author:		Ashish Kamble
-- Create date: 12 Jan 2015
-- Description:	Proc to get the lattitude and longitude of the given city.
-- exec BW_GetAreaLatLong 18
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetAreaLatLong] @AreaId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ISNULL(Lattitude, 0) Lattitude
		,ISNULL(Longitude, 0)  Longitude
	FROM Areas WITH (NOLOCK)
	WHERE IsDeleted = 0
		AND ID = @AreaId
END

