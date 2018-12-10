IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_APIUpdateDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_APIUpdateDealerDetails]
GO

	
-- =============================================
-- Author:		Deepak Tripathi
-- Create date: 8 APR 2015
-- Description:	Update Dealer Basic details
-- =============================================
CREATE PROCEDURE [dbo].[TC_APIUpdateDealerDetails]
	-- Add the parameters for the stored procedure here
	@BranchId		NUMERIC(18, 0),
	@Organization	VARCHAR(100),
	@MobileNo		VARCHAR(50)
AS
BEGIN

	IF @BranchId IS NOT NULL AND @BranchId > 0 AND @Organization IS NOT NULL AND @Organization <> '' AND @MobileNo IS NOT NULL AND @MobileNo <> ''
	BEGIN
		UPDATE Dealers SET MobileNo = @MobileNo, Organization = @Organization WHERE ID = @BranchId
	END
END

