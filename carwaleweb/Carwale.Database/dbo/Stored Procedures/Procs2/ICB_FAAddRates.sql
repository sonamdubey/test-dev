IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_FAAddRates]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_FAAddRates]
GO

	
  
  
  
CREATE PROCEDURE [dbo].[ICB_FAAddRates]  
 @Id    NUMERIC,  
 @FAID   NUMERIC,  
 @ZoneId   NUMERIC,  
 @GroupId  NUMERIC,  
 @StartTenure NUMERIC,  
 @EndTenure  NUMERIC,  
 @IRSalaried  DECIMAL(10,2),  
 @IRSelfEmp  DECIMAL(10,2),  
 @FinCommission DECIMAL(10,2),  
 @CWCommission DECIMAL(10,2),  
 @Waiver   DECIMAL(10,2),  
 @LastUpdated DATETIME,  
 @Status   BIT OUTPUT  
 AS  
   
BEGIN  
 SET @Status = 0  
   
 IF @Id <> -1  
  
  BEGIN  
   UPDATE ICB_FARates   
   SET StartTenure = @StartTenure, EndTenure = @EndTenure,  
    IRSalaried = @IRSalaried, IRSelfEmp = @IRSelfEmp,   
    FinCommission = @FinCommission, CWCommission = @CWCommission,  
    Waiver = @Waiver, LastUpdated = @LastUpdated  
   WHERE Id = @Id  
     
   SET @Status = 1  
  END  
  
 ELSE  
  
  BEGIN  
   INSERT INTO ICB_FARates  
   ( FAID, ZoneId, GroupId, StartTenure, EndTenure, IRSalaried, IRSelfEmp,   
    FinCommission, CWCommission, Waiver, LastUpdated   
   )     
   Values  
   ( @FAID, @ZoneId, @GroupId, @StartTenure, @EndTenure, @IRSalaried, @IRSelfEmp,   
    @FinCommission, @CWCommission, @Waiver, @LastUpdated   
   )  
   SET @Status = 1  
  END  
     
END  
  
  
  
  
  
  
