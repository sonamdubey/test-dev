IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealer_UniqueId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealer_UniqueId]
GO

	
---------------------------------------------------------------------
  
-- ============================================= [Microsite_GetDealer_UniqueId] 5      
-- Author:  Kritika Choudhary     
-- Create date : 30th Oct 2015     
-- Description : To get dealers's unique id

-- =============================================      
CREATE PROCEDURE [dbo].[Microsite_GetDealer_UniqueId]      
(        
 @DealerId INT
)          
AS      
BEGIN      
 SET NOCOUNT ON;      
 
SELECT U.UniqueId
FROM TC_Users U WITH(NOLOCK)
JOIN TC_UsersRole UR WITH(NOLOCK) ON U.Id = UR.UserId and UR.RoleId = 16 and U.BranchId = @DealerId
         
END 

