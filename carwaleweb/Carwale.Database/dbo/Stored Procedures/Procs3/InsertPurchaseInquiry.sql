IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPurchaseInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPurchaseInquiry]
GO

	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR PurchaseInquiries and PurchaseInquiryDetails TABLES.

CREATE PROCEDURE [dbo].[InsertPurchaseInquiry]
	@Id		NUMERIC,	-- PurchaseInquiry Id. Will be -1 if Its Insertion
	@DealerId	NUMERIC,	-- Dealer ID
	@CarVersionId	NUMERIC,	-- Car Version Id
	@CarModelId	NUMERIC,	-- Car Model Id
	@StatusId	NUMERIC,	-- Inquiry Status Id
	@EntryDate 	DATETIME,	-- Entry Date
	@YearFrom	NUMERIC,	-- Car Year From
	@YearTo	NUMERIC,	-- Car Year To
	@BudgetFrom	NUMERIC,	-- Budget From
	@BudgetTo	NUMERIC,	-- Budget To
	@MileageFrom	NUMERIC,	-- Mileage From
	@MileageTo	NUMERIC,	-- Mileage To
	@Comments	VARCHAR(500)	-- Dealer Comments
 AS
	
BEGIN
	
	IF @Id = -1 -- Insertion
		BEGIN
			INSERT INTO PurchaseInquiries( DealerId, CarModelId, CarVersionId, StatusId, EntryDate, Comments) 
			VALUES(@DealerId, @CarModelId, @CarVersionId, @StatusId, @EntryDate, @Comments)
			
			-- Now fetch the Id for this Insertion.
			SELECT @Id  = @@IDENTITY  		
			
			-- Start Inserting into Details Table.
			INSERT INTO PurchaseInquiriesDetails( PurchaseInquiryId, StartYear,EndYear,StartBudget,EndBudget,
			StartMileage, EndMileage) 
			VALUES(@Id, @YearFrom, @YearTo, @BudgetFrom, @BudgetTo, @MileageFrom, @MileageTo)
		END
	ELSE -- Updation
		BEGIN
			UPDATE PurchaseInquiries SET CarModelId=@CarModelId, CarVersionId=@CarVersionId, 
			StatusId=@StatusId, Comments=@Comments WHERE ID = @Id

			UPDATE PurchaseInquiriesDetails SET StartYear=@YearFrom, EndYear=@YearTo, StartBudget=@BudgetFrom,
			EndBudget=@BudgetTo, StartMileage=@MileageFrom, EndMileage=@MileageTo  WHERE PurchaseInquiryId=@Id
		END
	
	
END
