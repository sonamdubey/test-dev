IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateUnverifiedCustomer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateUnverifiedCustomer]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 20-01-2015
-- Description:	to update isverified = 0 if customer comes from carwale unverified source
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateUnverifiedCustomer]
@BranchId INT,
@CustomerId INT,
@InqSourceId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @InqSourceId <> 93 -- carwale unverified leads 
	BEGIN
	   UPDATE TC_CustomerDetails 
	   SET IsVerified = 1
	   WHERE 
		   Id = @CustomerId 
	   --AND BranchId = @BranchId 
	   AND (IsVerified = 0 OR IsVerified IS NULL)
	END
     
END

