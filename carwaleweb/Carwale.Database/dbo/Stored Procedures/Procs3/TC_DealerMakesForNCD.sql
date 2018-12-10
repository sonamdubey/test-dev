IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerMakesForNCD]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerMakesForNCD]
GO

	-- =============================================  
-- Author:      <Nilesh Utture>  
-- Create date: <05/10/2012>  
-- Description: <Show Makes for NCD Dealers>  
-- EXEC TC_DealerMakesForNCD  NULL 1028
-- Modified By: Nilesh Utture on 13th September, 2013 ADdded condition "OR @DealerID IS NULL" to get all makes
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DealerMakesForNCD]  
 -- Add the parameters for the stored procedure here  
 @DealerID INT 
 AS  
 BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
	 SET NOCOUNT ON;  
	  
	 SELECT ID AS Value, Name AS Text   
	 FROM CarMakes M 
	  INNER JOIN TC_DealerMakes D ON M.ID=D.MakeId  
	 WHERE M.IsDeleted = 0  
	 AND M.Futuristic = 0 
	 AND M.New = 1 
	 AND (D.DealerId=@DealerID OR @DealerID IS NULL)
	 GROUP BY ID, Name
	 ORDER BY Text  

	 IF(@@ROWCOUNT=0)
		EXECUTE cw.GetCarMakes 'New'   

 END  
 
 
 
 
 
 
/****** Object:  StoredProcedure [dbo].[TC_AddUsers]    Script Date: 09/17/2013 18:49:33 ******/
SET ANSI_NULLS ON
