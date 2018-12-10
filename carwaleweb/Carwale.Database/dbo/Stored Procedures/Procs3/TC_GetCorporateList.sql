IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCorporateList]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCorporateList]
GO
	-- =============================================  
-- Author:  Umesh Ojha  
-- Create date: 25 Jun 2013  
-- Description: Fetching all corporate list for the paticular make  
-- Added a parameter for autosearch of corporate name from starting
--  Modified By: Vivek Gupta on 17-12-2015, removed check of makeid
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetCorporateList]  
@DealerId INT 
AS  
BEGIN  
	SELECT TC_CorporateListId AS Value ,Name AS Text FROM
	  TC_CorporateList WITH(NOLOCK) WHERE 
	  --MakeId IN 
	  --(SELECT MakeId FROM TC_DealerMakes WHERE DealerId=@DealerId)
	  --AND 
	  IsActive=1   
END 
