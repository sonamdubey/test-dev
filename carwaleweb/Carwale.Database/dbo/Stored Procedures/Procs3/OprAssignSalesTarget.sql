IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OprAssignSalesTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OprAssignSalesTarget]
GO

	




--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Invoices TABLE


CREATE PROCEDURE [dbo].[OprAssignSalesTarget]
	@BId			NUMERIC,		-- Id. Will be -1 if Its Insertion
	@BudgetMonth		NUMERIC,	
	@ExecutiveId		NUMERIC,	
	@versionId		NUMERIC,	
	@Qty			NUMERIC,
	@EntryDate		DATETIME,
	@Status		INTEGER OUTPUT
	
	
 AS
	DECLARE @tempid AS NUMERIC 
		
BEGIN
					
	
	SET @Status = 0

	SELECT @tempid = ID FROM OprAssignSaleTarget WHERE BudgetYear = @BId AND BudgetMonth = @BudgetMonth  AND ExecutiveId = @ExecutiveId
	
	IF @@ROWCOUNT = 0 
	BEGIN
		INSERT INTO OprAssignSaleTarget 
			( BudgetYear, BudgetMonth, executiveId, target , EntryDate  )
		 VALUES 
			( @BId, @BudgetMonth,  @ExecutiveId, @Qty , @EntryDate )
		
		SET @Status = 1	
	END

	ELSE
	BEGIN
		UPDATE OprAssignSaleTarget  SET 
			target 	= @Qty 
		WHERE
			ID = @tempid
		SET @Status = 1	
	END
	
END
