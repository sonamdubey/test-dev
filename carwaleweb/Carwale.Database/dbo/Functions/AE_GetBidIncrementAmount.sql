IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AE_GetBidIncrementAmount]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AE_GetBidIncrementAmount]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: Nov 12, 2009
-- Description:	Function returns bid increment amount on the basis of last bid amount
				-- default bid amount will be 1000 rs.
-- =============================================
CREATE FUNCTION [dbo].[AE_GetBidIncrementAmount]
(
	-- Add the parameters for the function here
	@BidAmount		NUMERIC
)
RETURNS INT
AS
BEGIN
	Declare @BidStepAmount	INT = 1000
			
	IF @BidAmount >= 0 AND @BidAmount < 100000		
		Set @BidStepAmount = 1000;
	ELSE IF @BidAmount >= 100000 AND @BidAmount < 500000		
		Set @BidStepAmount = 2000;			
	ELSE IF @BidAmount >= 500000
		Set @BidStepAmount = 3000;
			
	-- Return the result of the function
	RETURN @BidStepAmount

END

