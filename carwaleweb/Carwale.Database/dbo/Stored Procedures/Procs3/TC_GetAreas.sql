IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetAreas]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetAreas]
GO
	-- =============================================  
-- Author:  Binumon George  
-- Create date: 20 Jun 2012  
-- Description: loading All Areas  
-- Modified By: Nilesh Utture on 12th August, 2013 Added parameter @CityId
-- Modified By: Vivek Gupta on 10 Oct 2013, Added condition of BranchId.
-- =============================================  
CREATE PROCEDURE [dbo].[TC_GetAreas]  
@BranchId BIGINT,
@CityId INT = NULL  
AS  
BEGIN  

 IF(@BranchId IS NOT NULL)
 BEGIN
	 SELECT A.Id, Name FROM Areas A  WITH (NOLOCK) 
	 INNER JOIN TC_DealerCities D WITH (NOLOCK) ON A.CityId=D.CityId  
	 WHERE D.DealerId=@BranchId AND A.IsDeleted=0 AND D.IsActive=1 AND (A.CityId = @CityId OR @CityId IS NULL)
	 ORDER BY Name
 END
 ELSE
 BEGIN
     SELECT A.Id, Name FROM Areas A  WITH (NOLOCK) 	 
	 WHERE A.IsDeleted=0 AND (A.CityId = @CityId OR @CityId IS NULL)
	 ORDER BY Name
 END
END 






/****** Object:  StoredProcedure [dbo].[TC_TDDriver]    Script Date: 09/17/2013 18:38:37 ******/
SET ANSI_NULLS ON
