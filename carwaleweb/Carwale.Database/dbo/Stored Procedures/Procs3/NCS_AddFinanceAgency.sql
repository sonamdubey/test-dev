IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddFinanceAgency]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddFinanceAgency]
GO

	


--THIS PROCEDURE INSERTS THE VALUES FOR THE FinanceAgency

CREATE PROCEDURE [dbo].[NCS_AddFinanceAgency]
	@Id				NUMERIC,
	@Name			VARCHAR(100),
	@TDS			DECIMAL(5,2),
	@IRType			SMALLINT,
	@LastUpdated	DATETIME,
	@IsActive		BIT,
	@Status			BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_FinanceAgency WHERE Name = @Name

				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO NCS_FinanceAgency
						(Name, TDS, IRType, LastUpdated, IsActive )	
		
						Values
						(@Name, @TDS, @IRType, @LastUpdated, @IsActive)	

						SET @Status = 1
					END
				ELSE
					SET @Status = 0
		END

	ELSE

		BEGIN
			SELECT ID FROM NCS_FinanceAgency WHERE Name = @Name AND ID <>@Id

				IF @@ROWCOUNT = 0
					BEGIN
						UPDATE NCS_FinanceAgency
 
						SET Name = @Name, TDS = @TDS, IRType = @IRType, 
						LastUpdated = @LastUpdated, IsActive = @IsActive
			
						WHERE Id = @Id	

						SET @Status = 1
					END
				ELSE
					SET @Status = 0
		END
END


