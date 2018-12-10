IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FOB_BRDUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[FOB_BRDUpdate]
GO

	
--CREATED ON 10 SEP 2009 BY SENTIL        
--PROCEDURE FOR SUBMIT BOOKING REQUEST DATA        
        
CREATE PROCEDURE [dbo].[FOB_BRDUpdate]         
(        
  @Id AS BIGINT,        
  @DeliveryCityId AS NUMERIC(18,0) = 0,        
  @DealerId AS NUMERIC(18,0) = 0,        
  @VersionID AS NUMERIC(18,0)=0,   
  @Color AS NUMERIC(18,0)=0       
)        
AS        
BEGIN    
    
   
 UPDATE FOB_BookingRequestData    
  SET DealerId = @DealerId , DeliveryCityId = @DeliveryCityId, VersionID = @VersionID, Color = @Color  
  WHERE ID = @ID  
        
--SELECT * FROM FOB_BookingRequestData        
--TRUNCATE TABLE FOB_BookingRequestData         
        
END  