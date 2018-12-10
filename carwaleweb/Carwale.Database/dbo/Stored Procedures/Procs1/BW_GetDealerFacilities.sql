IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetDealerFacilities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetDealerFacilities]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to get the dealer facilites
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetDealerFacilities]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT F.Id, F.Facility, F.IsActive
	FROM BW_DealerFacilities F WITH(NOLOCK)
	WHERE DealerId = @DealerId
	ORDER BY F.IsActive DESC, F.Id DESC

END

