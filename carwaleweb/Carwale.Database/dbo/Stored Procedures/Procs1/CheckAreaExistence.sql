IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckAreaExistence]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckAreaExistence]
GO

	
-- =============================================
-- Author:		<Jitendra singh>
-- Create date: <16/06/2016>
-- Description:	Return AreaAvailable for city or not
-- =============================================
CREATE PROCEDURE [dbo].[CheckAreaExistence]
	-- Add the parameters for the stored procedure here
	@CityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT isAreaAvailable from Cities WITH(NOLOCK) where id = @CityId
END
