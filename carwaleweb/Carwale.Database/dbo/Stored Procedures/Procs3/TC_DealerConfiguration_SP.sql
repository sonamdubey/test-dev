IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerConfiguration_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerConfiguration_SP]
GO

	



-- =============================================
-- Author:		Tejashree Patil.
-- Create date: 4 May 2012
-- Description:	Save dealer's setting related to displaying Worksheet and inquiry page
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerConfiguration_SP]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@IsWorksheet BIT,
	@FreshLeadCount INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	IF EXISTS(SELECT Id FROM TC_DealerConfiguration WHERE DealerId=@BranchId)
	BEGIN
		UPDATE TC_DealerConfiguration SET isWorksheetOnly=@IsWorksheet,freshLeadCount=@FreshLeadCount WHERE DealerId=@BranchId
		RETURN 1
	END
	ELSE
	BEGIN
		INSERT INTO TC_DealerConfiguration (DealerId,isWorksheetOnly,freshLeadCount) VALUES(@BranchId,@IsWorksheet,@FreshLeadCount)
		RETURN 0
	END	
END