IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[HDFCDealerRepresenEdit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[HDFCDealerRepresenEdit]
GO

	-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [cw].[HDFCDealerRepresenEdit]  
 -- Add the parameters for the stored procedure here  
 
  
 @DealerId numeric(18)    
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
SELECT * FROM HDFCDealerRepresentatives where DealerId=@DealerId

SELECT Stateid,CityId FROM Dealers where id=@DealerId
END 