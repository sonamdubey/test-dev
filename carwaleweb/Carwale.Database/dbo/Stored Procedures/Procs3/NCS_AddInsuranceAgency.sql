IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_AddInsuranceAgency]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_AddInsuranceAgency]
GO

	
--THIS PROCEDURE INSERTS THE VALUES FOR THE InsuranceAgency

CREATE PROCEDURE [dbo].[NCS_AddInsuranceAgency]
	@Id				NUMERIC,
	@Name			VARCHAR(100),
	@IsActive		BIT,
	@Status         BIT OUTPUT
 AS
	
BEGIN
	IF @Id = -1 --Insertion
		BEGIN
			SELECT ID FROM NCS_InsuranceAgency WHERE Name = @Name

				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO NCS_InsuranceAgency( Name, IsActive )			
						Values( @Name, @IsActive)	

						SET @Status = 1
					END
				ELSE
					SET @Status = 0
		END

	ELSE

		BEGIN
			SELECT ID FROM NCS_InsuranceAgency WHERE Name = @Name AND ID <>@Id

				IF @@ROWCOUNT = 0
					BEGIN
						UPDATE NCS_InsuranceAgency 
						SET Name = @Name, IsActive = @IsActive			
						WHERE Id = @Id	

						SET @Status = 1
					END
				ELSE
					SET @Status = 0
		END
END
