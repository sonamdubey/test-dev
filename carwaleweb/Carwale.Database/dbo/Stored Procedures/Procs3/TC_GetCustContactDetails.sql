IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCustContactDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCustContactDetails]
GO
	-- =============================================  
-- Author:  Yuga Hatolkar
-- Created On: 8th Dec, 2015
-- Description: Get Customer Contact Details.
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetCustContactDetails]  	 
	 @CustId BIGINT
AS  
BEGIN 
	
	SELECT Mobile AS CustomerContactNumber, CustomerName FROM TC_CustomerDetails WITH(NOLOCK) 
	WHERE Id = @CustId
	
END




