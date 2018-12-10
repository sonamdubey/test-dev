IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckPaidDealerUCD]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckPaidDealerUCD]
GO

	-- Author		:	Upendra
-- Create date	:	30-09-2015
-- Description	:	To fetch whether dealer is paid or not for UCD case


-- =================================================================================================================================   
CREATE PROCEDURE [dbo].[TC_CheckPaidDealerUCD] 
   @BranchId INT,
   @IsPaid BIT = NULL OUTPUT
AS
BEGIN
		SELECT @IsPaid=CASE WHEN (
		SELECT C.ConsumerId FROM 
		ConsumerCreditPoints C WITH(NOLOCK) 
		JOIN Dealers D WITH(NOLOCK)  ON D.Id=C.ConsumerId
		WHERE C.ConsumerType = 1 AND C.PackageType <> 28 
		AND CONVERT(DATE,C.ExpiryDate) >= CONVERT(DATE,GETDATE())
		AND C.ConsumerId = @BranchId	
		AND (D.TC_DealerTypeId=1 OR D.TC_DealerTypeId=3 )
		) IS NULL THEN 0 ELSE 1 END 

		
		

END