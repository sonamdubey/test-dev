IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveDealerCoordinates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveDealerCoordinates]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 20th July 2015
-- Description:    To Save dealer's geographical co-ordinates
-- =============================================
CREATE PROCEDURE AbSure_SaveDealerCoordinates
    @UserId        INT,
    @Latitude    FLOAT,
    @Longitude    FLOAT,
    @DealerId    INT,
    @Status        INT OUTPUT
AS
BEGIN
    UPDATE Dealers SET Lattitude = @Latitude,Longitude = @Longitude , TC_UserId = @UserId
    WHERE ID = @DealerId

    SET @Status = 1
END
