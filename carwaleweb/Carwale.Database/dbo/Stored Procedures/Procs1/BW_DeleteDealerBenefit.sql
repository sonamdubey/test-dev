IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_DeleteDealerBenefit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_DeleteDealerBenefit]
GO

	
-- =============================================
-- Author		:	Sumit Kate
-- Create date	:	10 Mar 2016
-- Description	:	Deletes dealer benefits by making it inactive
-- Parameters	
--	@BenefitIds	:	Benefit Ids (Comma separated values)
-- =============================================
CREATE PROC BW_DeleteDealerBenefit
(
	@BenefitIds VARCHAR(255)
)
AS
BEGIN
	UPDATE Benefits
	SET IsActive = 0
	FROM BW_PQ_DealerBenefit AS Benefits
	INNER JOIN dbo.fnSplitCSVValues(@BenefitIds) val
	ON Benefits.Id = val.ListMember 
END
