IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[GetFUserCarAlertBudget]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [UCAlert].[GetFUserCarAlertBudget]
GO

	CREATE FUNCTION   [UCAlert].[GetFUserCarAlertBudget](@UsedCarAlertId int)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE     @iReturnCode       int,
            @iNextRowId        int,
            @iCurrentRowId     int,
            @iLoopControl      int,            
            @budget varchar(100),
            @budgetcond  varchar(1000) = null

 
		SELECT @budget= BudgetId from UCAlert.UserCarAlerts where UsedCarAlert_Id=@UsedCarAlertId
		
		SELECT @iLoopControl = 1
		SELECT @iNextRowId = MIN(BudgetId)
		FROM UCAlert.Budget WHERE BudgetId in (SELECT * FROM UCAlert.Split(@budget))
		IF @iNextRowId IS NULL
		   BEGIN
					RETURN @budgetcond
		   END


		SELECT   @iCurrentRowId   = BudgetId
		FROM UCAlert.Budget WHERE BudgetId in (SELECT * FROM UCAlert.Split(@budget))
		and  BudgetId = @iNextRowId

		SELECT @budgetcond= ' AND((Price Between '  + cast(LowerVal as varchar(100)) + ' AND '+cast(UpperVal as varchar(100))+')'
		FROM UCAlert.Budget 
		WHERE BudgetId in (SELECT * FROM UCAlert.Split(@budget))
		and  BudgetId = @iNextRowId

		WHILE @iLoopControl = 1

		   BEGIN      

					SELECT  @iNextRowId = NULL 
					SELECT   @iNextRowId = MIN(BudgetId)  
					FROM     UCAlert.Budget      
					WHERE    BudgetId > @iCurrentRowId
					and BudgetId in (SELECT * FROM UCAlert.Split(@budget))       

					IF ISNULL(@iNextRowId,0) = 0
					   BEGIN
								BREAK
					   END          

					SELECT  @iCurrentRowId =   BudgetId,
					        @budgetcond=@budgetcond+ ' OR (Price Between '  + cast(LowerVal as varchar(100)) + ' AND '+cast(UpperVal as varchar(100))+')'
					FROM    UCAlert.Budget  
					WHERE   BudgetId = @iNextRowId   
					   and BudgetId in (SELECT * FROM UCAlert.Split(@budget))   
		               
	             			 

   END
    set @budgetcond=@budgetcond+')'	
    RETURN @budgetcond
    END