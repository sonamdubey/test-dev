IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckCWAccessToTC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckCWAccessToTC]
GO

	-- ==================================================================================================
-- Author:		Surendra Chouksey
-- Create date: 5th Dec,2011
-- Description:	This procedure is checking whether CW has permission to login Trading Cars or Not
-- Modified By : Tejashree Patil on 10 Jan 2016, Access Trading cars automatically for deals dealers.
-- Modified By : Suresh Prajapti on 10th Mar, 2016
-- Description : 1. Removed Password check
--				 2. Added HashSalt and PasswordHash check
-- ===================================================================================================
CREATE PROCEDURE [dbo].[TC_CheckCWAccessToTC] --14130
	(@BranchId NUMERIC)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @UserId VARCHAR(50)
		--,@Password VARCHAR(50)
		,@HashSalt VARCHAR(10)
		,@PasswordHash VARCHAR(100)

	SET @UserId = CONVERT(VARCHAR, @BranchId) + 'cw@carwale.com'
	--SET @Password = 'Default'
	SET @HashSalt = '8g2GlY'
	SET @PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'

	-- Modified By : Tejashree Patil on 10 Jan 2016, Access Trading cars automatically for deals dealers.
	IF EXISTS (
			SELECT TC_DealerTypeId
			FROM Dealers D WITH (NOLOCK)
			INNER JOIN TC_Deals_Dealers DD WITH (NOLOCK) ON DD.DealerId = D.ID
			WHERE Id = @BranchId
				AND TC_DealerTypeId IN (
					2
					,3
					)
				AND IsTCDealer = 1
				AND IsDealerDealActive = 1
				AND IsDealerActive = 1
			)
		EXEC TC_AlertsUpdate @BranchId
			,1

	IF EXISTS (
			SELECT Id
			FROM TC_Users WITH (NOLOCK)
			WHERE Email = @UserId
				--AND Password = @Password
				AND BranchId = @BranchId
				AND IsActive = 1
				AND HashSalt = '8g2GlY'
				AND PasswordHash = '3517e98e1007e43e47ac78e3c2b0bdfbe347d15ec0e77b078f6dfb4087c5e484'
			)
	BEGIN
		PRINT 1

		RETURN 0
	END
	ELSE
	BEGIN
		PRINT 2

		RETURN - 1
	END
END


