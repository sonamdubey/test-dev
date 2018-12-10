IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteDealerLoanAmounts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteDealerLoanAmounts]
GO

	

-- =============================================
--	Created By	:	Sumit Kate on 10 Mar 2016
--	Description	:	Delete a loan amount for a dealer
-- =============================================
CREATE PROCEDURE [dbo].[BW_DeleteDealerLoanAmounts]
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN	
	UPDATE BW_DealerLoanAmounts
	SET IsActive = 0
	WHERE ID = @Id
END
