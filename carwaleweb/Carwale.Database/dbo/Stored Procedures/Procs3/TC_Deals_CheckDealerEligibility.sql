IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_CheckDealerEligibility]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_CheckDealerEligibility]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 7th Jan, 2015
-- Description	: Check if dealer is eligible for deals or not
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_CheckDealerEligibility]
@BranchId INT = NULL,
@IsDealerEligible BIT = NULL OUTPUT
AS
BEGIN

	IF EXISTS(SELECT * FROM TC_Deals_Dealers WITH(NOLOCK) WHERE DealerId = @BranchId AND IsDealerDealActive = 1)
		SET @IsDealerEligible = 1
	else
		SET @IsDealerEligible = 0

END

