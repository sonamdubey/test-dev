IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_DateValidation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_DateValidation]
GO

	
-- =============================================
-- Author	:	Ajay Singh(8rd August 2015)
-- Description	:	Get data for Inventory Date Validation
--EXEC ESM_DateValidation '2015/11/18','2015/11/22',8,NULL,NUll,NULL
-- =============================================
CREATE  PROCEDURE [dbo].[ESM_DateValidation]
@Start DATETIME= NULL,
@End DATETIME =NULL,
@AdUnitId INT=NULL,
@Retval DATETIME OUTPUT
AS
BEGIN

SET @Retval=(SELECT MIN(CAST(InventoryDate AS DATE)) FROM
(SELECT  EI.Id,EIB.InventoryDate,EI.ProposalId,EP.Probability
FROM ESM_Inventory EI WITH(NOLOCK)
INNER JOIN ESM_InventoryBooking EIB WITH(NOLOCK) ON EI.Id=EIB.InventoryId
LEFT JOIN ESM_Proposal EP WITH(NOLOCK) ON EP.id=EI.ProposalId
WHERE EI.AdUnit=@AdUnitId AND EP.Probability>=100 AND EIB.isActive=1 AND CAST(EIB.InventoryDate AS DATE) BETWEEN CAST(@Start AS DATE) AND CAST(@End AS DATE)
)RESULT)

 
END

 


