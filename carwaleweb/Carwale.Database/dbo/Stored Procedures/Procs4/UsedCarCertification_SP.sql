IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UsedCarCertification_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UsedCarCertification_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 8-Apr-2009
-- Description:	This SP will store customer information who is intersted in
				--certification of used car
-- =============================================
CREATE PROCEDURE [dbo].[UsedCarCertification_SP]
	-- Add the parameters for the stored procedure here
	@VersionId		INT,
	@CustomerId		NUMERIC,
	@ProfileId		VARCHAR(50),
	@EntryDate		DateTime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS( SELECT Id FROM UsedCarCertification WHERE VersionId = @VersionId AND CustomerId = @CustomerId )
		BEGIN
			-- Insert statements for procedure here
			INSERT INTO UsedCarCertification(VersionId, CustomerId, ProfileId, EntryDate)
			VALUES(@VersionId, @CustomerId, @ProfileId, @EntryDate)
		END    
END
