IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetStates]
GO

	-- =============================================        
-- Author:  Umesh Ojha        
-- Create date: 21 Feb 2013      
-- Description: Fetch All states      
-- =============================================        
CREATE PROCEDURE [dbo].[GetStates]      
AS        
BEGIN        
 -- SET NOCOUNT ON added to prevent extra result sets from        
 -- interfering with SELECT statements.        
 SET NOCOUNT ON;       
	SELECT Name, Id FROM States WHERE IsDeleted = 0 ORDER BY Name 
END
