IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_SaveDealerFacility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_SaveDealerFacility]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 31 Oct 2014
-- Description:	Proc to save the facility for the given dealer id
-- =============================================
CREATE PROCEDURE [dbo].[BW_SaveDealerFacility]
	-- Add the parameters for the stored procedure here
	@Facility VARCHAR(500),
	@IsActive BIT,
	@DealerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO BW_DealerFacilities
	(DealerId, Facility, IsActive)
	VALUES (@DealerId, @Facility, @IsActive)
END

