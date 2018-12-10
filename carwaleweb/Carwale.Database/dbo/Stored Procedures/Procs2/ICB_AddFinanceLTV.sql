IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ICB_AddFinanceLTV]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ICB_AddFinanceLTV]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE ICB_FinanceLTV  
  
CREATE PROCEDURE [dbo].[ICB_AddFinanceLTV]  
 @Id     NUMERIC,  
 @FinanceAgencyId NUMERIC,  
 @ModelId   NUMERIC,  
 @VersionId   NUMERIC,  
 @StartTenure  INT,  
 @EndTenure   INT,  
 @Value    DECIMAL(5,2),  
 @Tag    VARCHAR(100),  
 @LastUpdated  DATETIME,  
 @StartIncome  DECIMAL(18,2),  
 @EndIncome   DECIMAL(18,2),  
 @IsSalaried   BIT,  
 @Status    BIT OUTPUT  
 AS  
   
BEGIN  
 IF @Id = -1 --Insertion  
  
  BEGIN  
   INSERT INTO ICB_FinanceLTV  
   ( FinanceAgencyId, ModelId, VersionId, StartTenure,   
    EndTenure, Value, Tag, LastUpdated,   
    StartIncome, EndIncome, IsSalaried  
   )   
    
   Values  
   ( @FinanceAgencyId, @ModelId, @VersionId, @StartTenure,   
    @EndTenure, @Value, @Tag, @LastUpdated,  
    @StartIncome, @EndIncome, @IsSalaried   
   )   
  
   SET @Status = 1  
  END    
   
 ELSE  
  
  BEGIN  
  
   UPDATE ICB_FinanceLTV   
   SET StartTenure = @StartTenure, EndTenure = @EndTenure,   
   Value = @Value, Tag = @Tag, LastUpdated  = @LastUpdated,  
   StartIncome = @StartIncome, EndIncome = @EndIncome,   
   IsSalaried = @IsSalaried   
   WHERE Id = @Id  
  
   SET @Status = 1  
  END   
END  
  
  
  
  
  
  
