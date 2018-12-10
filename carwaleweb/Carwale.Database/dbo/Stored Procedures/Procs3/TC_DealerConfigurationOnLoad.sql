IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerConfigurationOnLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerConfigurationOnLoad]
GO

	-- =============================================
-- Author:		Tejashree Patil.
-- Create date: 4 May 2012
-- Description:	Load dealer's setting related to displaying Worksheet and inquiry page
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerConfigurationOnLoad]
	-- Add the parameters for the stored procedure here
	@BranchId INT,
	@IsWorksheetOnly SMALLINT OUTPUT,
	@FreshLeadCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	IF EXISTS(SELECT Id FROM TC_DealerConfiguration WHERE DealerId=@BranchId)
	--If dealer exist then retrive isWorksheet value and return 1
	BEGIN
		SELECT @IsWorksheetOnly=isWorksheetOnly,@FreshLeadCount=freshLeadCount FROM TC_DealerConfiguration WHERE DealerId=@BranchId
		RETURN 1
	END
	ELSE--If no entry for dealer then return 0
	BEGIN
		RETURN 0
	END	
END

