IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CompareCars_Sponsored_Logs_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CompareCars_Sponsored_Logs_SP]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 2014-Aug-4
-- Description:	To track that how many times sponsered version appered to the user against compared versions
-- =============================================
CREATE PROCEDURE [dbo].[CompareCars_Sponsored_Logs_SP] -- EXEC CompareCars_Sponsored_Logs_SP 12,13
	-- Add the parameters for the stored procedure here
	@VersionCompared INT,
	@VersionSponsored INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CompareCars_Sponsored_Logs(VersionCompared, VersionSponsored, EntryDate)
	VALUES(@VersionCompared, @VersionSponsored, GETDATE())
END

