IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetServiceType]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetServiceType]
GO

	
-- =============================================
-- Author:		Umesh Ojha
-- Create date: 11-April-2012
-- Description:	Return Service type of service request raised by customer
-- =============================================
CREATE FUNCTION [dbo].[GetServiceType]
(	
	-- Add the parameters for the function here
	@ServiceValue INT
)
RETURNS Varchar(50)
AS
BEGIN
	DECLARE @ServiceText Varchar(50)	
	SET @ServiceText = (CASE @ServiceValue
		WHEN  1 THEN 'Regular Car Maintenance Service'
		WHEN  2 THEN 'Specific Repair/Maintenance Service'		
	ELSE 'Invalid' END)
	RETURN @ServiceText
END
