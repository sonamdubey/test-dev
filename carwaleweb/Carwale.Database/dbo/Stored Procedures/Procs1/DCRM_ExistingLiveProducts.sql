IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_ExistingLiveProducts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_ExistingLiveProducts]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(19th Sep 2014)
-- Description	:	Show existing products for the present DealerId that is 
--					consumerId in ConsumerCreditPoints table
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_ExistingLiveProducts] 
	
	@ConsumerId		INT 
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
			CCP.ConsumerId , 
			IPC.Name,CCP.Points,
			CONVERT(CHAR,CCP.ExpiryDate,106) AS ExpiryDate 
	FROM 
			ConsumerCreditPoints CCP WITH(NOLOCK) 
			INNER JOIN InquiryPointCategory IPC WITH(NOLOCK) ON CCP.PackageType = IPC.Id  
	WHERE 
			CCP.ConsumerId = @ConsumerId AND CCP.ExpiryDate > GETDATE() 
	ORDER BY 
			CCP.ExpiryDate DESC
END

