IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateDealerFacility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateDealerFacility]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to save the facility for the given dealer id
-- =============================================
CREATE PROCEDURE [dbo].[BW_UpdateDealerFacility]
	-- Add the parameters for the stored procedure here
	@Facility VARCHAR(500),
	@IsActive BIT,
	@FacilityId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE BW_DealerFacilities
	SET Facility = @Facility, IsActive = @IsActive
	where Id = @FacilityId
END

