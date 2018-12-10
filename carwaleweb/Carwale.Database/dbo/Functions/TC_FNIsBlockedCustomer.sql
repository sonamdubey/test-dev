IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FNIsBlockedCustomer]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[TC_FNIsBlockedCustomer]
GO

	

-- Created By:	Deepak
-- Create date: 6 Jan 2012
-- Description:	Check if customer is blocked or not

CREATE FUNCTION [dbo].[TC_FNIsBlockedCustomer](        
	@CustomerMobile VARCHAR(10)
)
RETURNS  BIT   
              
	BEGIN 
	
		DECLARE @IsBlocked BIT = 0
		        
		SELECT @IsBlocked = CASE CustomerMobile WHEN NULL THEN 0 ELSE 1 END FROM TC_BlockedCustomers WITH (NOLOCK) WHERE CustomerMobile = @CustomerMobile
		
		RETURN @IsBlocked
	END


