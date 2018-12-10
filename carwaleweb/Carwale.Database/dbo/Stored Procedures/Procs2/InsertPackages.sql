IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPackages]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertPackages]
GO
	
--THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR Class Packages TABLE Packages

CREATE PROCEDURE [dbo].[InsertPackages]
	@Id			INT,	-- Id. Will be -1 if Its Insertion
	@Name			VARCHAR(50),	
	@Validity		INT,	--validaty in days
	@InquiryPoints		INT,
	@InqPtCategoryId	INT,
	@ForDealer		BIT,
	@ForIndividual		BIT,
	@Amount		INT,
	@Description		VARCHAR(1000),
	@STATUS		INTEGER OUTPUT	--return value, -1 for unsuccessfull attempt, and 0 for success
	
 AS
	
BEGIN
	SET @Status = 0
	
	IF @Id = -1 

		BEGIN
		
			INSERT INTO Packages (Name, Validity, InquiryPoints,InqPtCategoryId, ForDealer, ForIndividual, Amount, Description )
		
			VALUES (@Name, @Validity, @InquiryPoints, @InqPtCategoryId, @ForDealer, @ForIndividual, @Amount, @Description)
		
			SET @Status = SCOPE_IDENTITY()
		END
	ELSE

		BEGIN
			UPDATE Packages SET Name=@Name, Validity=@Validity, InquiryPoints=@InquiryPoints, InqPtCategoryId=@InqPtCategoryId,
			
			ForDealer=@ForDealer, ForIndividual=@ForIndividual, Amount=@Amount, Description = @Description  WHERE Id = @Id
			
			SET @Status=2
		END
	
END
