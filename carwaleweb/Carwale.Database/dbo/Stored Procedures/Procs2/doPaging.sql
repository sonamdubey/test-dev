IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[doPaging]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[doPaging]
GO

	
CREATE PROCEDURE [dbo].[doPaging] 

AS

BEGIN

	SET ROWCOUNT  10	
	
	SELECT 
		* FROM SellInquiries
	ORDER BY CarRegNo

END
