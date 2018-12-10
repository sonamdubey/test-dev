IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CMS_InsertAgency]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CMS_InsertAgency]
GO

	

CREATE  PROCEDURE CMS_InsertAgency

@Name			VARCHAR(200),
@Aliases		VARCHAR(500),
@ContactPerson	VARCHAR(500),
@ContactNumber	VARCHAR(200),
@ContactEmail		VARCHAR(100),
@OtherDetails		VARCHAR(2000),
@IsActive		BIT,
@Id			VARCHAR(10) OUTPUT

AS	
BEGIN
	SET @Id = '-1'

	INSERT INTO CMS_Agencies ([Name], Aliases, ContactPerson,
		ContactNumber, ContactEmail, OtherDetails, IsActive)
	VALUES (@Name, @Aliases, @ContactPerson, @ContactNumber, 
		@ContactEmail, @OtherDetails, @IsActive)

	SET @Id = SCOPE_IDENTITY()
END
